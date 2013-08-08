using System;
using Qorpent;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Developer.MetaStorage;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.Actions {
	/// <summary>
	/// �������� �������� ������ ���� � ���� BXL (�� ��������� HQL-����������� ������)
	/// </summary>
	[Action("zdev.exporttree",Arm="dev",Help="��������� ����������� HQL-������ ������ �����",Role="DEVELOPER")]
	public class ExportTreeAction  : ActionBase {
		[Bind(
			Name = "format",
			Constraint = new object[] { ExportTreeFormat.BxlMeta,ExportTreeFormat.Hql,  },
			Help = "������ ������������� ����������� ������",
			Default = ExportTreeFormat.Hql
		)]
		private ExportTreeFormat _format = ExportTreeFormat.Hql;

		[Bind(
			Name = "root",
			Required=true,
			Default= "",
			Help = "�������� ������ ��� ��������"
		)] 
		private string _root = "";

		[Bind(
			Name = "exttoprimary",
			Required = false,
			Default = false,
			Help = "������������ ����������� ������� � ���������"
		)]
		private bool _exttoprimary = false;

		[Bind(
			Name = "codereplace", 
			ValidatePattern = @"^[^~]+~[^~]+$",
			Default = "",
			Help = "��������� ������� ���� ������ ~ ������ ��� �������������� ��������� �����"
		)] 
		private string _codereplace = "";

		[Bind(
			Name = "rootmode",
			Default = false,
			Help = "����� �������� � ���-������ ��� �������� �������� �������� �����"
		)] 
		private bool _rootmode = false;


		[Bind(
			Name = "exclude",
			Default = "",
			Help = "������ ������ ���������� ������ (� �� ���������, ���������� � ����� � ������)"
		)] 
		private string _excluderegex="";



		/// <summary>
		/// ��������� ������� ������ �� ������������ MetaCache � ���� �������
		/// </summary>
		/// <returns></returns>
		protected override object MainProcess() {
			var root = MetaCache.Default.Get<IZetaRow>(_root);
			if (null == root) throw new Exception("��� �������� ������");
			var exportroot = PerformFilter(root);
			var rowexporter = TreeExporter.Create(_format);
			var result = rowexporter.ProcessExport(exportroot,_rootmode);
			return result;
		}

		private IZetaRow PerformFilter(IZetaRow root) 
		{
			var filter = new ExportTreeFilter();
			filter.ExcludeRegex = _excluderegex;
			filter.ConvertExtToPrimary = _exttoprimary;
			if (!string.IsNullOrWhiteSpace(_codereplace)) {
				var pattern = _codereplace.Split('~')[0];
				var replace = _codereplace.Split('~')[1];
				filter.CodeReplacer = new ReplaceDescriptor {Pattern = pattern, Replacer = replace};
			}
			var result = filter.Execute(root);
			return result;
		}
	}
}