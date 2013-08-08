using System;
using System.IO;
using System.Linq;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.MetaStorage {
	/// <summary>
	/// 
	/// </summary>
	public abstract class TreeExporter:ITreeExporter {
		/// <summary>
		/// ������, ������������� ������
		/// </summary>
		protected StringWriter Buffer;
		/// <summary>
		/// ������� ������� ���������
		/// </summary>
		protected int Level;
		/// <summary>
		/// �������� ������ ��� ��������
		/// </summary>
		protected IZetaRow Root;
		/// <summary>
		/// ����� ��������� � ������� ������ (��� ��������)
		/// </summary>
		protected bool Rootmode;
		/// <summary>
		/// ������� ������ �� ���������
		/// </summary>
		protected IZetaRow Current;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="format"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public static ITreeExporter Create(ExportTreeFormat format) {
			if (format == ExportTreeFormat.Hql) {
				return new HqlTreeExporter();
			}
			if (format == ExportTreeFormat.BxlMeta)
			{
				return new BxlMetaTreeExporter();
			}
			throw new Exception("format not supported for now " + format);
		}


		/// <summary>
		/// ��������� ������� ������ � ������
		/// </summary>
		/// <param name="root"></param>
		/// <param name="rootmode"></param>
		/// <returns></returns>
		public string ProcessExport(IZetaRow root, bool rootmode) {
			lock (this) {
				Level = 0;
				Buffer = new StringWriter();
				Root = root;
				Rootmode = rootmode;
				if (root.LocalProperties.ContainsKey("source")) {
					SourceRow = root.LocalProperties["source"] as IZetaRow;
				}
				else {
					SourceRow = root;
				}
				
				if (root.LocalProperties.ContainsKey("filter")) {
					Filter = root.LocalProperties["filter"] as ExportTreeFilter;
				}
				else {
					Filter = new ExportTreeFilter();
				}
				WriteHeader();
				WriteStartScript();
				WriteRow(root);
				WriteEndScript();
				WriteFooter();
				Buffer.Flush();
				return Buffer.ToString();
			}
		}
		/// <summary>
		/// ������ ��� ������
		/// </summary>
		protected ExportTreeFilter Filter { get; set; }

		/// <summary>
		/// �������� ������ �������
		/// </summary>
		protected IZetaRow SourceRow { get; set; }

		/// <summary>
		/// ����� ������ ����� ������� ��������
		/// </summary>
		protected void WriteHeader()
		{
			Buffer.WriteLine(GetSingleCommentStart() + "----------------------------------------------------------------------");
			Buffer.WriteLine(GetSingleCommentStart() + "		���� ������ ZETA");
			Buffer.WriteLine(GetSingleCommentStart() + "		�����:			{0}",GetType().Name);
			Buffer.WriteLine(GetSingleCommentStart() + "		���. ���.:		{0}", SourceRow.Code);
			Buffer.WriteLine(GetSingleCommentStart() + "		������ �����:		{0}", Filter.CodeReplacer != null ? (Filter.CodeReplacer.Pattern + "~" + Filter.CodeReplacer.Replacer) : "���");
			Buffer.WriteLine(GetSingleCommentStart() + "		������ ��������:	{0}", string.IsNullOrWhiteSpace(Filter.ExcludeRegex) ? "�����������" : Filter.ExcludeRegex);
			Buffer.WriteLine(GetSingleCommentStart() + "		��������� ��������:	{0}", string.IsNullOrWhiteSpace(Filter.ExcludeTotalString) ? "�����������" : Filter.ExcludeTotalString);
			Buffer.WriteLine(GetSingleCommentStart() + "		����� ����.-����.:	{0}", Filter.ConvertExtToPrimary ? "��" : "���");
			Buffer.WriteLine(GetSingleCommentStart() + "		����� ���� :		{0}", Rootmode ? "��" : "���");
			Buffer.WriteLine(GetSingleCommentStart() + "		����� ������� :		{0}", Filter.ResetAutoIndex ? "��" : "���");
			Buffer.WriteLine(GetSingleCommentStart() + "---------------------------------------------------------------------");
		}
		/// <summary>
		/// ������ ������� (���������������� ��������)
		/// </summary>
		protected virtual void WriteStartScript() { }
		/// <summary>
		/// ���������� ������� (�������)
		/// </summary>
		protected virtual void WriteEndScript() { }
		/// <summary>
		/// ������ ������������ �����������
		/// </summary>
		protected virtual void WriteFooter() { }
		/// <summary>
		/// ��������� ���� ������ ������
		/// </summary>
		protected virtual void WritePreRow() { }
		/// <summary>
		/// ������ ���� ������
		/// </summary>
		protected virtual void WriteRowBody() { }
		/// <summary>
		/// ������ ��������� ������
		/// </summary>
		protected virtual void WritePostRow() { }
		private void WriteCurrentRow() {
			WritePreRow();
			WriteRowBody();
			WritePostRow();
		}

		private void WriteRow(IZetaRow row) {
			Current = row;
			WriteCurrentRow();
			if (Current.HasChildren()) {
				Level++;
				var children = Current.Children.ToArray();
				foreach (var c in children.OrderBy(_=>_.GetSortKey())) {
					WriteRow(c);
				}
				Level--;
			}
		}

		/// <summary>
		/// ���������� ������ �����������
		/// </summary>
		/// <returns></returns>
		protected string GetSingleCommentStart() {
			return "#";
		}
	}
}