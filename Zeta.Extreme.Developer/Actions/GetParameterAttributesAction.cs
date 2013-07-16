using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qorpent.Mvc;

namespace Zeta.Extreme.Developer.Actions
{
	/// <summary>
	/// Возвращает атрибуты параметров
	/// </summary>
	[Action("zdev.paramattributes",Role="DEVELOPER")]
	public class GetParameterAttributesAction :AnalyzerActionBase
	{
		/// <summary>
		/// Возвращает атрибуты параметров
		/// </summary>
		/// <returns></returns>
		protected override object MainProcess() {
			return Analyzer.GetParameterAttributes();
		}
	}
}
