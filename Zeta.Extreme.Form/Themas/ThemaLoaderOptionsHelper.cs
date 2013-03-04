namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// Формирует стандартные конфигурации загрузчика тем
	/// </summary>
	public static class ThemaLoaderOptionsHelper {
		/// <summary>
		/// 	Формирует стандартные опции загрузки тем для Zeta.Extreme форм
		/// </summary>
		/// <param name="rootdirectory"> </param>
		/// <returns> </returns>
		public static ThemaLoaderOptions GetExtremeFormOptions(string rootdirectory = null)
		{
			var result = new ThemaLoaderOptions();
			if (!string.IsNullOrWhiteSpace(rootdirectory))
			{
				result.RootDirectory = rootdirectory;
			}
			result.LoadLibraries = false; //библиотеки тоже надо помечать на совместимость
			result.ElementTypes = ElementType.Form;
			result.LoadIerarchy = false;
			result.FilterParameters = "extreme";
			result.ClassRedirectMap["Comdiv.Zeta.Web.Themas.EcoThema, Comdiv.Zeta.Web"] = typeof(EcoThema).AssemblyQualifiedName;
			return result;
		}
	}
}