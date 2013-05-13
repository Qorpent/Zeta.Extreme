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
// PROJECT ORIGIN: Zeta.Extreme.FrontEnd/CanLockStateAction.cs
#endregion

using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions.States {
	/// <summary>
	/// 	���������� ������ �����
	/// </summary>
	[Action("zefs.getlockstate")]
	public class GetLockStateAction : FormSessionActionBase
	{
		
		/// <summary>
		/// 	���������� ������ ����� �� ����������
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess()
		{
			return MySession.GetStateInfo();
		}
	}
}