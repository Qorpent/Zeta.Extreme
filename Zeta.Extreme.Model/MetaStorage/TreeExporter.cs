using System;
using System.IO;
using System.Linq;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.MetaStorage {
	/// <summary>
	/// 
	/// </summary>
	public abstract class TreeExporter:ITreeExporter {
		/// <summary>
		/// ������, ������������� ������
		/// </summary>
		protected StringWriter Buffer;
		/// <summary>
		/// ������� ������� ���������
		/// </summary>
		protected int Level;
		/// <summary>
		/// �������� ������ ��� ��������
		/// </summary>
		protected IZetaRow Root;
		/// <summary>
		/// ����� ��������� � ������� ������ (��� ��������)
		/// </summary>
		protected bool Rootmode;
		/// <summary>
		/// ������� ������ �� ���������
		/// </summary>
		protected IZetaRow Current;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="format"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public static ITreeExporter Create(ExportTreeFormat format) {
			if (format == ExportTreeFormat.Hql) {
				return new HqlTreeExporter();
			}
			throw new Exception("format not supported for now " + format);
		}



		public string ProcessExport(IZetaRow root, bool rootmode) {
			lock (this) {
				Level = 0;
				Buffer = new StringWriter();
				Root = root;
				Rootmode = rootmode;
				WriteHeader();
				WriteStartScript();
				WriteRow(root);
				WriteEndScript();
				WriteFooter();
				Buffer.Flush();
				return Buffer.ToString();
			}
		}
		/// <summary>
		/// ����� ������ ����� ������� ��������
		/// </summary>
		protected virtual void WriteHeader() {}
		/// <summary>
		/// ������ ������� (���������������� ��������)
		/// </summary>
		protected virtual void WriteStartScript() { }
		/// <summary>
		/// ���������� ������� (�������)
		/// </summary>
		protected virtual void WriteEndScript() { }
		/// <summary>
		/// ������ ������������ �����������
		/// </summary>
		protected virtual void WriteFooter() { }
		/// <summary>
		/// ��������� ���� ������ ������
		/// </summary>
		protected virtual void WritePreRow() { }
		/// <summary>
		/// ������ ���� ������
		/// </summary>
		protected virtual void WriteRowBody() { }
		/// <summary>
		/// ������ ��������� ������
		/// </summary>
		protected virtual void WritePostRow() { }
		private void WriteCurrentRow() {
			WritePreRow();
			WriteRowBody();
			WritePostRow();
		}

		private void WriteRow(IZetaRow row) {
			Current = row;
			WriteCurrentRow();
			if (Current.HasChildren()) {
				Level++;
				var children = Current.Children.ToArray();
				foreach (var c in children) {
					WriteRow(c);
				}
				Level--;
			}
		}
	}
}