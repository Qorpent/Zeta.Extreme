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
// PROJECT ORIGIN: Zeta.Extreme.Model/NativeZetaReader_Internals.cs
#endregion
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Qorpent.Applications;
using Zeta.Extreme.Model.Deprecated;
using Zeta.Extreme.Model.MetaCaches;

namespace Zeta.Extreme.Model.SqlSupport {
	/// <summary>
	/// 	������ ORM ��� �������� ���������� �� �� ��������
	/// </summary>
	public partial class NativeZetaReader {
		private IEnumerable<T> Read<T>(string condition, string commandbase, Func<IDataRecord, T> serializer) {

			var cmdtext = commandbase + (string.IsNullOrWhiteSpace(condition) ? "" : " where " + condition);

            if (null != DebugHook) {
                bool empty = true;
                foreach (var item in DebugHook(cmdtext)) {
                    empty = false;
                    yield return (T) item;
                }
                if(!empty)yield break;
            }

			using (var c = GetConnection()) {
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
		/// 	����������� ������ �� �� � ������ ������
		/// </summary>
		/// <param name="r"> </param>
		/// <returns> </returns>
		public static Row ReaderToRow(IDataRecord r) {
			var x = new Row
				{
					Id = r.GetInt32(0),
					Version = r.GetDateTime(1),
					IsFormula = r.GetBoolean(2),
					Name = r.GetString(5),
					Code = r.GetString(6),
					Index = r.GetInt32(12),
					Path = r.GetString(14),
					Formula = r.GetString(3),
					FormulaType = r.GetString(4),
					Comment = r.GetString(7),
					OuterCode = r.GetString(11),
					Measure = r.GetString(13),
					GroupCache = r.GetString(15),
					MarkCache = r.GetString(16),
					Currency = r.GetString(17),
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

		/// <summary>
		/// �������� ���������� � Zeta
		/// </summary>
		/// <returns></returns>
		public IDbConnection GetConnection() {
			if (!string.IsNullOrWhiteSpace(ConnectionString)) {
				return new SqlConnection(ConnectionString);
			}
			return Application.Current.DatabaseConnections.GetConnection("Default");
		}


		public string ConnectionString { get; set; }

		/// <summary>
		/// 	����������� ������ �� �� � ������ �������
		/// </summary>
		/// <param name="r"> </param>
		/// <returns> </returns>
		public static Column ReaderToCol(IDataRecord r) {
			var x = new Column
				{
					Id = r.GetInt32(0),
					Version = r.GetDateTime(1),
					DataType = (ValueDataType) Enum.Parse(typeof (ValueDataType), r.GetString(2)),
					IsFormula = r.GetBoolean(3),
					Name = r.GetString(6),
					Code = r.GetString(7),
					Formula = r.GetString(4),
					FormulaType = r.GetString(5),
					Comment = r.GetString(8),
					Measure = r.GetString(9),
					MarkCache = r.GetString(12),
					Currency = r.GetString(10),
					Tag = r.GetString(11),
				};


			return x;
		}


		/// <summary>
		/// 	����������� ������ �� �� � ������ ������-���������
		/// </summary>
		/// <param name="r"> </param>
		/// <returns> </returns>
		public static BizProcess ReaderToBizProcess(IDataRecord r)
		{
			var x = new BizProcess
			{
				Id = r.GetInt32(0),
				Code = r.GetString(1),
				Name = r.GetString(2),
				Comment = r.GetString(3),
				Tag = r.GetString(4),
				InProcess =  r.GetString(5),
				Role =  r.GetString(6),
				IsFinal =  r.GetBoolean(7),
				RootRows =  r.GetString(8),
				Process =  r.GetString(9),
                Version = r.GetDateTime(10),
			};


			return x;
		}

		/// <summary>
		/// 	����������� ������ �� �� � ������ ������
		/// </summary>
		/// <param name="r"> </param>
		/// <returns> </returns>
		public static Division ReaderToDiv(IDataRecord r) {
			var x = new Division
				{
					Id = r.GetInt32(0),
					Code = r.GetString(1),
					Name = r.GetString(2),
					Comment = r.GetString(3),
					Version = r.GetDateTime(4),
					Tag = r.GetString(5),
					Index = r.GetInt32(6)
				};
			return x;
		}


        /// <summary>
        /// 	����������� ���� �� �� � ������ ������
        /// </summary>
        /// <param name="r"> </param>
        /// <returns> </returns>
        public static Zone ReaderToZone(IDataRecord r)
        {
            var x = new Zone
            {
                Id = r.GetInt32(0),
                Code = r.GetString(1),
                Name = r.GetString(2),
                Comment = r.GetString(3),
                Tag = r.GetString(4),
                Version = r.GetDateTime(5),
            };
            return x;
        }

        /// <summary>
        /// 	����������� ������� (�����������) �� �� � ������ ������
        /// </summary>
        /// <param name="r"> </param>
        /// <returns> </returns>
        public static Department ReaderToDepartment(IDataRecord r)
        {
            var x = new Department
            {
                Id = r.GetInt32(0),
                Code = r.GetString(1),
                Name = r.GetString(2),
                Comment = r.GetString(3),
                Tag = r.GetString(4),
                Version = r.GetDateTime(5),
            };
            return x;
        }

        /// <summary>
        /// 	����������� ������ �� �� � ������ ������
        /// </summary>
        /// <param name="r"> </param>
        /// <returns> </returns>
        public static Region ReaderToRegion(IDataRecord r)
        {
            var x = new Region
            {
                Id = r.GetInt32(0),
                Code = r.GetString(1),
                Name = r.GetString(2),
                Comment = r.GetString(3),
                Tag = r.GetString(4),
                Version = r.GetDateTime(5),
                ZoneId = r.GetInt32(6),
            };
            
            return x;
        }

        /// <summary>
        /// 	����������� ������ �� �� � ������ ������
        /// </summary>
        /// <param name="r"> </param>
        /// <returns> </returns>
        public static Point ReaderToPoint(IDataRecord r)
        {
            var x = new Point
            {
                Id = r.GetInt32(0),
                Code = r.GetString(1),
                Name = r.GetString(2),
                Comment = r.GetString(3),
                Tag = r.GetString(4),
                Version = r.GetDateTime(5),
                RegionId = r.GetInt32(6),
            };

            return x;
        }

		/// <summary>
		/// 	����������� ������ �� �� � ������ ������
		/// </summary>
		/// <param name="r"> </param>
		/// <returns> </returns>
		public static Obj ReaderToObj(IDataRecord r) {
			var x = new Obj
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
					FormulaType = r.GetString(16),
					Tag = r.GetString(17),
					GroupCache = r.GetString(18),
					Currency = r.GetString(19),

					//TODO: Role as access
					//TODO: Active
					Start = r.GetDateTime(22),
					Finish = r.GetDateTime(23),
					//TODO: IsInner
				};

			if (!r.IsDBNull(7)) {
				x.PointId = r.GetInt32(7);
			}
			if (!r.IsDBNull(8)) {
				x.DepartmentId = r.GetInt32(8);
			}
			if (!r.IsDBNull(9)) {
				x.DivisionId = r.GetInt32(9);
			}
			if (!r.IsDBNull(11)) {
				x.ParentId = r.GetInt32(11);
			}
			if (!r.IsDBNull(13)) {
				x.ObjTypeId = r.GetInt32(13);
			}

			return x;
		}


		/// <summary>
		/// 	����������� ������ �� �� � ������ ������
		/// </summary>
		/// <param name="r"> </param>
		/// <returns> </returns>
		public static Obj ReaderToObjWithTypes(IDataRecord r) {
			var x = ReaderToObj(r);
			if (x.ObjTypeId.HasValue && 0!=x.ObjTypeId.Value) {
				var type = new ObjectType {Id = x.ObjTypeId.Value, Code = r.GetString(25), Name = r.GetString(26), Tag=r.GetString(30)};
#pragma warning disable 612,618
				var cls = new ObjectClass {Id = r.GetInt32(27), Code = r.GetString(28), Name = r.GetString(29),Tag=r.GetString(31)};
				type.Class = cls;
				x.ObjType = type;
#pragma warning restore 612,618

			}
			return x;
		}

		/// <summary>
		/// 	����������� ������ �� �� � ������ ������
		/// </summary>
		/// <param name="r"> </param>
		/// <returns> </returns>
		public static Period ReaderToPeriod(IDataRecord r) {
			var x = new Period
				{
					BizId = r.GetInt32(0),
					Name = r.GetString(1),
					Index = r.GetInt32(2),
					MonthCount = r.GetInt32(3),
					IsFormula = r.GetBoolean(4),
					Formula = r.GetString(5),
					Tag = r.GetString(6),
					StartDate = r.GetDateTime(7),
					EndDate = r.GetDateTime(8),
					Category = r.GetString(9),
                    Version = r.GetDateTime(10),
				};
			return x;
		}

		/// <summary>
		/// 	����������� ������ �� �� � ������ ������
		/// </summary>
		/// <param name="r"> </param>
		/// <returns> </returns>
		public static FormState ReaderToFormstate(IDataRecord r) {
			var x = new FormState
				{
					Id = r.GetInt32(0),
					Version = r.GetDateTime(1),
					Comment = r.IsDBNull(2)? "" : r.GetString(2),
					State = r.IsDBNull(3)?"0ISOPEN" : r.GetString(3),
					User = r.IsDBNull(4)?"unk": r.GetString(4),
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
		/// 	����������� ������ �� �� � ������ ������
		/// </summary>
		/// <param name="r"> </param>
		/// <returns> </returns>
		public static Form ReaderToForm(IDataRecord r) {
			var x = new Form
				{
					Id = r.GetInt32(0),
					Version = r.GetDateTime(1),
					Code = r.GetString(2),
					Year = r.GetInt32(3),
					Period = r.GetInt32(4),
					BizCaseCode = r.GetString(5),
				};
			if (!r.IsDBNull(6)) {
				x.ObjectId = r.GetInt32(6);
			}
			return x;
		}

		/// <summary>
		/// 	����������� ������ �� �� � ������ ������
		/// </summary>
		/// <param name="r"> </param>
		/// <returns> </returns>
		public static User ReaderToUsr(IDataRecord r) {
			var x = new User
				{
					Id = r.GetInt32(0),
					Version = r.GetDateTime(1),
					IsLocalAdmin = r.GetBoolean(2),
					Occupation = r.GetString(3),
					Contact = r.GetString(4),
					Name = r.GetString(5),
					Comment = r.IsDBNull(6)?"": r.GetString(6),
					Login = r.GetString(9),
					Active = r.GetBoolean(10),
					Roles =r.IsDBNull(11)?"": r.GetString(11),
					SlotList = r.IsDBNull(12)?"": r.GetString(12),
				};

			if (!r.IsDBNull(7)) {
				x.Object = ObjCache.Get(r.GetInt32(7));
			}

			return x;
		}
		/// <summary>
		/// ����������� ������ �� �� � ������ ������� ������
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		public static CellHistory ReaderToHistory(IDataRecord r) {
			// Id,Time,CellId,BizKey,Value,Deleted,Usr
			var _ = new CellHistory
				{
					Id = r.GetInt32(0),
					Time = r.GetDateTime(1),
					CellId = r.GetInt32(2),
					BizKey =  r.IsDBNull(3)?"NOKEY": r.GetString(3),
					Value = r.IsDBNull(4)? "0" : r.GetDecimal(4).ToString(),
					User = r.IsDBNull(6)?"NOUSER" :r.GetString(6)
				};
			return _;
		}
		/// <summary>
		/// ����������� ������ �� ������
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		private Cell ReaderToCell(IDataRecord r) {
			//Id, Version, Year, Period, RowId, ColId, ObjId, DetailId, DecimalValue, StringValue, Usr, Currency, ContragentId
			//0    1		2       3       4     5      6      7            8            9         10     11          12
			var _= new Cell
			{
				Id = r.GetInt32(0),
				Version = r.GetDateTime(1),
				Year = r.GetInt32(2),
				Period = r.GetInt32(3),
				NumericValue = r.GetDecimal(8),
				StringValue = r.GetString(9),
				User = r.GetString(10),
				Currency = r.GetString(11)
			};
			if (!r.IsDBNull(4)) {
				_.RowId = r.GetInt32(4);
				_.Row = MetaCache.Default.Get<Row>(_.RowId);
			}

			if (!r.IsDBNull(5))
			{
				_.ColumnId = r.GetInt32(5);
				_.Column = MetaCache.Default.Get<Column>(_.ColumnId);
			}

			if (!r.IsDBNull(6))
			{
				_.ObjectId = r.GetInt32(6);
				_.Object = MetaCache.Default.Get<Obj>(_.ObjectId);
			}

			if (!r.IsDBNull(12))
			{
				_.ContragentId = r.GetInt32(6);
				_.Contragent = MetaCache.Default.Get<Obj>(_.ContragentId);
			}

			if (!r.IsDBNull(7))
			{
				_.DetailId = r.GetInt32(7);
				///_.Detail = MetaCache.Default.Get<Obj>(_.ObjectId); //TODO : detail load is not supported for now
			}

			return _;
		}

		private T GetScalar<T>(string commandbase, T def, params object[] parameters) {
			var commandText = string.Format(commandbase,(object[])parameters);
			try
			{
				using (var c = GetConnection())
				{
					c.Open();
					var cmd = c.CreateCommand();
					cmd.CommandText = commandText;
					return (T)cmd.ExecuteScalar();
				}
			}
			catch
			{
				return def;
			}
		}

#pragma warning disable 612,618
	    private ObjectClass ReaderToObjectClass(IDataRecord r) {
#pragma warning restore 612,618
            var _ = new ObjectClass
            {
                Id = r.GetInt32(0),
                Code = r.GetString(1),
                Name = r.GetString(2),
                Comment =r.GetString(3),
                Version = r.GetDateTime(4),
                Tag = r.GetString(5),
            };
            return _;
	    }

	    private ObjectType ReaderToObjectType(IDataRecord r) {
            var _ = new ObjectType
            {
                Id = r.GetInt32(0),
                Code = r.GetString(1),
                Name = r.GetString(2),
                Comment = r.GetString(3),
                Version = r.GetDateTime(4),
                ClassId =r.GetInt32(5),
                Tag = r.GetString(6),
            };
            return _;
	    }

	    
	}
}