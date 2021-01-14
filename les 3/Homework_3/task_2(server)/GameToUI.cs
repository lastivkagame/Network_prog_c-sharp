using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_2_server_
{
    [Serializable]
    public class GameToUI
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Owner { get; set; }

        public int GendreID { get; set; }

    }

    [Serializable]
    public class GendreToUI
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<GameToUI> Games { get; set; }
    }
}
