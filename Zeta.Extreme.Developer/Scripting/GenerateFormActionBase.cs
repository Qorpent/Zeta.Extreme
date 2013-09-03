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
		/// Комманда экспорта форм
		/// </summary>
		public const string GENERATE_FORM_COMMAND = "zdev/exporttree";

		/// <summary>
		/// Команда зависимостей формы
		/// </summary>
		public const string FORM_DEPENDENCY_COMMAND = "zdev/exportdependencydot";

		/// <summary>
		/// Целевое пространство имен
		/// </summary>
		protected string Namespace { get; set; }

		/// <summary>
		/// Целевая папка
		/// </summary>
		protected string Into { get; set; }
		/// <summary>
		/// 
		/// </summary>
		protected bool UseDependency { get; set; }
		/// <summary>
		/// 
		/// </summary>
		protected string FormCode { get; set; }

		/// <summary>
		/// 
		/// </summary>
		protected string AppName { get; set; }

		/// <summary>
		/// Инициализатор
		/// </summary>
		/// <param name="def"></param>
		public override void Initialize(System.Xml.Linq.XElement def) {
			this.Namespace = def.Attr("namespace");
			this.AppName = def.Attr("node");
			this.FormCode = def.Attr("code");
			this.Into = def.Attr("into");
			this.ClassName = def.Attr("name");
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
		/// Целевое имя класса
		/// </summary>
		public string ClassName { get; set; }
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
			var getcontent = client.GetString(GENERATE_FORM_COMMAND, parameters);
			getcontent.Wait();
			Log.Debug("data retrieved");
			if (!Directory.Exists(Into)) {
				Directory.CreateDirectory(Into);
			}
			var n = ClassName;
			if (string.IsNullOrWhiteSpace(n)) {
				n = s;
			}
			var filename = Path.Combine(Into, Namespace + "." + n + ".bxls");
			File.WriteAllText(filename,getcontent.Result);
			Log.Debug("form wrote to "+filename);
			Log.Debug("finish form "+s);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="client"></param>
		/// <returns></returns>
		protected string[] GetFormList(MvcClient client) {
			var txml = client.GetXml(FORM_DEPENDENCY_COMMAND, 
			                         new {root = FormCode, listonly = true});
			txml.Wait();
			return txml.Result.Descendants("item").Select(_ => _.Value).ToArray();
		}
		/// <summary>
		/// Выполнение скрипта
		/// </summary>
		/// <param name="context"></param>
		public override void Run(IConfig context)
		{
			var credentials = context.Get<ICredentials>("credentials");
			var client = new MvcClient(AppName, credentials);
			var forms = new[] { FormCode };
			Log.Debug(string.Format("begin "+this.GetType().Name+" on {0} for {1} with dep={2}", AppName, FormCode, UseDependency));
			if (UseDependency)
			{
				Log.Debug("start get dep list");
				forms = GetFormList(client);
				Log.Debug("dep list catched");
			}
			foreach (var f in forms)
			{
				ExecuteFormImport(f, client);
			}
		}
	}
}