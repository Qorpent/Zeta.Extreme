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
// PROJECT ORIGIN: Zeta.Extreme.Core/CacheKeyGeneratorBase.cs
#endregion
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme {
	/// <summary>
	/// 	������� ����� ��� �������� � �������, �������� �� ����������� ���-������
	/// </summary>
	public abstract class CacheKeyGeneratorBase : IWithCacheKey {
		/// <summary>
		/// 	���������� ���-������ �������
		/// </summary>
		/// <returns> </returns>
		public string GetCacheKey(bool save = true) {
			if (null != _cacheKey) {
				return _cacheKey;
			}
			if (save) {
				return _cacheKey ?? (_cacheKey = EvalCacheKey());
			}
			return EvalCacheKey();
		}

		/// <summary>
		/// 	���������� ���-������
		/// </summary>
		public virtual void InvalidateCacheKey() {
			_cacheKey = null;
		}

		/// <summary>
		/// 	���������� ������, ������� ������������ ������� ������.
		/// </summary>
		/// <returns> ������, �������������� ������� ������. </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString() {
			//������ ������� ��� ������, �� �� ��������� ��!!!
			return GetCacheKey(false);
		}

		/// <summary>
		/// 	������� ����������������� ���������� ������� ������
		/// </summary>
		/// <returns> </returns>
		protected abstract string EvalCacheKey();

		private string _cacheKey; //cached value of key
	}
}