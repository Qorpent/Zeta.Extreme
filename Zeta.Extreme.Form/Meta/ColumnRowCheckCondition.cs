namespace Zeta.Extreme.Meta {
    /// <summary>
    /// Правило сопоставления колонок
    /// </summary>
    public class ColumnRowCheckCondition {
		/// <summary>
		/// Действие
		/// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Value2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool DenyBlock { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RowTag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RowClass { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RowStyle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CellClass { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CellStyle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Comment { get; set; }
    }
}