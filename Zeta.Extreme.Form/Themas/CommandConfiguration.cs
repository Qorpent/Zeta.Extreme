#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : CommandConfiguration.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Themas;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	������������ �������
	/// </summary>
	public class CommandConfiguration : ItemConfigurationBase<ICommand> {
		/// <summary>
		/// 	������� �� ���������������� �������
		/// </summary>
		/// <returns> </returns>
		public override ICommand Configure() {
			return new Command().BindFrom(this);
		}
	}
}