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
	/// 	—ервис работы с пользовател€ми и полномочи€ми в контексте Zeta
	/// </summary>
	public interface ISecurityStorage {
		/// <summary>
		/// 	—оздает нового пользовател€ с указанным контекстом по умолчанию
		/// </summary>
		/// <param name="login"> Ћогин </param>
		/// <param name="objectContextId"> ѕредпри€ти€ - хост карты по умолчанию </param>
		/// <returns> 1. только SYS_SECURITYMASTER может создавать учетки без контекстного предпри€ти€ 2. ORGADMIN в переданном контексте может создавать учетные записи (но только дл€ своего предпри€ти€) 3. повторно создать пользовател€ нельз€ 4. дл€ ORGADMIN дополнительно производитс€ контроль маски учетной записи (UserCard.ObjRole пользовател€ ZETASYS_INTERNAL\TEMPLATE) 5. если шаблона дл€ предпри€ти€ еще нет - ORGADMIN не срабатывает </returns>
		User CreateUser(string login);

        /// <summary>
        /// 	ѕолучение списка всех возможных ролей
        /// </summary>
        /// <returns> 1. только SYS_SECURITYMASTER может создавать учетки без контекстного предпри€ти€ 2. ORGADMIN в переданном контексте может создавать учетные записи (но только дл€ своего предпри€ти€) 3. повторно создать пользовател€ нельз€ 4. дл€ ORGADMIN дополнительно производитс€ контроль маски учетной записи (UserCard.ObjRole пользовател€ ZETASYS_INTERNAL\TEMPLATE) 5. если шаблона дл€ предпри€ти€ еще нет - ORGADMIN не срабатывает </returns>
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
		/// 	¬ыставл€ет или убирает признак защищенности у учетной записи
		/// </summary>
		/// <param name="login"> </param>
		/// <param name="protect"> </param>
		/// <returns> true if changed </returns>
		bool SetProtection(string login, bool protect);

		/// <summary>
		/// 	јктивирует или деактивирует пользовател€
		/// </summary>
		/// <param name="login"> </param>
		/// <param name="active"> </param>
		/// <param name="objid"> </param>
		/// ///
		/// <returns> true if changed </returns>
		bool SetActivity(string login, bool active, int objid);

		/// <summary>
		/// 	”станавливает некритические свойства паспорта пользовател€
		/// </summary>
		/// <param name="login"> ”четна€ запись </param>
		/// <param name="name"> ‘»ќ </param>
		/// <param name="comment"> комментарий </param>
		/// <param name="contact"> контактна€ информаци€ </param>
		/// <param name="idx"> индекс </param>
		/// <param name="tag"> tag (только дл€ sys) </param>
		/// <returns> </returns>
		User SetUserInfo(string login, string name = "", string comment = "", string contact = "", int idx = 0,
		                 string tag = "");

		/// <summary>
		/// 	¬озвращает описание пользовател€ по указанному логину
		/// </summary>
		/// <param name="login"> </param>
		/// <returns> </returns>
		User GetUser(string login);

		/// <summary>
		/// 	¬озвращает True - если пользователь существует
		/// </summary>
		/// <param name="login"> </param>
		/// <returns> </returns>
		bool CheckUser(string login);

		/// <summary>
		/// 	ƒобавл€ет карту к пользователю
		/// </summary>
		/// <param name="login"> </param>
		/// <param name="objid"> </param>
		/// <param name="primary"> </param>
		AccountCard AddCard(string login, int objid, bool primary);

		/// <summary>
		/// 	”становить дополнительные свойства карты
		/// </summary>
		/// <param name="login"> </param>
		/// <param name="objectId"> </param>
		/// <param name="objectrole"> </param>
		/// <param name="tag"> </param>
		/// <returns> </returns>
		AccountCard SetCardInfo(string login, int objectId, string objectrole = "", string tag = "");

	    /// <summary>
	    /// ќсуществл€ет поиск пользователей в реестре
	    /// </summary>
	    /// <param name="query"></param>
	    /// <returns></returns>
	    IEnumerable<User> FindUsers(string query);
	}
}