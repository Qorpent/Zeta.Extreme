using System;
using Qorpent;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaStorage;

namespace Zeta.Extreme.FrontEnd.Actions.Info {
	/// <summary>
	/// Действие экспорта дерева форм в виде BXL (по умолчанию HQL-совместимый скрипт)
	/// </summary>
	[Action("zefs.exporttree",Arm="dev",Help="Формирует переносимый HQL-скрипт дерева формы",Role="DEVELOPER")]
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
			Name = "codereplace", 
			ValidatePattern = @"^[^~]+~[^~]+$",
			Default = "",
			Help = "Позволяет указать пару регекс ~ замена для автоматической коррекции кодов"
		)] 
		private string _codereplace = "";

		[Bind(
			Name = "rootmode",
			Default = false,
			Help = "Режим экспорта в рут-режиме без указания родителя корневой папки"
		)] 
		private bool _rootmode = false;


		[Bind(
			Name = "exclude",
			Default = "",
			Help = "Полный регекс исключения строки (и ее поддерева, применямый к тегам и меткам)"
		)] 
		private string _excluderegex="";



		/// <summary>
		/// Выполняет экспорт дерева из стандартного MetaCache в виде скрипта
		/// </summary>
		/// <returns></returns>
		protected override object MainProcess() {
			var root = MetaCache.Default.Get<IZetaRow>(_root);
			if (null == root) throw new Exception("Нет корневой строки");
			var exportroot = PerformFilter(root);
			var rowexporter = TreeExporter.Create(_format);
			var result = rowexporter.ProcessExport(exportroot,_rootmode);
			return result;
		}

		private IZetaRow PerformFilter(IZetaRow root) {
			var exportroot = (IZetaRow) root.GetCopyOfHierarchy();
			var filter = new ExportTreeFilter();
			filter.ExcludeRegex = _excluderegex;
			if (!string.IsNullOrWhiteSpace(_codereplace)) {
				var pattern = _codereplace.Split('~')[0];
				var replace = _codereplace.Split('~')[1];
				filter.CodeReplacer = new ReplaceDescriptor {Pattern = pattern, Replacer = replace};
			}
			exportroot = filter.Execute(exportroot);
			return exportroot;
		}
	}
}