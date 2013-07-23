using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Qorpent.Serialization;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Developer.Analyzers;

namespace Zeta.Extreme.Developer.Model {
	/// <summary>
	/// Ссылка на исходный контент
	/// </summary>
	[Serialize]
	public class ItemReference {
		/// <summary>
		/// 
		/// </summary>
		public ItemReference() {
			
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		public ItemReference(XElement e) {
			var d = e.Describe();
			File = d.File;
			Line = d.Line;
			var ctx = CodeIndex.GetContextString(e);
			var maincontext = ctx.SmartSplit(false, true, '/')[0];
			var subcontext = String.Join("/", ctx.SmartSplit(false, true, '/').Skip(1));
			SubContext = subcontext;
			MainContext = maincontext;
		}

		private IEnumerable<ItemReference> _subReferences;

		/// <summary>
		/// Файл
		/// </summary>
		[SerializeNotNullOnly]
		public string File { get; set; }
		/// <summary>
		/// Строка
		/// </summary>
		[SerializeNotNullOnly]
		public int Line { get; set; }

		/// <summary>
		/// Главный контекст
		/// </summary>
		[SerializeNotNullOnly]
		public string MainContext { get; set; }

		/// <summary>
		/// Дочерний контекст
		/// </summary>
		[SerializeNotNullOnly]
		public string SubContext { get; set; }
	
		/// <summary>
		/// Дочерние ссылки
		/// </summary>
		[Serialize]
		public IEnumerable<ItemReference> Children {
			get { return _subReferences ?? (_subReferences = new List<ItemReference>()); }
			set { _subReferences = value; }
		}
	}
}