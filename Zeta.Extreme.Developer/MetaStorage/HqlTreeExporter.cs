namespace Zeta.Extreme.Developer.MetaStorage {
	/// <summary>
	/// Экспортер дерева в имеющийся традиционный 'HQL'
	/// </summary>
	public class HqlTreeExporter : TreeExporter {
		/// <summary>
		/// Метод записи шапки скрипта экспорта
		/// </summary>
		protected override void WriteHeader()
		{
			Buffer.WriteLine("#----------------------------------------------------------------------");
			Buffer.WriteLine("#		ЭКСПОРТ ДЕРЕВА                   ");
			Buffer.WriteLine("#		исх. код.: {0}                   ",Root.Code);
			Buffer.WriteLine("#		режим рута : {0}                 ", Rootmode);
			Buffer.WriteLine("#-------------------------------------- -------------------------------");
		}

		/// <summary>
		/// Начало скрипта (подготовительные операции)
		/// </summary>
		protected override void WriteStartScript()
		{
			Buffer.WriteLine("create row tree=PARENT");
		}

		/// <summary>
		/// Начальная фаза записи строки
		/// </summary>
		protected override void WritePreRow()
		{
			for (var i = 0; i <= Level; i++) {
				Buffer.Write("\t");
			}
			Buffer.Write("row ");
			Buffer.Write(Current.Code);
			Buffer.Write(" '");
			Buffer.Write(Current.Name);
			Buffer.Write("'");
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
			if (Current == Root) {
				if (!Rootmode) {
					WriteAttribute("Parent", Current.ParentCode);		
				}
			}
			WriteAttribute("Tag",Current.Tag);
			WriteAttribute("MarkCache",Current.MarkCache);
			WriteAttribute("GroupCache",Current.GroupCache);
			WriteAttribute("OuterCode",Current.OuterCode);
			WriteAttribute("Measure",Current.Measure);
			if (null != Current.RefTo) {
				WriteAttribute("RefTo", Current.RefTo.Code);	
			}
			if (0 != Current.Index) {
				WriteAttribute("idx", Current.Index.ToString());	
			}
			if (Current.IsFormula) {
				WriteAttribute("IsFormula","1");
				WriteAttribute("FormulaEvaluator", Current.FormulaType);
				WriteAttribute("Formula", Current.Formula.Replace("'","\'"));
			}
		}

		private void WriteAttribute(string name, string value) {
			//if (!string.IsNullOrWhiteSpace(value)) {
				Buffer.Write(" ");
				Buffer.Write(name);
				Buffer.Write("='");
				Buffer.Write(value.Trim());
				Buffer.Write("'");
		//	}
		}
	}
}