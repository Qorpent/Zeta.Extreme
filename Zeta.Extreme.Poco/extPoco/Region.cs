#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : Region.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Poco {
	public partial class region : IZetaRegion {
/*
        public virtual int CountMunicipals(){
            var crit = DetachedCriteria.For(typeof (IZetaPoint))
                .SetProjection(Projections.Count("Id"))
                .Add(Restrictions.Eq("Region", this));
            return myapp.storage.Get<point>().First<IZetaPoint, int>(crit);
        }
 */
	}
}