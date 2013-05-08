using System.Linq;
using System.Security;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.FrontEnd.Actions.Info {
	/// <summary>
	/// Действие получения технических данных по ячейке и истории ее формирования
	/// </summary>
	[Action("zefs.cellhistory",Role = "DEFAULT")]
	public class GetCellHistory : FormSessionActionBase {
		/// <summary>
		///	Идентификатор ячейки
		/// </summary>
		[Bind]protected int CellId;

		private Cell _cell;
		private CellHistory[] _history;

		/// <summary>
		/// 	4 part of execution - all internal context is ready - authorize it with custom logic
		/// </summary>
		/// <exception cref="SecurityException">try access data of cell that not exists in requested session</exception>
		protected override void Authorize()
		{
			base.Authorize();
			if (MySession.Data.All(_ => _.c != CellId)) {
				throw new SecurityException("try access data of cell that not exists in requested session");
			}	
		}

		/// <summary>
		/// 	Third part of execution - setup system-bound internal state here (called after validate, but before authorize)
		/// </summary>
		protected override void Prepare()
		{
			var reader = new NativeZetaReader();
			_cell = reader.GetCells("Id = " + CellId).FirstOrDefault();
			if (null == _cell) {
				throw new ValidationException("cell not exists "+CellId);
			}
			_history = reader.GetCellHistory(CellId).OrderByDescending(_=>_.Time).ToArray();
		}

		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			var result = new
				{
					cell = new
						{
							id = _cell.Id,
							rowcode = _cell.Row.Code,
							rowname = _cell.Row.Name,
							colcode = _cell.Column.Code,
							colname = _cell.Column.Name,
							objid = _cell.Object.Id,
							objname = _cell.Object.Name,
							detailid = _cell.DetailId.HasValue?_cell.DetailId.Value:0,
							contragentid = _cell.ContragentId.HasValue? _cell.ContragentId.Value:0,
							contragentname = _cell.Contragent!=null?_cell.Contragent.Name:"",
							year = _cell.Year,
							period = _cell.Period,
							value = _cell.NumericValue,
							user = _cell.User,
							version = _cell.Version,
							currency = _cell.Currency,
						},
					history = _history.Select(
						_=>new
							{
								id = _.Id,
								time = _.Time,
								user = _.User,
								value = _.Value,
								bizkey = _.BizKey
							}
					).ToArray()
				};
			return result;
		}
		

	}
}