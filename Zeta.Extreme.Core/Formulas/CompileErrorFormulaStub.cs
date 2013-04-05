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
// PROJECT ORIGIN: Zeta.Extreme.Core/CompileErrorFormulaStub.cs
#endregion

using System;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Специальная заглушка для возврата псевдозначения
	/// </summary>
	public class CompileErrorFormulaStub : FormulaBase {
		private Exception error;

		/// <summary>
		/// 	Устанавливает контекст использования формулы
		/// </summary>
		/// <param name="request"> </param>
		public override void SetContext(FormulaRequest request) {
			base.SetContext(request);
			this.error = request.ErrorInCompilation;
		}

		/// <summary>
		/// 	Метод для перекрытия при формировании динамической формулы
		/// </summary>
		/// <returns> </returns>
		protected override QueryResult InternalEval() {
			return new QueryResult{IsComplete = false, Error = new QueryException(Query,error)};
		}
	}
}