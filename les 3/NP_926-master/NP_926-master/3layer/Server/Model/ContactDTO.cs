using System;

namespace Server.Model
{

    [Serializable]
    public class ContactDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
