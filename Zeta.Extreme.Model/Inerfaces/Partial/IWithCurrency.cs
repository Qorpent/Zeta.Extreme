namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// Marks that entity is attached to currency
	/// </summary>
	public interface IWithCurrency {
		/// <summary>
		///Currency of entity
		/// </summary>
		string Currency { get; set; }
	}
}