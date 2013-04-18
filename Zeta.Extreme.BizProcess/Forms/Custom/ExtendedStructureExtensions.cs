using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Zeta.Extreme.BizProcess.Forms.Custom
{
	/// <summary>
	/// Вспомогательный класс для чтения расширений структуры форм
	/// </summary>
	public static class ExtendedStructureExtensions
	{
		/// <summary>
		/// Обеспечивает чтение сведений из структуры по SQL запросу с заданными именами полей
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="command"></param>
		/// <returns></returns>
		public static IEnumerable<ExtendedStructureItem> ReadNativeStructure(string connectionString,string command) {
			using (var c = new SqlConnection(connectionString)) {
				IDictionary<string, int> fields = new Dictionary<string, int>();
				c.Open();
				var cmd = c.CreateCommand();
				cmd.CommandText = command;
				IList<ExtendedStructureItem> result = new List<ExtendedStructureItem>();

				using (var reader = cmd.ExecuteReader()) {
					while (reader.Read()) {
						if (fields.Count == 0) {
							for (var i = 0; i < reader.FieldCount; i++) {
								fields[reader.GetName(i).ToLower()] = i;
							}
						}
						var subresult = new ExtendedStructureItem();
						foreach (var field in fields) {
							if (reader.IsDBNull(field.Value)) continue;
							if (field.Key == "detail") {
								subresult.Detail = reader.GetInt32(field.Value);
							}
							else if (field.Key == "detailname") {
								subresult.DetailName = reader.GetString(field.Value);
							}
							else if (field.Key == "rowcode") {
								subresult.RowCode = reader.GetString(field.Value);
							}
							else if (field.Key == "obj") {
								subresult.Obj = reader.GetInt32(field.Value);
							}
							else if (field.Key == "type") {
								subresult.Type = reader.GetInt32(field.Value);
							}
							else if (field.Key == "rowname") {
								subresult.RowName = reader.GetString(field.Value);
							}
							else if (field.Key == "rowpath") {
								subresult.RowPath = reader.GetString(field.Value);
							}
						}

						result.Add(subresult);
					}
				}
				return result;
			}
		}


		

		/// <summary>
		/// Обеспечивает чтение сведений из структуры по SQL запросу с заданными именами полей
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="command"></param>
		/// <returns></returns>
		public static IEnumerable<ExtendedSplitStructureItem> ReadNativeSplitStructure(string connectionString, string command)
		{
			using (var c = new SqlConnection(connectionString))
			{
				IDictionary<string, int> fields = new Dictionary<string, int>();
				c.Open();
				var cmd = c.CreateCommand();
				cmd.CommandText = command;
				IList<ExtendedSplitStructureItem> result = new List<ExtendedSplitStructureItem>();

				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						if (fields.Count == 0)
						{
							for (var i = 0; i < reader.FieldCount; i++)
							{
								fields[reader.GetName(i).ToLower()] = i;
							}
						}
						var subresult = new ExtendedSplitStructureItem();
						foreach (var field in fields)
						{
							if(reader.IsDBNull(field.Value))continue;
							if (field.Key == "id")
							{
								subresult.AltObj = Convert.ToInt32(reader[field.Value]);
							}
							else if (field.Key == "name")
							{
								subresult.AltObjName = reader.GetString(field.Value);
							}
							else if (field.Key == "bill")
							{
								subresult.BillCode = reader.GetString(field.Value);
							}
							else if (field.Key == "grp") {
								subresult.GrpId = reader.GetString(field.Value);
							}
							else if (field.Key == "grpname")
							{
								subresult.GrpName = reader.GetString(field.Value);
							}
							else if (field.Key == "idx")
							{
								subresult.Idx = reader.GetInt32(field.Value);
							}
							else if (field.Key == "detail")
							{
								subresult.Detail = reader.GetInt32(field.Value);
							}
							else if (field.Key == "tid")
							{
								subresult.Type = reader.GetInt32(field.Value);
							}
							else if (field.Key == "grpidx")
							{
								subresult.GrpIdx = reader.GetInt32(field.Value);
							}
							else if (field.Key == "sname")
							{
								subresult.SimpleName = reader.GetString(field.Value);
							}
						}

						result.Add(subresult);
					}
				}
				return result;
			}
		}
	}
}
