﻿#region LICENSE
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
// PROJECT ORIGIN: Zeta.Extreme.FrontEnd.Tests/JsonSaveParseTest.cs
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Qorpent.Dsl;
using Qorpent.Json;

namespace Zeta.Extreme.FrontEnd.Tests
{
	[TestFixture]
	public class JsonSaveParseTest
	{
		[Test]
		public void BasicParse() {
			var str = @"{""0"":{""id"":""6:3"",""value"":""32345""},""1"":{""id"":""7:3"",""value"":""23626""}}";
			Console.WriteLine(new JsonParser().Parse(str));
		}
	}
}
