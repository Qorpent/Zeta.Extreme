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
// PROJECT ORIGIN: Zeta.Extreme.MongoDB.Integration/MongoBsonExtensions.cs
#endregion
using MongoDB.Bson;

namespace Zeta.Extreme.MongoDB.Integration {
    /// <summary>
    /// Расширения для сериали
    /// </summary>
    public static class MongoBsonExtensions {
        
        /// <summary>
        /// Обеспечивает NUll-safe доступ к значениям документа
        /// </summary>
        /// <param name="e"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static BsonValue SafeGet(this BsonDocument e, string name) {
            BsonValue value;

            if (
                (null == e)
                ||
                (!e.TryGetValue(name, out value))
                ||
                (null == value)
                ) {
                return "";
            }

            return value;
        }

       

    }
}