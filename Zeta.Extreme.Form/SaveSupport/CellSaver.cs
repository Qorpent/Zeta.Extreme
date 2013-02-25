#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : CellSaver.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using System.Linq;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Inversion;
using Comdiv.MVC;
using Comdiv.Model.Interfaces;
using Comdiv.Zeta.Model;
using Zeta.Extreme.Form.InputTemplates;

namespace Zeta.Extreme.Form.SaveSupport {
	/// <summary>
	/// 	—редство сохранени€ €чеек
	/// </summary>
	public class CellSaver {
		/// <summary>
		/// 	—оздает CellSaver со стандартным набором валидаторов
		/// </summary>
		public CellSaver() {
			Validators
				.fillFromLocator()
				.ensureInstance(() => (IRecreateCellValidator) new RecreateCellValidator())
				.ensureInstance(() => (ICheckRequestObject) new CellsAreOfTargetOrg())
				.ensureInstance(() => (ICellFixingValidator) new CellFixingValidator())
				;
		}

		private IList<IFormDataValidator> Validators {
			get { return validators; }
		}

		//public IController Controller { get; set; }
		/// <summary>
		/// 	«апрос формы
		/// </summary>
		public InputTemplateRequest TemplateRequest { get; set; }

		/// <summary>
		/// 	¬ыполн€ет сохранние €чейки
		/// </summary>
		/// <param name="cells"> </param>
		public void Save(IEnumerable<IWithId> cells) {
			var storage = myapp.storage.Get<IZetaCell>();
			cells = cells.ToList();
			var isvalid = Validate(cells);
			if (!isvalid.IsValid) {
				isvalid.Throw();
			}


			IList<int> objectsIds = new List<int>();
			IList<int> detailIds = new List<int>();
			foreach (var cell in cells) {
#if USEPKG
				IPkg pkg = null;
                if (cell is IPkg){
                    pkg = (IPkg) cell;
                }
                if (cell is IZetaCell && null != pkg){
                    ((IZetaCell) cell).Pkg = pkg;
                }
#endif
				if (cell is IZetaCell) {
					var _c = cell as IZetaCell;
					if (!objectsIds.Contains(_c.Object.Id)) {
						objectsIds.Add(_c.Object.Id);
					}
					if (_c.DetailObject != null && !detailIds.Contains(_c.DetailObject.Id)) {
						detailIds.Add(_c.DetailObject.Id);
					}
				}
				if (cell is IZetaCell && ((IZetaCell) cell).Tag is IZetaCell) {
					var realcell = ((IZetaCell) cell).Tag as IZetaCell;
#if USEPKG
                    realcell.Pkg = ((IZetaCell) cell).Pkg;
#endif
					realcell.Value = ((IZetaCell) cell).Value;
					realcell.Usr = ((IZetaCell) cell).Usr;
					storage.Save(realcell);
					continue;
				}

				if (cell is IZetaCell && "ignore".Equals(((IZetaCell) cell).Tag)) {
					continue;
				}
				if (cell is IZetaCell && "delete".Equals(((IZetaCell) cell).Tag)) {
					storage.Delete(cell);
				}
				else {
					storage.Save(cell);
				}
			}
			foreach (var i in objectsIds) {
				Evaluator.DefaultCache.Clear(@"(?i)obj\(" + i + @"\)");
			}
			foreach (var i in detailIds) {
				Evaluator.DefaultCache.Clear(@"(?i)detail\(" + i + @"\)");
			}
			Evaluator.InvokeChanged();
		}

		/// <summary>
		/// 	выполн€ет проверку €чеек
		/// </summary>
		/// <param name="cells"> </param>
		/// <returns> </returns>
		public ValidationResult Validate(IEnumerable<IWithId> cells) {
			var result = new ValidationResult {IsValid = true};
			foreach (var validator in Validators) {
				result = validator.Validate(result, TemplateRequest, cells.OfType<IZetaCell>());
			}
			return result;
		}

		private readonly IList<IFormDataValidator> validators = new List<IFormDataValidator>();
	}
}