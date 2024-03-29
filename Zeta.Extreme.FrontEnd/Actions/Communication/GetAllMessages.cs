using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.Form.Themas;

namespace Zeta.Extreme.FrontEnd.Actions.Communication {
    /// <summary>
    ///     ��������, ������������ ���������� ������� ��������� ����������
    /// </summary>
    [Action("zecl.get", Role = "DEFAULT")]
    public class GetAllMessages : ChatProviderActionBase {
        /// <summary>
        /// ��������� ����
        /// </summary>
        [Bind]
        public DateTime From { get; set; }

        /// <summary>
        /// ������� ������ ��������������
        /// </summary>
        [Bind]
        public bool ShowArchived { get; set; }

        /// <summary>
        /// ������ ����� ���������
        /// </summary>
        [Bind]
        public string TypeFilter { get; set; }

        /// <summary>
        ///     processing of execution - main method of action
        /// </summary>
        /// <returns>
        /// </returns>
        protected override object MainProcess() {
            var lastread = _provider.GetLastRead(Context.User.Identity.Name);
            var items = new FormChatItem[] { };

            if (IsInRole("BUDGET")) {
                var myobjs = GetMyOwnObjects();
                if (0 != myobjs.Length) {
                    items = _provider.FindAll(
                        Context.User.Identity.Name, From, myobjs, new[] { "objcurrator", "locks" }, null, ShowArchived).OrderByDescending(_ => _.Time).ToArray();
                }

                var curforms = ((ExtremeFormProvider)FormServer.Default.FormProvider).Factory.GetAll().Where(
                    _ => _.GetParameter("hold.responsibility", "") == Context.User.Identity.Name.ToLower()
                    ).SelectMany(_ => new[] { _.Code + "A.in", _.Code + "B.in" }).ToArray();

                if (curforms.Length != 0) {
                    var curformitems = _provider.FindAll(Context.User.Identity.Name, From, null,
                                                         new[] { "formcurrator" }, curforms.ToArray(), ShowArchived).OrderByDescending(_ => _.Time).ToArray();
                    if (curformitems.Any()) {
                        items = items.Union(curformitems).OrderByDescending(_ => _.Time).ToArray();
                    }
                }
            } else if (!IsInRole("ADMIN") && !IsInRole("SYS_ALLOBJECTS")) {
                var myobjs = GetMyAccesibleObjects();
                var myforms = GetMyFormCodes();
                items = _provider.FindAll(Context.User.Identity.Name, From, myobjs, myforms, null, ShowArchived).OrderByDescending(_ => _.Time).ToArray();
            }

            if (IsInRole("SYS_NOCONTROLPOINTS", true)) {
                var objs = (IsInRole("ADMIN")) ? null : GetMyOwnObjects();
                var supportitems = _provider.FindAll(
                    Context.User.Identity.Name, From, objs, new[] { "locks" }, null, ShowArchived
                ).OrderByDescending(
                    _ => _.Time
                ).ToArray();

                if (supportitems.Any()) {
                    items = items.Union(supportitems.Where(_ => !items.Any(__ => __.Id == _.Id))).OrderByDescending(_ => _.Time).ToArray();
                }
            }

            if (IsInRole("SUPPORT", true)) {
                var supportitems = _provider.FindAll(Context.User.Identity.Name, From, null,
                                                     new[] { "support" }, null, ShowArchived).OrderByDescending(_ => _.Time).ToArray();
                if (supportitems.Any()) {
                    items = items.Union(supportitems).OrderByDescending(_ => _.Time).ToArray();
                }

            }

            if (IsInRole("ADMIN")) {
                var supportitems = _provider.FindAll(Context.User.Identity.Name, From, null,
                                                     new[] { "admin" }, null, ShowArchived).OrderByDescending(_ => _.Time).ToArray();
                if (supportitems.Any()) {
                    items = items.Union(supportitems).OrderByDescending(_ => _.Time).ToArray();
                }

            }



            foreach (var chatitem in items) {
                if (chatitem.Time > lastread) {
                    chatitem.Userdata["isnew"] = true;
                }
            }
            return items;
        }
    }
}