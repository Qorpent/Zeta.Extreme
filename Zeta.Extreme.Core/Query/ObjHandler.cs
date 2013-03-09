#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ObjHandler.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme {
	/// <summary>
	/// 	�������� ������� �� ������
	/// </summary>
	public sealed class ObjHandler : CachedItemHandlerBase<IZoneElement>, IObjHandler {
		/// <summary>
		/// 	��� ����
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
		/// ����� ������ � �������� �� ������ ��������� ��������, �� ��������� NONE - ����� �������� �� ��������
		/// </summary>
		public DetailMode DetailMode { get; set; }

		/// <summary>
		/// 	������ ��� ������� �������� ��� ���� ���� � �����������
		/// </summary>
		public bool IsForObj {
			get { return Type == ObjType.Obj; }
		}

		/// <summary>
		/// 	������ ��� ������� �������� ��� ���� ���� �� � �����������
		/// </summary>
		public bool IsNotForObj {
			get { return Type != ObjType.Obj; }
		}

		/// <summary>
		/// 	������ �� �������� ��������� �������� �������
		/// </summary>
		public IZetaMainObject ObjRef {
			get { return Native as IZetaMainObject; }
		}

		/// <summary>
		/// 	��������� �������� �� �������� ��� ��������� �� Native
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
		/// 	������� ����������������� ���������� ������� ������
		/// </summary>
		/// <returns> </returns>
		protected override string EvalCacheKey() {
			var prefix = (int)Type + "::";
			if(Type!=ObjType.Detail && DetailMode!=DetailMode.None) {
				prefix += "d:" + (int)DetailMode + "/";
			}
			return prefix + base.EvalCacheKey();
		}

		/// <summary>
		/// 	������� ����� ����
		/// </summary>
		/// <returns> </returns>
		public IObjHandler Copy() {
			return MemberwiseClone() as ObjHandler;
		}

		/// <summary>
		/// 	���������� ��� ������� ��������� ����, ������������ ��������� �������
		/// 	��� ��� �������� Native, � �������� ��� ����������� ���
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
		/// 	����������� ������ ����
		/// </summary>
		/// <param name="session"> </param>
		/// <exception cref="NotImplementedException"></exception>
		public override void Normalize(ISession session) {
			if (IsStandaloneSingletonDefinition()) {
				var cache = session == null ? MetaCache.Default : session.MetaCache;
				switch (Type) {
					case ObjType.Obj:
						Native = cache.Get<IZetaMainObject>(GetEffectiveKey());
						break;
					case ObjType.Div:
						Native = cache.Get<IZetaMainObject>(GetEffectiveKey());
						break;
					case ObjType.Grp:
						Native = cache.Get<IZetaMainObject>(GetEffectiveKey());
						break;
				}
			}

		}
		

		private ObjType _type;
	}
}