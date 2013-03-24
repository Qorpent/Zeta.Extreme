#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Core/ObjHandler.cs
#endregion
using System;
using System.Linq;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme {
	/// <summary>
	/// 	�������� ������� �� ������
	/// </summary>
	public sealed class ObjHandler : CachedItemHandlerBase<IZetaObject>, IObjHandler {
		/// <summary>
		/// 
		/// </summary>
		public ObjHandler() {
			DetailMode = DetailMode.None;
			Type = ZoneType.Obj;


		}

		/// <summary>
		/// 	��� ����
		/// </summary>
		/// <exception cref="Exception">cannot set type on natived zones</exception>
		public ZoneType Type {
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
		/// 	����� ������ � �������� �� ������ ��������� ��������, �� ��������� NONE - ����� �������� �� ��������
		/// </summary>
		public DetailMode DetailMode { get; set; }

		/// <summary>
		/// 	������ ��� ������� �������� ��� ���� ���� � �����������
		/// </summary>
		public bool IsForObj {
			get { return Type == ZoneType.Obj; }
		}

		/// <summary>
		/// 	������ ��� ������� �������� ��� ���� ���� �� � �����������
		/// </summary>
		public bool IsNotForObj {
			get { return Type != ZoneType.Obj; }
		}

		/// <summary>
		/// 	������ �� �������� ��������� �������� �������
		/// </summary>
		public IZetaMainObject ObjRef {
			get { return Native as IZetaMainObject; }
		}
		/// <summary>
		/// ������ ������� �� ������������
		/// </summary>
		/// <remarks>ZC-248 �����-����������� ���������� </remarks>
		public string AltObjFilter {
			get { return _altObjFilter; }
			set {
				_altObjFilter = value;
				InvalidateCacheKey();
			}
		}

		/// <summary>
		/// 	������� ����� ����
		/// </summary>
		/// <returns> </returns>
		public IObjHandler Copy() {
			return MemberwiseClone() as ObjHandler;
		}

		/// <summary>
		/// 	����������� ������ ����
		/// </summary>
		/// <param name="session"> </param>
		/// <exception cref="NotImplementedException"></exception>
		public override void Normalize(ISession session) {
			if (Type == ZoneType.None) {
				Type = ZoneType.Obj;
			}
			NormalizeNative(session);
			NormalizeAltObjFilter(session);
		}

		private void NormalizeAltObjFilter(ISession session) {
			if (!string.IsNullOrWhiteSpace(AltObjFilter)) {
				var cache = session.GetMetaCache();
				var ids = AltObjFilter.SmartSplit().SelectMany(cache.ResolveZoneAliasToObjectIds).Distinct().OrderBy(_ => _);
				AltObjFilter = string.Join(",", ids);
			}
		}

		private void NormalizeNative(ISession session) {
			if (IsStandaloneSingletonDefinition()) {
				var cache = session == null ? MetaCache.Default : session.GetMetaCache();
				switch (Type) {
					case ZoneType.Obj:
						Native = cache.Get<IZetaMainObject>(GetEffectiveKey());
						break;
					case ZoneType.Div:
						Native = cache.Get<IZetaMainObject>(GetEffectiveKey());
						break;
					case ZoneType.Grp:
						Native = cache.Get<IZetaMainObject>(GetEffectiveKey());
						break;
				}
			}
		}

		/// <summary>
		/// 	��������� �������� �� �������� ��� ��������� �� Native
		/// </summary>
		public override void Apply(IZetaObject item) {
			_type = GetNativeType(item);
			base.Apply(item);
		}

		private ZoneType GetNativeType(IZetaObject native)
		{
			if (null != (native as IZetaMainObject)) {
				return ZoneType.Obj;
			}
			if (null != (native as IZetaDetailObject)) {
				return ZoneType.Detail;
			}
			if (null != (native as IZetaObjectGroup)) {
				return ZoneType.Grp;
			}
			if (null != (native as IObjectDivision)) {
				return ZoneType.Div;
			}
			return ZoneType.Unknown;
		}

		/// <summary>
		/// 	������� ����������������� ���������� ������� ������
		/// </summary>
		/// <returns> </returns>
		protected override string EvalCacheKey() {
			var prefix = (int) Type + "::";
			if (Type != ZoneType.Detail && DetailMode != DetailMode.None)
			{
				prefix += "d:" + (int) DetailMode + "/";
			}
			if (!string.IsNullOrWhiteSpace(AltObjFilter)) {
				prefix += AltObjFilter + "/";
			}
			return prefix + base.EvalCacheKey();
		}

		/// <summary>
		/// 	���������� ��� ������� ��������� ����, ������������ ��������� �������
		/// 	��� ��� �������� Native, � �������� ��� ����������� ���
		/// </summary>
		/// <returns> </returns>
		public override bool IsStandaloneSingletonDefinition(bool nativefalse = true) {
			var result = base.IsStandaloneSingletonDefinition(nativefalse);
			if (result) {
				if (Type == ZoneType.None)
				{
					return false;
				}
				if (Type == ZoneType.Unknown)
				{
					return false;
				}
			}
			return result;
		}


		private ZoneType _type;
		private string _altObjFilter;
	}
}