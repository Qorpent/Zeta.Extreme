﻿using MongoDB.Bson;

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