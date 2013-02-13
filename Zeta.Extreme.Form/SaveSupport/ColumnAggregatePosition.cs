namespace Zeta.Forms {
	/// <summary>
	/// </summary>
	public class ColumnAggregatePosition {
		/// <summary>
		/// </summary>
		public ColumnAggregatePosition() {
			Multiplier = 1;
		}

		/// <summary>
		/// 
		/// </summary>
		public Column Column { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public decimal Multiplier { get; set; }
	}
}