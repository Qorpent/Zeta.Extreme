using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using Zeta.Extreme.DbfsToMongo.Connector;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.DbfsToMongo.Wrapper
{
    /// <summary>
    ///
    /// </summary>
    public class DbfsToMongoWrapper
    {
        private const string GET_UIDS_QUERY = "SELECT code, size FROM [comdiv_dbfs].[usr_view]";
        private const string FIND_ATT_QUERY = "SELECT code, name, comment,version, usr, revision, mime, hash, size, _doctype as type, extension, _form, _year, _period, _obj FROM [comdiv_dbfs].[usr_view] WHERE code = '{0}'";

        private readonly DbfsToMongoConnector _connector;
        /// <summary>
        /// Connector copy for tests
        /// </summary>
        public DbfsToMongoConnector Сonnector;
        /// <summary>
        ///
        /// </summary>
        public IDictionary<string, Int64> attSignaturesDbfs;
        /// <summary>
        ///
        /// </summary>
        public IDictionary<string, Int64> attSignaturesMongo;
        /// <summary>
        ///
        /// </summary>
        public List<string> attNeedUpdate;

        /// <summary>
        ///
        /// </summary>
        public DbfsToMongoWrapper() {
            _connector = new DbfsToMongoConnector();
            CheckNeedUpdate();
        }

        private void GetAllUidsFromDbfs() {
            attSignaturesDbfs = new Dictionary<string, Int64>();

            using (var c = _connector.OpenConnection()) {

                var cmd = c.CreateCommand();
                cmd.CommandText = GET_UIDS_QUERY;

                using (var r = cmd.ExecuteReader()) {
                    while (r.Read()) {
                        attSignaturesDbfs.Add(r.GetString(0), r.GetInt64(1));
                    }
                }
            }
        }

        private void GetUidsAndLengthFromMongo() {
            attSignaturesMongo = new Dictionary<string, Int64>();

            var found = _connector.MongoDb.Find(new Attachment());

            foreach (var attachment in found) {
                attSignaturesMongo.Add(attachment.Uid, attachment.Size);
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void CheckNeedUpdate()
        {
            attNeedUpdate = new List<string>();
            Int64 length;

            GetAllUidsFromDbfs();
            GetUidsAndLengthFromMongo();

            foreach (var el in attSignaturesDbfs)
            {
                if (attSignaturesMongo.TryGetValue(el.Key, out length))
                {
                    if (length != el.Value)
                    {
                        attNeedUpdate.Add(el.Key);
                    }
                }
                else
                {
                    attNeedUpdate.Add(el.Key);
                }
            }
        }

        /// <summary>
        /// Получение следующего аттачмента из Dbfs
        /// </summary>
        /// <returns></returns>
        public Attachment GetNextAttachmentFromDbfs()
        {
            string uid = attNeedUpdate.FirstOrDefault();
            attNeedUpdate.Remove(uid);
            var clause = new Attachment();

            using (var c = _connector.OpenConnection())
            {

                var cmd = c.CreateCommand();

                cmd.CommandText = string.Format(FIND_ATT_QUERY, uid);

                using (var r = cmd.ExecuteReader())
                {

                    while (r.Read())
                    {
                        clause.Uid = r.GetString(0);
                        clause.Name = r.GetString(1);
                        clause.Comment = r.GetString(2);
                        clause.Version = r.GetDateTime(3);
                        clause.User = r.GetString(4);
                        clause.Revision = r.GetInt32(5);
                        clause.MimeType = r.GetString(6);
                        clause.Hash = r.GetString(7);
                        clause.Size = r.GetInt64(8);
                        if (!r.IsDBNull(9)) r.GetString(9);
                        clause.Extension = r.GetString(10);
                        clause.Metadata["template"] =  (!r.IsDBNull(11)) ? r.GetString(11) : "";
                        if (!r.IsDBNull(12)) {
                            clause.Metadata["year"] =
                                Convert.ToInt32(string.IsNullOrEmpty(r.GetString(12)) ? "0" : r.GetString(12));
                        }
                        else {
                            clause.Metadata["year"] = "";
                        }
                        if (!r.IsDBNull(13)) {
                            clause.Metadata["period"] =
                                Convert.ToInt32(string.IsNullOrEmpty(r.GetString(13)) ? "0" : r.GetString(13));
                        }
                        else {
                            clause.Metadata["period"] = "";
                        }

                        if (!r.IsDBNull(14)) {
                            var t = r.GetString(14);
                            if (string.IsNullOrEmpty(t)) t = "0";
                            clause.Metadata["obj"] = Convert.ToInt32(t);
                        }
                        else {
                            clause.Metadata["obj"] = "";
                        }
                    }
                }
            }

            return clause;
        }

        /// <summary>
        /// Сохранение аттачмента в MongoDB
        /// </summary>
        /// <param name="attachment">Представление аттачмета</param>
        private void PutAttachmentToMongoDb(Attachment attachment)
        {
            _connector.MongoDb.Save(attachment);
        }

        /// <summary>
        /// Прокачка данных из одного потока в другой
        /// </summary>
        /// <param name="attachment">Представление аттачмета</param>
        private void ProrollingBinaryFromDbfsToMongo(Attachment attachment)
        {
            using (var source = _connector.Dbfs.Open(attachment, FileAccess.Read))
            {
                using (var destination = _connector.MongoDb.Open(attachment, FileAccess.Write))
                {
                    source.CopyTo(destination);
                }
            }
        }

        private void MigrateAttachmentToMongo(Attachment attachment)
        {
            PutAttachmentToMongoDb(attachment);
            ProrollingBinaryFromDbfsToMongo(attachment);
        }

        /// <summary>
        /// Migrate next attachment to MongoDB
        /// </summary>
        /// <returns></returns>
        public Attachment MigrateNextAttachmentToMongo() {
            var attachment = GetNextAttachmentFromDbfs();

            if (attachment != null) {
                MigrateAttachmentToMongo(attachment);
            }

            return attachment;
        }

        /// <summary>
        /// find
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public IEnumerable<Attachment> Find(Attachment attachment) {
            return _connector.MongoDb.Find(attachment);
        }

        /// <summary>
        /// open
        /// </summary>
        /// <param name="attachment"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public Stream Open(Attachment attachment, FileAccess mode) {
            return _connector.MongoDb.Open(attachment, mode);
        }
    }
}