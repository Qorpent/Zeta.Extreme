using System;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.MetaStorage {
	/// <summary>
	/// 
	/// </summary>
	public class TreeExporter:ITreeExporter {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="format"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public static ITreeExporter Create(ExportTreeFormat format) {
			throw new NotImplementedException();
		}

		public string ProcessExport(IZetaRow exportroot, bool rootmode) {
			throw new NotImplementedException();
		}
	}
}