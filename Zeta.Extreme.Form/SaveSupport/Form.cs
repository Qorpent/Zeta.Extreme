using System;
using System.Collections.Generic;
using System.Linq;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Model.Interfaces;

namespace Zeta.Forms {
    /// <summary>
    /// 
    /// </summary>
    public class Form : FormElement {
        /// <summary>
        /// 
        /// </summary>
        public Form() {
            this.Rows = new List<Row>();
            this.Columns =new List<Column>();
            this.Cells = new List<Cell>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public Row AddRow(Row parent = null) {
            return new Row(this,parent);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Column AddColumn() {
            return new Column(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetEvalCount() {
            return Cells.Where(x => x.IsEvaluated).Count();
        }
        /// <summary>
        /// 
        /// </summary>
        public IList<Row> Rows { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IList<Column> Columns { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IList<Cell> Cells { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Period { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ObjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public LongTask Task { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Valuta { get; set; }

    	/// <summary>
    	/// 
    	/// </summary>
    	public IPseudoThema Thema { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
    	protected override void innerToXml(System.Xml.Linq.XElement e)
        {
            base.innerToXml(e);
            foreach (var column in Columns) {
                e.Add(column.ToXml());
            }
            foreach (var row in Rows) {
                e.Add(row.ToXml());
            }
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
        protected override void innerFromXml(System.Xml.Linq.XElement e)
        {
            base.innerFromXml(e);
            
            foreach (var cx in e.Elements("Column"))
            {
                var row = AddColumn();
                row.FromXml(cx);
            }

            foreach (var rx in e.Elements("Row"))
            {
                var row = AddRow();
                row.FromXml(rx);
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowcode"></param>
        /// <param name="colcode"></param>
        /// <param name="year"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public Cell GetCell(string rowcode, string  colcode, int year, int period) {
            var key = rowcode + "_" + colcode + "_" + year + "_" + period;
            return index.get(key.ToUpper(), () =>
                                      {
                                          var r = this.Rows.FirstOrDefault(x => x.Code == rowcode);
                                          if (null == r) return null;
                                          var cell =
                                              r.Cells.FirstOrDefault(
                                                  x =>
                                                  x.Column.ColumnCode == colcode && x.Column.Year == year &&
                                                  x.Column.Period == period);
                                          return cell;
                                      },true);
        }
        IDictionary<string ,Cell> index = new Dictionary<string, Cell>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Cell GetCell(string code) {
            return index.get(code.ToUpper(), (Cell)null);
        }
        /// <summary>
        /// 
        /// </summary>
        public void MakeCellIndex() {
            int i = 0;
            foreach (var row in Rows) {
                row.Idx = i++;
            }
            i = 0;
            foreach (var col in Columns)
            {
                col.Idx = i++;
            }
              index.Clear();
            foreach (var cell in Cells) {
                index[cell.Key.ToUpper()] = cell;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearData() {
            foreach (var cell in this.Cells) {
                cell.Usages = 0;
                cell.Value = null;
                cell.NumericValue = 0;
                cell.IsEvaluated = false;
                cell.Error = "";
                this.Valuta = "";
            }
        }

    	/// <summary>
    	/// 
    	/// </summary>
    	public void Clean() {
    		this.Rows.Clear();
    		this.Rows = null;
    		this.Columns.Clear();
    		this.Columns = null;
			this.Cells.Clear();
    		this.Cells = null;
			this.index.Clear();
    	}
    }
}