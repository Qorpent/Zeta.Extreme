using System;
using NUnit.Framework;
using Qorpent.Serialization;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.Core.Tests.CoreTests;
using Zeta.Extreme.Form.InputTemplates;
using Zeta.Extreme.FrontEnd.Actions.Attachments;
using Zeta.Extreme.Model.PocoClasses;

namespace Zeta.Extreme.FrontEnd.Tests {
	[TestFixture]
	public class AttachmentBugTests:SessionTestBase
	{
		[Test]
		public void ZC405_Session_Must_Return_Valid_Attach_List() {

			var session = new FormSession(new InputTemplate {Code = "balans2011A.in"}, 2012, 13, new Obj {Id = 352});
			var attachs = session.GetAttachedFiles();
			Assert.AreEqual(2,attachs.Length);

		}

		[Test]
		public void ZC405_Check_Attachments_With_Actions() {
			var action = new GetAttachmentListAction();
			action.MySession = new FormSession(new InputTemplate { Code = "balans2011A.in" }, 2012, 13, new Obj { Id = 352 });
			var result =(FormAttachment[]) action.Process();
			Assert.AreEqual(2, result.Length);
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