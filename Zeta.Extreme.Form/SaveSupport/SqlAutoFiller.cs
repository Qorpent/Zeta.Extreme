#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : SqlAutoFiller.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Inversion;
using Comdiv.Persistence;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Form.SaveSupport {
	/// <summary>
	/// 	Автозаполнение на SQL
	/// </summary>
	public class SqlAutoFiller : IAutoFillExecutor {
		/// <summary>
		/// 	Ссылка на контейнер
		/// </summary>
		public IInversionContainer Container {
			get {
				if (_container.invalid()) {
					lock (this) {
						if (_container.invalid()) {
							Container = myapp.ioc;
						}
					}
				}
				return _container;
			}
			set { _container = value; }
		}


		/// <summary>
		/// 	Выполнить автозаполнение
		/// </summary>
		/// <param name="autoFill"> </param>
		/// <param name="obj"> </param>
		public void Execute(AutoFill autoFill, IZetaMainObject obj) {
			var sql = autoFill.CallData;
			var code = (autoFill.Template.Thema).GetParameter("fillcode", "");
			if (code.noContent()) {
				code = autoFill.Template.Form.Code;
				if (autoFill.Template.Rows.Count > 1) {
					code = (autoFill.Template.Thema).Code;
				}
			}

			var dict = new Dictionary<string, object>
				{
					{"@obj", obj.Id},
					{"@form", code},
					{"@year", autoFill.Template.Year},
					{"@period", autoFill.Template.Period},
					{"@dobj", 0},
					{"@date", autoFill.Template.DirectDate}
				};

			if (sql == "DEFAULT") {
				sql =
					@"
                exec usm.AutoFill 
                        @obj=@obj,@form=@form,
                        @year=@year,@period=@period,
                        @dobj=@dobj,@date=@date
                ";
			}
			Container.getConnection().ExecuteNonQuery(sql, dict);
		}

		private IInversionContainer _container;
	}
}