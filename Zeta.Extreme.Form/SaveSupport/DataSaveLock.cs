#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : DataSaveLock.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Configuration;
using System.Threading;
using Comdiv.Extensions;

namespace Zeta.Extreme.Form.SaveSupport {
	/// <summary>
	/// 	Устаревший механизм паралельной блокировки сохранения
	/// </summary>
	public class DataSaveLock : IDisposable {
		private static readonly object sync = new object();


		private static readonly Mutex mutex;

		static DataSaveLock() {
			var mutexname = Guid.NewGuid().ToString();
			if (ConfigurationManager.AppSettings["savelock"].hasContent()) {
				mutexname = ConfigurationManager.AppSettings["savelock"];
			}
			mutex = new Mutex(false, mutexname);
		}

		private DataSaveLock() {}

		/// <summary>
		/// 	Выполняет определяемые приложением задачи, связанные с высвобождением или сбросом неуправляемых ресурсов.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose() {
			mutex.ReleaseMutex();
		}

		/// <summary>
		/// 	Получить в пользование объект блокировки
		/// </summary>
		/// <returns> </returns>
		public static DataSaveLock Get() {
			return Get(-1);
		}

		/// <summary>
		/// 	Получить блокировку с задержкой
		/// </summary>
		/// <param name="timeout"> </param>
		/// <returns> </returns>
		/// <exception cref="TimeoutException"></exception>
		public static DataSaveLock Get(int timeout) {
			lock (sync) {
				var result = mutex.WaitOne(timeout, false);
				if (!result) {
					throw new TimeoutException("cannot get data save lock");
				}
				return new DataSaveLock();
			}
		}
	}
}