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
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override object MainProcess() {
			return FormDependencyHelper.GetDependencyDot(MetaCache.Default.Get<IZetaRow>(root));
		}
	}
}