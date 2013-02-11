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
	/// 	�������� ������� �� ������
	/// </summary>
	public sealed class ObjHandler : CachedItemHandlerBase<IZoneElement> {
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
		/// ������ ��� ������� �������� ��� ���� ���� � �����������
		/// </summary>
		public bool IsForObj {
			get { return Type == ObjType.Obj; }
		}
		/// <summary>
		/// ������ ��� ������� �������� ��� ���� ���� �� � �����������
		/// </summary>
		public bool IsNotForObj
		{
			get { return Type != ObjType.Obj; }
		}
		/// <summary>
		/// ������ �� �������� ��������� �������� �������
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
			var prefix = Type.ToString() + "::";
			return prefix + base.EvalCacheKey();
		}

		/// <summary>
		/// 	������� ����� ����
		/// </summary>
		/// <returns> </returns>
		public ObjHandler Copy() {
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