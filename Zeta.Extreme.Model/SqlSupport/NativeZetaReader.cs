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
	}
}