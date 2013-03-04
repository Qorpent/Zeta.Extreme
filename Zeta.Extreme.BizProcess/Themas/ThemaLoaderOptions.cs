#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ThemaLoaderOptions.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;

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
	}
}