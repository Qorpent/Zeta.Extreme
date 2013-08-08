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
		/// Буффер, накапливающий скрипт
		/// </summary>
		protected StringWriter Buffer;
		/// <summary>
		/// Текущий уровень развертки
		/// </summary>
		protected int Level;
		/// <summary>
		/// Исходный корень для экспорта
		/// </summary>
		protected IZetaRow Root;
		/// <summary>
		/// Режим отрисовки в рутовом режиме (без родителя)
		/// </summary>
		protected bool Rootmode;
		/// <summary>
		/// Текущая строка на отрисовку
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
		/// Выполняет экспорт дерева в строку
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
		/// Фильтр при выгоне
		/// </summary>
		protected ExportTreeFilter Filter { get; set; }

		/// <summary>
		/// Исходная строка импорта
		/// </summary>
		protected IZetaRow SourceRow { get; set; }

		/// <summary>
		/// Метод записи шапки скрипта экспорта
		/// </summary>
		protected void WriteHeader()
		{
			Buffer.WriteLine(GetSingleCommentStart() + "----------------------------------------------------------------------");
			Buffer.WriteLine(GetSingleCommentStart() + "		ФАЙЛ ДЕРЕВА ZETA");
			Buffer.WriteLine(GetSingleCommentStart() + "		метод:			{0}",GetType().Name);
			Buffer.WriteLine(GetSingleCommentStart() + "		исх. код.:		{0}", SourceRow.Code);
			Buffer.WriteLine(GetSingleCommentStart() + "		замена кодов:		{0}", Filter.CodeReplacer != null ? (Filter.CodeReplacer.Pattern + "~" + Filter.CodeReplacer.Replacer) : "нет");
			Buffer.WriteLine(GetSingleCommentStart() + "		регекс удаления:	{0}", string.IsNullOrWhiteSpace(Filter.ExcludeRegex) ? "отсутствует" : Filter.ExcludeRegex);
			Buffer.WriteLine(GetSingleCommentStart() + "		удаленные элементы:	{0}", string.IsNullOrWhiteSpace(Filter.ExcludeTotalString) ? "отсутствует" : Filter.ExcludeTotalString);
			Buffer.WriteLine(GetSingleCommentStart() + "		режим расш.-перв.:	{0}", Filter.ConvertExtToPrimary ? "да" : "нет");
			Buffer.WriteLine(GetSingleCommentStart() + "		режим рута :		{0}", Rootmode ? "да" : "нет");
			Buffer.WriteLine(GetSingleCommentStart() + "		сброс индекса :		{0}", Filter.ResetAutoIndex ? "да" : "нет");
			Buffer.WriteLine(GetSingleCommentStart() + "---------------------------------------------------------------------");
		}
		/// <summary>
		/// Начало скрипта (подготовительные операции)
		/// </summary>
		protected virtual void WriteStartScript() { }
		/// <summary>
		/// Завершение скрипта (очистка)
		/// </summary>
		protected virtual void WriteEndScript() { }
		/// <summary>
		/// Запись завершающего комментария
		/// </summary>
		protected virtual void WriteFooter() { }
		/// <summary>
		/// Начальная фаза записи строки
		/// </summary>
		protected virtual void WritePreRow() { }
		/// <summary>
		/// Запись тела строки
		/// </summary>
		protected virtual void WriteRowBody() { }
		/// <summary>
		/// Запись окончания строки
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
		/// Возвращает префик комментария
		/// </summary>
		/// <returns></returns>
		protected string GetSingleCommentStart() {
			return "#";
		}
	}
}