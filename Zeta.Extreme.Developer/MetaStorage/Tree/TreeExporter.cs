using System;
using System.IO;
using System.Linq;
using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.MetaStorage.Tree {
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
		/// Текущая строка на отрисовку
		/// </summary>
		protected IZetaRow Current;
		/// <summary>
		/// Текущие опции
		/// </summary>
		protected TreeExporterOptions Options; 

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
			if (format == ExportTreeFormat.BSharp)
			{
				return new BSharpTreeExporter();
			}
			throw new Exception("format not supported for now " + format);
		}


		/// <summary>
		/// Выполняет экспорт дерева в строку
		/// </summary>
		/// <param name="root"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public string ProcessExport(IZetaRow root, TreeExporterOptions options = null) {
			lock (this) {
				
				Level = 0;
				Buffer = new StringWriter();
				Root = root;
				Options = options ?? new TreeExporterOptions();
				ValidateOptions();
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
		/// Метод проверяет валидность опций генератора
		/// </summary>
		protected virtual void ValidateOptions() {
			
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
			Buffer.WriteLine(GetSingleCommentStart() + "		без родителя :		{0}", Options.DetachRoot ? "да" : "нет");
			Buffer.WriteLine(GetSingleCommentStart() + "		режим кодировки :	{0}", Options.CodeMode);
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
			CurrentCode = GetCurrentCode();
			WritePreRow();
			WriteRowBody();
			WritePostRow();
		}
		/// <summary>
		/// Вычисленный скорректированный код текущего узла
		/// </summary>
		protected string CurrentCode;
		/// <summary>
		/// Метод подготовки кода текущего узла
		/// </summary>
		protected virtual string GetCurrentCode() {
			switch (Options.CodeMode) {
				case TreeExporterCodeMode.Full:
					return Current.Code;
				case TreeExporterCodeMode.NoCode:
					return "";
				case TreeExporterCodeMode.ParentPrefix:
					if (null == Current.Parent) return Current.Code;
					if (Root == Current) return Current.Code;
					if (!Current.Code.StartsWith(Current.Parent.Code)) return Current.Code;
					return Current.Code.Substring(Current.Parent.Code.Length);
				case TreeExporterCodeMode.RootPrefix:
					if (null == Current.Parent) return Current.Code;
					if (Root == Current) return Current.Code;
					if (!Current.Code.StartsWith(Root.Code)) return Current.Code;
					return Current.Code.Substring(Root.Code.Length);
				default:
					throw new Exception("unknown coding method " + Options.CodeMode);
			}
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