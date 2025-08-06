using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.EntityLayer.Concrete
{
    public class FAQ
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; } = true;
        public int Order { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
} 