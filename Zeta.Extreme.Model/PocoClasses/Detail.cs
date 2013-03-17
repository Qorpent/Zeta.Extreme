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
// PROJECT ORIGIN: Zeta.Extreme.Model/Detail.cs
#endregion
using System;
using System.Collections.Generic;
using Qorpent;
using Qorpent.Model;
using Qorpent.Serialization;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model {
	/// <summary>
	/// Detail object implementation
	/// </summary>
	public sealed partial class Detail : Entity, IZetaDetailObject {
		/// <summary>
		/// </summary>
		public Detail() {
			
			Start = QorpentConst.Date.Begin;
			Finish = QorpentConst.Date.End;
		
		}

		/// <summary>
		/// Main obj of detail
		/// </summary>
		public IZetaMainObject Object { get; set; }

		/// <summary>
		/// Second obj of detail
		/// </summary>
		public IZetaMainObject AltObject { get; set; }


		/// <summary>
		/// Path in hierarhy
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		///Currency of entity
		/// </summary>
		public string Currency { get; set; }

		/// <summary>
		/// Start date of detail existence
		/// </summary>
		public DateTime Start { get; set; }

		/// <summary>
		/// Finish date of detail existence
		/// </summary>
		public DateTime Finish { get; set; }


		/// <summary>
		/// Helper code that maps any foreign coding system
		/// </summary>
		public string OuterCode { get; set; }


		/// <summary>
		/// ObjType to which instance is attached
		/// </summary>
		[Serialize] public IObjectType ObjType { get; set; }


		/// <summary>
		/// Parent <see cref="IZetaDetailObject"/>
		/// </summary>
		public IZetaDetailObject Parent { get; set; }

		/// <summary>
		/// Geo location of detail
		/// </summary>
		public IZetaPoint Point { get; set; }

		/// <summary>
		/// list of attached details
		/// </summary>
		public IList<IZetaDetailObject> Details { get; set; }

		/// <summary>
		/// Full name of detail
		/// </summary>
		public string FullName { get; set; }

		/// <summary>
		/// Resolves tag value by it's <c>name</c>
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public string ResolveTag(string name) {
			var tag = TagHelper.Value(Tag, name);
			if (tag.IsEmpty()) {
				tag = ObjType.ResolveTag(name);
			}
			if (tag.IsEmpty() && null != Parent) {
				tag = Parent.ResolveTag(name);
			}
			if (tag.IsEmpty()) {
				tag = Object.ResolveTag(name);
			}
			return tag ?? "";
		}


		/// <summary>
		/// </summary>
		/// <param name="detail"></param>
		/// <returns></returns>
		private bool Equals(IZetaDetailObject detail) {
			if (detail == null) {
				return false;
			}
			if (Id != detail.Id) {
				return false;
			}
			if (!Equals(Name, detail.Name)) {
				return false;
			}
			if (!Equals(Code, detail.Code)) {
				return false;
			}

			return true;
		}

		/// <summary>
		/// Определяет, равен ли заданный объект <see cref="T:System.Object"/> текущему объекту <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// true, если заданный объект равен текущему объекту; в противном случае — false.
		/// </returns>
		/// <param name="obj">Объект, который требуется сравнить с текущим объектом.</param><filterpriority>2</filterpriority>
		public override bool Equals(object obj) {
			if (ReferenceEquals(this, obj)) {
				return true;
			}
			return Equals(obj as IZetaDetailObject);
		}

		/// <summary>
		/// Играет роль хэш-функции для определенного типа.
		/// </summary>
		/// <returns>
		/// Хэш-код для текущего объекта <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode() {
			var result = Id;
			result = 29*result + (Code != null ? Code.GetHashCode() : 0);
			result = 29*result + (Name != null ? Name.GetHashCode() : 0);

			return result;
		}
	}
}