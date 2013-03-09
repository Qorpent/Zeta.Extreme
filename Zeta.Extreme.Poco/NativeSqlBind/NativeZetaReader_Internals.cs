#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : NativeZetaReader_Internals.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Data;
using Comdiv.Application;
using Qorpent.Applications;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Poco.NativeSqlBind {
	/// <summary>
	/// 	Замена ORM для загрузки метаданных из БД напрямую
	/// </summary>
	public partial class NativeZetaReader {
		private IEnumerable<T> Read<T>(string condition, string commandbase, Func<IDataRecord, T> serializer) {
			var cmdtext = commandbase + (string.IsNullOrWhiteSpace(condition) ? "" : " where " + condition);
			using (var c = getConnection()) {
				c.Open();
				var cmd = c.CreateCommand();
				cmd.CommandText = cmdtext;
				using (var r = cmd.ExecuteReader(CommandBehavior.SingleResult)) {
					while (r.Read()) {
						yield return serializer(r);
					}
				}
			}
		}

		/// <summary>
		/// 	Сериализует строку из БД в объект строки
		/// </summary>
		/// <param name="r"> </param>
		/// <returns> </returns>
		public static row ReaderToRow(IDataRecord r) {
			var x = new row
				{
					Id = r.GetInt32(0),
					Version = r.GetDateTime(1),
					IsFormula = r.GetBoolean(2),
					Name = r.GetString(5),
					Code = r.GetString(6),
					Idx = r.GetInt32(12),
					Path = r.GetString(14),
					Formula = r.GetString(3),
					FormulaEvaluator = r.GetString(4),
					Comment = r.GetString(7),
					OuterCode = r.GetString(11),
					Measure = r.GetString(13),
					Grp = r.GetString(15),
					MarkCache = r.GetString(16),
					Valuta = r.GetString(17),
					Tag = r.GetString(18),
					ExtremeFormulaMode = r.GetInt32(21)
				};

			// no x USR = 19
			if (!r.IsDBNull(8)) {
				x.RefId = r.GetInt32(8);
			}
			if (!r.IsDBNull(9)) {
				x.ParentId = r.GetInt32(9);
			}
			if (!r.IsDBNull(10)) {
				x.ObjectId = r.GetInt32(10);
			}
			if (!r.IsDBNull(20)) {
				x.ExRefToId = r.GetInt32(20);
			}
			return x;
		}

		private IDbConnection getConnection() {
			if (null == Application.Current.DatabaseConnections) {
				return myapp.getConnection("Default");
			}
			return Application.Current.DatabaseConnections.GetConnection("Default") ??
			       myapp.getConnection("Default");
		}

		/// <summary>
		/// 	Сериализует строку из БД в объект строки
		/// </summary>
		/// <param name="r"> </param>
		/// <returns> </returns>
		public static col ReaderToCol(IDataRecord r) {
			var x = new col
				{
					Id = r.GetInt32(0),
					Version = r.GetDateTime(1),
					DataType = (ValueDataType) Enum.Parse(typeof (ValueDataType), r.GetString(2)),
					IsFormula = r.GetBoolean(3),
					Name = r.GetString(6),
					Code = r.GetString(7),
					Formula = r.GetString(4),
					FormulaEvaluator = r.GetString(5),
					Comment = r.GetString(8),
					Measure = r.GetString(9),
					MarkCache = r.GetString(12),
					Valuta = r.GetString(10),
					Tag = r.GetString(11),
				};


			return x;
		}

		/// <summary>
		/// 	Сериализует строку из БД в объект строки
		/// </summary>
		/// <param name="r"> </param>
		/// <returns> </returns>
		public static objdiv ReaderToDiv(IDataRecord r) {
			var x = new objdiv
				{
					Id = r.GetInt32(0),
					Code = r.GetString(1),
					Name = r.GetString(2),
					Comment = r.GetString(3),
					Version = r.GetDateTime(4),
					Tag = r.GetString(5),
					Idx = r.GetInt32(6)
				};
			return x;
		}

		/// <summary>
		/// 	Сериализует строку из БД в объект строки
		/// </summary>
		/// <param name="r"> </param>
		/// <returns> </returns>
		public static obj ReaderToObj(IDataRecord r) {
			var x = new obj
				{
					Id = r.GetInt32(0),
					Code = r.GetString(1),
					Name = r.GetString(2),
					Comment = r.GetString(3),
					Version = r.GetDateTime(4),
					ShortName = r.GetString(5),
					OuterCode = r.GetString(6),
					ShowOnStartPage = r.GetBoolean(10),
					Path = r.GetString(12),
					IsFormula = r.GetBoolean(14),
					Formula = r.GetString(15),
					FormulaEvaluator = r.GetString(16),
					Tag = r.GetString(17),
					GroupCache = r.GetString(18),
					Valuta = r.GetString(19),

					//TODO: Role as access
					//TODO: Active
					Start = r.GetDateTime(22),
					Finish = r.GetDateTime(23),
					//TODO: IsInner
				};

			if (!r.IsDBNull(7)) {
				x.ZoneId = r.GetInt32(7);
			}
			if (!r.IsDBNull(8)) {
				x.RoleId = r.GetInt32(8);
			}
			if (!r.IsDBNull(9)) {
				x.DivId = r.GetInt32(9);
			}
			if (!r.IsDBNull(11)) {
				x.ParentId = r.GetInt32(11);
			}
			if (!r.IsDBNull(13)) {
				x.TypeId = r.GetInt32(13);
			}
			return x;
		}

		/// <summary>
		/// 	Сериализует строку из БД в объект строки
		/// </summary>
		/// <param name="r"> </param>
		/// <returns> </returns>
		public static period ReaderToPeriod(IDataRecord r) {
			var x = new period
				{
					ClassicId = r.GetInt32(0),
					Name = r.GetString(1),
					Idx = r.GetInt32(2),
					MonthCount = r.GetInt32(3),
					IsFormula = r.GetBoolean(4),
					Formula = r.GetString(5),
					Tag = r.GetString(6),
					StartDate = r.GetDateTime(7),
					EndDate = r.GetDateTime(8),
				};
			return x;
		}

		/// <summary>
		/// 	Сериализует строку из БД в объект строки
		/// </summary>
		/// <param name="r"> </param>
		/// <returns> </returns>
		public static formstate ReaderToFormstate(IDataRecord r) {
			var x = new formstate
				{
					Id = r.GetInt32(0),
					Version = r.GetDateTime(1),
					Comment = r.GetString(2),
					State = r.GetString(3),
					Usr = r.GetString(4),
				};
			if (!r.IsDBNull(5)) {
				x.FormId = r.GetInt32(5);
			}
			if (!r.IsDBNull(6)) {
				x.ParentId = r.GetInt32(6);
			}
			return x;
		}

		/// <summary>
		/// 	Сериализует строку из БД в объект строки
		/// </summary>
		/// <param name="r"> </param>
		/// <returns> </returns>
		public static form ReaderToForm(IDataRecord r) {
			var x = new form
				{
					Id = r.GetInt32(0),
					Version = r.GetDateTime(1),
					Code = r.GetString(2),
					Year = r.GetInt32(3),
					Period = r.GetInt32(4),
					Template = r.GetString(5),
				};
			if (!r.IsDBNull(6)) {
				x.ObjectId = r.GetInt32(6);
			}
			return x;
		}

		/// <summary>
		/// 	Сериализует строку из БД в объект строки
		/// </summary>
		/// <param name="r"> </param>
		/// <returns> </returns>
		public static usr ReaderToUsr(IDataRecord r) {
			var x = new usr
				{
					Id = r.GetInt32(0),
					Version = r.GetDateTime(1),
					Boss = r.GetBoolean(2),
					Dolzh = r.GetString(3),
					Contact = r.GetString(4),
					Name = r.GetString(5),
					Comment = r.GetString(6),
					Login = r.GetString(9),
					Active = r.GetBoolean(10),
					Roles = r.GetString(11),
					SlotList = r.GetString(12),
				};

			if (!r.IsDBNull(7)) {
				x.Object = ObjCache.Get(r.GetInt32(7));
			}

			return x;
		}
	}
}