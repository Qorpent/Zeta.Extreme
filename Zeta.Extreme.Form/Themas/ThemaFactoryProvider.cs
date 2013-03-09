#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ThemaFactoryProvider.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Applications;
using Zeta.Extreme.BizProcess.Reports;
using Zeta.Extreme.BizProcess.Themas;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	����������� ������� ���
	/// </summary>
	public class ThemaFactoryProvider : IThemaFactoryProvider {
		/// <summary>
		/// 	������������ �������
		/// </summary>
		protected internal IThemaFactory factory;


		/// <summary>
		/// 	����������� �������
		/// </summary>
		public void Reload() {
			factory = null;
		}

		/// <summary>
		/// 	������ �� ������������ ���
		/// </summary>
		public IThemaConfigurationProvider ConfigurationProvider { get; set; }


		/// <summary>
		/// 	�������� �������
		/// </summary>
		/// <returns> </returns>
		public IThemaFactory Get() {
			lock (this) {
				if (null == factory) {
					var cfg = ConfigurationProvider.Get();
					factory = cfg.Configure();
				}
				return factory;
			}
		}


		/// <summary>
		/// 	���������� ����������� �������� ������� �������
		/// </summary>
		/// <param name="code"> </param>
		/// <typeparam name="T"> </typeparam>
		/// <returns> </returns>
		public static T GetDefault<T>(string code) where T : class {
			var factory = Application.Current.Container.Get<IThemaFactoryProvider>().Get();
			if (typeof (IThema).IsAssignableFrom(typeof (T))) {
				return factory.Get(code) as T;
			}
			if (typeof (IReportDefinition).IsAssignableFrom(typeof (T))) {
				return factory.GetReport(code) as T;
			}
			if (typeof (IInputTemplate).IsAssignableFrom(typeof (T))) {
				return factory.GetForm(code) as T;
			}
			return null;
		}
	}
}