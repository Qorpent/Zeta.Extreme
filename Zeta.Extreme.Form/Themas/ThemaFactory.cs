#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : ThemaFactory.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Reporting;
using Zeta.Extreme.Form.InputTemplates;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	Фабрика тем
	/// </summary>
	public class ThemaFactory : IThemaFactory {
		/// <summary>
		/// 	Исходный XML
		/// </summary>
		public string SrcXml { get; set; }

		/// <summary>
		/// 	Получить тему по коду
		/// </summary>
		/// <param name="code"> </param>
		/// <returns> </returns>
		public IThema Get(string code) {
			return Themas.FirstOrDefault(x => x.Code == code);
		}

		/// <summary>
		/// 	Кэш тем
		/// </summary>
		public IDictionary<string, object> Cache {
			get { return cache; }
		}

		/// <summary>
		/// 	Версия
		/// </summary>
		public DateTime Version { get; set; }

		/// <summary>
		/// 	Получить все темы
		/// </summary>
		/// <returns> </returns>
		public IEnumerable<IThema> GetAll() {
			return new List<IThema>(Themas);
		}

		/// <summary>
		/// 	Получить тему в адаптации на текущего пользователя
		/// </summary>
		/// <returns> </returns>
		public IEnumerable<IThema> GetForUser() {
			return GetForUser(myapp.usr);
		}

		/// <summary>
		/// 	Очистить кэш пользователя
		/// </summary>
		/// <param name="usrname"> </param>
		public void CleanUser(string usrname) {
			lock (this) {
				if (cache.ContainsKey(usrname)) {
					cache.Remove(usrname);
				}
			}
		}

		/// <summary>
		/// 	Получить тему в адаптации на конкретного пользователя
		/// </summary>
		/// <param name="usr"> </param>
		/// <returns> </returns>
		public IEnumerable<IThema> GetForUser(IPrincipal usr) {
			return cache.get(usr.Identity.Name, () => internalGetForUser(usr));
		}


		/// <summary>
		/// 	Получить шаблон формы
		/// </summary>
		/// <param name="code"> </param>
		/// <param name="throwerror"> </param>
		/// <returns> </returns>
		public IInputTemplate GetForm(string code, bool throwerror = false) {
			if(code.EndsWith(".in")) {
				code = code.Replace(".in", "");
			}
			return cache.get(code + ".in", () =>
				{
					var thema =
						Themas.OrderByDescending(x => x.Code.Length).FirstOrDefault(
							t => code.StartsWith(t.Code));
					if (null == thema) {
						return null;
					}
					var result = thema.GetForm(code);
					if (null == result && throwerror) {
						throw new Exception("cannto find form with code " + code);
					}
					return result;
				}, true
				);
		}

		/// <summary>
		/// 	Получить дефиницию отчета
		/// </summary>
		/// <param name="code"> </param>
		/// <returns> </returns>
		public IReportDefinition GetReport(string code) {
			return cache.get(code + ".out", () =>
				{
					var thema =
						Themas.OrderByDescending(x => x.Code.Length).FirstOrDefault(
							t => code.StartsWith(t.Code));
					if (null == thema) {
						return null;
					}
					return thema.GetReport(code);
				}, true);
		}

		/// <summary>
		/// 	Выполняет определяемые приложением задачи, связанные с высвобождением или сбросом неуправляемых ресурсов.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose() {
			foreach (var thema in Themas) {
				((Thema) thema).Reports.Clear();
				((Thema) thema).Forms.Clear();
				((Thema) thema).Documents.Clear();
				((Thema) thema).Commands.Clear();
			}
			Themas.Clear();
		}


		private IEnumerable<IThema> internalGetForUser(IPrincipal usr) {
			var personalized = Themas.Select(x => x.Personalize(usr)).ToArray();
			var active = personalized.Where(x => x.IsActive(usr)).BindParents().ToArray();

			active = active.OrderBy(x => x, new themaidxcomparer()).ToArray();
			return active;
		}

		#region Nested type: themaidxcomparer

		private class themaidxcomparer : IComparer<IThema> {
			/// <summary>
			/// 	Сравнивает два объекта и возвращает значение, показывающее, что один объект меньше или больше другого или равен ему.
			/// </summary>
			/// <returns> Знаковое целое число, которое определяет относительные значения <paramref name="x" /> и <paramref name="y" /> , как показано в следующей таблице. Значение Значение Меньше нуля Значение параметра <paramref
			/// 	 name="x" /> меньше значения параметра <paramref name="y" /> . Zero Значение параметра <paramref name="x" /> равно значению параметра <paramref
			/// 	 name="y" /> . Больше нуля. Значение <paramref name="x" /> больше значения <paramref name="y" /> . </returns>
			/// <param name="x"> Первый сравниваемый объект. </param>
			/// <param name="y"> Второй сравниваемый объект. </param>
			public int Compare(IThema x, IThema y) {
				if (x.Parent.hasContent() && y.Parent.noContent()) {
					return -1;
				}
				if (y.Parent.hasContent() && x.Parent.noContent()) {
					return 1;
				}
				if (y.Parent.hasContent() && x.Parent.hasContent()) {
					return 0;
				}
				if (x.Parent.hasContent() && y.Code == x.Parent) {
					return 1;
				}
				if (y.Parent.hasContent() && x.Code == y.Parent) {
					return -1;
				}
				return x.Idx.CompareTo(y.Idx);
			}
		}

		#endregion

		/// <summary>
		/// 	Кэш тем
		/// </summary>
		public readonly IList<IThema> Themas = new List<IThema>();

		private readonly IDictionary<string, object> cache = new Dictionary<string, object>();
	}
}