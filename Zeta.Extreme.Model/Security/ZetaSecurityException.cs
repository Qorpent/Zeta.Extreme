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
// Original file : ZetaSecurityException.cs
// Project: Zeta.Data.Repository
// 
// ALL MODIFICATIONS MADE TO FILE MUST BE DOCUMENTED IN SVN

#endregion

using System;
using System.Collections.Generic;
using Qorpent.Security;

namespace Zeta.Extreme.Model.Security {
	/// <summary>
	/// 	Ошибка безопасности Zeta
	/// </summary>
	[Serializable]
	public class ZetaSecurityException : QorpentSecurityException {
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		/// <summary>
		/// </summary>
		public ZetaSecurityException() {}

		/// <summary>
		/// </summary>
		/// <param name="message"> </param>
		public ZetaSecurityException(string message) : base(message) {}

		/// <summary>
		/// </summary>
		/// <param name="message"> </param>
		/// <param name="inner"> </param>
		public ZetaSecurityException(string message, Exception inner) : base(message, inner) {}

		/// <summary>
		/// 	Запрос политики
		/// </summary>
		public PolicyRequest PolicyRequest { get; set; }

		/// <summary>
		/// 	Результат политики
		/// </summary>
		public PolicyResult PolicyResult { get; set; }

		/// <summary>
		/// </summary>
		public override IDictionary<string, string> ExceptionRegisryData {
			get {
				var result = new Dictionary<string, string>();
				if (null != PolicyRequest) {
					result["Request_Login"] = PolicyRequest.Login;
					result["Request_Operation"] = PolicyRequest.Operation;
					result["Request_ContextObject"] = PolicyRequest.ContextObject.ToString();
					if (null != PolicyRequest.Parameters) {
						result["Request_Parameters"] = PolicyRequest.Parameters.ToString();
					}
				}
				if (null == PolicyResult) {
					result["Policy_ErrorCode"] = PolicyResult.ErrorCode;
					result["Policy_ErrorMessage"] = PolicyResult.Message;
				}
				return result;
			}
			set { throw new NotSupportedException(); }
		}
	}
}