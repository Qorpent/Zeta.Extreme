﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Developer.Analyzers;
using Zeta.Extreme.Developer.Model;

namespace Zeta.Extreme.Developer.Utils {
	/// <summary>
	/// Вспомогательный класс для документации
	/// </summary>
	public static class DocumentationHelper {
		/// <summary>
		/// Подготовка документации для атрибута 
		/// </summary>
		/// <param name="attr"></param>
		/// <param name="filter"></param>
		/// <param name="doc"></param>
		public static AttributeDescriptor SetupDocument(this AttributeDescriptor attr, SearchFilter filter, IDictionary<string, Documentation> doc) {
			if (filter.IncludeDoc) {
				var key = filter.DocRoot + ":attr[" + attr.Name + "]";
				if (doc.ContainsKey(key)) {
					attr.Doc = doc[key];
				}
			}
			return attr;
		}

		/// <summary>
		/// Подготовить документацию для варианта значения атрибута
		/// </summary>
		/// <param name="variant"></param>
		/// <param name="filter"></param>
		/// <param name="doc"></param>
		public static AttributeValueVariant SetupDocument(this AttributeValueVariant variant, SearchFilter filter, IDictionary<string, Documentation> doc)
		{
			if (filter.IncludeDoc) {
				var key = filter.DocRoot + ":attr[" + variant.Parent.Name + "]" + ":value[" + variant.Value + "]";
				if (doc.ContainsKey(key)) {
					variant.Doc = doc[key];
				}
			}
			return variant;
		}


		/// <summary>
		/// Подготовить документацию для варианта значения атрибута
		/// </summary>
		/// <param name="elementmap"></param>
		/// <param name="filter"></param>
		/// <param name="doc"></param>
		public static ElementCodeTypeMap SetupDocument(this ElementCodeTypeMap elementmap, SearchFilter filter, IDictionary<string, Documentation> doc)
		{
			if (filter.IncludeDoc) {
				var typekey = filter.DocRoot + ":codetype[" + elementmap.Type + "]";
				var selfkey = filter.DocRoot + ":codemap[" + elementmap.Path + "]";
				var resultdoc = new Documentation();

				if (doc.ContainsKey(typekey)) {
					var main = doc[typekey];
					resultdoc.Merge(main);
				}
				if (doc.ContainsKey(selfkey))
				{
					var self = doc[selfkey];
					resultdoc.Merge(self);
				}
				if (!string.IsNullOrWhiteSpace(resultdoc.Key)) {
					elementmap.Doc = resultdoc;
				}
			}
			return elementmap;
		}

		/// <summary>
		/// Десериализует документацию из XML
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		public static IDictionary<string, Documentation> LoadDocuments(XElement xml) {
			var result = new Dictionary<string, Documentation>();
			foreach (var root in xml.Elements()) {
				var key = root.Name.LocalName;
				foreach (var item in root.Elements()) {
					var itemdoc = CreateDocument(item, key);
					result[itemdoc.Key] = itemdoc;
					var itemkey = itemdoc.Key;
					foreach (var val in item.Elements()) {
						var valdoc = CreateDocument(val, itemkey);
						result[valdoc.Key] = valdoc;
					}
				}
			}
			return result;
		}

		

		/// <summary>
		/// Десериализует отдельный документа из XML
		/// </summary>
		/// <param name="item"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static Documentation CreateDocument(XElement item, string key) {
			var d = item.Describe(true);
			key += ":" + item.Name.LocalName + "[" + item.Attr("code") + "]";
			var itemdoc = new Documentation();
			itemdoc.Key = key;
			itemdoc.Name = d.Name;
			var commentnode = item.Nodes().OfType<XText>().FirstOrDefault();
			if (null != commentnode) {
				itemdoc.Comment = commentnode.Value.Trim().Replace("\r", "<BR/>").Replace("\t", "&#160;&#160;&#160;&#160;");
			}
			itemdoc.Obsolete = item.Attr("obsolete");
			itemdoc.SubComment = item.Attr("comment");
			itemdoc.Error = item.Attr("error");
			itemdoc.Question = item.Attr("question");
			itemdoc.IsBiztran = item.Attr("biztran").ToBool();
			itemdoc.IsSystem = item.Attr("system").ToBool();
			return itemdoc;
		}
	}
}