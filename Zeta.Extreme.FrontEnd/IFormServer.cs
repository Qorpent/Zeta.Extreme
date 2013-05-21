using System;
using System.Security.Principal;
using Qorpent.Applications;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Form.SaveSupport;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.FrontEnd {
    /// <summary>
    ///     FormServer Interface
    /// </summary>
    public interface IFormServer {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetCommonETag();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        DateTime GetCommonLastModified();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GetUserETag(IPrincipal user = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        void Execute(IApplication application);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IFormSessionDataSaver GetSaver();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        object GetServerStateInfo();

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="template"></param>
	    /// <param name="obj"></param>
	    /// <param name="year"></param>
	    /// <param name="period"></param>
	    /// <param name="initsavemode"></param>
	    /// <param name="initstatemode"></param>
	    /// <param name="subobject"></param>
	    /// <returns></returns>
	    FormSession Start(
            IInputTemplate template,
            IZetaMainObject obj,
            int year,
            int period,
            bool initsavemode = false,
			bool initstatemode = false,
			IZetaMainObject subobject =null
        );

        /// <summary>
        /// 
        /// </summary>
        void Reload();

        /// <summary>
        /// 
        /// </summary>
        void CheckGlobalReload();
    }
}
