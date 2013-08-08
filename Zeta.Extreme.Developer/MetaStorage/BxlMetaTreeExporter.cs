namespace Zeta.Extreme.Developer.MetaStorage {
	/// <summary>
	/// Экспортер дерева в имеющийся традиционный 'HQL'
	/// </summary>
	public class BxlMetaTreeExporter : TreeExporter
	{
		/// <summary>
		/// Начальная фаза записи строки
		/// </summary>
		protected override void WritePreRow()
		{
			for (var i = 0; i < Level; i++)
			{
				Buffer.Write("\t");
			}
			
			Buffer.Write("row ");
			Buffer.Write(Current.Code);
			if (Current.RefTo == null || Current.Name == Current.RefTo.Name) {
				Buffer.Write(" '");
				Buffer.Write(Current.Name);
				Buffer.Write("'");	
			}
		}

		/// <summary>
		/// Запись окончания строки
		/// </summary>
		protected override void WritePostRow()
		{
			Buffer.WriteLine();
		}

		/// <summary>
		/// Запись тела строки
		/// </summary>
		protected override void WriteRowBody()
		{
			if (Current == Root)
			{
				if (!Rootmode)
				{
					WriteAttribute("Parent", Current.ParentCode);
				}
			}
			if (null != Current.RefTo)
			{
				WriteAttribute("ref", Current.RefTo.Code);
			}

			WriteAttribute("tag", Current.Tag);
			WriteAttribute("marks", Current.MarkCache);
			WriteAttribute("groups", Current.GroupCache);
			WriteAttribute("outercode", Current.OuterCode);
			WriteAttribute("measure", Current.Measure);
			
			if (0 != Current.Index)
			{
				WriteAttribute("idx", Current.Index.ToString());
			}
		
			if (Current.IsFormula)
			{
				WriteAttribute("isformula", "1");
				
			}
			WriteAttribute("formulatype", Current.FormulaType);
			WriteAttribute("formula", Current.Formula.Replace("'", "\'"));
		}

		private void WriteAttribute(string name, string value)
		{
			if (!string.IsNullOrWhiteSpace(value)) {
				Buffer.Write(" ");
				Buffer.Write(name);
				Buffer.Write("='");
				Buffer.Write(value.Trim());
				Buffer.Write("'");
			}
		}
	}
}