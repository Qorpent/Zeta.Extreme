using System;
using System.Runtime.Serialization;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class FormulaCompilationException : Exception {
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		/// <summary>
		/// 
		/// </summary>
		public FormulaCompilationException() {}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public FormulaCompilationException(string message) : base(message) {}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public FormulaCompilationException(string message, Exception inner) : base(message, inner) {}

		/// <summary>
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected FormulaCompilationException(
			SerializationInfo info,
			StreamingContext context) : base(info, context) {}
	}
}