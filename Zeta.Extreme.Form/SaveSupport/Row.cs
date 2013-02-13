using System;
using System.Collections.Generic;
using System.Linq;
using Comdiv.Extensions;

namespace Zeta.Forms {
    /// <summary>
    /// 
    /// </summary>
    public class Row : FormElement {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        public Row(Form form) : this(form, null) {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <param name="parent"></param>
        public Row(Form form, Row parent) {
            this.Form = form;
            this.Cells = new List<Cell>();
            this.Form.Rows.Add(this);
            this.Children = new List<Row>();
            this.Parent = parent;
            if(null!=parent) {
                parent.Children.Add(this);
            }
        }
		/// <summary>
		/// 
		/// </summary>
        protected Row Parent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPrimary { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RowCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsAggregate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsTitle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool SelfColumnsOnly { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsNotSupported()
        {
            return !IsSupported();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsSupported()
        {
            return IsPrimary || IsAggregate;
        }

        /// <summary>
        /// 
        /// </summary>
        public IList<Row> Children { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IList<Cell> Cells { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Form Form { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsNoSum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsRoot { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public Cell AddCell(Column column) {
            return new Cell(this,column);
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
        protected override void innerToXml(System.Xml.Linq.XElement e)
        {
            base.innerToXml(e);
            foreach (var cell in Cells) {
                e.Add(cell.ToXml());
            }
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
        protected override void innerFromXml(System.Xml.Linq.XElement e)
        {
            base.innerFromXml(e);
            foreach (var cx in e.Elements("Cell")) {
                var colcode = cx.attr("column");
                var column = this.Form.Columns.First(x => x.Code == colcode);
                var cell = AddCell(column);
                cell.FromXml(cx);
            }
        }

        private Row[] _aggset = null;
        /// <summary>
        /// 
        /// </summary>
        public bool IsMinus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Row[] GetAggregateSet() {
            var result = new List<Row>();
            if(null==_aggset) {
                if (this.Group.noContent()) {
                    Row current = this;
                    processRowInAggregateSetPart(current, result);
                   
                }else {
                    processRowInAggregateSetGroup(result);
                }
                _aggset = result.ToArray();
            }
           
            return _aggset;
        }

        private void processRowInAggregateSetGroup(List<Row> result) {
            var groups = this.Group.split();
            foreach (var row in this.Form.Rows) {
                if(row.Group.hasContent()) {
                    if(row.Group.split().Intersect(groups).Count()!=0) {
                        result.Add(row);
                    }
                }
            }
        }

        private void processRowInAggregateSetPart(Row current, List<Row> result) {
            foreach (var child in current.Children) {
                if(!child.IsTitle && !child.IsNoSum && !child.IsAggregate) {
                    result.Add(child);
                }
                if (!child.IsNoSum && child.IsAggregate) {
                    var set = child.GetAggregateSet();
                    foreach (var row in set) {
                        result.Add(row);
                    }
                    
                }
            }
        }
    }
}