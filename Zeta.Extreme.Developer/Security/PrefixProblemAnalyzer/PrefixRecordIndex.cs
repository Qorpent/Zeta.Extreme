﻿using System.Collections.Generic;
using System.Linq;
using Qorpent.Serialization;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.Developer.Security.PrefixProblemAnalyzer {
    /// <summary>
    /// Индекс информации о предприятиях
    /// </summary>
    [Serialize]
    public class PrefixRecordIndex {

        /// <summary>
        /// Параметры формаирования индекса
        /// </summary>
        public PrefixRecordParameters Parameters { get; set; }

        private string[] _allPrefixes;
        /// <summary>
        /// Строит индекс по всем активным учетным записям
        /// </summary>
        /// <returns></returns>
        public static PrefixRecordIndex Build(PrefixRecordParameters paramters = null) {
            paramters = paramters ?? new PrefixRecordParameters();
            var result = new PrefixRecordIndex {Parameters = paramters};
            var users = new NativeZetaReader().ReadUsers("Active = 1");
            foreach (var user in users) {
                result.RegisterRecord(user);
            }
            return result;
        }
        /// <summary>
        /// Список префиксов
        /// </summary>
        [SerializeNotNullOnly]
        public string[] AllPrefixes {
            get {
                if (null == _allPrefixes) {
                    _allPrefixes =
                        _raw.Values.SelectMany(_ => _.PrefixRecords).Select(_ => _.Prefix).Distinct().ToArray();
                }
                return _allPrefixes;
            }
        }

        /// <summary>
        /// Регистрирует запись пользователя
        /// </summary>
        /// <param name="user"></param>
        public void RegisterRecord(IZetaUser user) {
            if (Parameters.IsMatch(user)) {
                var obj = user.Object;
                if (!_raw.ContainsKey(obj.Id)) {
                    _raw[obj.Id] = new PrefixObjectStats {ObjectId = obj.Id, ObjectName = obj.Name, Index = this};
                }
                var objs = _raw[obj.Id];
                var roles = RoleRecord.Collect(user.Roles);
                foreach (var roleRecord in roles) {
                    if (Parameters.IsMatch(roleRecord)) {
                        objs.Register(roleRecord);
                    }
                }
            }
        }


        IDictionary<int,PrefixObjectStats> _raw = new Dictionary<int, PrefixObjectStats>();
        private PrefixObjectStats[] _problems;
        /// <summary>
        /// Статистика объектов 
        /// </summary>
        [Serialize]
        public PrefixObjectStats[] ProblemPrefixObjects {
            get {
                if (null == _problems) {
                    _problems = _raw.Values.Where(_ => !Parameters.IsValid(_,AllPrefixes)).ToArray();
                }
                return _problems;
            }
        }

    }
}