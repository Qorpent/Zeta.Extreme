using System.Collections.Generic;

namespace Zeta.Extreme.Model.Security
{
	/// <summary>
	/// Низкоуровневый менеджер 
	/// </summary>
	public class NativeZetaSecurityStorage:ISecurityStorage
	{
		public const string CreateUserQueryTemplate = ""

		/// <summary>
		/// 	Создает нового пользователя с указанным контекстом по умолчанию
		/// </summary>
		/// <param name="login"> Логин </param>
		/// <param name="objectContextId"> Предприятия - хост карты по умолчанию </param>
		public User CreateUser(string login) {
			if(CheckUser(login))throw new ZetaSecurityException("try to recreate existed user");

		}

		/// <summary>
		/// 	Получение списка всех возможных ролей
		/// </summary>
		/// <returns> 1. только SYS_SECURITYMASTER может создавать учетки без контекстного предприятия 2. ORGADMIN в переданном контексте может создавать учетные записи (но только для своего предприятия) 3. повторно создать пользователя нельзя 4. для ORGADMIN дополнительно производится контроль маски учетной записи (UserCard.ObjRole пользователя ZETASYS_INTERNAL\TEMPLATE) 5. если шаблона для предприятия еще нет - ORGADMIN не срабатывает </returns>
		public IEnumerable<RolePolicy> GetRoles() {
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// 	Assigns or un-assigns roles of given user
		/// </summary>
		/// <param name="login"> User for which role must be changed </param>
		/// <param name="roles"> roles array to be changed </param>
		/// <param name="assign"> assign role if true and unassign if false </param>
		/// <param name="objectId"> target object context </param>
		/// <returns> true if something changed </returns>
		public bool AssignRoles(string login, string[] roles, bool assign, int objectId) {
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// 	Выставляет или убирает признак защищенности у учетной записи
		/// </summary>
		/// <param name="login"> </param>
		/// <param name="protect"> </param>
		/// <returns> true if changed </returns>
		public bool SetProtection(string login, bool protect) {
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// 	Активирует или деактивирует пользователя
		/// </summary>
		/// <param name="login"> </param>
		/// <param name="active"> </param>
		/// <param name="objid"> </param>
		/// ///
		/// <returns> true if changed </returns>
		public bool SetActivity(string login, bool active, int objid) {
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// 	Устанавливает некритические свойства паспорта пользователя
		/// </summary>
		/// <param name="login"> Учетная запись </param>
		/// <param name="name"> ФИО </param>
		/// <param name="comment"> комментарий </param>
		/// <param name="contact"> контактная информация </param>
		/// <param name="idx"> индекс </param>
		/// <param name="tag"> tag (только для sys) </param>
		/// <returns> </returns>
		public User SetUserInfo(string login, string name = "", string comment = "", string contact = "", int idx = 0, string tag = "") {
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// 	Возвращает описание пользователя по указанному логину
		/// </summary>
		/// <param name="login"> </param>
		/// <returns> </returns>
		public User GetUser(string login) {
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// 	Возвращает True - если пользователь существует
		/// </summary>
		/// <param name="login"> </param>
		/// <returns> </returns>
		public bool CheckUser(string login) {
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// 	Добавляет карту к пользователю
		/// </summary>
		/// <param name="login"> </param>
		/// <param name="objid"> </param>
		/// <param name="primary"> </param>
		public AccountCard AddCard(string login, int objid, bool primary) {
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// 	Установить дополнительные свойства карты
		/// </summary>
		/// <param name="login"> </param>
		/// <param name="objectId"> </param>
		/// <param name="objectrole"> </param>
		/// <param name="tag"> </param>
		/// <returns> </returns>
		public AccountCard SetCardInfo(string login, int objectId, string objectrole = "", string tag = "") {
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Осуществляет поиск пользователей в реестре
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public IEnumerable<User> FindUsers(string query) {
			throw new System.NotImplementedException();
		}
	}
}
