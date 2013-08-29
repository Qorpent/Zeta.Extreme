using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Developer.MetaStorage.Tree;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.Actions {
	/// <summary>
	/// Ёкспорт сфорировать файл периодов
	/// </summary>
	[Action("zdev.exportformuladependencydot", Arm = "dev", Help = "—формировать DOT файл зависимости формулы", Role = "DEVELOPER")]
	public class ExportFormulaDependencyDot : ActionBase
	{
		[Bind]
		private string root { get; set; }
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override object MainProcess()
		{
			return FormDependencyHelper.GetFormulaDependencyDot(MetaCache.Default.Get<IZetaRow>(root));
		}
	}
}