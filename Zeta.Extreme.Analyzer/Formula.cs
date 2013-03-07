using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeta.Extreme.Analyzer
{
    /// <summary>
    /// Инкапсуляция формулы измерения (строки, колнки, объекта)
    /// </summary>
    public class Formula
    {
        private string _definition;
        /// <summary>
        /// Создает формулу с указанным строчным определением
        /// </summary>
        /// <param name="definition"></param>
        public Formula(string definition)
        {
            // TODO: Complete member initialization
            this._definition = definition;
        }
    }
}
