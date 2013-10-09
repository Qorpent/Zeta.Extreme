using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Qorpent.Applications;
using Qorpent.Mvc;
using Qorpent.Security;
using Zeta.Extreme.Core.Tests.CoreTests;
using Zeta.Extreme.Form.Meta;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.Core.Tests.NativeMaping
{
    [TestFixture]
    public class MiniHoldingHelperTests : DatabaseAwaredTestBase
    {
        [TestCase("OCM", true)]
        [TestCase("AGRO", true)]
        [TestCase("NAUKA", false)]
        public void DivisionIsMiniholdingDetection(string divisionCode, bool result) {
            Assert.AreEqual(result, ObjCache.DivByCode[divisionCode].IsMiniholding());
        }

        [TestCase("OCM", 1046)]
        [TestCase("AGRO", 527)]
        [TestCase("NAUKA", 0)]
        public void ManageCompanyIdDetectedForDivision(string divisionCode, int result)
        {
            Assert.AreEqual(result, ObjCache.DivByCode[divisionCode].GetManageCompanyId());
        }

        [Test]
        public void CanRetrieveManageCompany() {
            var uc = ObjCache.DivByCode["NAUKA"].GetManageCompany();
            Assert.Null(uc);
            uc = ObjCache.DivByCode["OCM"].GetManageCompany();
            Assert.NotNull(uc);
            Assert.AreEqual(1046,uc.Id);
        }

        class StubRR : IRoleResolver
        {


            public bool IsInRole(IPrincipal principal, string role, bool exact = false, IMvcContext callcontext = null,
                                 object customcontext = null)
            {
                return role == MiniholdingHelper.AllMiniholdingRole;
            }

            public bool IsInRole(string username, string role, bool exact = false, IMvcContext callcontext = null,
                                 object customcontext = null)
            {
                return role == MiniholdingHelper.AllMiniholdingRole;
            }

            public IEnumerable<string> GetRoles(IPrincipal principal, IMvcContext callcontext = null, object customcontext = null)
            {
                yield return MiniholdingHelper.AllMiniholdingRole;
            }
        }
        /// <summary>
        /// Проверяем работу роли "весь минихолдинг"
        /// </summary>
        /// <returns></returns>
        [Test]
        public void CanCheckAllMiniholdingRole() {
            var currentRoles = Application.Current.Roles;
            var currentHook = NativeZetaReader.DebugHook;
            try {
                 Application.Current.Roles  = new StubRR();
                NativeZetaReader.DebugHook = OnNativeZetaHook;
                var user = new GenericPrincipal(new GenericIdentity("best\\test"), null);
                Assert.True(ObjCache.Get(1046).Authorize(user));
                Assert.True(ObjCache.Get(415).Authorize(user));
                Assert.True(ObjCache.Get(468).Authorize(user));
                Assert.False(ObjCache.Get(352).Authorize(user));

            }
            finally {
                Application.Current.Roles = currentRoles;
                NativeZetaReader.DebugHook = currentHook;

            }

        }

        private static IEnumerable OnNativeZetaHook(string _)
        {
            if (_.Contains("Login"))
            {
                yield return new User { Object = ObjCache.Get(1046) };
            }
        }
    }
}
