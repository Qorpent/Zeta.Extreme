#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IStateManager.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.BizProcess.StateManagement {
	/// <summary>
	/// 	Интерфейс класса управления статусами
	/// </summary>
	public interface IStateManager {
		/// <summary>
		/// 	Признак возможности установить статус
		/// </summary>
		/// <param name="template"> </param>
		/// <param name="obj"> </param>
		/// <param name="detail"> </param>
		/// <param name="state"> </param>
		/// <returns> </returns>
		bool CanSet(IInputTemplate template, IZetaMainObject obj, IZetaDetailObject detail, string state);

		/// <summary>
		/// 	ВЫполнить установку статуса
		/// </summary>
		/// <param name="template"> </param>
		/// <param name="obj"> </param>
		/// <param name="detail"> </param>
		/// <param name="state"> </param>
		/// <param name="parent"> </param>
		void Process(IInputTemplate template, IZetaMainObject obj, IZetaDetailObject detail, string state, int parent);

		/// <summary>
		/// 	Найти зависимые формы
		/// </summary>
		/// <param name="source"> </param>
		/// <returns> </returns>
		IInputTemplate[] GetDependentTemplates(IInputTemplate source);

		/// <summary>
		/// 	Найти формы от которых зависит текущая
		/// </summary>
		/// <param name="target"> </param>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		IInputTemplate[] GetSourceTemplates(IInputTemplate target, IZetaMainObject obj);

		/// <summary>
		/// 	Определить главную форму
		/// </summary>
		/// <param name="safer"> </param>
		/// <returns> </returns>
		IInputTemplate GetMainTemplate(IInputTemplate safer);

		/// <summary>
		/// 	Определеить форму-сейфер
		/// </summary>
		/// <param name="main"> </param>
		/// <returns> </returns>
		IInputTemplate GetSaferTemplate(IInputTemplate main);

		/// <summary>
		/// 	Определяет возможность установить статус
		/// </summary>
		/// <param name="template"> </param>
		/// <param name="obj"> </param>
		/// <param name="detail"> </param>
		/// <param name="state"> </param>
		/// <param name="cause"> </param>
		/// <returns> </returns>
		bool CanSet(IInputTemplate template, IZetaMainObject obj, IZetaDetailObject detail, string state,
		            out string cause);

		/// <summary>
		/// 	Выполнить установку статуса
		/// </summary>
		/// <param name="objid"> </param>
		/// <param name="year"> </param>
		/// <param name="period"> </param>
		/// <param name="template"> </param>
		/// <param name="templatecode"> </param>
		/// <param name="usr"> </param>
		/// <param name="state"> </param>
		/// <param name="comment"> </param>
		/// <param name="parent"> </param>
		/// <returns> </returns>
		int DoSet(int objid, int year, int period, string template, string templatecode, string usr, string state,
		          string comment, int parent);

		/// <summary>
		/// 	Выполнить установку статуса
		/// </summary>
		/// <param name="objid"> </param>
		/// <param name="year"> </param>
		/// <param name="period"> </param>
		/// <param name="template"> </param>
		/// <returns> </returns>
		string DoGet(int objid, int year, int period, string template);

		/// <summary>
		/// 	Получить статус периода
		/// </summary>
		/// <param name="year"> </param>
		/// <param name="period"> </param>
		/// <returns> </returns>
		int GetPeriodState(int year, int period);

		/// <summary>
		/// 	Еще один вариант проверки возможности установки статуса
		/// </summary>
		/// <param name="template"> </param>
		/// <param name="obj"> </param>
		/// <param name="detail"> </param>
		/// <param name="state"> </param>
		/// <param name="cause"> </param>
		/// <param name="parent"> </param>
		/// <returns> </returns>
		bool CanSet(IInputTemplate template, IZetaMainObject obj, IZetaDetailObject detail, string state,
		            out string cause, int parent);
	}
}