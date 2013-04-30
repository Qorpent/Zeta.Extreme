using System;
using System.Collections.Generic;
using System.Security.Principal;
using NUnit.Framework;
using Qorpent.Applications;
using Qorpent.IoC;
using Qorpent.Security;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.Form.InputTemplates;
using Zeta.Extreme.Form.Themas;
using Zeta.Extreme.Model;

namespace Zeta.Extreme.FrontEnd.Tests {
    class NodeLoadTests {

        [Test]
        public void CanGetStat() {
            var stat = new Actions.ZefsServer.GetClusterNodeLoad().Process();
            Assert.IsNotNull(stat);
        }
    }
}
