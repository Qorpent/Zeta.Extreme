using System;
using Qorpent;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Developer.MetaStorage;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.Actions {



	/// <summary>
	/// Действие экспорта дерева форм в виде BXL (по умолчанию HQL-совместимый скрипт)
	/// </summary>
	[Action("zdev.exporttree",Arm="dev",Help="Формирует переносимый HQL-скрипт дерева формы",Role="DEVELOPER")]
	public class ExportTreeAction  : ActionBase {
		[Bind(
			Name = "format",
			Constraint = new object[] { ExportTreeFormat.BxlMeta,ExportTreeFormat.Hql,  },
			Help = "Формат представления экспортного дерева",
			Default = ExportTreeFormat.Hql
		)]
		private ExportTreeFormat _format = ExportTreeFormat.Hql;

		[Bind(
			Name = "root",
			Required=true,
			Default= "",
			Help = "Корневая строка для экспорта"
		)] 
		private string _root = "";

		[Bind(
			Name = "exttoprimary",
			Required = false,
			Default = false,
			Help = "Конвертирует расширяемые разделы в первичные"
		)]
		private bool _exttoprimary = false;

		[Bind(
			Name = "codereplace", 
			ValidatePattern = @"^[^~]+~[^~]+$",
			Default = "",
			Help = "Позволяет указать пару регекс ~ замена для автоматической коррекции кодов"
		)] 
		private string _codereplace = "";

		[Bind(
			Name = "detachroot",
			Default = false,
			Help = "Режим экспорта в рут-режиме без указания родителя корневой папки"
		)] 
		private bool _detachroot = false;

		[Bind(
			Name = "codemode",
			Default = TreeExporterCodeMode.Default,
			Help = "Режим генерации с неполным кодом, когда префикс кода удаляется из дочерних"
		)]
		private TreeExporterCodeMode _codemode = TreeExporterCodeMode.Default;


		[Bind(
			Name = "exclude",
			Default = "",
			Help = "Полный регекс исключения строки (и ее поддерева, применямый к тегам и меткам)"
		)] 
		private string _excluderegex="";


		[Bind(
			Name = "resetindex",
			Default = false,
			Help = "Сбрасывает поли индекса (в связи с обновленным алгоритмом расчета порядка строк)"
		)]
		private bool _resetindex = false;

		[Bind(
			Name = "excludetags",
			Default = "",
			ValidatePattern = @"^(((grp)|(tag)|(mark)):[\w\d_\-]+[\s,]*)+$",
			Help = "Позволяет настроить удаление ненужных тегов, групп и прочего"
		)]
		private string _excludetags = "";



		/// <summary>
		/// Выполняет экспорт дерева из стандартного MetaCache в виде скрипта
		/// </summary>
		/// <returns></returns>
		protected override object MainProcess() {
			var root = MetaCache.Default.Get<IZetaRow>(_root);
			if (null == root) throw new Exception("Нет корневой строки");
			var exportroot = PerformFilter(root);
			var rowexporter = TreeExporter.Create(_format);
			var options = new TreeExporterOptions {DetachRoot = _detachroot, CodeMode = _codemode};
			var result = rowexporter.ProcessExport(exportroot,options);
			return result;
		}

		private IZetaRow PerformFilter(IZetaRow root) 
		{
			var filter = new ExportTreeFilter();
			filter.ExcludeRegex = _excluderegex;
			filter.ConvertExtToPrimary = _exttoprimary;
			filter.ResetAutoIndex = _resetindex;
			if (!string.IsNullOrWhiteSpace(_codereplace)) {
				var pattern = _codereplace.Split('~')[0];
				var replace = _codereplace.Split('~')[1];
				filter.CodeReplacer = new ReplaceDescriptor {Pattern = pattern, Replacer = replace};
			}
			if (!string.IsNullOrWhiteSpace(_excludetags)) {
				filter.ParseExcludes(_excludetags);
			}
			var result = filter.Execute(root);
			return result;
		}
	}
}