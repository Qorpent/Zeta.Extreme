using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeta.Extreme.Form.MongoDBAttachmentSource;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.Form.Tests.MongoDBAttachmentSource {
    public class CanOpenStream {

        public string CanOpen() {
            //MongoDBAttachment mdba = new MongoDBAttachment();
            Attachment ex_att = new Attachment();
            
            ex_att.Uid = "Some_UID";
            ex_att.Comment = "Some_comment";
            return "Going Ok.";

           // mdba.Open(ex_att, FileAccess.ReadWrite);
        }

        
    }
}
