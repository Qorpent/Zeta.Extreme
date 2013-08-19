using System.Linq;
using System.Text.RegularExpressions;
using Qorpent;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.MetaStorage {
	/// <summary>
	/// Настройки фильтрации дерева
	/// </summary>
	public class ExportTreeFilter {
		/// <summary>
		/// Позволяет настроить исключения
		///  </summary>
		/// <param name="excludestring"></param>
		public void ParseExcludes(string excludestring) {
			var allexcludes = excludestring.SmartSplit(splitters:new[]{',',' '});
			ExcludeTotalString = excludestring;
			ExcludeGroups = allexcludes.Where(_ => _.StartsWith("grp:")).Select(_ => _.Substring(4)).ToArray();
			ExcludeMarks = allexcludes.Where(_ => _.StartsWith("mark:")).Select(_ => _.Substring(5)).ToArray();
			ExcludeTags = allexcludes.Where(_ => _.StartsWith("tag:")).Select(_ => _.Substring(4)).ToArray();
		}
		/// <summary>
		/// Полная запись об удаляемых элементах
		/// </summary>
		public string ExcludeTotalString { get; set; }

		/// <summary>
		/// Регулярное выражение исключения узла
		/// </summary>
		public string ExcludeRegex { get; set; }

		/// <summary>
		/// Список тегов на удаление - чистит теги
		/// </summary>
		public string[] ExcludeTags { get; set; }
		/// <summary>
		/// Список групп на удаление - чистит теги
		/// </summary>
		public string[] ExcludeGroups { get; set; }
		/// <summary>
		/// Список меток на удаление - чистит теги
		/// </summary>
		public string[] ExcludeMarks { get; set; }
		/// <summary>
		/// Замена кода по регексу
		/// </summary>
		public ReplaceDescriptor CodeReplacer { get; set; }

		/// <summary>
		/// Преобразовать расширяемые разделы в первичные строки
		/// </summary>
		public bool ConvertExtToPrimary { get; set; }

		/// <summary>
		/// Позволяет удалить явные отметки об индексе и использовать обновленный скорректированный обработчик outercode
		/// </summary>
		public bool ResetAutoIndex { get; set; }

		/// <summary>
		/// Выполняет фильтрацию дерева
		/// </summary>
		/// <param name="root"></param>
		/// <returns></returns>
		public IZetaRow Execute(IZetaRow root) {
			var copy =(Row) root.GetCopyOfHierarchy();
			copy.LocalProperties["source"] = root;
			copy.LocalProperties["filter"] = this;
			PerformExclude(copy);
			ReplaceCode(copy);
			PerformConvertExtToPrimary(copy);
			RemoveIndex(copy);
			RemoveTags(copy);
			RemoveGroups(copy);
			RemoveMarks(copy);
			copy.ResetAllChildren(); 
			return copy;
		}

		private void RemoveMarks(Row copy) {

			if (ExcludeMarks.ToBool()) {
				var all = new[] {copy}.Union(copy.AllChildren).Where(_=>!string.IsNullOrWhiteSpace(_.MarkCache)).ToArray();
				foreach (var r in all) {
					foreach (var m in ExcludeMarks) {
#pragma warning disable 612,618
						r.MarkCache = SlashListHelper.RemoveMark(r.MarkCache, m);
#pragma warning restore 612,618
					}
				}
				
			}

		}

		private void RemoveGroups(Row copy) {
			if (ExcludeGroups.ToBool())
			{
				var all = new[] { copy }.Union(copy.AllChildren).Where(_ => !string.IsNullOrWhiteSpace(_.GroupCache)).ToArray();
				foreach (var r in all)
				{
					foreach (var g in ExcludeGroups)
					{
#pragma warning disable 612,618
						r.GroupCache = SlashListHelper.RemoveMark(r.GroupCache, g);
#pragma warning restore 612,618
					}
				}
			}
		}

		private void RemoveTags(Row copy) {
			if (ExcludeTags.ToBool())
			{
				var all = new[] { copy }.Union(copy.AllChildren).Where(_ => !string.IsNullOrWhiteSpace(_.Tag)).ToArray();
				foreach (var r in all)
				{
					foreach (var t in ExcludeTags)
					{
#pragma warning disable 612,618
						r.Tag = TagHelper.RemoveTag(r.Tag, t);
#pragma warning restore 612,618
					}
				}
			}
		}

		private void RemoveIndex(IZetaRow copy) {
			if (ResetAutoIndex) {
				copy.Index = 0;
				foreach (var c in copy.AllChildren) {
					c.Index = 0;
				}
			}
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