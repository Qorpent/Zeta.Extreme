#region LICENSE

// Copyright 2007-2012 Comdiv (F. Sadykov) - http://code.google.com/u/fagim.sadykov/
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
// http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// Solution: Qorpent
// Original file : ISecurityManager.cs
// Project: Zeta.Data.Repository
// 
// ALL MODIFICATIONS MADE TO FILE MUST BE DOCUMENTED IN SVN

#endregion

using System.Collections.Generic;

namespace Zeta.Extreme.Model.Security {
	/// <summary>
	/// 	������ ������ � �������������� � ������������ � ��������� Zeta
	/// </summary>
	public interface ISecurityStorage {
		/// <summary>
		/// 	������� ������ ������������ � ��������� ���������� �� ���������
		/// </summary>
		/// <param name="login"> ����� </param>
		/// <param name="objectContextId"> ����������� - ���� ����� �� ��������� </param>
		/// <returns> 1. ������ SYS_SECURITYMASTER ����� ��������� ������ ��� ������������ ����������� 2. ORGADMIN � ���������� ��������� ����� ��������� ������� ������ (�� ������ ��� ������ �����������) 3. �������� ������� ������������ ������ 4. ��� ORGADMIN ������������� ������������ �������� ����� ������� ������ (UserCard.ObjRole ������������ ZETASYS_INTERNAL\TEMPLATE) 5. ���� ������� ��� ����������� ��� ��� - ORGADMIN �� ����������� </returns>
		User CreateUser(string login);

        /// <summary>
        /// 	��������� ������ ���� ��������� �����
        /// </summary>
        /// <returns> 1. ������ SYS_SECURITYMASTER ����� ��������� ������ ��� ������������ ����������� 2. ORGADMIN � ���������� ��������� ����� ��������� ������� ������ (�� ������ ��� ������ �����������) 3. �������� ������� ������������ ������ 4. ��� ORGADMIN ������������� ������������ �������� ����� ������� ������ (UserCard.ObjRole ������������ ZETASYS_INTERNAL\TEMPLATE) 5. ���� ������� ��� ����������� ��� ��� - ORGADMIN �� ����������� </returns>
        IEnumerable<RolePolicy> GetRoles();

	    /// <summary>
	    /// 	Assigns or un-assigns roles of given user
	    /// </summary>
	    /// <param name="login"> User for which role must be changed </param>
	    /// <param name="roles"> roles array to be changed </param>
	    /// <param name="assign"> assign role if true and unassign if false </param>
	    /// <param name="objectId"> target object context </param>
	    /// <returns> true if something changed </returns>
	    bool AssignRoles(string login, string[] roles, bool assign, int objectId);

		/// <summary>
		/// 	���������� ��� ������� ������� ������������ � ������� ������
		/// </summary>
		/// <param name="login"> </param>
		/// <param name="protect"> </param>
		/// <returns> true if changed </returns>
		bool SetProtection(string login, bool protect);

		/// <summary>
		/// 	���������� ��� ������������ ������������
		/// </summary>
		/// <param name="login"> </param>
		/// <param name="active"> </param>
		/// <param name="objid"> </param>
		/// ///
		/// <returns> true if changed </returns>
		bool SetActivity(string login, bool active, int objid);

		/// <summary>
		/// 	������������� ������������� �������� �������� ������������
		/// </summary>
		/// <param name="login"> ������� ������ </param>
		/// <param name="name"> ��� </param>
		/// <param name="comment"> ����������� </param>
		/// <param name="contact"> ���������� ���������� </param>
		/// <param name="idx"> ������ </param>
		/// <param name="tag"> tag (������ ��� sys) </param>
		/// <returns> </returns>
		User SetUserInfo(string login, string name = "", string comment = "", string contact = "", int idx = 0,
		                 string tag = "");

		/// <summary>
		/// 	���������� �������� ������������ �� ���������� ������
		/// </summary>
		/// <param name="login"> </param>
		/// <returns> </returns>
		User GetUser(string login);

		/// <summary>
		/// 	���������� True - ���� ������������ ����������
		/// </summary>
		/// <param name="login"> </param>
		/// <returns> </returns>
		bool CheckUser(string login);

		/// <summary>
		/// 	��������� ����� � ������������
		/// </summary>
		/// <param name="login"> </param>
		/// <param name="objid"> </param>
		/// <param name="primary"> </param>
		AccountCard AddCard(string login, int objid, bool primary);

		/// <summary>
		/// 	���������� �������������� �������� �����
		/// </summary>
		/// <param name="login"> </param>
		/// <param name="objectId"> </param>
		/// <param name="objectrole"> </param>
		/// <param name="tag"> </param>
		/// <returns> </returns>
		AccountCard SetCardInfo(string login, int objectId, string objectrole = "", string tag = "");

	    /// <summary>
	    /// ������������ ����� ������������� � �������
	    /// </summary>
	    /// <param name="query"></param>
	    /// <returns></returns>
	    IEnumerable<User> FindUsers(string query);
	}
}