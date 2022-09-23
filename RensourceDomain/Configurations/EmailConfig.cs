using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Configurations
{
    public class EmailConfig
    {
        public string? From { get; set; }
        public string? SmtpServer { get; set; }
        public int Port { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Subject { get; set; }
        public string? DisplayName { get; set; }
        public string? RensourceEmailAddress { get; set; }

    }
}
