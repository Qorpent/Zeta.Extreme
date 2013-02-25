using System.Linq;
using Comdiv.Extensions;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Meta
{
	/// <summary>
	/// Фильтр по группам предприятий для колонок
	/// </summary>
	public static class GroupFilterHelper
	{
		/// <summary>
		/// Проверяет соответвие группе
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="forgroupstr"></param>
		/// <returns></returns>
		public static bool IsMatch(IZetaMainObject obj, string forgroupstr) {
			if(forgroupstr.noContent()) return true;
			if (null == obj) return true;
			var rules = forgroupstr.split(false,true,'/');
			var includes = rules.Where(x => !x.StartsWith("!")).ToArray();
			var excludes = rules.Where(x => x.StartsWith("!")).Select(x => x.Substring(1)).ToArray();
			if (excludes.Any(exclude => IsMatchRule(obj, exclude))) {
				return false;
			}
			if (includes.Any(include => IsMatchRule(obj, include))) {
				return true;
			}
			if( includes.Length != 0 ) {
				return false; //должен был просоответствовать хотя бы одному правилу включения (если есть)
			}

			return true;
		}
		/// <summary>
		/// Проверяет соответствие правилу
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="rule"></param>
		/// <returns></returns>
		public static bool IsMatchRule(IZetaMainObject obj, string rule) {
			bool ismatch = false;
			if (rule.StartsWith("div_")) {
				ismatch = obj.Group.Code == rule.Substring(4);
			}
			else if (rule.StartsWith("obj_")) {
				ismatch = obj.Id == rule.Substring(4).toInt();
			}
			else if (rule.StartsWith("otr_")) {
				ismatch = obj.Role.Code == rule.Substring(4);
			}
			else {
				ismatch = obj.GroupCache.Contains("/" + rule + "/");
			}
			return ismatch;
		}
	}
}
