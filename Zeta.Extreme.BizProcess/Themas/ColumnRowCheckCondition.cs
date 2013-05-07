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
// PROJECT ORIGIN: Zeta.Extreme.BizProcess/ColumnRowCheckCondition.cs
#endregion

using Qorpent.Serialization;

namespace Zeta.Extreme.BizProcess.Themas {
    /// <summary>
    /// Правило сопоставления колонок
    /// </summary>
    [Serialize]
	public class ColumnRowCheckCondition {
		/// <summary>
		/// Действие
		/// </summary>
        [Serialize]
		public string Action { get; set; }
        /// <summary>
        /// 
        /// </summary>
		[SerializeNotNullOnly]
		public decimal Value { get; set; }
        /// <summary>
        /// 
        /// </summary>
		[SerializeNotNullOnly]
		public decimal Value2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
		[SerializeNotNullOnly]
		public bool DenyBlock { get; set; }
        /// <summary>
        /// 
        /// </summary>
		[SerializeNotNullOnly]
		public string RowTag { get; set; }
        /// <summary>
        /// 
        /// </summary>
		[SerializeNotNullOnly]
		public string RowClass { get; set; }
        /// <summary>
        /// 
        /// </summary>
		[SerializeNotNullOnly]
		public string RowStyle { get; set; }
        /// <summary>
        /// 
        /// </summary>
		[SerializeNotNullOnly]
		public string CellClass { get; set; }
        /// <summary>
        /// 
        /// </summary>
		[SerializeNotNullOnly]
		public string CellStyle { get; set; }
        /// <summary>
        /// 
        /// </summary>
		[SerializeNotNullOnly]
		public string Comment { get; set; }
    }
}