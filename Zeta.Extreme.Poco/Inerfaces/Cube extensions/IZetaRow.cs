// // Copyright 2007-2010 Comdiv (F. Sadykov) - http://code.google.com/u/fagim.sadykov/
// // Supported by Media Technology LTD 
// //  
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //  
// //      http://www.apache.org/licenses/LICENSE-2.0
// //  
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// // 
// // MODIFICATIONS HAVE BEEN MADE TO THIS FILE
using System;
using System.Collections.Generic;
using Comdiv.Model;
using Comdiv.Olap.Model;
using Comdiv.Persistence;

namespace Comdiv.Zeta.Model{

    [global::Zeta.Extreme.Poco.Deprecated.ForSearch("Строка, признак")]
    public interface IZetaRow :
        IOlapForm<IZetaRow>,
		IZetaQueryDimension,
        IZetaFormsSupport,
        IWithFixRules,
        IWithMarkCache,
        IWithMarks<IZetaRow, IZetaRowMark>,
        IWithCells<IZetaCell, IZetaRow, IZetaColumn, IZetaMainObject, IZetaDetailObject>,
        IWithMainObject<IZetaMainObject>{
        IDictionary<string, object> LocalProperties { get; }
        [Map]
        string ObjectGroups { get; set; }
        [Map]
        string FormElementType { get; set; }
        [Map]
        string Validator { get; set; }
        [Map]
        string ColumnSubstitution { get; set; }
        [Map]
        string FullName { get; set; }
        [Map]
        string Role { get; set; }
    [Map]
        string Valuta { get; set; }
        int? ParentId { get; set; }
        int? RefId { get; set; }
        int? ObjectId { get; set; }
        IList<IZetaRow> NativeChildren { get; }
        IZetaRow[] AllChildren { get; }
        string FullRole { get; }
        string ResolveColumnCode(string incode);
        IZetaRow Copy(bool withchildren);
        void ResetAllChildren();
        void CleanupByChildren(IEnumerable<string> codes);
        void ApplyProperty(string property, object value, bool cascade = true);
        void ApplyPropertyIfNew(string property, object value, bool children = false);
        bool HasHelp();
        MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null, string system = "Default");
    	void ApplyPropertyByCondition (string prop, object value, bool applyUp, bool applyDown, Func<IZetaRow,bool> test );
    	void PropagateGroupAsProperty(string groupname, bool applyUp = true, string propname = null);
    	object GetLocal(string name);
        int Level { get; }

        [Ref(ClassName = typeof (IZetaRow))]
        IZetaRow ExRefTo { get; set; }

        int? ExRefToId { get; set; }
    	[Map] bool Active { get; set; }
    	IZetaRow TemporalParent { get; set; }
    	string ResolveTag (string name);

    	/// <summary>
    	/// Возвращает копию строки, отфильтрованную по крыжу для предприятия по крыжу и (опционально) по филиалам
    	/// </summary>
    	/// <param name="objid">ИД предприятия </param>
    	/// <param name="selectorname">Код крыжа</param>
    	/// <param name="usefilialfilter">Использовать фильтр по филиалам</param>
    	/// <returns></returns>
    	IZetaRow FilterTree(int objid, string selectorname, bool usefilialfilter = false);

    	/// <summary>
    	/// Проверяет - является ли строка избранной для предприятия по указанному крыж-селектору
    	/// </summary>
    	/// <param name="objid">ИД предприятия</param>
    	/// <param name="selectorname"></param>
    	/// <returns></returns>
    	bool IsFavorite(int objid, string selectorname);

    	string ResolveMeasure();
	    bool IsActiveFor(IZetaMainObject obj);
	    bool IsObsolete(int year);
        }
}