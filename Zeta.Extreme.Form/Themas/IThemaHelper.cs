using System;
using System.Collections.Generic;
using System.Text;
using Comdiv.Reporting;
using Comdiv.Zeta.Web.InputTemplates;

namespace Comdiv.Zeta.Web.Themas
{
    public interface IThemaHelper
    {
        IThema get(string  code);
        IThemaConfiguration getcfg(string code);
        IInputTemplate getform(string code);
        IReportDefinition getreport(string code);
        IThema[] getchildren(IThema thema);
        IThema[] getchildren(string themacode);
        IThema[] getgroups();
        IThema[] getgrouproots(IThema group);
        IThema[] getgrouproots(string groupcode);
        bool istest(IThema thema);
        bool isdesign(IThema thema);
        bool isadmin(IThema thema);
    }
}
