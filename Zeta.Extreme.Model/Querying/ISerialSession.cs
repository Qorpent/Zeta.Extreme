#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Model/ISerialSession.cs
#endregion
using System.Threading.Tasks;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// 	ќписывает самый простой, синхронизированный вариант доступа к сессии
	/// 	все запросы должны четко следовать один за другим
	/// </summary>
	public interface ISerialSession {
		/// <summary>
		/// 	√арантирует синхронный, последовательный доступ к сессии, вычисл€ет значение
		/// </summary>
		/// <param name="query"> </param>
		/// <param name="timeout"> </param>
		/// <returns> </returns>
		QueryResult Eval(IQuery query, int timeout = -1);

		/// <summary>
		/// 	√арантирует синхронный, последовательный доступ к сессии, вычисл€ет значение
		/// </summary>
		/// <param name="query"> </param>
		/// <returns> </returns>
		Task<QueryResult> EvalAsync(IQuery query);

		/// <summary>
		/// 	¬озвращает ссылку на реальную сессию
		/// </summary>
		/// <returns> </returns>
		ISession GetUnderlinedSession();
	}
}