#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : Region.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using Zeta.Extreme.Model.Deprecated;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.PocoClasses {
	public partial class region : IZetaRegion {
		[Ref(ClassName = typeof (Zone))] public virtual IZetaZone Country { get; set; }

		[Map] public virtual Guid Uid { get; set; }

		public virtual string Tag { get; set; }

		[Many(ClassName = typeof (Point))] public virtual IList<IZetaPoint> Points { get; set; }

		public virtual IZetaZone Zone {
			get { return Country; }
			set { Country = value; }
		}

		[Map] public virtual int Id { get; set; }

		[Map] public virtual string Name { get; set; }

		[Map] public virtual string Code { get; set; }

		[Map] public virtual string Comment { get; set; }

		[Map] public virtual DateTime Version { get; set; }

		public virtual int Idx { get; set; }

		public virtual IList<IZetaMainObject> MainObjects {
			get {
				if (null == _mainobjects) {
					var result = new List<IZetaMainObject>();
					foreach (var town in Points) {
						foreach (var mainObject in town.MainObjects) {
							result.Add(mainObject);
						}
					}
					_mainobjects = result;
				}
				return _mainobjects;
			}
			set { _mainobjects = value; }
		}

		public virtual IList<IZetaDetailObject> DetailObjects {
			get {
				if (null == _detailobjects) {
					var result = new List<IZetaDetailObject>();
					foreach (var town in Points) {
						foreach (var mainObject in town.DetailObjects) {
							result.Add(mainObject);
						}
					}
					_detailobjects = result;
				}
				return _detailobjects;
			}
			set { _detailobjects = value; }
		}

		private IList<IZetaDetailObject> _detailobjects;
		private IList<IZetaMainObject> _mainobjects;
	}
}