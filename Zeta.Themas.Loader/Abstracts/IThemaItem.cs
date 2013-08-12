using System.Collections.Generic;
using Zeta.Themas.Loader.Model.ThemaItem;
using Zeta.Themas.Loader.Model.ThemaItemContent;
using Zeta.Themas.Loader.ZetaIntegration;

namespace Zeta.Themas.Loader.Abstracts {
	public interface IThemaItem : IElementBasis {
		IThema Thema { get; set; }
		IList<ILibraryLink> LibraryLinks { get; }
		IList<ILibraryLink> InLibraryUsage { get; }
		string FullCode { get; }
		string ForGroup { get; set; }
		int[] ListForPeriods { get; }
		string ForPeriods { get; set; }
		string PeriodRedirect { get; set; }
		PeriodRedirectDefinition[] ListPeriodRedirect { get; }
		string FixedObject { get; set; }
		IList<ThemaItemParameter> Parameters { get; }
		IDictionary<string, string> NativeXmlParameters { get; }
		IZetaEntityIntermediate ZetaObject { get; set; }
		IEnumerable<T> GetElements<T>() where T : IThemaItemElement;
		IEnumerable<IThemaItemElement> GetElements(string type);
		IEnumerable<IColumnItemElement> GetColumns();
		IEnumerable<IRowItemElement> GetRows();
		IEnumerable<IObjItemElement> GetObjs();
	}
}