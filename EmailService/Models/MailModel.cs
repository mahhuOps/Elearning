using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService.Models
{
    public class MailModel
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> To { get; set; }
    }
}
