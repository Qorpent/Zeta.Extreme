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
// PROJECT ORIGIN: Zeta.Extreme.Model/TagResolutionExtension.cs

#endregion

using System.Collections.Generic;
using Qorpent.Model;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.Extensions {

	/// <summary>
	///     Extension, that provides Tag support for entities without implementation at
	///     POCO level
	/// </summary>
	public static class TagResolutionExtension {
		/// <summary>resolvetag.</summary>
		public const string RESOLVETAG_KEY_BASE = "resolvetag.";

		/// <summary>
		///     Resolves tag value for <see cref="IZetaRow" />
		/// </summary>
		/// <param name="row">row to test tag</param>
		/// <param name="name">tag name</param>
		/// <returns></returns>
		public static string ResolveTag(this IZetaRow row, string name) {
			if (null == row) {
				return string.Empty;
			}
			if (string.IsNullOrWhiteSpace(name)) {
				return string.Empty;
			}
			return null == row.TemporalParent
				       ? ResolveTagPersistently(row, name)
				       : // usual mode - value will be cached
				       InternalResolveTag(row, name); // "presentation" mode with TemporalParent - value will not cached
		}

		/// <summary>
		///     Row's tag resolution with caching in local properties for reuse
		/// </summary>
		/// <param name="row"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		private static string ResolveTagPersistently(IZetaRow row, string name) {
			var resolvedKey = RESOLVETAG_KEY_BASE + name;
			string persistentResolveResult;
			if (row.LocalProperties.ContainsKey(resolvedKey)) {
				persistentResolveResult = (string) row.LocalProperties[resolvedKey];
			}
			else {
				persistentResolveResult = InternalResolveTag(row, name);
				row.LocalProperties[resolvedKey] = persistentResolveResult;
			}
			return persistentResolveResult;
		}

		/// <summary>
		///     Direct row's tag resolution without caching
		/// </summary>
		/// <param name="row"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		private static string InternalResolveTag(IZetaRow row, string name) {
			if (TagHelper.Has(row.Tag, name)) {
				return TagHelper.Value(row.Tag, name) ?? "";
			}
			if (null != row.TemporalParent) {
				return row.TemporalParent.ResolveTag(name);
			}
			if (null == row.Parent) {
				return "";
			}
			return row.Parent.ResolveTag(name);
		}
	}
}