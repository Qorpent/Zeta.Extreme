using System;
using System.Threading.Tasks;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// ��������� ��� ����������� ���������� ����� ������ � ����������
	/// </summary>
	public interface ISessionWithExtendedServices:ISession {


		/// <summary>
		/// 	���������� ����������� ���������� ������� � ����������
		/// 	���������� �� �� ������, ��� � �����������
		/// </summary>
		/// <param name="query"> </param>
		/// <returns> </returns>
		Task PrepareAsync(IQuery query);

		/// <summary>
		/// 	������� ��������� ���� ��������� ����������� �����������
		/// </summary>
		/// <param name="timeout"> </param>
		void WaitPreparation(int timeout = -1);

		/// <summary>
		/// 	������� ��������� ���� ��������� ����������� �����������
		/// </summary>
		/// <param name="timeout"> </param>
		void WaitEvaluation(int timeout = -1);

		/// <summary>
		/// 	���������� ������ ���������������� ������ �����������
		/// </summary>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		IQueryPreparator GetPreparator();

		/// <summary>
		/// 	���������� ������������ � ���
		/// </summary>
		/// <param name="preparator"> </param>
		void Return(IQueryPreparator preparator);

		/// <summary>
		/// 	���������� ������ ���������������� ������ �����������
		/// </summary>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		IRegistryHelper GetRegistryHelper();

		/// <summary>
		/// 	���������� ������������ � ���
		/// </summary>
		/// <param name="helper"> </param>
		void ReturnRegistryHelper(IRegistryHelper helper);

		/// <summary>
		/// 	���������� ������ �������������
		/// </summary>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		IPreloadProcessor GetPreloadProcessor();

		/// <summary>
		/// 	���������� ������������ � ���
		/// </summary>
		/// <param name="processor"> </param>
		void Return(IPreloadProcessor processor);


		/// <summary>
		/// 	������ �������������� ���������� ����� � �������� �������� ����������
		/// </summary>
		/// <param name="timeout"> </param>
		void SyncPreEval(int timeout);
	}
}