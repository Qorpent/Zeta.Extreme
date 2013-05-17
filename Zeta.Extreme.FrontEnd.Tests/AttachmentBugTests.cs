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
// PROJECT ORIGIN: Zeta.Extreme.FrontEnd.Tests/AttachmentBugTests.cs
#endregion
using System;
using NUnit.Framework;
using Qorpent.Serialization;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.Core.Tests.CoreTests;
using Zeta.Extreme.Form.InputTemplates;
using Zeta.Extreme.FrontEnd.Actions.Attachments;
using Zeta.Extreme.Model;

namespace Zeta.Extreme.FrontEnd.Tests {
	[TestFixture]
	public class AttachmentBugTests:SessionTestBase
	{
		[Test]
		public void ZC405_Session_Must_Return_Valid_Attach_List() {

			var session = new FormSession(new InputTemplate {Code = "balans2011A.in"}, 2012, 13, new Obj {Id = 352});
			var attachs = session.GetAttachedFiles();
			Assert.AreEqual(7,attachs.Length);

		}

		[Test]
		public void ZC405_Check_Attachments_With_Actions() {
			var action = new GetAttachmentListAction();
			action.MySession = new FormSession(new InputTemplate { Code = "balans2011A.in" }, 2012, 13, new Obj { Id = 352 });
			var result =(FormAttachment[]) action.Process();
			Assert.AreEqual(7, result.Length);
		}

		[Test]
		public void ZC405_Serialize_Result_Of_Action()
		{
			var action = new GetAttachmentListAction();
			action.MySession = new FormSession(new InputTemplate { Code = "balans2011A.in" }, 2012, 13, new Obj { Id = 352 });
			var result = (FormAttachment[])action.Process();
			var serialized = new JsonSerializer().Serialize("",result);
			Console.WriteLine(serialized);
		}
	}
}