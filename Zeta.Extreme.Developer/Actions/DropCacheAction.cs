using Qorpent.Events;
using Qorpent.IoC;
using Qorpent.Mvc;
using Zeta.Extreme.Developer.Analyzers;
using Zeta.Extreme.Model.MetaCaches;

namespace Zeta.Extreme.Developer.Actions {
	/// <summary>
	/// Сбрасывает кэши индексаторов кода
	/// </summary>
	[Action("zdev.dropcache",Role="DEVELOPER",Arm="dev")]
	public class DropCacheAction : ActionBase {
		/// <summary>
		/// Индекс кода, в котором сбрасывается кэш
		/// </summary>
		[Inject]
		public ICodeIndex CodeIndex { get; set; }
		/// <summary>
		/// Сбрасывает Reset именно для Index
		/// </summary>
		/// <returns></returns>
		protected override object MainProcess() {
			Periods.Reload();
			RowCache.start();
			ColumnCache.Start();
			ObjCache.Start();
			return Application.Events.Call<ResetEventResult>(new ResetEventData(new[] {"zdev.cache"}), User);
			
		}
	}
}