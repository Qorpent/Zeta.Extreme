using System;
using MongoDB.Bson;
using Zeta.Extreme.BizProcess.Forms;

namespace DbfsToMongo.Connector.Tests
{
    class DbfsToMongoConnectorTestsBase
    {
        public static Attachment GetNewAttach(string uid = null)
        {
            return new Attachment
            {
                Uid = ObjectId.GenerateNewId().ToString(),
                Name = "Name",
                Comment = "Comment",
                Revision = 115,
                Version = new DateTime(1, 1, 1, 1, 1, 1, 1),
                User = "User",
                MimeType = "MimeType",
                Extension = "Extension",
                Metadata = {
                    {
                        "m1", "v1"
                    }, {
                        "m2", "v2"
                    }
                }
            };
        }
    }
}