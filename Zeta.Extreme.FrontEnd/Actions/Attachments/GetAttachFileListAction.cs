using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd {
	///<summary>
	///	Вызывает сохранение данных
	///</summary>
	[Action("zefs.attachlist")]
	public class GetAttachFileListAction : FormSessionActionBase
	{
		
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return MySession.GetAttachedFiles();
		}
		
	}
}