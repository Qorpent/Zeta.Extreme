#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : Pkg.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using Comdiv.Model;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Poco {
	public class Pkg : IPkg {
		public virtual Guid Uid { get; set; }

		public virtual string Tag { get; set; }
		public virtual IList<IZetaCell> Cells { get; set; }

		public virtual string Code { get; set; }

		public virtual string Comment { get; set; }

		public virtual int Id { get; set; }

		public virtual string Name { get; set; }

		[Deprecated.Ref(Alias = "PkgType", Title = "Тип")] public virtual IPkgType Type { get; set; }

		public virtual DateTime Version { get; set; }

		[Deprecated.Ref(Alias = "Pkg", Title = "Родительский пакет", Nullable = true, IsAutoComplete = true,
			AutoCompleteType = "pkg")] public virtual IPkg Parent { get; set; }

		public virtual IList<IPkg> Children { get; set; }

		[Deprecated.Ref(Alias = "Org", Title = "Объект", Nullable = true, IsAutoComplete = true, AutoCompleteType = "org")] public virtual IZetaDetailObject DetailObject { get; set; }

		[Deprecated.Ref(Alias = "Subpart", Title = "Деталь", Nullable = true, IsAutoComplete = true, AutoCompleteType = "sp")] public virtual IZetaMainObject Object { get; set; }

		[Deprecated.Map(Title = "Состояние", CustomType = typeof (int))] public virtual PkgState State { get; set; }

		[Deprecated.Map(Title = "Дата")] public virtual DateTime Date { get; set; }

		[Deprecated.Map(Title = "Номер")] public virtual string Number { get; set; }


		[Deprecated.Map(Title = "Дата создания")] public virtual DateTime CreateTime { get; set; }

		[Deprecated.Map(Title = "Пользователь")] public virtual string Usr { get; set; }

		public virtual bool IsClosed() {
			if (State == PkgState.Closed) {
				return true;
			}
			if (Parent != null) {
				return Parent.IsClosed();
			}
			return false;
		}

		public override string ToString() {
			return string.Format("pkg:{0}/{1}/{2}/{3}/{4}", Type.Code(), Object.Code(),
			                     Date.ToString("yyyy-MM-dd"),
			                     Number, Code);
		}
	}
}