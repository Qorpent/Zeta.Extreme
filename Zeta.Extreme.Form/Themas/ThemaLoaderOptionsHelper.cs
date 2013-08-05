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
// PROJECT ORIGIN: Zeta.Extreme.Form/ThemaLoaderOptionsHelper.cs
#endregion

using Qorpent.IO;
using Zeta.Extreme.BizProcess.Themas;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// Формирует стандартные конфигурации загрузчика тем
	/// </summary>
	public static class ThemaLoaderOptionsHelper {
		/// <summary>
		/// 	Формирует стандартные опции загрузки тем для Zeta.Extreme форм
		/// </summary>
		/// <param name="rootdirectory"> </param>
		/// <param name="source"></param>
		/// <returns> </returns>
		public static ThemaLoaderOptions GetExtremeFormOptions(string rootdirectory = null, IFileSource source =null)
		{
			var result = new ThemaLoaderOptions();
			if (!string.IsNullOrWhiteSpace(rootdirectory))
			{
				result.RootDirectory = rootdirectory;
			}
			result.FileSource = source;
			result.LoadLibraries = false; //библиотеки тоже надо помечать на совместимость
			result.ElementTypes = ElementType.Form;
			result.LoadIerarchy = false;
			result.FilterParameters = "extreme";
#pragma warning disable 612,618
			result.ClassRedirectMap["Comdiv.Zeta.Web.Themas.EcoThema, Comdiv.Zeta.Web"] = typeof(EcoThema).AssemblyQualifiedName;
#pragma warning restore 612,618
			return result;
		}
	}
}