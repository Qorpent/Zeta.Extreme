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
// PROJECT ORIGIN: Zeta.Extreme.Form/ThemaFactoryConfiguration.cs
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Form.InputTemplates;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	������������ ������� ���
	/// </summary>
	public class ThemaFactoryConfiguration : IThemaFactoryConfiguration {
		/// <summary>
		/// 	������
		/// </summary>
		public DateTime Version { get; set; }

		/// <summary>
		/// 	������ ������������
		/// </summary>
		public IList<IThemaConfiguration> Configurations {
			get { return _configurations; }
		}

		/// <summary>
		/// 	�������� XML
		/// </summary>
		public XElement[] SrcXml { get; set; }

		/// <summary>
		/// 	����� ������ ������������ �������
		/// </summary>
		/// <returns> </returns>
		public IThemaFactory Configure() {
			var result = new ThemaFactory {Version = Version, SrcXml = SrcXml.ToString()};
			ConfigureStandaloneThemas(result);
			ConfigureGroups(result);
			ConfigureLibraries(result);
			ConfigureXmlSourceForForms(result);
			ConfigureExtendedXml(result);
			ConfigureBiztranDependency(result);
			return result;
		}

		private void ConfigureBiztranDependency(ThemaFactory result) {
			var bizprocessDefs =
				result.Themas.Select(_ => new {thema = _, bp = _.GetParameter("bizprocess.object", (Model.BizProcess) null)})
				      .Where(_ => null != _.bp)
				      .ToArray();

		}

		private void ConfigureExtendedXml(ThemaFactory result) {
			var newx = new XElement("root");
			foreach (var e in SrcXml) {
				foreach (var x in e.XPathSelectElements("//*[@preservexml]")) {
					newx.Add(x);
				}
			}


			result.SrcXml = newx.ToString();
		}

		private static void ConfigureXmlSourceForForms(ThemaFactory result) {
			foreach (Thema thema in result.Themas) {
				thema.Configuration = null;
				foreach (var it in thema.Forms.Values) {
					((InputTemplate) it).Configuration = null;
				}
				foreach (var document in thema.Forms.Values) {
					document.SourceXmlConfiguration = null;
				}
			}
		}

		private static void ConfigureLibraries(ThemaFactory result) {
			foreach (var thema in result.Themas) {
				foreach (var r in thema.GetAllForms()) {
					var c = ((InputTemplate) r).Configuration;
					if (c.Sources.Length != 0) {
						foreach (var sourcecode in c.Sources) {
							var lib = result.GetForm(sourcecode);
							r.Sources.Add(lib);
						}
					}
				}
			}
		}

		private static void ConfigureGroups(ThemaFactory result) {
			var allthemas = result.Themas.ToArray();
			foreach (var thema in allthemas) {
				if (thema.Group.IsNotEmpty()) {
					var grp = allthemas.FirstOrDefault(x => x.Code == thema.Group);
					if (null != grp) {
						((Thema) grp).GroupMembers.Add(thema);
					}
				}
			}
		}

		private void ConfigureStandaloneThemas(ThemaFactory result) {
			foreach (var thcfg in _configurations) {
				if (thcfg.Active) {
					var thema = thcfg.Configure();
					result.Themas.Add(thema);
				}
			}
		}

		private readonly IList<IThemaConfiguration> _configurations = new List<IThemaConfiguration>();
	}
}