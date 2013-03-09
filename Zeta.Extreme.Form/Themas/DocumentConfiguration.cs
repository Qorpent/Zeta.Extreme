#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : DocumentConfiguration.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Themas;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	Конфигуратор документов темы
	/// </summary>
	public class DocumentConfiguration : ItemConfigurationBase<IDocument> {
		/// <summary>
		/// 	Команда на конфигурирование документа
		/// </summary>
		/// <returns> </returns>
		public override IDocument Configure() {
			return new Document().BindFrom(this);
		}
	}
}