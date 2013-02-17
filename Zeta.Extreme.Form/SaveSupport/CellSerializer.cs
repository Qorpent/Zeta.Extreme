#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : CellSerializer.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Model.Interfaces;
using Comdiv.Persistence;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Form.SaveSupport {
	/// <summary>
	/// 	—ериализатор €чеек из XML
	/// </summary>
	public class CellSerializer {
		/// <summary>
		/// </summary>
		public CellSerializer() {
			storage = myapp.storage.Get<IPkg>();
		}

		/// <summary>
		/// 	—читывает €чейки из XML
		/// </summary>
		/// <param name="xml"> </param>
		/// <returns> </returns>
		public IEnumerable<IWithId> ReadXml(string xml) {
			var doc = new XPathDocument(new StringReader(xml));
			var nav = doc.CreateNavigator();
			var main = nav.Select("/data/item");
#if USEPKG
            var pkg = storage.New<IPkg>();
            pkg.Type = storage.Load<IPkgType>("0FS");
            pkg.PkgState = PkgState.Closed;
            pkg.CreateTime = DateTime.Now;
            pkg.DocumentDate = DateTime.Now;
            pkg.Usr = myapp.usrName;
            pkg.Code = "0FS_" + pkg.Usr.Replace("\\", "-") + "_" + pkg.DocumentDate.ToString("yyyy-MM-dd-hh-mm-ss");
            pkg.Name = pkg.Code;
            yield return pkg;
#endif
			while (main.MoveNext()) {
				var n = main.Current.Clone();
				var val = n.GetAttribute("value", string.Empty);
				if (val.Trim() == "-") {
					val = "";
				}
				IZetaCell cell = null;
				var id = n.GetAttribute("id", string.Empty).toInt();
				if (0 != id) {
					cell = storage.Load<IZetaCell>(id);
					if (val.Trim() == "--") {
						cell.Tag = "delete";
						yield return cell;
						continue;
					}
					val = val.Replace("\xa0", "");
					cell.Value = val;
				}
				else {
					if (val.Trim() == "--") {
						continue;
					}
					cell = storage.New<IZetaCell>();
					var valueType = n.GetAttribute("valueType", string.Empty);
					if (Regex.IsMatch(valueType, "^\\d+$")) {
						cell.Column = ColumnCache.get(valueType.toInt());
					}
					else {
						//valueType = SpecialObjectHelper.ResolveCode(valueType);
						cell.Column = ColumnCache.get(valueType);
					}

					var meta = n.GetAttribute("meta", string.Empty);
					if (Regex.IsMatch(meta, "^\\d+$")) {
						cell.Row = RowCache.get(meta.toInt());
					}
					else {
						//meta = SpecialObjectHelper.ResolveCode(meta);
						cell.Row = RowCache.get(meta);
					}


					var subpart = n.GetAttribute("subpart", string.Empty);
					var year = n.GetAttribute("year", string.Empty);
					var kvart = n.GetAttribute("kvart", string.Empty);
					//var scenario = n.GetAttribute("scenario", string.Empty);
					var directdate = n.GetAttribute("date", string.Empty);

					//if (scenario.noContent()){
					//    scenario = "2";
					//}

					var org = n.GetAttribute("org", string.Empty);

					if (org.hasContent()) {
						cell.Object = storage.Load<IZetaMainObject>(org.toInt());
					}
					if (subpart.hasContent() && "0" != subpart) {
						cell.DetailObject = storage.Load<IZetaDetailObject>(subpart.toInt());
						cell.Object = cell.DetailObject.Object;
						cell.AltObj = cell.DetailObject.AltObject;
					}

					cell.Valuta = "RUB";


					var orgvaluta = cell.Object.Valuta;
					if (orgvaluta.hasContent() && orgvaluta != "NONE") {
						cell.Valuta = orgvaluta;
					}

					var detailvaluta = "";
					if (cell.DetailObject != null) {
						detailvaluta = cell.DetailObject.Valuta;
						if (detailvaluta.hasContent() && detailvaluta != "NONE") {
							cell.Valuta = detailvaluta;
						}
					}
					var rowvaluta = cell.Row.Valuta;
					if (rowvaluta.hasContent() && rowvaluta != "NONE") {
						cell.Valuta = rowvaluta;
					}
					var colvaluta = cell.Column.Valuta;
					if (colvaluta.hasContent() && colvaluta != "NONE") {
						cell.Valuta = colvaluta;
					}

					var vattr = n.GetAttribute("valuta", "");
					if (vattr.hasContent()) {
						cell.Valuta = vattr;
					}


					if (null == cell.Object && null != cell.DetailObject) {
						cell.Object = cell.DetailObject.Object;
					}


					cell.Year = year.toInt();
					if (kvart.hasContent()) {
						cell.Period = kvart.toInt();
					}


					//cell.Scenario = storage.Load<IScenario>(scenario);
					cell.Value = val;
					cell.IsAuto = false;

					//row.Month = 0;

					cell.DirectDate = new DateTime(1900, 1, 1);
					if (directdate.hasContent()) {
						cell.DirectDate = DateTime.ParseExact(directdate, "d.M.yyyy", CultureInfo.InvariantCulture);
						if (cell.DirectDate != DateExtensions.Begin) {
							cell.Year = cell.DirectDate.Year;
							cell.Period = 365;
						}
					}
					if (cell.DirectDate.Year <= 1900) {
						var pd = Periods.Get(cell.Period);
						if (null != pd && !pd.IsDayPeriod) {
							var edate = pd.EndDate;
							if (edate.Year == 1899) {
								edate = new DateTime(cell.Year, edate.Month, edate.Day);
							}
							else if (edate.Year == 1898) {
								edate = new DateTime(cell.Year - 1, edate.Month, edate.Day);
							}
							if (edate.Month == 2 && edate.Day == 28) {
								edate = new DateTime(edate.Year, 3, 1).AddDays(-1);
							}
							cell.DirectDate = edate;
						}
					}

					//if (month.HasContent())
					//    row.Month = int.Parse(month);
				}

				cell.Usr = myapp.usrName + (myapp.principals.BasePrincipal.Identity.Name.ToLower() !=
				                            myapp.usrName.ToLower()
					                            ? "(" + myapp.principals.BasePrincipal.Identity.Name + ")"
					                            : "");

				// cell.Pkg = pkg;
				yield return cell;
			}
		}

		private readonly StorageWrapper<IPkg> storage;
	}
}