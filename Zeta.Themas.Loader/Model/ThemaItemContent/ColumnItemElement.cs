using System.Xml.Linq;
using Zeta.Themas.Loader.Abstracts;
using Zeta.Themas.Loader.Wrap;

namespace Zeta.Themas.Loader.Model.ThemaItemContent {
	public class ColumnItemElement : ThemaItemElement, IColumnItemElement {
		public ColumnItemElement(XElement e = null) : base(e) {
			Type = "col";
		}

		#region IColumnItemElement Members

		public string ForPeriods { get; set; }
		public string ForGroup { get; set; }
		public string Valuta { get; set; }

		public string MatrixId { get; set; }
		public string MatrixFormula { get; set; }
		public string MatrixFormulaType { get; set; }
		public string MatrixTotalFormula { get; set; }

		public bool Fixed { get; set; }

		public int Year { get; set; }
		public int Period { get; set; }

		public bool DoSum { get; set; }


		public override IThemaItemElementWrapper GetWrapper(IThemaItemWrapper itemwrapper) {
			return new ColumnItemElementWrapper(this, itemwrapper);
		}

		#endregion
	}
}