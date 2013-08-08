using System.Linq;
using System.Text.RegularExpressions;
using Qorpent;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.MetaStorage {
	/// <summary>
	/// ��������� ���������� ������
	/// </summary>
	public class ExportTreeFilter {
		/// <summary>
		/// ���������� ��������� ���������� ����
		/// </summary>
		public string ExcludeRegex { get; set; }
		/// <summary>
		/// ������ ���� �� �������
		/// </summary>
		public ReplaceDescriptor CodeReplacer { get; set; }

		/// <summary>
		/// ������������� ����������� ������� � ��������� ������
		/// </summary>
		public bool ConvertExtToPrimary { get; set; }

		/// <summary>
		/// ��������� ���������� ������
		/// </summary>
		/// <param name="root"></param>
		/// <returns></returns>
		public IZetaRow Execute(IZetaRow root) {
			var copy =(Row) root.GetCopyOfHierarchy();
			PerformExclude(copy);
			ReplaceCode(copy);
			PerformConvertExtToPrimary(copy);
			copy.ResetAllChildren();
			return copy;
		}

		private void PerformConvertExtToPrimary(Row copy) {
			if (ConvertExtToPrimary) {
				var exts = copy.AllChildren.Where(_ =>  _.IsMarkSeted("0EXT")).ToArray();
				foreach (var e in exts) {
					e.Children.Clear();
					e.MarkCache = "";
				}
				copy.ResetAllChildren();
			}
		}

		string ExcludeMask(IZetaRow row) {
			return string.Format("code: {0}; marks: {1}; tags: {2}; grp: {3};", row.Code, row.MarkCache, row.Tag, row.GroupCache);
		}

		private void PerformExclude(Row root) {
			if (!string.IsNullOrWhiteSpace(ExcludeRegex)) {
				var excludes = root.AllChildren.Where(_ => Regex.IsMatch(ExcludeMask(_), ExcludeRegex)).ToArray();
				foreach (var e in excludes) {
					e.Parent.Children.Remove(e);
				}
			}
		}

		private void ReplaceCode(IZetaRow root) {
			if (null != CodeReplacer && !string.IsNullOrWhiteSpace(CodeReplacer.Pattern)) {
				var all = new[] {(Row) root}.Union(root.AllChildren);
				foreach (var r in all) {
					r.Code = CodeReplacer.Execute(r.Code);
					if (null != r.Tag) {
						r.Tag = CodeReplacer.Execute(r.Tag);
					}
					if (r.IsFormula) {
						r.Formula = CodeReplacer.Execute(r.Formula);
					}

				}
			}
		}
	}
}