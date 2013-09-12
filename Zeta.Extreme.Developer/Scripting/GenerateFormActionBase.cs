using System.IO;
using System.Linq;
using System.Net;
using Qorpent.Config;
using Qorpent.Mvc;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.Developer.Scripting {
    /// <summary>
	/// 
	/// </summary>
	public abstract class GenerateFormActionBase : ScriptCommandBase {
        /// <summary>
		/// 
		/// </summary>
		protected bool UseDependency { get; set; }
		/// <summary>
		/// 
		/// </summary>
		protected string FormCode { get; set; }

        /// <summary>
		/// Инициализатор
		/// </summary>
		/// <param name="def"></param>
		public override void Initialize(System.Xml.Linq.XElement def) {
            base.Initialize(def);
            this.FormCode = def.Attr("code");
			this.UseDependency = null != def.Attribute("withdependency") || def.Attr("name") == "withdependency";
			this.ValueRedirectAttribute = def.Attr("value");
			if (UseDependency) {
				ClassName = "";
			}
		}
		/// <summary>
		/// Специальное поле для словарей
		/// </summary>
		protected string ValueRedirectAttribute { get; set; }

        /// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected abstract string GetDialect();
		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <param name="client"></param>
		protected void ExecuteFormImport(string s, MvcClient client) {
			Log.Debug("begin form "+s);
			var parameters = new {root = s, 
				format = GetDialect(), 
				@namespace = Namespace,
				classname=ClassName,
			value=ValueRedirectAttribute};
			var getcontent = client.GetString(ScriptConstants.GENERATE_FORM_COMMAND, parameters);
			getcontent.Wait();
			Log.Debug("data retrieved");
			var n = ClassName;
			if (string.IsNullOrWhiteSpace(n)) {
				n = s;
			}
           var  filename =Save(getcontent.Result, Namespace + "." + n + ".bxls");
			Log.Debug("form wrote to "+filename);
			Log.Debug("finish form "+s);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="client"></param>
		/// <returns></returns>
		protected string[] GetFormList(MvcClient client) {
			var txml = client.GetXml(ScriptConstants.FORM_DEPENDENCY_COMMAND, 
			                         new {root = FormCode, listonly = true});
			txml.Wait();
			return txml.Result.Descendants("item").Select(_ => _.Value).ToArray();
		}
		
        /// <summary>
        /// Внутренний метод выполнения
        /// </summary>
        /// <param name="context"></param>
        /// <param name="client"></param>
        protected override void InternalRun(IConfig context, MvcClient client) {
            var forms = new[] {FormCode};
            Log.Debug(string.Format("begin " + GetType().Name + " on {0} for {1} with dep={2}", AppName, FormCode,
                                    UseDependency));
            if (UseDependency) {
                Log.Debug("start get dep list");
                forms = GetFormList(client);
                Log.Debug("dep list catched");
            }
            foreach (var f in forms) {
                ExecuteFormImport(f, client);
            }
        }
    }
}