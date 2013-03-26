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
// PROJECT ORIGIN: Zeta.Extreme.Model/IZetaCell.cs
#endregion
using System;
using Qorpent.Model;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// 	���������� ������ ����
	/// </summary>
	public interface IZetaCell :
		IWithUser, IWithId, IWithVersion, IWithCurrency {
		/// <summary>
		/// ������
		/// </summary>
		IZetaRow Row { get; set; }
		/// <summary>
		/// �������
		/// </summary>
		IZetaColumn Column { get; set; }
		/// <summary>
		/// ������� ������
		/// </summary>
		IZetaMainObject Object { get; set; }
		/// <summary>
		/// ������� ������
		/// </summary>
		IZetaDetailObject Detail { get; set; }
		/// <summary>
		/// ����������
		/// </summary>
		IZetaMainObject Contragent { get; set; }
		
		/// <summary>
		/// ������
		/// </summary>
		int Period { get; set; }
		/// <summary>
		/// ���
		/// </summary>
		int Year { get; set; }
		/// <summary>
		/// ������ ����
		/// </summary>
		DateTime DirectDate { get; set; }
		
		/// <summary>
		/// �� �����������
		/// </summary>
		int? ContragentId { get; set; }
		/// <summary>
		/// �� ������
		/// </summary>
		int RowId { get; set; }
		/// <summary>
		/// �� �������
		/// </summary>
		int ColumnId { get; set; }
		/// <summary>
		/// �� �������
		/// </summary>
		int ObjectId { get; set; }
		/// <summary>
		/// �� ������
		/// </summary>
		int? DetailId { get; set; }

		/// <summary>
		/// ��������� ��������
		/// </summary>
		decimal NumericValue { get; set; }
		/// <summary>
		/// ��������� ��������
		/// </summary>
		string StringValue { get; set; }
		
		}
}