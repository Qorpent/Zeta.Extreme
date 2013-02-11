#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : ObjHandler.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Comdiv.Application;
using Comdiv.Olap.Model;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Описание условия на объект
	/// </summary>
	public sealed class ObjHandler : CachedItemHandlerBase<IZoneElement> {
		/// <summary>
		/// 	Тип зоны
		/// </summary>
		public ObjType Type {
			get {
				if (null != Native) {
					return GetNativeType(Native);
				}
				return _type;
			}
			set {
				if (null != Native) {
					throw new Exception("cannot set type on natived zones");
				}
				if (value == _type) {
					return;
				}
				_type = value;
				InvalidateCacheKey();
			}
		}

		/// <summary>
		/// Шоткат для быстрой проверки что речь идет о предприятии
		/// </summary>
		public bool IsForObj {
			get { return Type == ObjType.Obj; }
		}
		/// <summary>
		/// Шоткат для быстрой проверки что речь идет не о предприятии
		/// </summary>
		public bool IsNotForObj
		{
			get { return Type != ObjType.Obj; }
		}
		/// <summary>
		/// Ссылка на реальный экземпляр старшего объекта
		/// </summary>
		public IZetaMainObject ObjRef {
			get { return Native as IZetaMainObject; }
		}

		/// <summary>
		/// 	Применяет свойства от сущности без установки ее Native
		/// </summary>
		public override void Apply(IZoneElement item) {
			_type = GetNativeType(item);
			base.Apply(item);
		}

		private ObjType GetNativeType(IZoneElement native) {
			if (null != (native as IZetaMainObject)) {
				return ObjType.Obj;
			}
			if (null != (native as IZetaDetailObject)) {
				return ObjType.Detail;
			}
			if (null != (native as IZetaObjectGroup)) {
				return ObjType.Grp;
			}
			if (null != (native as IMainObjectGroup)) {
				return ObjType.Div;
			}
			return ObjType.Unknown;
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
		/// 	Простая копия зоны
		/// </summary>
		/// <returns> </returns>
		public ObjHandler Copy() {
			return MemberwiseClone() as ObjHandler;
		}

		/// <summary>
		/// 	Определяет что условие описывает одну, определенную инстанцию объекта
		/// 	еще без загрузки Native, у объектов еще проверяется тип
		/// </summary>
		/// <returns> </returns>
		public override bool IsStandaloneSingletonDefinition(bool nativefalse = true) {
			var result = base.IsStandaloneSingletonDefinition(nativefalse);
			if (result) {
				if (Type == ObjType.None) {
					return false;
				}
				if (Type == ObjType.Unknown) {
					return false;
				}
			}
			return result;
		}

		/// <summary>
		/// 	Нормализует объект зоны
		/// </summary>
		/// <param name="session"> </param>
		/// <exception cref="NotImplementedException"></exception>
		public void Normalize(ZexSession session) {
			if (IsStandaloneSingletonDefinition()) {
				switch (Type) {
					case ObjType.Obj:
						Native = myapp.storage.Get<IZetaMainObject>().Load(GetEffectiveKey());
						break;
					case ObjType.Div:
						Native = myapp.storage.Get<IZetaMainObject>().Load(GetEffectiveKey());
						break;
					case ObjType.Grp:
						Native = myapp.storage.Get<IZetaMainObject>().Load(GetEffectiveKey());
						break;
				}
			}
		}

		private ObjType _type;
	}
}