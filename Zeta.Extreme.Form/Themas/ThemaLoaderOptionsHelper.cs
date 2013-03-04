namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// ��������� ����������� ������������ ���������� ���
	/// </summary>
	public static class ThemaLoaderOptionsHelper {
		/// <summary>
		/// 	��������� ����������� ����� �������� ��� ��� Zeta.Extreme ����
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
			result.LoadLibraries = false; //���������� ���� ���� �������� �� �������������
			result.ElementTypes = ElementType.Form;
			result.LoadIerarchy = false;
			result.FilterParameters = "extreme";
			result.ClassRedirectMap["Comdiv.Zeta.Web.Themas.EcoThema, Comdiv.Zeta.Web"] = typeof(EcoThema).AssemblyQualifiedName;
			return result;
		}
	}
}