using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// ������� ����������� ���� ��������
    /// </summary>
    [Action(DeveloperConstants.ExportObjectsCommand, Arm = "dev", Help = "������������ ��������� ���� ������� ��������", Role = "DEVELOPER")]
    public class ExportObjectsAction : ExportActionBase<ObjectExporter> {
        [Bind]
        private bool UseOutOrganization { get; set; }

        /// <summary>
        /// ������� ������������� ������� �����������
        /// </summary>
        [Bind]private bool OnlyOwnOnRoot { get; set; }

        /// <summary>
        /// �������������� ���������
        /// </summary>
        /// <returns></returns>
        protected override ObjectExporter InitializeExporter()
        {
            var result= base.InitializeExporter();
            result.UseOutOrganization = UseOutOrganization;
            result.OnlyOwnOnRoot = OnlyOwnOnRoot;
            return result;
        }
    }
}