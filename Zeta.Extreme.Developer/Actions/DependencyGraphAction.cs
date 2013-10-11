using System.Linq;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Developer.MetaStorage.Tree.DependencyAnalyzer;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
	/// ������� ����������� ���� ��������
	/// </summary>
	[Action("zdev.dependencygraph", Arm = "dev", Help = "������������ ���� ������������ ������", Role = "DEVELOPER")]
	public class DependencyGraphAction : ActionBase
	{
		private DependencyGraphTask task;

		[Bind(Required = true,Help = "��������� �������� ������")]
		private string root { get; set; }
		[Bind(Default = DependencyDirection.Default, Help ="����������� ���������")]
		private DependencyDirection dir { get; set; }
		[Bind(Default = DependencyNodeType.Default, Help="���� ����� ��� ���������")]
		private DependencyNodeType nodetypes { get; set; }
		[Bind(Default = DependencyEdgeType.Default, Help = "���� ��������� ��� ���������")]
		private DependencyEdgeType edgetypes { get; set; }
		[Bind( Help = "���������� ������ � �������� �� ������")]
		private bool clusterize { get; set; }
		[Bind( Help="����� ������������ �����")]
		private string terminals { get; set; }
		[Bind(Help = "����� ������������ �����")]
		private string ignores { get; set; }
		[Bind(Help="������� ������� ���������")]
		private int depth { get; set; }
		[Bind(Help = "�������� �������")]
		private bool showlegend { get; set; }
		/// <summary>
		/// 
		/// </summary>
		protected override void Prepare()
		{
			base.Prepare();
			task = PrepareTask();
		}

		private DependencyGraphTask PrepareTask() {
			var task = new DependencyGraphTask {
				StartRow = MetaCache.Default.Get<IZetaRow>(root),
				Clusterize = clusterize,
				Depth= depth,
				Direction = dir,
				EdgeTypes = edgetypes,
				NodeTypes = nodetypes,
				IgnoreMasks = ignores.SmartSplit().ToArray(),
				TerminalMasks = terminals.SmartSplit().ToArray(),
				ShowLegend = showlegend,
                BaseUri = Context.Uri,
                CalculateEdgeInWeight = true
			};
			return task;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override object MainProcess() {
		    return task;
		}
	}
}