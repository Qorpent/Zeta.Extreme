using System.Linq;
using System.Security;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.FrontEnd.Helpers;

namespace Zeta.Extreme.FrontEnd.Actions.States {
	/// <summary>
	/// 	���������� ������ �����
	///</summary>
	[Action("zefs.getcurratorlockstate")]
	public class GetCurratorLockStateAction : FormSessionActionBase
	{
		/// <summary>
		/// 
		/// </summary>
		[Bind] public int ObjId { get; set; }
		/// <summary>
		/// ������������� ��������� ������ � �����������
		/// </summary>
		protected override void Authorize()
		{
			base.Authorize();
			var accessibleObjects = new AccessibleObjectsHelper().GetAccessibleObjects().objs;
			if (!accessibleObjects.Any(_ => _.id == ObjId)) {
				throw new SecurityException("not allowed object for currator lock checking");
			}
		}
		
		/// <summary>
		/// 	���������� ������ ����� �� ����������
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess()
		{

				return MySession.GetSimpleLockState(ObjId);

		}
	}
}