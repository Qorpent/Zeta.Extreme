using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// ������� ����������� ���� ��������
    /// </summary>
    [Action(DeveloperConstants.ExportThemastructureCommand, Arm = "dev", Help = "������������ ��������� ���� ��������� ���", Role = "DEVELOPER")]
    public class ExportThemaStructureAction : ExportActionBase<ThemaStructureExporter>
    {
        [Bind]
        bool BlockOnly { get; set; }
        [Bind]
        string SubsystemAliases { get; set; }
        /// <summary>
        /// �������� ���������� ����� �����
        /// </summary>
        [Bind]
        string ExcludeRoots { get; set; }

        /// <summary>
        /// ����� ���������� ������� �� �������� ����
        /// </summary>
        [Bind]
        bool DisableStatusFilter { get; set; }
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override ThemaStructureExporter InitializeExporter()
        {
            var result = base.InitializeExporter();
            result.BlocksOnly = BlockOnly;
            result.SubsystemAliases = SubsystemAliases;
            result.ExcludeRoots = ExcludeRoots;
            result.DisableStatusFilter = DisableStatusFilter;
            return result;
        }
    }
}