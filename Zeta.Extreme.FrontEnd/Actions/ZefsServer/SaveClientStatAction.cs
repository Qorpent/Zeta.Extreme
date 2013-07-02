using Qorpent.Mvc;
using Qorpent.Mvc.Binding;

namespace Zeta.Extreme.FrontEnd.Actions.ZefsServer {

    [Action("zefs.saveclientstat", Role = "DEFAULT")]
    class SaveClientStatAction : ActionBase {
        [Bind(Required = true)]
        protected string Stat;

        protected override object MainProcess() {
            var s = Container.Get<IClientStatStorage>();
            if (null != s)
            {
                s.Write(Stat);
                return true;
            }
            return false;
        }
    }
}
