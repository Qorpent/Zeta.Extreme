#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : col.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Linq;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Poco {
	public partial class col {
		public virtual string GetStaticMeasure(string format) {
			if (IsDynamicMeasure) {
				return "";
			}
			if (Measure.IsNotEmpty()) {
				if (format.IsNotEmpty()) {
					return string.Format(format, Measure);
				}
				return Measure;
			}
			return "";
		}

		public virtual string GetDynamicMeasure(IZetaRow source, string format) {
			if (!IsDynamicMeasure) {
				return "";
			}
			if (source.Measure.IsNotEmpty()) {
				if (format.IsNotEmpty()) {
					return string.Format(format, source.Measure);
				}
				return source.Measure;
			}
			return GetStaticMeasure(format);
		}

		public virtual bool IsMarkSeted(string code) {
			return WithMarksExtension.IsMarkSeted(this, code);
		}
	}
}