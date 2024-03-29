#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.BizProcess/RowDescriptor.cs
#endregion
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.BizProcess.Themas{
    /// <summary>
    /// ��������� ������
    /// </summary>
    public class RowDescriptor : DimensionDescriptor<IZetaRow, RowDescriptor>{
        /// <summary>
        /// 
        /// </summary>
        public RowDescriptor(){
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        public RowDescriptor(IZetaRow row){
            Target = row;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        public RowDescriptor(string code){
            Code = code;
        }


	    /// <summary>
	    /// ������ ������� �����
	    /// </summary>
	    /// <returns></returns>
	    public string GetNumberFormat()
        {
            if (null == this.Target) return NumberFormat;
            var customformat = TagHelper.Value(this.Target.Tag, "numberformat");
            if (customformat.IsNotEmpty()) return customformat;
            return NumberFormat;
        }
    }
}