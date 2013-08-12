﻿using Zeta.Themas.Loader.Abstracts;

namespace Zeta.Themas.Loader.Wrap {
	public class FormThemaItemWrapper : ThemaItemWrapper, IFormThemaItemWrapper {
		protected internal FormThemaItemWrapper(IThemaItem item, IThemaWrapper wrapper) : base(item, wrapper) {
		}

		#region IFormThemaItemWrapper Members

		public IFormThemaItem Form {
			get { return Item as IFormThemaItem; }
		}

		#endregion
	}
}