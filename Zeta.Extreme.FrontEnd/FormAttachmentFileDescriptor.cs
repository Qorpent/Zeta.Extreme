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
// PROJECT ORIGIN: Zeta.Extreme.FrontEnd/FormAttachmentFileDescriptor.cs
#endregion
using System;
using System.IO;
using Qorpent.Mvc;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// 	������� ����� � ���� ����������� ����� ��� ��������
	/// </summary>
	public class FormAttachmentFileDescriptor : IFileDescriptor {
		/// <summary>
		/// 	������� ������� � ��������������
		/// </summary>
		/// <param name="attach"> </param>
		/// <param name="storage"> </param>
		public FormAttachmentFileDescriptor(FormAttachment attach, IFormAttachmentStorage storage) {
			_attach = attach;
			_storage = storage;
		}

		/// <summary>
		/// 	���� (������ �����), ������� ���������� ��� �������
		/// </summary>
		public string Role {
			get { return ""; }
			set { }
		}

		/// <summary>
		/// 	��� ������� �������� � �����
		/// </summary>
		public string Name {
			get {
				var srcname = _attach.Name.Replace("\"", "_");
				var ext = Path.GetExtension(srcname);
				if (ext.Length <= 1 || ext.Length > 4) {
					return (srcname + "." + _attach.Extension).Replace("..", ".");
				}
				return srcname;
			}
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// 	���������� �����
		/// </summary>
		public string Content {
			get { return null; }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// 	Mime-Type �����
		/// </summary>
		public string MimeType {
			get { return _attach.MimeType; }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// 	����� �����
		/// </summary>
		public int Length {
			get { return (int) _attach.Size; }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// 	����� ���������� ���������
		/// </summary>
		public DateTime LastWriteTime {
			get { return _attach.Version; }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// 	������� ���� ��� ���� ������������ ����� ������������ �����
		/// </summary>
		public bool NeedDisposition {
			get { return true; }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// 	������� ���������� �����
		/// </summary>
		public bool IsStream {
			get { return true; }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// 	���������� ����� ������ �����
		/// </summary>
		/// <returns> </returns>
		public Stream GetStream() {
			return _storage.Open(_attach, FileAccess.Read);
		}

		private readonly FormAttachment _attach;
		private readonly IFormAttachmentStorage _storage;
	}
}