using System.Collections.Generic;

namespace SendEmail.Models
{
    public class ServeSettings
    {
        public string Name { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public List<string> Patterns { get; set; }
    }
}