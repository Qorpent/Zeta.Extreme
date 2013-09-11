using System;
using System.Collections.Generic;
using System.Linq;
using Qorpent.Applications;
using Qorpent.Data;
using Qorpent.Mvc;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.Developer.Security.AdminAnalyzer {
    /// <summary>
    /// Класс, выполняющий анализ настройки администраторов
    /// </summary>
    public class AdminAnalyzerHelper {
        private IMvcContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public AdminAnalyzerHelper(IMvcContext context=null) {
            _context = context;
        }
        /// <summary>
        /// Формирует список записей для каждого предприятия
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AdminAnalyzerRecord> Collect() {
            var objects = ObjCache.ObjById.Values.Where(_ => _.GroupCache.Contains("/G1/"));
            foreach (var o in objects) {
                yield return GetAdminAnalyzerRecord(o);
            }
        }

        private AdminAnalyzerRecord GetAdminAnalyzerRecord(IZetaMainObject o) {
            var result = new AdminAnalyzerRecord {Index = o.Index, ObjectId = o.Id, ObjectName = o.Name};
            ReadAdmins(o, result);
            return result;
        }

        private static void ReadAdmins(IZetaMainObject o, AdminAnalyzerRecord result) {
            var users = new NativeZetaReader().ReadUsers("ObjId = " + o.Id + " and ObjAdmin = 1 and Active = 1").ToArray();
            result.ActiveAdminCount = users.Length;
            foreach (var u in users) {
                result.Admins.Add(new AdminRecord { Login = u.Login, Name = u.Name, Occupation = u.Occupation, SlotList = u.SlotList});
            }
            GetAttachState(o, result);
        }

        private static void GetAttachState(IZetaMainObject o, AdminAnalyzerRecord result) {
            var docstorage = Application.Current.Container.Get<IDocumentStorage>().SetContext("zetadoc", "fs.files");
            var query = "{'metadata.TargetEntityCode':'" + o.Code + "', 'metadata.FileType':'PRIKAZ'}";
            var queryoptions = new DocumentStorageOptions {Limit = 1, Fields = new[] {"_id"}};
            var docresult = docstorage.ExecuteQuery(query, queryoptions);
            var doc = docresult.Element("doc");
            if (null != doc) {
                result.FileAttached = true;
                result.FileUid = doc.Attr("_id");
            }
        }
    }
}