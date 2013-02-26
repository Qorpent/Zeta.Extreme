using System.Collections.Generic;
using Zeta.Extreme.Poco;

namespace Zeta.Extreme.Meta {
	public partial class NativeZetaReader {

		private const string Divquerybase = @"
				select 
					Id, Code, Name, Comment, Version, Tag,Idx
				from zeta.normaldiv
		";
		private const string Peiodquerybase = @"
				select 
					Id,Name,Idx,MonthCount,IsFormula,Formula,Tag 
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

		/// <summary>
		/// Сериализует периоды
		/// Внимание! ТОЧКА ДЛЯ SQL-атаки, API для экспорта не предназначено!
		/// </summary>
		/// <param name="condition"></param>
		/// <returns></returns>
		public IEnumerable<period> ReadPeriods(string condition = "")
		{
			return Read(condition, Peiodquerybase, ReaderToPeriod);
		}


		/// <summary>
		/// Сериализует дивизионы
		/// Внимание! ТОЧКА ДЛЯ SQL-атаки, API для экспорта не предназначено!
		/// </summary>
		/// <param name="condition"></param>
		/// <returns></returns>
		public IEnumerable<objdiv> ReadDivisions(string condition = "")
		{
			return Read(condition, Divquerybase, ReaderToDiv);
		}

		/// <summary>
		/// Сериализует объекты
		/// Внимание! ТОЧКА ДЛЯ SQL-атаки, API для экспорта не предназначено!
		/// </summary>
		/// <param name="condition"></param>
		/// <returns></returns>
		public IEnumerable<obj> ReadObjects(string condition = "")
		{
			return Read(condition, Objquerybase, ReaderToObj);
		}

		/// <summary>
		/// Сериализует строки по стандартному виду с использованием переданного условия condition
		/// Внимание! ТОЧКА ДЛЯ SQL-атаки, API для экспорта не предназначено!
		/// </summary>
		/// <param name="condition"></param>
		/// <returns></returns>
		public IEnumerable<col> ReadColumns(string condition = "")
		{
			return Read(condition, Colquerybase, ReaderToCol);
		}

		/// <summary>
		/// Сериализует строки по стандартному виду с использованием переданного условия condition
		/// Внимание! ТОЧКА ДЛЯ SQL-атаки, API для экспорта не предназначено!
		/// </summary>
		/// <param name="condition"></param>
		/// <returns></returns>
		public IEnumerable<row> ReadRows(string condition = "") {
			return Read(condition, Rowquerybase, ReaderToRow);
		}
	}
}