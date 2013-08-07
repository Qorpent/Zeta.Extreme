using System;
using System.IO;
using System.Linq;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.MetaStorage {
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
			throw new Exception("format not supported for now " + format);
		}



		public string ProcessExport(IZetaRow root, bool rootmode) {
			lock (this) {
				Level = 0;
				Buffer = new StringWriter();
				Root = root;
				Rootmode = rootmode;
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
		/// Метод записи шапки скрипта экспорта
		/// </summary>
		protected virtual void WriteHeader() {}
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
				foreach (var c in children) {
					WriteRow(c);
				}
				Level--;
			}
		}
	}
}