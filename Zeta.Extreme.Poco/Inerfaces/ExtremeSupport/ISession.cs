using System;
using System.Threading.Tasks;

namespace Comdiv.Zeta.Model.ExtremeSupport {
	/// <summary>
	/// �����������, Zeta.Extrem cec���, ������� ���������
	/// </summary>
	public interface ISession {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="timeout"> </param>
		/// <returns></returns>
		QueryResult Get(string key,int timeout =-1);

		/// <summary>
		/// 	���������� ����������� ������� � ������
		/// </summary>
		/// <param name="query"> �������� ������ </param>
		/// <param name="uid"> ��������� ���� ������� ��������� ��� ��� ����������� ���������������� ��������� �������� </param>
		/// <returns> ������ �� ������ ����������� � ������ </returns>
		/// <remarks>
		/// 	��� ����������� �������, �� �������� �������������� ����������� � �������� �� ������,
		/// 	������������ ������ �������� ������
		/// </remarks>
		/// <exception cref="NotImplementedException"></exception>
		IQuery Register(IQuery query, string uid = null);

		/// <summary>
		/// 	����������� ����������� ������� � ������
		/// </summary>
		/// <param name="query"> �������� ������ </param>
		/// <param name="uid"> ��������� ���� ������� ��������� ��� ��� ����������� ���������������� ��������� �������� </param>
		/// <returns> ������, �� ����������� ������� ������������ ������ �� ������ ����������� � ������ </returns>
		/// <remarks>
		/// 	��� ����������� �������, �� �������� �������������� ����������� � �������� �� ������,
		/// 	������������ ������ �������� ������
		/// </remarks>
		Task<IQuery> RegisterAsync(IQuery query, string uid = null);

		/// <summary>
		/// 	��������� ������������� � ������ �������� � ������
		/// </summary>
		/// <param name="timeout"> </param>
		void Execute(int timeout = -1);


		/// <summary>
		/// 	��������� ��� ��������� ������
		/// </summary>
		IMetaCache MetaCache { get;  }
	}
}