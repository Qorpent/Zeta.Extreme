using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Zeta.Extreme.Core.Tests.FormulaStorage
{
	[TestFixture]
	public class DllCacheTests
	{
		/// <summary>
		/// Проверяем, что DLL, имеющие версию старше базовых сборок не попадают в кэш 
		/// ZC-397
		/// </summary>
		[Test]
		public void CanDecideWichDllsToLoad() {
			
		}
	}
}
