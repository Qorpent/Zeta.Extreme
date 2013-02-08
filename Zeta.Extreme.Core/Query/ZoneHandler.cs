#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : ZoneHandler.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Comdiv.Olap.Model;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Описание условия на объект
	/// </summary>
	public sealed class ZoneHandler : CachedItemHandlerBase<IZoneElement>
	{
		private ZoneType _type;

		/// <summary>
		/// Тип зоны
		/// </summary>
		public ZoneType Type {
			get {
				if(null!=Native) {
					return GetNativeType(Native);
				}
				return _type;
			}
			set {
				if(null!=Native) {
					throw new Exception("cannot set type on natived zones");
				}
				if(value==_type) {
					return;
				}
				_type = value;
				InvalidateCacheKey();
			}
		}

		/// <summary>
		/// 	Применяет свойства от сущности без установки ее Native
		/// </summary>
		public override void Apply(IZoneElement item) {
			_type = GetNativeType(item);
			base.Apply(item);
			
		}

		private ZoneType GetNativeType(IZoneElement native) {
			if (null != (native as IZetaMainObject)) return ZoneType.Obj;
			if (null != (native as IZetaDetailObject)) return ZoneType.Detail;
			if (null != (native as IZetaObjectGroup)) return ZoneType.Grp;
			if (null != (native as IMainObjectGroup)) return ZoneType.Div;
			return ZoneType.Unknown;
		}

		/// <summary>
		/// 	Функция непосредственного вычисления кэшевой строки
		/// </summary>
		/// <returns> </returns>
		protected override string EvalCacheKey() {
			var prefix = Type.ToString() + "::";
			return prefix + base.EvalCacheKey();
		}

		/// <summary>
		/// Простая копия зоны
		/// </summary>
		/// <returns></returns>
		public ZoneHandler Copy() {
			return MemberwiseClone() as ZoneHandler;
		}
	}
}