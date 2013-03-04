// // Copyright 2007-2010 Comdiv (F. Sadykov) - http://code.google.com/u/fagim.sadykov/
// // Supported by Media Technology LTD 
// //  
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //  
// //      http://www.apache.org/licenses/LICENSE-2.0
// //  
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// // 
// // MODIFICATIONS HAVE BEEN MADE TO THIS FILE

using Comdiv.Extensions;
using Comdiv.Olap.Model;

#if NEWMODEL
using Comdiv.Extensions;
using Comdiv.Olap.Model;

#endif

namespace Zeta.Extreme.Poco.NativeSqlBind{
    /// <summary>
    /// ��������������� ����� ��� ������ � ���������
    /// </summary>
    public static class IWithFormulaExtensions{
        /// <summary>
        /// Null-safe formula
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public static string Formula(this IWithFormula formula){
            if (null==formula){
                return null;
            }
            return formula.Formula;
        }
		/// <summary>
		/// null-safe formula type
		/// </summary>
		/// <param name="formula"></param>
		/// <returns></returns>
        public static string FormulaEvaluator(this IWithFormula formula){
            if (null==formula){
                return null;
            }
            return formula.FormulaEvaluator;
        }
		/// <summary>
		/// null-safe parsed formula
		/// </summary>
		/// <param name="formula"></param>
		/// <returns></returns>
        public static string ParsedFormula(this IWithFormula formula){
            if (formula.no()){
                return null;
            }
            return formula.ParsedFormula;
        }
		/// <summary>
		/// null-safe is formula
		/// </summary>
		/// <param name="formula"></param>
		/// <returns></returns>
        public static bool IsFormula(this IWithFormula formula){
            if (formula.no()){
                return false;
            }
            return formula.IsFormula;
        }
    }
}