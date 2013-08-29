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
// PROJECT ORIGIN: Zeta.Extreme.BizProcess/ThemaLoaderOptions.cs
#endregion
using System.Collections.Generic;
using System.Xml.Linq;
using Qorpent.IO;

namespace Zeta.Extreme.BizProcess.Themas {
	/// <summary>
	/// 	Описывает опции загрузки тем
	/// </summary>
	public class ThemaLoaderOptions {
		/// <summary>
		/// 	Опции провайдера тем по умолчанию
		/// </summary>
		public ThemaLoaderOptions() {
			ClassRedirectMap = new Dictionary<string, string>();
		}

		/// <summary>
		/// 	Замещения классов, упомянутых в темах
		/// </summary>
		public IDictionary<string, string> ClassRedirectMap { get; private set; }

		/// <summary>
		/// Прямое указание массива определений тем
		/// </summary>
		public XElement[] DirectThemaConfigurations { get; set; }

		/// <summary>
		/// Дополнительный источник свойств
		/// </summary>
		public IBizCasePropertySource[] PropertySources { get; set; }
		/// <summary>
		/// Источник для загрузки файлов
		/// </summary>
		public IFileSource FileSource { get; set; }


		/// <summary>
		/// 	Флаги элементов, которые загружаются в процессе обработки
		/// </summary>
		public ElementType ElementTypes = ElementType.All;

		/// <summary>
		/// 	Строка - описатель проверки параметров темы при фильтрации
		/// </summary>
		public string FilterParameters = null;

		/// <summary>
		/// 	Загружать с контролем иерархии (загружает и отслеживает группы, родительские и дочерние темы)
		/// </summary>
		public bool LoadIerarchy = true;

		/// <summary>
		/// 	Требование грузить темы - библиотеки в любом случае
		/// </summary>
		/// <remarks>
		/// 	Если включено - FilterParameters и аналогичные игнорируются
		/// </remarks>
		public bool LoadLibraries = true;

		/// <summary>
		/// Флаг проверки целостности ссылок зависимостей
		/// </summary>
		public bool CheckBizLinksConsistentry = false;

		/// <summary>
		/// 	Папка для загрузки тем
		/// </summary>
		public string RootDirectory = "~/tmp/compiled_themas";
	}
}