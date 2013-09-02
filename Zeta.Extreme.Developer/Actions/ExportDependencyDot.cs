using System.Linq;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Developer.MetaStorage.Tree;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.Actions {
	/// <summary>
	/// Ёкспорт сфорировать файл периодов
	/// </summary>
	[Action("zdev.exportdependencydot", Arm = "dev", Help = "—формировать DOT файл зависимости формы", Role = "DEVELOPER")]
	public class ExportDependencyDot : ActionBase {
		[Bind] private string root { get; set; }
		[Bind] private bool listonly { get; set; }
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override object MainProcess() {
			if (listonly) {
				var graph= FormDependencyHelper.GetDependencyGraph(MetaCache.Default.Get<IZetaRow>(root));
				return graph.Nodes.ToArray();
			}
			else {
				return FormDependencyHelper.GetDependencyDot(MetaCache.Default.Get<IZetaRow>(root));
			}
		}
	}
}