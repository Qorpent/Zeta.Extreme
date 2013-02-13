#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : ThemaLoaderOptions.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	Описывает опции загрузки тем
	/// </summary>
	public class ThemaLoaderOptions {
		/// <summary>
		/// Опции провайдера тем по умолчанию
		/// </summary>
		public ThemaLoaderOptions() {
			this.ClassRedirectMap = new Dictionary<string, string>();
		}
		/// <summary>
		/// 	Формирует стандартные опции загрузки тем для Zeta.Extreme форм
		/// </summary>
		/// <param name="rootdirectory"> </param>
		/// <returns> </returns>
		public static ThemaLoaderOptions GetExtremeFormOptions(string rootdirectory = null) {
			var result = new ThemaLoaderOptions();
			if (!string.IsNullOrWhiteSpace(rootdirectory)) {
				result.RootDirectory = rootdirectory;
			}
			result.LoadIerarchy = false;
			result.FilterParameters = "extreme";
			result.ClassRedirectMap["Comdiv.Zeta.Web.Themas.EcoThema, Comdiv.Zeta.Web"] = typeof (EcoThema).AssemblyQualifiedName;
			return result;
		}

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
		/// 	Папка для загрузки тем
		/// </summary>
		public string RootDirectory = "~/tmp/compiled_themas";

		public IDictionary<string, string> ClassRedirectMap { get; private set; }
	}
}