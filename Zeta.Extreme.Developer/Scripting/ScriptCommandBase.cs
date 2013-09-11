using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using Qorpent.Config;
using Qorpent.Log;
using Qorpent.Mvc;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.Developer.Scripting {
	/// <summary>
	/// 
	/// </summary>
	public abstract class ScriptCommandBase : IScriptCommand {
		/// <summary>
		/// Инициализатор
		/// </summary>
		/// <param name="def"></param>
		public virtual void Initialize(XElement def) {
			Definition = def;
            this.Namespace = def.Attr("namespace");
            this.AppName = def.Attr("node");
            this.Into = def.Attr("into");
            this.ClassName = def.Attr("name");
		}
		/// <summary>
		/// Исходное определение
		/// </summary>
		protected XElement Definition { get; set; }
        /// <summary>
        /// Получить имя  файла по умолчанию
        /// </summary>
        /// <returns></returns>
        protected string GetDefaultFileName() {
            var ns = Namespace;
            if (string.IsNullOrWhiteSpace(ns)) {
                ns = "global";
            }
            var cs = ClassName;
            if (string.IsNullOrWhiteSpace(ClassName)) {
                cs = this.GetType().Name;
            }
            return ns + "." + cs + ".bxls";
        }
       

		/// <summary>
		/// Установить родительский скрипт
		/// </summary>
		/// <param name="script"></param>
		public virtual void SetParent(Script script) {
			Script = script;
			Log = script.Log;
		}
		/// <summary>
		/// Журнал
		/// </summary>
		protected IUserLog Log { get; set; }

		/// <summary>
		/// Родительский скрипт
		/// </summary>
		protected Script Script { get; set; }

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
	    protected string AppName { get; set; }

	    /// <summary>
	    /// Целевое имя класса
	    /// </summary>
	    public string ClassName { get; set; }

        /// <summary>
        /// Выполнение скрипта
        /// </summary>
        /// <param name="context"></param>
        public void Run(IConfig context)
        {
            var client = SetupMvcClient(context);
            InternalRun(context, client);
        }
        /// <summary>
        /// Собственно тело действия
        /// </summary>
        /// <param name="context"></param>
        /// <param name="client"></param>
        protected virtual void InternalRun(IConfig context, MvcClient client) {
	        Log.Info("start " + GetType().Name);
            var commandName = GetCommandName();
            if (string.IsNullOrWhiteSpace(commandName)) {
                throw new Exception("no command detected");
            }
            var parameters = SetupParameters(commandName,context);
            var result = client.GetString(commandName, parameters);
            result.Wait();
            Save(result.Result,GetFileName(commandName,context,result));
			Log.Info("finish " + GetType().Name);
        }
        /// <summary>
        /// Получить имя файла
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="context"></param>
        /// <param name="result"></param>
        /// <returns></returns>
	    protected virtual string GetFileName(string commandName, IConfig context, Task<string> result) {
	        return GetDefaultFileName();
	    }

	    /// <summary>
	    /// Сформировать параметры запроса
	    /// </summary>
	    /// <param name="commandName"></param>
	    /// <param name="context"></param>
	    /// <returns></returns>
	    protected virtual object SetupParameters(string commandName, IConfig context) {
		    var result = new Dictionary<string, object>();
		    result["namespace"] = Namespace;
		    result["classname"] = ClassName;
		    return result;
	    }

	    /// <summary>
        /// Конфигурирует удаленного клиента
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
	    protected virtual MvcClient SetupMvcClient(IConfig context) {
	        var credentials = context.Get<ICredentials>("credentials");
	        var client = new MvcClient(AppName, credentials);
	        return client;
	    }

	    /// <summary>
	    /// Производит запись продукции на диск
	    /// </summary>
	    /// <param name="content"></param>
	    /// <param name="localfile"></param>
	    protected virtual string Save(string content, string localfile = null) {
            if (string.IsNullOrWhiteSpace(localfile)) {
                localfile = GetDefaultFileName();
            }
            if (string.IsNullOrWhiteSpace(Into)) {
                throw new Exception("Into target not configured");
            }
            if (!Directory.Exists(Into)) {
                Directory.CreateDirectory(Into);
            }
            var filename = Path.Combine(Into, localfile);
            File.WriteAllText(filename,content);
		    Log.Info("file " + filename + " wrote");
            return filename;
        }
        /// <summary>
        /// Получить имя команды
        /// </summary>
        /// <returns></returns>
        protected virtual string GetCommandName() {
            return null;
        }
	}
}