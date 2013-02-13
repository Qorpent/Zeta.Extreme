using System;
using Comdiv.Extensions;

namespace Zeta.Forms {
    /// <summary>
    /// Инкапсуляция ячейки формы
    /// </summary>
    public class Cell : FormElement {
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public Cell(Row row, Column column) {
            this.Form = row.Form;
            this.Row = row;
            this.Column = column;
            this.Row.Cells.Add(this);
            this.Column.Cells.Add(this);
            this.Form.Cells.Add(this);
        }

        /// <summary>
        /// Проверяет первичность ячейки
        /// </summary>
        /// <returns></returns>
        public bool IsPrimary() {
            if (!_cachedprimary.HasValue) {
                if (DirectPrimary.HasValue)
                {
                    _cachedprimary = DirectPrimary.Value;
                }
                else
                if (!Column.IsPrimary) {
                    _cachedprimary = false;
                }else
                _cachedprimary = this.Column.IsPrimary && this.Row.IsPrimary;
            }
            return _cachedprimary.Value;
        }

        /// <summary>
        /// Счет использования ячейки
        /// </summary>
        public int Usages;

        private string key;
        /// <summary>
        /// Ключ
        /// </summary>
        public string Key {
            get {
                if(null==key) {
                    this.key = this.Row.Code + "_" + this.Column.Code + "_" + this.Column.Year +"_"+ this.Column.Period;
                    key = key.ToUpper();
                }
                return key;
            }
        }

        /// <summary>
        /// Признак агергата
        /// </summary>
        /// <returns></returns>
        public bool IsAggregate() {
            return
                (this.Column.IsAggregate && this.Row.IsSupported())
                ||
                (this.Row.IsAggregate);
        }

        /// <summary>
        /// Колонка
        /// </summary>
        public Column Column { get; set; }

        /// <summary>
        /// Строка
        /// </summary>
        public Row Row { get; set; }

        /// <summary>
        /// Форма
        /// </summary>
        public Form Form { get; set; }

	    private bool _isEvaluated;
        /// <summary>
        /// Признак вычисленности
        /// </summary>
        public bool IsEvaluated {
            get { return _isEvaluated; }
            set { _isEvaluated = value; }
        }

	    private string _value;
        /// <summary>
        /// Значение
        /// </summary>
        public string Value {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Численное значение
        /// </summary>
        public decimal NumericValue {
            get { return Value.toDecimal(true); }
            set { Value = value.ToString(); }
        }

	    private int _cellId;
        private int _fix;
        private bool? _cachedprimary;

        /// <summary>
        /// Ид 
        /// </summary>
        public int CellId {
            get { return _cellId; }
            set { _cellId = value; }
        }

        /// <summary>
        /// Фиксация
        /// </summary>
        public int Fix {
            get { return _fix; }
            set { _fix = value; }
        }

        /// <summary>
        /// Признак прямой первичной строки
        /// </summary>
        public bool? DirectPrimary { get; set; }

        /// <summary>
        /// Признак ошибки
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Валюта
        /// </summary>
        public string Valuta { get; set; }
    }
}