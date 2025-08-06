using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.EntityLayer.Concrete
{
    public class Reference
    {
        public int Id { get; set; }
        public string NameSurname { get; set; }
        public string PositionName { get; set; }
        public string Comment { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public int Order { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
} 