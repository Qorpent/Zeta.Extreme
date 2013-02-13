#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : IUsrThemaMapRepository.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	Интерфейс получения ответственных
	/// </summary>
	public interface IUsrThemaMapRepository {
		/// <summary>
		/// 	Первый ответственный
		/// </summary>
		/// <param name="thema"> </param>
		/// <param name="system"> </param>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		IUsrThemaMap GetResponsibility(string thema, string system, IZetaMainObject obj);

		/// <summary>
		/// 	Второй ответственный
		/// </summary>
		/// <param name="thema"> </param>
		/// <param name="system"> </param>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		IUsrThemaMap GetResponsibility2(string thema, string system, IZetaMainObject obj);

		/// <summary>
		/// 	Установить ответсвенноо
		/// </summary>
		/// <param name="thema"> </param>
		/// <param name="system"> </param>
		/// <param name="obj"> </param>
		/// <param name="usr"> </param>
		void SetResponsibility(string thema, string system, IZetaMainObject obj, IZetaUnderwriter usr);

		/// <summary>
		/// 	Установить второго ответственного
		/// </summary>
		/// <param name="thema"> </param>
		/// <param name="system"> </param>
		/// <param name="obj"> </param>
		/// <param name="usr"> </param>
		void SetResponsibility2(string thema, string system, IZetaMainObject obj, IZetaUnderwriter usr);
	}
}