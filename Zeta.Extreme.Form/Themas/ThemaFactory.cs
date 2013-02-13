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
	/// 	������� ���
	/// </summary>
	public class ThemaFactory : IThemaFactory {
		/// <summary>
		/// 	�������� XML
		/// </summary>
		public string SrcXml { get; set; }

		/// <summary>
		/// 	�������� ���� �� ����
		/// </summary>
		/// <param name="code"> </param>
		/// <returns> </returns>
		public IThema Get(string code) {
			return Themas.FirstOrDefault(x => x.Code == code);
		}

		/// <summary>
		/// 	��� ���
		/// </summary>
		public IDictionary<string, object> Cache {
			get { return cache; }
		}

		/// <summary>
		/// 	������
		/// </summary>
		public DateTime Version { get; set; }

		/// <summary>
		/// 	�������� ��� ����
		/// </summary>
		/// <returns> </returns>
		public IEnumerable<IThema> GetAll() {
			return new List<IThema>(Themas);
		}

		/// <summary>
		/// 	�������� ���� � ��������� �� �������� ������������
		/// </summary>
		/// <returns> </returns>
		public IEnumerable<IThema> GetForUser() {
			return GetForUser(myapp.usr);
		}

		/// <summary>
		/// 	�������� ��� ������������
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
		/// 	�������� ���� � ��������� �� ����������� ������������
		/// </summary>
		/// <param name="usr"> </param>
		/// <returns> </returns>
		public IEnumerable<IThema> GetForUser(IPrincipal usr) {
			return cache.get(usr.Identity.Name, () => internalGetForUser(usr));
		}


		/// <summary>
		/// 	�������� ������ �����
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
		/// 	�������� ��������� ������
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
		/// 	��������� ������������ ����������� ������, ��������� � �������������� ��� ������� ������������� ��������.
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
			/// 	���������� ��� ������� � ���������� ��������, ������������, ��� ���� ������ ������ ��� ������ ������� ��� ����� ���.
			/// </summary>
			/// <returns> �������� ����� �����, ������� ���������� ������������� �������� <paramref name="x" /> � <paramref name="y" /> , ��� �������� � ��������� �������. �������� �������� ������ ���� �������� ��������� <paramref
			/// 	 name="x" /> ������ �������� ��������� <paramref name="y" /> . Zero �������� ��������� <paramref name="x" /> ����� �������� ��������� <paramref
			/// 	 name="y" /> . ������ ����. �������� <paramref name="x" /> ������ �������� <paramref name="y" /> . </returns>
			/// <param name="x"> ������ ������������ ������. </param>
			/// <param name="y"> ������ ������������ ������. </param>
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
		/// 	��� ���
		/// </summary>
		public readonly IList<IThema> Themas = new List<IThema>();

		private readonly IDictionary<string, object> cache = new Dictionary<string, object>();
	}
}