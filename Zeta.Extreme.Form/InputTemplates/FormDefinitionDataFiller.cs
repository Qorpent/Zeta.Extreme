using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Inversion;
using Comdiv.Persistence;
using Comdiv.Zeta.Data;
using Comdiv.Zeta.Model;
using Zeta.Forms;

namespace Comdiv.Zeta.Web.InputTemplates {
    public class FormDefinitionDataFiller {
        private IEnumerable<IFormMatrixSpecialProcedure> specialProcedures;

        public FormDefinitionDataFiller() {
            this.specialProcedures = myapp.ioc.all<IFormMatrixSpecialProcedure>();
        }

        public string Valuta { get; set; }

        public void Fill(Form matrix) {
           
            if(this.Valuta.noContent()) {
                this.Valuta = "NONE";
            }
            matrix.Valuta = this.Valuta;
            using (var s = new TemporarySimpleSession()) {
                fillPrimary(matrix);
                evaluateRowAggregates(matrix);
           //     evaluateRowAggregates(matrix);
                evaluateColAggregates(matrix);
          //      evaluateColAggregates(matrix);
                Task.PhaseFinished("aggregates 1");
                evaluateSpecialRows(matrix);
                if (EvaluateRows) {
                    fillRowBlackBoxes(matrix);
                    Task.PhaseFinished("row black-boxes");
                    evaluateRowAggregates(matrix);
                    //     evaluateRowAggregates(matrix);
                    evaluateColAggregates(matrix);
                    Task.PhaseFinished("aggregates 2");
                }
                //     evaluateColAggregates(matrix);
                
                evaluateSpecialColumns(matrix);
                if (EvaluateColumns) {
                    fillColumnBlackBoxes(matrix);
                    Task.PhaseFinished("col black-boxes");
                    evaluateRowAggregates(matrix);
                    //    evaluateRowAggregates(matrix);
                    evaluateColAggregates(matrix);
                    //   evaluateColAggregates(matrix);
                    Task.PhaseFinished("aggregates 3");
                }

                evaluateColAggregates(matrix);
            }
            //evaluateColumnAggregates(matrix);
        }

        public bool EvaluateColumns { get; set; }

        public bool EvaluateRows { get; set; }

        private void evaluateSpecialColumns(Form matrix) {
            foreach (var procedure in specialProcedures)
            {
                procedure.Execute(matrix, ProcedureStage.Columns);
            }
        }

        private void evaluateSpecialRows(Form matrix) {
            foreach (var procedure in specialProcedures) {
                procedure.Execute(matrix, ProcedureStage.Rows);
            }
        }

        private void fillColumnBlackBoxes(Form matrix) {
            var cells = matrix.Cells.Where(x => x.Column.IsNotSupported() && !x.Row.IsAggregate && !x.IsEvaluated).ToArray();
            Task.Set("col-black-count", cells.Count().ToString());
            fillBlackBoxes(matrix, cells);
        }

        private void evaluateRowAggregates(Form matrix) {
            var rows = matrix.Rows.Where(x => x.IsAggregate).Reverse().ToArray();
            foreach (var row in rows) {
                var aset = row.GetAggregateSet();
                foreach (var cell in row.Cells) {
                    if (cell.IsEvaluated) continue;
                    if(cell.IsAggregate()) {
                        decimal summ = 0;
                        bool evaluated = true;
                        foreach (var r in aset) {
                            
                            var corcel = r.Cells.FirstOrDefault(x=>x.Column==cell.Column);
                           if(null==corcel) {
                               continue;
                               ;
                           }
                            if( !corcel.IsEvaluated) {
                                evaluated = false;
                                break;
                            }
                            else {
                                corcel.Usages++;
                                decimal mult = 1;
                                if(row.Group.hasContent() && r.IsMinus) {
                                    mult = -1;
                                }
                                summ += mult * corcel.NumericValue;
                            }
                        }
                        if(evaluated) {
                            cell.IsEvaluated = true;
                            cell.NumericValue = summ;
                          
                        }
                    }
                }
            }
        }

        private void evaluateColAggregates(Form matrix)
        {
            var cols = matrix.Columns.Where(x => x.IsAggregate).ToArray();
            foreach (var col in cols)
            {
                foreach (var cell in col.Cells)
                {
                    if (cell.IsEvaluated) continue;
                    var row = cell.Row;
                    decimal summ = 0m;
                    bool evaluated = true;
                    if (col.AggregateType == "sum") {
                        foreach (var pos in col.AggregateSources) {
                            var src = row.Cells.First(x => x.Column == pos.Column);
                            if (!src.IsEvaluated) {
                                evaluated = false;
                                break;
                            }
                            src.Usages++;
                            var val = src.NumericValue*pos.Multiplier;
                            summ += val;
                        }
                    }
                    if(col.AggregateType == "proc") {
                    	decimal n1 = 0m;
                    	decimal n2 = 0m;
						try {
							var v1 = row.Cells.First(x => x.Column == col.AggregateSources[0].Column);
							var v2 = row.Cells.First(x => x.Column == col.AggregateSources[1].Column);
							if (v1.IsEvaluated && v2.IsEvaluated) {
								n1 = v1.NumericValue;
								n2 = v2.NumericValue;
								if (Math.Abs(n1) < 0.0000001m) n1 = 0;
								if (Math.Abs(n2) < 0.0000001m) n2 = 0;
								decimal val = 0;
								if (n2 == 0) {
									if (n1 == 0) {
										val = 0;
									}
									else {
										if (n1 > 0) {
											val = 100;
										}
										else {
											val = -100;
										}
									}
								}
								else {

									val = ((n1 - n2)/n2)*100;
								}
								if(Math.Abs(val)>1000000000000) {
									val = Math.Sign(val)*100;
								}
								summ = val;

							}
							else {
								evaluated = false;
							}
						}catch(OverflowException) {
							throw new Exception(string.Format("overflow caught from {0} and {1} ",n1,n2));
						}

                    }

                    if (evaluated)
                    {
                        cell.IsEvaluated = true;
                        cell.NumericValue = summ;
                    }
                    
                }
            }
        }

        private void fillRowBlackBoxes(Form matrix) {
            var cells = matrix.Cells.Where(x => x.Row.IsNotSupported() && x.Column.IsPrimary).ToArray();
            Task.Set("row-black-count", cells.Count().ToString());
            fillBlackBoxes(matrix, cells);
        }

        private void fillBlackBoxes(Form matrix, Cell[] cells) {
            foreach (var cell in cells) {
                var rd = cell.Row.Tag as RowDescriptor;
                if(null==rd) {
                    rd = new RowDescriptor(cell.Row.Tag as IZetaRow);
                }
                var cd = cell.Column.Tag as ColumnDesc;
                var q = new Query(new Zone(matrix.Tag as IZetaMainObject), rd, cd,matrix.Thema) {FormMatrix = matrix};
                if(this.Context!=null) {
                    q.Context = this.Context;
                }
                q.OutValuta = this.Valuta;
                string result = "";
                string error = "";
                try
                {
                    result = q.evals();
                    
                }
                catch (Exception e) {
                    result = "error";
                    error = e.ToString();
                }
                cell.Value = result;
                cell.Error = error;
                cell.IsEvaluated = true;
            }
        }

        private void fillPrimary(Form matrix) {
            var cnt = matrix.Cells.Where(x => x.IsPrimary()).Count();
             Task.Set("primary-count",cnt.ToString());
            Task.Inc("matrix-primary-count",cnt);
            var sw = Stopwatch.StartNew();
            var xml = new XElement("root");
            foreach (var row in matrix.Rows.Where(x=>x.IsRoot)) {
                foreach (var column in matrix.Columns.Where(x=>x.IsPrimary)) {
                    if(!column.IsMatch(row)) continue;
                    xml.Add(new XElement("cell",
                    new XAttribute("obj", matrix.ObjectId),
                     new XAttribute("row",((IZetaRow)row.Tag).IsFinal()? row.Code : row.Path),
                    
                    new XAttribute("col", column.Code),
                    new XAttribute("year", column.Year),
                    new XAttribute("period", column.Period)
                    ));    
                }
                
            }
            Task.PhaseFinished("matrix sql prepared");
            IDictionary<string, object> result;
            using (var connection = myapp.ioc.getConnection()) {
                connection.WellOpen();
                var cmd = "exec usm.get_form_matrix @obj=@obj, @xml=@xml, @valuta=@valuta";
                var parameters = new Dictionary<string, object>
                                     {
                                         {"@obj",matrix.ObjectId},
                                         {"@xml",xml.ToString()},
                                         {"@valuta",this.Valuta ?? ""},
                                     };
                var command = connection.CreateCommand(cmd, parameters);
                using(var reader=command.ExecuteReader(CommandBehavior.SingleResult)) {
                    while (reader.Read()) {
                        var key = reader.GetString(0);
                        var c = matrix.GetCell(key);
                        if (null != c) {
                            if (c.IsPrimary()||this.ApplyNoPrimary) {
                                c.IsEvaluated = true;
                                c.NumericValue = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2);
                                c._cellId = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                                c.Fix = reader.IsDBNull(3) ? 5 : reader.GetInt32(3);
                            }

                        }
                        
                    }
                    reader.Close();
                }
                sw.Stop();
                Task.Inc("matrix-fill-primary-time",sw.ElapsedMilliseconds);
                Task.PhaseFinished("matrix sql executed");
            }

        }

        public bool ApplyNoPrimary { get; set; }

        public LongTask Task { get; set; }

        public IFormulaContext Context { get; set; }
    }

    public interface IFormMatrixSpecialProcedure {
        void Execute(Form matrix, ProcedureStage stage);
    }

    public enum ProcedureStage {
        None,
        Rows,
        Columns
    }
}