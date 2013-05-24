#region LICENSE

// Copyright 2007-2012 Comdiv (F. Sadykov) - http://code.google.com/u/fagim.sadykov/
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
// http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// Solution: Qorpent
// Original file : ISecurityPolicyChecker.cs
// Project: Zeta.Data.Repository
// 
// ALL MODIFICATIONS MADE TO FILE MUST BE DOCUMENTED IN SVN

#endregion

using System;

namespace Zeta.Extreme.Model.Security {
	/// <summary>
	/// 	»нтерфейс службы проверки полномочий операций безопасности
	/// </summary>
	public interface ISecurityPolicyChecker {
		/// <summary>
		/// 	ѕроверка политики операции
		/// </summary>
		/// <param name="request"> </param>
		/// <returns> </returns>
		/// <exception cref="NotFiniteNumberException"></exception>
		PolicyResult CheckPolicy(PolicyRequest request);

		/// <summary>
		/// 	ѕровер€ет досутпность назначени€ (активации) указанной роли дл€ пользовател€
		/// </summary>
		/// <param name="role"> </param>
		/// <param name="zone"> </param>
		/// <param name="objectId"> </param>
		/// <param name="checkAssigner"> проверить полномочи€ вызывающего лица (при активации не требуетс€) </param>
		/// <exception cref="ZetaSecurityException"></exception>
		void CheckRolePolicy(string role, SecurityZone zone, int objectId, bool checkAssigner);

		/// <summary>
		/// 	ѕровер€ет возможность активации пользовател€
		/// </summary>
		/// <param name="user"> </param>
		void CheckUserActivationPolicy(User user);

		/// <summary>
		/// 	ѕровер€ет возможность активации карт
		/// </summary>
		/// <param name="card"> </param>
		void CheckCardActivationPolicy(AccountCard card);
	}
}