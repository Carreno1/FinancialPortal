using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carreno_Financial_Portal.Models
{
    public class CategoryItem
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        //Nav Props
        public virtual Category Category { get; set; }
    }
}