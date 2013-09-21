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
// PROJECT ORIGIN: Zeta.Extreme.Model/NativeZetaReader.cs
#endregion

using System;
using System.Collections.Generic;
using Zeta.Extreme.Model.Deprecated;

namespace Zeta.Extreme.Model.SqlSupport {
	public partial class NativeZetaReader {
		private const string Divquerybase = @"
				select 
					Id, Code, Name, Comment, Version, Tag,Idx
				from zeta.normaldiv
		";
		private const string Peiodquerybase = @"
				select 
					Id,Name,Idx,MonthCount,IsFormula,Formula,Tag, Start,Finish ,Category,Version
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

		private const string ObjAndTypequerybase = @"
				select 
					o.Id,			o.Code,			o.Name,	o.Comment,	o.Version,  -- 0 - 4
					ShortName,	OuterCode,		ZoneId,	ObjRoleId,	ObjDivId, -- 5 - 9
					Main,		ParentId,		Path,	TypeId,		IsFormula, -- 10 - 14
					Formula,	FormulaType,	o.Tag,	GroupCache,	Valuta,  -- 15 - 19
					Role,		Active,			Start,	Finish,		IsInner, -- 20 - 24

					t.Code as TypeCode, t.Name as TypeName, t.ClassId as ClassId,
					c.Code as ClassCode, c.Name as ClassName,
					t.Tag as TypeTag, c.Tag as ClassTag

				from zeta.normalobj o join zeta.normalobjtype t on t.Id = o.TypeId join zeta.normalobjclass c on c.Id = t.ClassId
		";

		private const string Colquerybase = @"
				select 
					Id,				Version,	DataType,	IsFormula,	Formula,  --0 - 4
					FormulaType,	Name,		Code,		Comment,	Measure,  --5 - 9
					Valuta,			Tag,		MarkCache,	Idx,		Usr		  --10-14
				from zeta.normalcol
		";

		private const string Bizprocessquerybase = @"
				select 
					Id, Code, Name,Comment,Tag,
					InProcess, Role, IsFinal, RootRows, Process,Version
				from zeta.normalbizprocess
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
	
		private const string Historyquerybase = @"
			select  Id,Time,CellId,BizKey,Value,Deleted,Usr from zeta.normalhist
		";
        private const string ObjClassQueryBase = @"
			select  Id,Code,Name, Comment, Version,   Tag from zeta.normalobjclass
		";
        private const string ObjTypeQueryBase = @"
			select  Id,Code,Name,Comment, Version,ClassId,Tag from zeta.normalobjtype
		";

		private const string Cellquerybase = @"
			select Id, Version, Year, Period, RowId, ColId, ObjId, DetailId, DecimalValue, StringValue, Usr, Currency, ContragentId from zeta.normalcell
		";

		private const string UserThemaQueryBase = "select login from usm.Underwriter u join usm.usrthemamap um on u.id = um.usr where ('{0}' = '' or thema = '{0}') and object = {1}";

		private const string GetCurrencyRateQueryBase =
				"select value from usm.periodcourse where  year = {0}  and period = {1} and intype ='{2}' and outtype ='{3}'";

		private const string GlobalRefreshDate = "select comdiv.get_global_refresh_time()";

        /// <summary>
        /// 	Сериализует классы объектов
        /// 	Внимание! ТОЧКА ДЛЯ SQL-атаки, API для экспорта не предназначено!
        /// </summary>
        /// <param name="condition"> </param>
        /// <returns> </returns>
#pragma warning disable 612,618
        public IEnumerable<ObjectClass> ReadObjClasses(string condition = "")
#pragma warning restore 612,618
        {
            return Read(condition, ObjClassQueryBase, ReaderToObjectClass);
        }

        /// <summary>
        /// 	Сериализует классы объектов
        /// 	Внимание! ТОЧКА ДЛЯ SQL-атаки, API для экспорта не предназначено!
        /// </summary>
        /// <param name="condition"> </param>
        /// <returns> </returns>
        public IEnumerable<ObjectType> ReadObjTypes(string condition = "")
        {
            return Read(condition, ObjTypeQueryBase, ReaderToObjectType);
        }

		/// <summary>
		/// 	Сериализует учетные записи пользователей
		/// 	Внимание! ТОЧКА ДЛЯ SQL-атаки, API для экспорта не предназначено!
		/// </summary>
		/// <param name="condition"> </param>
		/// <returns> </returns>
		public IEnumerable<User> ReadUsers(string condition = "") {
			return Read(condition, Usrquerybase, ReaderToUsr);
		}

		/// <summary>
		/// 	Сериализует периоды
		/// 	Внимание! ТОЧКА ДЛЯ SQL-атаки, API для экспорта не предназначено!
		/// </summary>
		/// <param name="condition"> </param>
		/// <returns> </returns>
		public IEnumerable<Period> ReadPeriods(string condition = "") {
			return Read(condition, Peiodquerybase, ReaderToPeriod);
		}


		/// <summary>
		/// 	Сериализует периоды
		/// 	Внимание! ТОЧКА ДЛЯ SQL-атаки, API для экспорта не предназначено!
		/// </summary>
		/// <param name="condition"> </param>
		/// <returns> </returns>
		public IEnumerable<BizProcess> ReadBizProcesses(string condition = "")
		{
			return Read(condition, Bizprocessquerybase, ReaderToBizProcess);
		}

		/// <summary>
		/// 	Сериализует формы
		/// 	Внимание! ТОЧКА ДЛЯ SQL-атаки, API для экспорта не предназначено!
		/// </summary>
		/// <param name="condition"> </param>
		/// <returns> </returns>
		public IEnumerable<Form> ReadForms(string condition = "") {
			return Read(condition, Formquerybase, ReaderToForm);
		}

		/// <summary>
		/// 	Сериализует статусы форм
		/// 	Внимание! ТОЧКА ДЛЯ SQL-атаки, API для экспорта не предназначено!
		/// </summary>
		/// <param name="condition"> </param>
		/// <returns> </returns>
		public IEnumerable<FormState> ReadFormStates(string condition = "") {
			return Read(condition, Formstatequerybase, ReaderToFormstate);
		}


		/// <summary>
		/// 	Сериализует дивизионы
		/// 	Внимание! ТОЧКА ДЛЯ SQL-атаки, API для экспорта не предназначено!
		/// </summary>
		/// <param name="condition"> </param>
		/// <returns> </returns>
		public IEnumerable<Division> ReadDivisions(string condition = "") {
			return Read(condition, Divquerybase, ReaderToDiv);
		}

		/// <summary>
		/// 	Сериализует объекты
		/// 	Внимание! ТОЧКА ДЛЯ SQL-атаки, API для экспорта не предназначено!
		/// </summary>
		/// <param name="condition"> </param>
		/// <returns> </returns>
		public IEnumerable<Obj> ReadObjects(string condition = "") {
			return Read(condition, Objquerybase, ReaderToObj);
		}

		/// <summary>
		/// 	Сериализует объекты
		/// 	Внимание! ТОЧКА ДЛЯ SQL-атаки, API для экспорта не предназначено!
		/// </summary>
		/// <param name="condition"> </param>
		/// <returns> </returns>
		public IEnumerable<Obj> ReadObjectsWithTypes(string condition = "")
		{
			return Read(condition, ObjAndTypequerybase, ReaderToObjWithTypes);
		}

		/// <summary>
		/// 	Сериализует строки по стандартному виду с использованием переданного условия condition
		/// 	Внимание! ТОЧКА ДЛЯ SQL-атаки, API для экспорта не предназначено!
		/// </summary>
		/// <param name="condition"> </param>
		/// <returns> </returns>
		public IEnumerable<Column> ReadColumns(string condition = "") {
			return Read(condition, Colquerybase, ReaderToCol);
		}

		/// <summary>
		/// 	Сериализует строки по стандартному виду с использованием переданного условия condition
		/// 	Внимание! ТОЧКА ДЛЯ SQL-атаки, API для экспорта не предназначено!
		/// </summary>
		/// <param name="condition"> </param>
		/// <returns> </returns>
		public IEnumerable<Row> ReadRows(string condition = "") {
			return Read(condition, Rowquerybase, ReaderToRow);
		}
		/// <summary>
		/// Сериализует историю по конкретной ячейке (не поддерживается полный поиск!!!)
		/// </summary>
		/// <param name="targetCellId"></param>
		/// <returns></returns>
		public IEnumerable<CellHistory> GetCellHistory(int targetCellId) {
			return Read("cellid = " + targetCellId, Historyquerybase, ReaderToHistory);
		}
		/// <summary>
		/// Запрашивет ячейки напрямую
		/// </summary>
		/// <param name="condition"></param>
		/// <returns></returns>
		public IEnumerable<Cell> GetCells(string condition = "") {
			return Read(condition, Cellquerybase, ReaderToCell);
		}

		/// <summary>
		/// Retrieves global refresh hook from Zeta DB
		/// </summary>
		/// <returns></returns>
		public DateTime GetLastGlobalRefreshTime() {
			return GetScalar(GlobalRefreshDate, DateTime.MinValue);
		}


		/// <summary>
		/// Retrieves global refresh hook from Zeta DB
		/// </summary>
		/// <returns></returns>
		public string GetThemaResponsiveLogin(string themacode,int objid) {
			return GetScalar(UserThemaQueryBase, "", themacode, objid);

		}


		/// <summary>
		/// Получение курса валюты на период
		/// </summary>
		/// <param name="year"></param>
		/// <param name="period"></param>
		/// <param name="intype"></param>
		/// <param name="outtype"></param>
		/// <returns></returns>
		public decimal GetCurrencyRate(int year, int period, string intype, string outtype = "RUB") {
			return GetScalar(GetCurrencyRateQueryBase,0m, year, period, intype, outtype);
			
		}
	}
}