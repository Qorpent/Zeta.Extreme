using System;
using System.Linq;
using NUnit.Framework;
using Qorpent.IoC;
using Zeta.Extreme.BizProcess.StateManagement;
using Zeta.Extreme.Form.InputTemplates;
using Zeta.Extreme.FrontEnd;
using Zeta.Extreme.Model;

namespace Zeta.Extreme.BizProcess.Tests.StateManagement
{

	[TestFixture]
	public class DefaultStateManagerTest
	{
		private DefaultStateManager manager;
		private InputTemplate template;
		private FormSession session;

		[SetUp]
		public void Setup() {
			var _c = new Container();
			_c.Register(_c.NewComponent<IFormStateAvailabilityChecker,FakeStateChecker>());
			_c.Register(_c.NewComponent<IFormStateRepository, FakeStateRepository>());
			_c.Register(_c.NewComponent<IFormStateManager, DefaultStateManager>());
			manager = (DefaultStateManager) _c.Get<IFormStateManager>();
			template = new InputTemplate {Code = "test.in", UnderwriteCode = "test.u"};
			session = new FormSession(template, 2012, 1, new Obj {Id = 2});
		}

		[Test]
		public void IocWellTest() {
			Assert.NotNull(manager);
			Assert.IsInstanceOf<FakeStateRepository>(manager.Repository);
			Assert.AreEqual(1,manager.StateAvailabilityCheckers.Count);
			Assert.IsInstanceOf<FakeStateChecker>(manager.StateAvailabilityCheckers.First());
		}

		[Test]
		public void DefaultStateIsOpen() {
			Assert.AreEqual(FormStateType.Open,manager.GetCurrentState(session));
		}

		[Test]
		public void CanCloseForm() {
			Assert.True(manager.GetCanSet(session,FormStateType.Closed).Allow);
		}

		[Test]
		public void CannotCheckForm() {
			var result = manager.GetCanSet(session, FormStateType.Checked);
			
			Assert.False(result.Allow);
			Console.WriteLine(result.Reason.Message);
			Assert.AreEqual(FormStateOperationDenyReasonType.InvalidBaseState,result.Reason.Type);
		}

		[Test]
		public void CanCheckAfterCloseForm() {
			manager.SetState(session, FormStateType.Closed);
			var result = manager.GetCanSet(session, FormStateType.Checked);
			Assert.True(result.Allow);
		}

		[Test]
		public void CanOpenAfterCloseForm()
		{
			manager.SetState(session, FormStateType.Closed);
			var result = manager.GetCanSet(session, FormStateType.Open);
			Assert.True(result.Allow);
		}

		

		[Test]
		public void CannotReOpenForm()
		{
			var result = manager.GetCanSet(session, FormStateType.Open);
			Assert.False(result.Allow);
			Console.WriteLine(result.Reason.Message);
			Assert.AreEqual(FormStateOperationDenyReasonType.InvalidBaseState, result.Reason.Type);
		}
	}
}
