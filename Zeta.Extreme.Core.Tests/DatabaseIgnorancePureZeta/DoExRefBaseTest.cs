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
// PROJECT ORIGIN: Zeta.Extreme.Core.Tests/DoExRefBaseTest.cs
#endregion
using System.Collections.Generic;
using NUnit.Framework;
using Zeta.Extreme.Model;

namespace Zeta.Extreme.Core.Tests.DatabaseIgnorancePureZeta {
	[TestFixture]
	public class DoExRefBaseTest : PureZetaTestFixtureBase
	{
		/// <summary>
		/// ������ ������ � ���������� ������ �������
		/// </summary>
		/// <returns></returns>
		protected override IEnumerable<Query> BuildModel() {
			var exrefrow = new Row {Code = "e", ExRefTo = new Row {Code = "t"}};
			var exrefcol = new Column {Code = "e", MarkCache = "/DOEXREF/"};
			var nexrefcol = new Column {Code = "ne"};
			var q1 = new Query //��� ������ ��������� exref
				{
					Row = {Native = exrefrow},
					Col = {Native = exrefcol}
				};
			yield return q1;
			var q2 = new Query //� ��� ���
				{
					Row = { Native = exrefrow },
					Col = { Native = nexrefcol }
				};
			yield return q2;

			Add(new Query{Row={Code="t"},Col = {Code="e"}}, 1);
			Add(new Query{Row={Code="t"},Col = {Code="ne"}}, 2);
			Add(new Query{Row={Code="e"},Col = {Code="ne"}}, 3);
			yield return q2;
		}

		protected override void Examinate(Query query)
		{
			if(query.Col.Code=="e") {
				Assert.AreEqual(1,query.Result.NumericResult);
			}
			if (query.Col.Code == "ne")
			{
				Assert.AreEqual(3, query.Result.NumericResult);
			}

			Assert.AreEqual(2, _session.Registry.Count); //��� �� ������ ���� ������� �����������

		}
	}
}