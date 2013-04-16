namespace Zeta.Extreme.BizProcess.Forms.Custom {
	/// <summary>
	/// Элемент описания расщепляемого по деталям элемента структуры
	/// id	name	bill	grp	idx	detail	tid	grpname	grpidx	sname
	///357	ООО "УГМК-Холдинг"	BA062	1	20	14728	188	Головные организации	1	0УГМК-Холдинг"
	/// </summary>
	public class ExtendedSplitStructureItem {
		/// <summary>
		/// Ид контрагента
		/// </summary>
		public int AltObj;
		/// <summary>
		/// Имя контрагента
		/// </summary>
		public string AltObjName;
		/// <summary>
		/// Код счетов
		/// </summary>
		public string BillCode;
		/// <summary>
		/// Номер группы
		/// </summary>
		public string Grp;
		/// <summary>
		/// Индекс
		/// </summary>
		public int Idx;
		/// <summary>
		///ID детали контейнера
		///  </summary>
		public int Detail;
		/// <summary>
		/// ID типа детали
		/// </summary>
		public int Type;
		/// <summary>
		///Индекс группы
		/// </summary>
		public int GrpIdx;
		/// <summary>
		/// Упрощенное имя контрагента
		/// </summary>
		public string SimpleName;
	}
}