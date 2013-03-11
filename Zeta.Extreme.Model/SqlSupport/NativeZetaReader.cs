#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : NativeZetaReader.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;

namespace Zeta.Extreme.Model.SqlSupport {
	public partial class NativeZetaReader {
		private const string Divquerybase = @"
				select 
					Id, Code, Name, Comment, Version, Tag,Idx
				from zeta.normaldiv
		";
		private const string Peiodquerybase = @"
				select 
					Id,Name,Idx,MonthCount,IsFormula,Formula,Tag, Start,Finish 
				from zeta.normalperiod
		";

		private const string Objquerybase = @"
				select 
					Id,			Code,			Name,	Comment,	Version,  -- 0 - 4
					ShortName,	OuterCode,		ZoneId,	ObjRoleId,	ObjDivId, -- 5 - 9
					Main,		ParentId,		Path,	TypeId,		IsFormula, -- 10 - 14
					Formula,	FormulaType,	Tag,	GroupCache,	Valuta,  -- 15 - 19
					Role,		Active,			Start,	Finish,		IsInner -- 20 - 24
				from zeta.normalobj
		";

		private const string Colquerybase = @"
				select 
					Id,				Version,	DataType,	IsFormula,	Formula,  --0 - 4
					FormulaType,	Name,		Code,		Comment,	Measure,  --5 - 9
					Valuta,			Tag,		MarkCache,	Idx,		Usr		  --10-14
				from zeta.normalcol
		";

		private const string Rowquerybase = @"
				select 
					Id,			Version,	IsFormula,	Formula,	FormulaType,	-- 0 - 4
					Name,		Code,		Comment,	RefId,		ParentId,		-- 5 - 9
					ObjectId,	OuterCode,	Idx,		Measure,	Path,			-- 10 - 14
					Grp,		MarkCache,	Valuta,		Tag,		Usr,			-- 15 - 19
					ExRefId,	ExtremeFormulaMode									-- 20
				from zeta.normalrow";

		private const string Formquerybase = @"
				select 
					Id, Version, Code, Year, Period, LockCode, Object from 
				zeta.normalform
		";
		private const string Formstatequerybase = @"
				select 
					Id, Version, Comment, State, Usr, Form, Parent , Year,Period,LockCode,Object --, Idx, Tag 
				from zeta.normalformstate
		";

		private const string Usrquerybase = @"
			select 
				Id, Version, ObjAdmin, Dolzh, Contact, Name, Email,  ObjId, ObjName, Login, Active, Roles, SlotList 
			from [zeta].[normalusr]
		";

		/// <summary>
		/// 	����������� ������� ������ �������������
		/// 	��������! ����� ��� SQL-�����, API ��� �������� �� �������������!
		/// </summary>
		/// <param name="condition"> </param>
		/// <returns> </returns>
		public IEnumerable<usr> ReadUsers(string condition = "") {
			return Read(condition, Usrquerybase, ReaderToUsr);
		}

		/// <summary>
		/// 	����������� �������
		/// 	��������! ����� ��� SQL-�����, API ��� �������� �� �������������!
		/// </summary>
		/// <param name="condition"> </param>
		/// <returns> </returns>
		public IEnumerable<period> ReadPeriods(string condition = "") {
			return Read(condition, Peiodquerybase, ReaderToPeriod);
		}

		/// <summary>
		/// 	����������� �����
		/// 	��������! ����� ��� SQL-�����, API ��� �������� �� �������������!
		/// </summary>
		/// <param name="condition"> </param>
		/// <returns> </returns>
		public IEnumerable<form> ReadForms(string condition = "") {
			return Read(condition, Formquerybase, ReaderToForm);
		}

		/// <summary>
		/// 	����������� ������� ����
		/// 	��������! ����� ��� SQL-�����, API ��� �������� �� �������������!
		/// </summary>
		/// <param name="condition"> </param>
		/// <returns> </returns>
		public IEnumerable<formstate> ReadFormStates(string condition = "") {
			return Read(condition, Formstatequerybase, ReaderToFormstate);
		}


		/// <summary>
		/// 	����������� ���������
		/// 	��������! ����� ��� SQL-�����, API ��� �������� �� �������������!
		/// </summary>
		/// <param name="condition"> </param>
		/// <returns> </returns>
		public IEnumerable<objdiv> ReadDivisions(string condition = "") {
			return Read(condition, Divquerybase, ReaderToDiv);
		}

		/// <summary>
		/// 	����������� �������
		/// 	��������! ����� ��� SQL-�����, API ��� �������� �� �������������!
		/// </summary>
		/// <param name="condition"> </param>
		/// <returns> </returns>
		public IEnumerable<obj> ReadObjects(string condition = "") {
			return Read(condition, Objquerybase, ReaderToObj);
		}

		/// <summary>
		/// 	����������� ������ �� ������������ ���� � �������������� ����������� ������� condition
		/// 	��������! ����� ��� SQL-�����, API ��� �������� �� �������������!
		/// </summary>
		/// <param name="condition"> </param>
		/// <returns> </returns>
		public IEnumerable<col> ReadColumns(string condition = "") {
			return Read(condition, Colquerybase, ReaderToCol);
		}

		/// <summary>
		/// 	����������� ������ �� ������������ ���� � �������������� ����������� ������� condition
		/// 	��������! ����� ��� SQL-�����, API ��� �������� �� �������������!
		/// </summary>
		/// <param name="condition"> </param>
		/// <returns> </returns>
		public IEnumerable<row> ReadRows(string condition = "") {
			return Read(condition, Rowquerybase, ReaderToRow);
		}
	}
}