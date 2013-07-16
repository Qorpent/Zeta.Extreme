
using System;
using NUnit.Framework;
using Qorpent.Serialization;
using Zeta.Extreme.Developer.Analyzers;
using Zeta.Extreme.Developer.Config;

namespace Zeta.Extreme.Developer.Tests {
	[TestFixture]
	public class AnalyzerTest :CodeIndexBasedTest {
		private Analyzer analyzer;

		public AnalyzerTest() {
			
			simplebx = @"
paramlib :
	param svod, ""Сводный"", type=bool, tab=_TAB_SYS, group=_ГП_ОЛАП: true
	param outercodes, ""Использовать внешний код"", tab=_TAB_FMT, type=bool, group=_ГП_ДЕРЕВО : true
	param viewname, ""Вид для отрисовки (не менять без понимания!)"", tab=_TAB_SYS, group=_ГП_СИСТЕМА : ""std/std""
	param generatorname, ""Вид генератора (не менять без понимания!)"",tab=_TAB_SYS, group=_ГП_СИСТЕМА : ""std/std""
	param useexteval, ""Использовать новый расчетчик"",tab=_TAB_SYS, group=_ГП_СИСТЕМА, type=bool : true
	param usenewdrs, ""Использовать новый вывод первичных значений"",tab=_TAB_SYS, group=_ГП_СИСТЕМА, type=bool : true
	
	param useformmatrix, ""Использовать матрицы"",tab=_TAB_SYS, group=_ГП_СИСТЕМА, type=bool : false
	param evaluateblackboxes, ""Вычислять 'черные ящики' в матрице"",tab=_TAB_SYS, group=_ГП_СИСТЕМА, type=bool : false
	param applynoprimaryonmatrix, ""Использовать формулы из матрицы"",tab=_TAB_SYS, group=_ГП_СИСТЕМА, type=bool : true
	param noformula, ""Не вычислять формулы"",tab=_TAB_SYS, group=_ГП_СИСТЕМА, type=bool :false
	param hideOwn, ""Скрывать расширяемые строки"", tab=_TAB_FMT, type=bool, group=_ГП_ДЕРЕВО : true
	param collapsespecials, ""Сворачивать расширяемые разделы"", tab=_TAB_FMT, type=bool, group=_ГП_ДЕРЕВО : false
	param year, Год, type=int, idx=10, group=_ГП_ВРЕМЯ :
		defaultvalue = 0
		listdefinition=""""""
			0 : Текущий |
			-1 : Предыдущий  |
			СПИСОКГОДОВ
		""""""
thema mythema 
	out Aa
		var myparam  'Крутой такой параметр' myattribute = 'cool'
";

		}

		public override void Setup()
		{
			base.Setup();
			this.analyzer = new Analyzer {CodeIndex = index};
		}

		[Test]
		[Explicit]
		public void CanGetAttributesFromParamlibParams() {
			var attributes = analyzer.GetParameterAttributes();
			var result = new XmlSerializer().Serialize("test", attributes);
			Console.WriteLine(result);
			Assert.AreEqual(@"{""0"": {""Name"": ""type"", ""ValueVariants"": {""0"": {""Value"": ""bool"", ""References"": {""0"": "" at simple : 3:0 : paramlib/param svod(Сводный)"", ""1"": "" at simple : 4:0 : paramlib/param outercodes(Использовать внешний код)"", ""2"": "" at simple : 7:0 : paramlib/param useexteval(Использовать новый расчетчик)"", ""3"": "" at simple : 8:0 : paramlib/param usenewdrs(Использовать новый вывод первичных значений)"", ""4"": "" at simple : 10:0 : paramlib/param useformmatrix(Использовать матрицы)"", ""5"": "" at simple : 11:0 : paramlib/param evaluateblackboxes(Вычислять 'черные ящики' в матрице)"", ""6"": "" at simple : 12:0 : paramlib/param applynoprimaryonmatrix(Использовать формулы из матрицы)"", ""7"": "" at simple : 13:0 : paramlib/param noformula(Не вычислять формулы)"", ""8"": "" at simple : 14:0 : paramlib/param hideOwn(Скрывать расширяемые строки)"", ""9"": "" at simple : 15:0 : paramlib/param collapsespecials(Сворачивать расширяемые разделы)""}}, ""1"": {""Value"": ""int"", ""References"": {""0"": "" at simple : 16:0 : paramlib/param year(Год)""}}}}, ""1"": {""Name"": ""tab"", ""ValueVariants"": {""0"": {""Value"": ""_TAB_SYS"", ""References"": {""0"": "" at simple : 3:0 : paramlib/param svod(Сводный)"", ""1"": "" at simple : 5:0 : paramlib/param viewname(Вид для отрисовки (не менять без понимания!))"", ""2"": "" at simple : 6:0 : paramlib/param generatorname(Вид генератора (не менять без понимания!))"", ""3"": "" at simple : 7:0 : paramlib/param useexteval(Использовать новый расчетчик)"", ""4"": "" at simple : 8:0 : paramlib/param usenewdrs(Использовать новый вывод первичных значений)"", ""5"": "" at simple : 10:0 : paramlib/param useformmatrix(Использовать матрицы)"", ""6"": "" at simple : 11:0 : paramlib/param evaluateblackboxes(Вычислять 'черные ящики' в матрице)"", ""7"": "" at simple : 12:0 : paramlib/param applynoprimaryonmatrix(Использовать формулы из матрицы)"", ""8"": "" at simple : 13:0 : paramlib/param noformula(Не вычислять формулы)""}}, ""1"": {""Value"": ""_TAB_FMT"", ""References"": {""0"": "" at simple : 4:0 : paramlib/param outercodes(Использовать внешний код)"", ""1"": "" at simple : 14:0 : paramlib/param hideOwn(Скрывать расширяемые строки)"", ""2"": "" at simple : 15:0 : paramlib/param collapsespecials(Сворачивать расширяемые разделы)""}}}}, ""2"": {""Name"": ""group"", ""ValueVariants"": {""0"": {""Value"": ""_ГП_ОЛАП"", ""References"": {""0"": "" at simple : 3:0 : paramlib/param svod(Сводный)""}}, ""1"": {""Value"": ""_ГП_ДЕРЕВО"", ""References"": {""0"": "" at simple : 4:0 : paramlib/param outercodes(Использовать внешний код)"", ""1"": "" at simple : 14:0 : paramlib/param hideOwn(Скрывать расширяемые строки)"", ""2"": "" at simple : 15:0 : paramlib/param collapsespecials(Сворачивать расширяемые разделы)""}}, ""2"": {""Value"": ""_ГП_СИСТЕМА"", ""References"": {""0"": "" at simple : 5:0 : paramlib/param viewname(Вид для отрисовки (не менять без понимания!))"", ""1"": "" at simple : 6:0 : paramlib/param generatorname(Вид генератора (не менять без понимания!))"", ""2"": "" at simple : 7:0 : paramlib/param useexteval(Использовать новый расчетчик)"", ""3"": "" at simple : 8:0 : paramlib/param usenewdrs(Использовать новый вывод первичных значений)"", ""4"": "" at simple : 10:0 : paramlib/param useformmatrix(Использовать матрицы)"", ""5"": "" at simple : 11:0 : paramlib/param evaluateblackboxes(Вычислять 'черные ящики' в матрице)"", ""6"": "" at simple : 12:0 : paramlib/param applynoprimaryonmatrix(Использовать формулы из матрицы)"", ""7"": "" at simple : 13:0 : paramlib/param noformula(Не вычислять формулы)""}}, ""3"": {""Value"": ""_ГП_ВРЕМЯ"", ""References"": {""0"": "" at simple : 16:0 : paramlib/param year(Год)""}}}}, ""3"": {""Name"": ""idx"", ""ValueVariants"": {""0"": {""Value"": ""10"", ""References"": {""0"": "" at simple : 16:0 : paramlib/param year(Год)""}}}}, ""4"": {""Name"": ""defaultvalue"", ""ValueVariants"": {""0"": {""Value"": ""0"", ""References"": {""0"": "" at simple : 16:0 : paramlib/param year(Год)""}}}}, ""5"": {""Name"": ""listdefinition"", ""ValueVariants"": {""0"": {""Value"": ""\r\n\t\t\t0 : Текущий |\r\n\t\t\t-1 : Предыдущий  |\r\n\t\t\tСПИСОКГОДОВ\r\n\t\t"", ""References"": {""0"": "" at simple : 16:0 : paramlib/param year(Год)""}}}}}",result);

		} 

		[Test]
		[Explicit]
		public void Integration_Test_In_Work_Themas_Parameters() {
			var config = new DeveloperConfig {
				ThemaSourceFolders = new[] {"C:\\code\\usr\\ugmk\\eco\\data", "C:\\code\\zeta\\zeta\\sys\\data"}
			};
			var codeindex = new CodeIndex {Config = config};
			var analyzer = new Analyzer {CodeIndex = codeindex};
			var result = new XmlSerializer().Serialize("test", analyzer.GetParameterAttributes());
			Console.WriteLine(result);
		}
		[Test]
		[Explicit]
		public void Integration_Test_In_Work_Themas_Colset()
		{
			var config = new DeveloperConfig
			{
				ThemaSourceFolders = new[] { "C:\\code\\usr\\ugmk\\eco\\data", "C:\\code\\zeta\\zeta\\sys\\data" }
			};
			var codeindex = new CodeIndex { Config = config };
			var analyzer = new Analyzer { CodeIndex = codeindex };
			var result = new XmlSerializer().Serialize("test", analyzer.GetColsetAttribtes());
			Console.WriteLine(result);
		}
	}
}