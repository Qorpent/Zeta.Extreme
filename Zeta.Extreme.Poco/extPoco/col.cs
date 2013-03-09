#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : col.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using System.Linq;
using Comdiv.Application;
using Comdiv.Olap.Model;
using Comdiv.Zeta.Model;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.Poco {
	public partial class col : IZetaColumn, IZetaQueryDimension {
		public virtual IList<IMarkLinkBase> GetMarkLinks() {
			if (null == _markLinks) {
				if (0 != Id && (null != myapp.storage.Get<col>(false))) {
					myapp.storage.Get<col>().Refresh(this);
				}
			}
			if (null == _markLinks) {
				_markLinks = MarkLinks;
			}
			return MarkLinks.OfType<IMarkLinkBase>().ToList();
		}

		public virtual void RemoveMark(IMark mark) {
			var todel = MarkLinks.FirstOrDefault(i => i.Mark.Id == mark.Id);
			if (null != todel) {
				MarkLinks.Remove(todel);
			}
		}

		public virtual bool IsMarkSeted(string code) {
			return WithMarksExtension.IsMarkSeted(this, code);
		}

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
	}
}