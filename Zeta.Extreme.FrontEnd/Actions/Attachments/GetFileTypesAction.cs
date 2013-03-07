using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.Attachments {
	///<summary>
	///	Вызывает сохранение данных
	///</summary>
	[Action("zefs.getfiletypes")]
	public class GetFileTypesAction : FormSessionActionBase
	{

		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess()
		{
			return MySession.GetAllowedFileTypes();
		}

	}
}