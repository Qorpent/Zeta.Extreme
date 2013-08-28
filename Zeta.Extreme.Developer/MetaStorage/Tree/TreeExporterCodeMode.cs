namespace Zeta.Extreme.Developer.MetaStorage.Tree {
	/// <summary>
	/// –ежим генерации кодов
	/// </summary>
	public enum TreeExporterCodeMode {
		/// <summary>
		/// Ќеопределенный
		/// </summary>
		Undefined = 1,
		/// <summary>
		/// »спользуетс€ полный €вный код
		/// </summary>
		Full = 1<<1,

		/// <summary>
		///  оды вообще не формируютс€ - должны быть сформированы системой при загрузке (как в презентации)
		/// </summary>
		NoCode = 1<<2,
		/// <summary>
		/// ≈сли код дочернего узла содержит код корн€ в качестве префикса, он удал€етс€
		/// </summary>
		RootPrefix = 1<<3,
		/// <summary>
		/// ≈сли код дочернего узла содержит код родительского в качестве префикса, он удал€етс€
		/// </summary>
		ParentPrefix = 1<<4,
		/// <summary>
		/// ѕо умолчанию коды не переписываютс€
		/// </summary>
		Default = Full,

	}
}