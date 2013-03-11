using Qorpent.Applications;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// ������� ��� �������� ������ � ��������
	/// </summary>
	public static class ExtremeFactory {
		private static IExtremeFactory _factory;
		private static readonly object Sync =new object();

		/// <summary>
		/// ������� ������
		/// </summary>
		/// <returns></returns>
		public static ISession CreateSession(SessionSetupInfo info = null) {
			return RealFactory.CreateSession(info);
		}

		/// <summary>
		/// ������� ��������� ������
		/// </summary>
		/// <returns></returns>
		public static IRowHandler CreateRowHandler()
		{
			return RealFactory.CreateRowHandler();
		}
		/// <summary>
		/// ������ � ��������� ������
		/// </summary>
		/// <returns></returns>
		public static IFormulaStorage GetFormulaStorage() {
			return RealFactory.GetFormulaStorage();
		}

		/// <summary>
		/// ������� ��������� �������
		/// </summary>
		/// <returns></returns>
		public static IColumnHandler CreateColumnHandler()
		{
			return RealFactory.CreateColumnHandler();
		}

		/// <summary>
		/// ������� ��������� �������
		/// </summary>
		/// <returns></returns>
		public static IObjHandler CreateObjHandler()
		{
			return RealFactory.CreateObjHandler();
		}


		/// <summary>
		/// ������� ��������� �������
		/// </summary>
		/// <returns></returns>
		public static ITimeHandler CreateTimeHandler()
		{
			return RealFactory.CreateTimeHandler();
		}



		/// <summary>
		/// ������� ������
		/// </summary>
		/// <returns></returns>
		public static IQuery CreateQuery(QuerySetupInfo info=null) {
			return RealFactory.CreateQuery(info);
		}
		/// <summary>
		/// �������� ������� �������
		/// </summary>
		public static IExtremeFactory RealFactory {
			get {
				if(null!=_factory) return _factory;
				lock (Sync) {
					_factory = Application.Current.Container.Get<IExtremeFactory>() ?? new StubExtremeFactory();
					return _factory;
				}
			}set {
				lock (Sync) {
					_factory = value;
				}
			}
		}
	}
}