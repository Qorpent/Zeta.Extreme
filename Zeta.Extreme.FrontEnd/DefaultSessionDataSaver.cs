using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
using Zeta.Extreme.Form;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// ������ ���������� ������ ����� �� ���������
	/// </summary>
	public class DefaultSessionDataSaver : IFormSessionDataSaver {
		/// <summary>
		/// ����� 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="savedata"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public Task<SaveResult> BeginSave(IFormSession session, XElement savedata) {
			if(null!=Current && !Current.IsCompleted) {
				Current.Wait();
			}
			Current = null;
			Stage = SaveStage.Load;
			Error = null;
			return (Current = Task.Run(() => InternalSave(session,savedata)));
		}
		/// <summary>
		/// ���������� ��������� ����������, ���������� ������ ����������
		/// </summary>
		/// <param name="session"></param>
		/// <param name="savedata"></param>
		/// <returns></returns>
		protected virtual SaveResult InternalSave(IFormSession session, XElement savedata) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// ������� ������ �������� ����������
		/// </summary>
		public SaveStage Stage { get; set; }
		/// <summary>
		/// ��������� ��������� ������
		/// </summary>
		public Exception Error { get; set; }
		/// <summary>
		/// ������� ������
		/// </summary>
		public Task<SaveResult> Current { get; set; }
	}
}