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
// PROJECT ORIGIN: Zeta.Extreme.Core/FormulaAssemblyCache.cs
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Qorpent;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme {
	/// <summary>
	/// Инкапсуляция кэша 
	/// </summary>
	public class FormulaAssemblyCache {
		/// <summary>
		/// Кэш уже загруженных в домен сборок - чтобы избежать AssemblyLeak
		/// </summary>
		/// <remarks>Ключ - нормальизованный полный путь к файлу</remarks>
		static readonly IDictionary<string,Assembly> Cache  = new Dictionary<string, Assembly>();


		/// <summary>
		/// Корневая директорая кэша
		/// </summary>
		protected string Rootdir { get; set; }

		/// <summary>
		/// Метод проверки готовности внутренней среды кэша для обслуживания прочих методов
		/// </summary>
		private void CheckCacheDirectory() {
			if(string.IsNullOrWhiteSpace(Rootdir))throw new Exception("не указана корневая директория кэша");
			if(!Path.IsPathRooted(Rootdir)) {
				Rootdir = Path.GetFullPath(Rootdir);
			}
			Directory.CreateDirectory(Rootdir);
		}


		private IEnumerable<string> GetBaseAssembliesFileNames() {
			//reference to Zeta.Extreme.Model
			yield return Assembly.GetAssembly(typeof (IZetaCell)).CodeBase.Replace(EnvironmentInfo.FULL_FILE_NAME_START, "");
			//reference to Zeta.Extreme.Core
			yield return Assembly.GetAssembly(typeof(Query)).CodeBase.Replace(EnvironmentInfo.FULL_FILE_NAME_START, "");
		} 

		/// <summary>
		/// Возвращает версию исходных файлов
		/// </summary>
		/// <returns></returns>
		public DateTime GetBaseLibraryVersion() {
			return GetBaseAssembliesFileNames().Select(File.GetLastWriteTime).Max();
		}

		/// <summary>
		/// Возвращает список файлов, пригодных для загрузки из кэша
		/// </summary>
		/// <returns>список файлов сборок</returns>
		/// <remarks>список упорядочен правильно</remarks>
		public IEnumerable<string> GetAssemblyFileNames() {
			CheckCacheDirectory();
			var baseversion = GetBaseLibraryVersion();
			var allfiles = Directory.GetFiles(Rootdir, "*.dll");
			return allfiles.Where(_ => File.GetLastWriteTime(_) >= baseversion).OrderBy(File.GetLastWriteTime);
		}
	
		/// <summary>
		/// Возвращает список DLL с формулами для загрузки
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Assembly> GetAssembliesToLoad() {
			foreach (var normalPath in GetAssemblyFileNames().Select(assemblyFileName => assemblyFileName.NormalizePath())) {
				if(Cache.ContainsKey(normalPath)) {
					yield return Cache[normalPath];
				}
				else {
					var assembly = Assembly.Load(File.ReadAllBytes(normalPath));
					Cache[normalPath] = assembly;
					yield return assembly;
				}
			}
		}

		/// <summary>
		/// 	Строит кэш из указанной директории
		/// </summary>
		/// <param name="root"> </param>
		public void Rebuild(string root) {
			Rootdir = root;
			_cachedTypes.Clear();
			foreach (var formula in GetAssembliesToLoad().SelectMany(_ => _.GetTypes()))
			{
				ExtractCachedFormula(formula);
			}
		}

		private void ExtractCachedFormula(Type formula)
		{
			var attr = ((FormulaAttribute)formula.GetCustomAttribute(typeof(FormulaAttribute), true));
			if (null == attr)
			{
				return;
			}
			_cachedTypes[attr.Key] = new CachedFormula { Version = attr.Version, Formula = formula };
		}
		/// <summary>
		/// Проверяет наличие в кэше формулы для ключа
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool ContainsKey(string key) {
			return _cachedTypes.ContainsKey(key);
		}
		/// <summary>
		/// Возвращает версию конкретной формулы
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public string GetVersion(string key) {
			return ContainsKey(key) ? _cachedTypes[key].Version : "<ILLEGAL-KEY-NO-ANY-VERSION>";
		}

		/// <summary>
		/// Возвращает тип формулы из кэша
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public Type GetFormulaType(string key) {
			return ContainsKey(key) ? _cachedTypes[key].Formula : null;
		}

		private readonly IDictionary<string, CachedFormula> _cachedTypes = new Dictionary<string, CachedFormula>();
		private class CachedFormula
		{
			/// <summary>
			/// </summary>
			public Type Formula;

			/// <summary>
			/// </summary>
			public string Version;
		}
	}
}