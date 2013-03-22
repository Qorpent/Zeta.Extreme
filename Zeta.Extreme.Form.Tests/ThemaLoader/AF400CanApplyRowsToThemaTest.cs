using System.Collections.Generic;
using System.Xml.Linq;
using NUnit.Framework;
using Zeta.Extreme.BizProcess.Themas;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Form.Themas;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.MetaCaches;

namespace Zeta.Extreme.Form.Tests.ThemaLoader
{
	
	/// <summary>
	/// Проверяем возможность дозагрузки параметров темы из RowCache
	/// </summary>
	[TestFixture]
	public class AF400CanApplyRowsToThemaTest
	{
		/// <summary>A</summary>
		public const string FORMCODE = "A";

		/// <summary>balans2001</summary>
		public const string THEMANAME = "balans2011";

		/// <summary>balans2011.xml</summary>
		public const string RESOURCENAME = "balans2011.xml";

		/// <summary>FIN_OPERATOR</summary>
		public const string OLDPARAMVALUE = "FIN_OPERATOR";

		

		/// <summary>MYROLE</summary>
		public const string NEWPARAMVALUE = "MYROLE";

		public const string PARAMNAME = "";

		private XElement _balansdef;
		private ExtremeFormProvider _thcp;

		class testsource :IBizCasePropertySource {
			private IDictionary<string, object> _data;

			public testsource(IDictionary<string, object> data) {
				_data = data;
			}
			public IDictionary<string, object> GetExtendedProperties(string themacode) {
				return _data;
			}
		}

		private IThemaFactory getFactory(IBizCasePropertySource source = null) {
			_balansdef = XElement.Load(typeof(AF400CanApplyRowsToThemaTest).Assembly.OpenManifestResource(RESOURCENAME));
			_thcp = new ExtremeFormProvider(null, new[] { _balansdef }, source==null?null:new[]{source});
			_thcp.DoLoad();
			return _thcp.Factory;
		}
		private IThema getThema(IBizCasePropertySource source=null) {
			return getFactory(source).Get(THEMANAME);
		}

		private IInputTemplate getForm(IBizCasePropertySource source = null) {
			return getThema(source).GetForm(FORMCODE);
		}
		[Test]
		public void BalansLoadedByDefaultWithProperties() {
			var form = getForm();
			Assert.AreEqual(OLDPARAMVALUE,form.Role);
		}
		[Test]
		[Ignore("пока вообше непонятно будем делать или нет")]
		public void CanOverridePropertyFromRowCache() {
			var form = getForm(new testsource(new Dictionary<string, object>{{PARAMNAME,NEWPARAMVALUE}}));
			Assert.AreEqual(NEWPARAMVALUE, form.Role);
		}
	}
}
