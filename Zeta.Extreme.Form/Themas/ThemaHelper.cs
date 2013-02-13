using System;
using System.Collections.Generic;
using System.Linq;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Inversion;
using Comdiv.Reporting;
using Comdiv.Zeta.Web.InputTemplates;
using InversionExtensions = Comdiv.Inversion.InversionExtensions;

namespace Comdiv.Zeta.Web.Themas
{
    public class ThemaHelper : IThemaHelper, IReportDefinitionExtension
    {
    	public IThemaConfigurationProvider tcp;
    	public IThemaFactoryProvider tfp;
    	public IThemaFactoryConfiguration cfg;
    	public IThemaFactory factory;
    	public IReportDefinition report;

        public void Contextualize(IReportDefinition definition)
        {
            this.report = definition;
            this.tcp = this.tcp ?? InversionExtensions.get<IThemaConfigurationProvider>((IInversionContainer)myapp.ioc) ?? new ThemaConfigurationProvider();
            this.tfp = this.tfp ?? InversionExtensions.get<IThemaFactoryProvider>((IInversionContainer)myapp.ioc) ?? new ThemaFactoryProvider();
            this.cfg = this.tcp.Get();
            this.factory = this.tfp.Get();
        }


		public EcoThema[] getcommonecothemas() {
			var result = new List<EcoThema>();
			foreach (var thema in factory.GetAll().OfType<EcoThema>()) {
				if (!thema.Visible) continue;
				if (thema.Roleprefix.noContent()) continue;
				if (0 == thema.GetAllForms().Count()) continue;
				result.Add(thema);
			}
			foreach (var ecoThema in result)
			{
				ecoThema.GroupThema = factory.Get(ecoThema.Group);
			}
			var r =  result.OrderBy(x => string.Format("{0:0000}{1,-10}{2:0000}",x.GroupThema.Idx, x.Roleprefix,x.Idx)).ToArray();
			
			return r;
		}

        public IThema get(string code)
        {
            return factory.Get(code);
        }

        public IThemaConfiguration getcfg(string code)
        {
            return cfg.Configurations.FirstOrDefault(x => x.Code == code);
        }

        public IInputTemplate getform(string code)
        {
            return factory.GetForm(code);
        }

        public bool istest(IThema thema)
        {
            return thema.Role.toStr().Contains("TESTER");
        }

        public bool isdesign(IThema thema)
        {
            return thema.Role.toStr().Contains("DESIGNER");
        }

        public bool isadmin(IThema thema)
        {
            return thema.Role.toStr().Contains("ADMIN");
        }

        public IReportDefinition getreport(string code)
        {
            return factory.GetReport(code);
        }

        public IThema[] getchildren(IThema thema)
        {
            return getchildren(thema.Code);
        }

        public IThema[] getchildren(string themacode)
        {
            return factory.GetAll().Where(x => x.Parent == themacode).OrderBy(x=>x.Idx).ToArray();
        }

        public IThema[] getgroups()
        {
            return factory.GetAll().Where(x => x.IsGroup).OrderBy(x => x.Idx).ToArray();
        }

        public IThema[] getgrouproots(IThema group)
        {
            return getgrouproots(group.Code);
        }

        public IThema[] getgrouproots(string groupcode)
        {
            return factory.GetAll().Where(x => x.Group == groupcode).OrderBy(x => x.Idx).ToArray();
        }

    }
}