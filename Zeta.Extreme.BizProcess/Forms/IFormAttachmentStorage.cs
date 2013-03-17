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
// PROJECT ORIGIN: Zeta.Extreme.BizProcess/IFormAttachmentStorage.cs
#endregion
using System.Collections.Generic;
using System.IO;


namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// ��������� ��������� ����������� ������ ����
	/// </summary>
	public interface IFormAttachmentStorage {
		/// <summary>
		/// ����� ��� �������������� �����, �������������� � ������ �������
		/// </summary>
		/// <returns></returns>
		IEnumerable<FormAttachment> GetAttachments(IFormSession session);

		/// <summary>
		/// ��������� ���������, ��������� � ������� �������
		/// </summary>
		/// <param name="session">������� ������ </param>
		/// <param name="attachment">�������������� �������</param>
		FormAttachment SaveAttachment(IFormSession session, Attachment attachment);
		/// <summary>
		/// ������� �������������� �������
		/// </summary>
		/// <param name="attachment"></param>
		void Delete(FormAttachment attachment);

		/// <summary>
		/// ��������� ����� �� ������ ��������
		/// </summary>
		/// <param name="attachment"></param>
		/// <param name="mode">����� �������� ����� </param>
		/// <returns></returns>
		Stream Open(FormAttachment attachment, FileAccess mode);
	}
}