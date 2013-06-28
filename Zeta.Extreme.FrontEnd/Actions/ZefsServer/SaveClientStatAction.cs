using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.MongoDB.Integration.FrontEndStat;

namespace Zeta.Extreme.FrontEnd.Actions.ZefsServer {

    [Action("zefs.saveclientstat", Role = "DEFAULT")]
    class SaveClientStatAction : ActionBase {
        [Bind(Required = true)]
        protected string Stat;

        protected override object MainProcess() {
            var s = Container.Get<IFrontEndStatStorage>("frontendstat.mongodb") ?? new FrontEndStatStorage();
            s.Write(Stat);

            return true;
        }
    }
}
