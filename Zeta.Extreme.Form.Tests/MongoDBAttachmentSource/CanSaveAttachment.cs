using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeta.Extreme.Form.MongoDBAttachmentSource;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.Form.Tests.MongoDBAttachmentSource {
    public class CanSaveAttachment {

        public string SaveAttachment() {
            MongoDBAttachmentStorage mdba = new MongoDBAttachmentStorage();
            Attachment ex_att = new Attachment();
            
            ex_att.Uid = "Some_UID";
            ex_att.Comment = "Some_comment";
            ex_att.Name = "filename";
            ex_att.Size = 111;
            ex_att.Type = "fsffsdf";
            ex_att.User = "rrr";
            ex_att.MimeType = "addsd";
            ex_att.Extension = "some";
             
            mdba.AttachmentSave(ex_att, "C:/TestMongo.dat");
            return "Going Ok.";
        }

        
    }
}
