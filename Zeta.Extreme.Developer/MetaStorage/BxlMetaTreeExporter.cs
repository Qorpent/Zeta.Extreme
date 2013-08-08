using System.Linq;

namespace Zeta.Extreme.Developer.MetaStorage {
	/// <summary>
	/// ��������� ������ � ��������� ������������ 'HQL'
	/// </summary>
	public class BxlMetaTreeExporter : TreeExporter
	{
		/// <summary>
		/// ��������� ���� ������ ������
		/// </summary>
		protected override void WritePreRow()
		{
			for (var i = 0; i <= Level; i++)
			{
				Buffer.Write("\t");
			}
		
			Buffer.Write(Current.Code);
			Buffer.Write(" ");
			WriteAttribute("outercode", Current.OuterCode, false);
			if (Current.RefTo == null || Current.Name == Current.RefTo.Name) {
				Buffer.Write(" '");
				Buffer.Write(Current.Name);
				Buffer.Write("' ");	
			}
		}

		/// <summary>
		/// ������ ������� (���������������� ��������)
		/// </summary>
		protected override void WriteStartScript() {
			Buffer.WriteLine("zetatree");
		}

		/// <summary>
		/// ������ ��������� ������
		/// </summary>
		protected override void WritePostRow()
		{
			Buffer.WriteLine();
		}

		/// <summary>
		/// ������ ���� ������
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