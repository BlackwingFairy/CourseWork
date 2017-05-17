using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_Client
{
    public class Record
    {
        public string name { get; set; }
        public string group { get; set; }
        public string subject { get; set; }
        public string mark { get; set; }

        public Record(string name, string group, string subject, string mark)
        {
            this.name = name;
            this.group = group;
            this.subject = subject;
            this.mark = mark;
        }
    }
}
