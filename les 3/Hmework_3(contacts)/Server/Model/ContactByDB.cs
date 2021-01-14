using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    [Serializable]
    public class ContactByDB
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string SurName { get; set; }

        public int Age { get; set; }

        public string City { get; set; }

        public string Telephone { get; set; }
        public byte[] ImageContact { get; set; }
    }
}
