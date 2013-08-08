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
			Name = "detachroot",
			Default = false,
			Help = "����� �������� � ���-������ ��� �������� �������� �������� �����"
		)] 
		private bool _detachroot = false;

		[Bind(
			Name = "codemode",
			Default = TreeExporterCodeMode.Default,
			Help = "����� ��������� � �������� �����, ����� ������� ���� ��������� �� ��������"
		)]
		private TreeExporterCodeMode _codemode = TreeExporterCodeMode.Default;


		[Bind(
			Name = "exclude",
			Default = "",
			Help = "������ ������ ���������� ������ (� �� ���������, ���������� � ����� � ������)"
		)] 
		private string _excluderegex="";


		[Bind(
			Name = "resetindex",
			Default = false,
			Help = "���������� ���� ������� (� ����� � ����������� ���������� ������� ������� �����)"
		)]
		private bool _resetindex = false;

		[Bind(
			Name = "excludetags",
			Default = "",
			ValidatePattern = @"^(((grp)|(tag)|(mark)):[\w\d_\-]+[\s,]*)+$",
			Help = "��������� ��������� �������� �������� �����, ����� � �������"
		)]
		private string _excludetags = "";



		/// <summary>
		/// ��������� ������� ������ �� ������������ MetaCache � ���� �������
		/// </summary>
		/// <returns></returns>
		protected override object MainProcess() {
			var root = MetaCache.Default.Get<IZetaRow>(_root);
			if (null == root) throw new Exception("��� �������� ������");
			var exportroot = PerformFilter(root);
			var rowexporter = TreeExporter.Create(_format);
			var options = new TreeExporterOptions {DetachRoot = _detachroot, CodeMode = _codemode};
			var result = rowexporter.ProcessExport(exportroot,options);
			return result;
		}

		private IZetaRow PerformFilter(IZetaRow root) 
		{
			var filter = new ExportTreeFilter();
			filter.ExcludeRegex = _excluderegex;
			filter.ConvertExtToPrimary = _exttoprimary;
			filter.ResetAutoIndex = _resetindex;
			if (!string.IsNullOrWhiteSpace(_codereplace)) {
				var pattern = _codereplace.Split('~')[0];
				var replace = _codereplace.Split('~')[1];
				filter.CodeReplacer = new ReplaceDescriptor {Pattern = pattern, Replacer = replace};
			}
			if (!string.IsNullOrWhiteSpace(_excludetags)) {
				filter.ParseExcludes(_excludetags);
			}
			var result = filter.Execute(root);
			return result;
		}
	}
}