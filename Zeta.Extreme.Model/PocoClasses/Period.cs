#region LICENSE

// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Model/Period.cs

#endregion

using System;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model {
	/// <summary>
	///     ������������� ���������� �������
	/// </summary>
	public class Period : IPeriod {
	    private object _context;

	    /// <summary>
		///     ������ �����
		/// </summary>
		public virtual string Tag { get; set; }

		/// <summary>
		///     ������ ��, ������������ � ����������
		/// </summary>
		public virtual int BizId { get; set; }

		/// <summary>
		///     ��������� �������
		/// </summary>
		public virtual string Category { get; set; }

		/// <summary>
		///     ����������� ���
		/// </summary>
		public virtual string ShortName { get; set; }

		/// <summary>
		///     ������� ������� ������ ���
		/// </summary>
		public virtual bool IsDayPeriod { get; set; }

		/// <summary>
		///     ��������� ���� ������� (�����������, 1899-01-01 ��� �������� 0)
		/// </summary>
		public virtual DateTime StartDate { get; set; }

		/// <summary>
		///     �������� ���� ������� (�����������, 1899-01-01 ��� �������� 0)
		/// </summary>
		public virtual DateTime EndDate { get; set; }

		/// <summary>
		///     ���������� ������� � �������
		/// </summary>
		public virtual int MonthCount { get; set; }

		/// <summary>
		///     ��� �������
		/// </summary>
		public string FormulaType { get; set; }

		/// <summary>
		///     ������� ���������� �������
		/// </summary>
		public virtual bool IsFormula { get; set; }

		/// <summary>
		///     ������ �������
		/// </summary>
		public virtual string Formula { get; set; }


		/// <summary>
		///     ��������� ���������� �������������
		/// </summary>
		public virtual string Code { get; set; }

		/// <summary>
		///     �����������
		/// </summary>
		public virtual string Comment { get; set; }

		/// <summary>
		///     ������������� ���������� �������������
		/// </summary>
		public virtual int Id { get; set; }

		/// <summary>
		///     An index of object
		/// </summary>
		public virtual int Index { get; set; }

		/// <summary>
		///     ��������/���
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		///     ��������
		/// </summary>
		public virtual DateTime Version { get; set; }
        /// <summary>
        /// ��������� ������� �������������� ����, ������� ��� ��������
        /// </summary>
        /// <param name="tagname"></param>
        /// <param name="testvalue"></param>
        /// <returns></returns>
        public virtual bool IsTagSet(string tagname, string testvalue = null)
        {
            if (string.IsNullOrWhiteSpace(Tag)) return false;
            if (null == testvalue)
            {
                return Tag.Contains("/" + tagname + ":");
            }
            return Tag.Replace(" ", "").Contains("/" + tagname + ":" + testvalue + "/");
        }

        /// <summary>
        /// ���������� ������� �������������� ����
        /// </summary>
        /// <param name="tagname"></param>
        /// <returns></returns>
        public virtual string TagGet(string tagname)
        {
            if (string.IsNullOrWhiteSpace(Tag)) return string.Empty;
            var teststr = "/" + tagname + ":";
            var startindex = Tag.IndexOf(teststr, StringComparison.Ordinal);
            if (-1 == startindex) return string.Empty;
            startindex += teststr.Length;
            var endindex = Tag.IndexOf("/", startindex, StringComparison.Ordinal);
            if (-1 == endindex) return string.Empty;
            var length = endindex - startindex;
            return Tag.Substring(startindex, length).Trim();
        }

	    /// <summary>
	    /// ����� ����� ������������ ������� ������������ �������, ������������� ������� �� ����������
	    /// </summary>
	    /// <param name="context"></param>
	    public void SetContext(object context) {
	        _context = context;
	    }

	    /// <summary>
	    /// ���������� ����������� ������
	    /// </summary>
	    /// <returns></returns>
	    public object GetContext() {
	        return _context;
	    }
	}
}