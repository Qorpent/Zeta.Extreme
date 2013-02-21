#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IFormSessionDataSaver.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Zeta.Extreme.Form.SaveSupport {
	/// <summary>
	/// 	Интерфейс класса для сохранения данных форм
	/// </summary>
	public interface IFormSessionDataSaver {
		/// <summary>
		/// 	Текущая стадия процесса сохранения
		/// </summary>
		SaveStage Stage { get; set; }

		/// <summary>
		/// 	Последняя возникшая ошибка
		/// </summary>
		Exception Error { get; set; }

		/// <summary>
		/// 	Метод
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="savedata"> </param>
		/// <param name="currentUser"> </param>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		Task<SaveResult> BeginSave(IFormSession session, XElement savedata, IPrincipal currentUser);
	}
}