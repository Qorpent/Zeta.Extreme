using System.Linq;

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
			for (var i = 0; i <= Level; i++)
			{
				Buffer.Write("\t");
			}
			if (string.IsNullOrWhiteSpace(CurrentCode)) {
				Buffer.Write("row");
			}
			else {
				Buffer.Write(CurrentCode);
			}
			Buffer.Write(" ");
			WriteAttribute("outercode", Current.OuterCode, false);
			if (Current.RefTo == null || Current.Name == Current.RefTo.Name) {
				Buffer.Write(" '");
				Buffer.Write(Current.Name);
				Buffer.Write("' ");	
			}
		}

		/// <summary>
		/// Начало скрипта (подготовительные операции)
		/// </summary>
		protected override void WriteStartScript() {
			Buffer.WriteLine("zetatree codemode="+Options.CodeMode);
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
				if (!Options.DetachRoot)
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
			
			WriteAttribute("measure", Current.Measure);
			
			if (0 != Current.Index)
			{
				WriteAttribute("idx", Current.Index.ToString());
			}
			if (!string.IsNullOrWhiteSpace(Current.Formula)) {
				
				if ("boo" != Current.FormulaType) {
					WriteAttribute("formulatype", Current.FormulaType);
				}
				Buffer.WriteLine();
				for (var i = 0; i <= Level+1; i++)
				{
					Buffer.Write("\t");
				}
				var name = "formula";
				if (!Current.IsFormula) {
					name = "ignoredformula";
				}
				Buffer.Write(name);
				Buffer.Write("=(");
				Buffer.Write(Current.Formula);
				Buffer.Write(")");
			}
		}

		private void WriteAttribute(string name, string value,bool writename =true)
		{
			if (!string.IsNullOrWhiteSpace(value)) {
				var val = value.Trim();
				if (writename) {
					Buffer.Write(name);
					Buffer.Write("=");
				}
				if (val.All(_ => char.IsLetterOrDigit(_) || '-' == _ || '_' == _ || '.' == _)) {
					Buffer.Write(val);
					Buffer.Write(" ");
				}
				else if (val.Contains('\r') || val.Contains('\n')) {
					Buffer.Write("\"\"\"");
					Buffer.Write(val.Trim());
					Buffer.Write("\"\"\" ");
				}
				else {
					Buffer.Write("'");
					Buffer.Write(val.Replace("'", "\'").Trim());
					Buffer.Write("' ");
				}
			}
		}
		
	}
}