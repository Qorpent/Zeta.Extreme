using System.Linq;
using Comdiv.Extensions;
using Comdiv.Model.Interfaces;
using Comdiv.Zeta.Data;
using Comdiv.Zeta.Model;
using Comdiv.Zeta.Report;
using Zeta.Forms;

namespace Comdiv.Zeta.Web.InputTemplates {
    public class ZetaReportDefinitionToFormDefinitionConverter {
        public Form PrepareForm(ZetaReportDefinitionBase template, IZetaMainObject obj) {

            var result = new Form();
        	result.Thema = (IPseudoThema)template.Thema;
            result.ObjectId = obj.Id;
            result.Tag = obj;
            var columns = template.GetColumns();
            var rows = template.GetRootRows().Select(x=>RowCache.get(x.Code));
            foreach (var column in columns.Where(x=>x.Visible)) {
                addColumn(result, column);
            }
            foreach (var column in columns.Where(x => !x.Visible))
            {
                if (null == result.Columns.FirstOrDefault(x => x.Code == column.Code && x.Year == column.Year && x.Period == column.Period)) {
                    addColumn(result, column);
                }
            }
            foreach (var column in result.Columns)
            {
                if (column.IsAggregate) {
                    var positions = column.MatrixFormula.split();
                    foreach (var position in positions) {
                        var n = position;
                        var m = 1;
                        if(n.StartsWith("-")) {
                            m = -1;
                            n = n.Substring(1);
                        }
                        bool canbeskiped = false;
                        if (n.StartsWith("?"))
                        {
                            canbeskiped = true;
                            n = n.Substring(1);
                        }
                        var target = result.Columns.FirstOrDefault(x => x.MatrixId == n);
                        if(null==target && !canbeskiped) {
                            column.IsAggregate = false;
                            break;
                        }
                        if (null != target) {
                            column.AggregateSources.Add(new ColumnAggregatePosition {Column = target, Multiplier = m});
                        }
                    }
                }
            }




            foreach (var row in rows) {
                if(null!=row)
                    addRow(result, row,null,false);
               
            }
            foreach (var row in template.MatrixExRows.split(false,true,' '))
            {
                if (row.hasContent()) {
                    var rc = row;
                    bool selfc = false;
                    if(row.StartsWith("~")) {
                        rc = row.Substring(1);
                        selfc = true;
                    }
                    
                    addRow(result, RowCache.get(rc), null, selfc );
                }
            }
            foreach (var row in result.Rows) {
                if(!row.IsTitle) {
                    foreach (var column in result.Columns) {
                        if (!column.IsMatch(row)) continue;
                        var cell = row.AddCell(column);

                        var r = row.Tag as IZetaRow;
                        if(null!=r) {
                            var year = TagHelper.Value(r.Tag, "fromyear").toInt();
                            if (r.IsFormula && year != 0) {
                                if (column.Year<year) {
                                    cell.DirectPrimary = true;
                                } 
                            }
                        }
                        if(cell.IsPrimary()) {
                            cell.IsEvaluated = true;
                            cell.Fix = 5;
                        }
                    }
                }
            }
            result.MakeCellIndex();
            return result;
        }

        private void addColumn(Form result, ColumnDesc column) {
            var c = result.AddColumn();
            c.Tag = column;
            c.Code = column.Code;
            if(column.Target!=null) {
                c.Code = column.Target.Code;    
            }
            
            c.Year = column.Year;
            c.Period = column.Period;
            c.ForRows = column.MatrixForRows;
            c.IsAggregate = column.MatrixFormula.hasContent();
            c.AggregateType = column.MatrixFormulaType;
            c.MatrixId = column.MatrixId;
            c.MatrixFormula = column.MatrixFormula;
            c.IsPrimary = !column.IsFormula && !c.IsAggregate;
        }

        private void addRow(Form result, IZetaRow row, Row parent, bool selfc) {
            if (null == row) return;
            
            var r = result.AddRow(parent);
            r.Code = row.Code;
            r.Tag = row;
            if (null == parent) {
                r.IsRoot = true;
                r.Path = row.Path + "%";
            }
            r.IsAggregate = row.IsMarkSeted("0SA");
            r.IsNoSum = row.IsMarkSeted("0NOSUM");
            r.IsTitle = row.IsMarkSeted("0CAPTION");
            r.IsMinus = row.IsMarkSeted("0MINUS");
            r.SelfColumnsOnly = selfc;
            r.Group = row.Group;
            r.IsPrimary = !row.IsFormula && !r.IsTitle && !r.IsAggregate;
            if (row.Tag.hasContent() && row.Tag.Contains("matrix:1")) {
                r.IsPrimary = true;
            }

            if (!row.IsFinal()) {
                foreach (var child in row.Children) {
                    addRow(result, child, r, selfc);
                }
            }
        }
    }
}