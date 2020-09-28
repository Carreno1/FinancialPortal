using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carreno_Financial_Portal.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public int HouseholdId { get; set; }
        public DateTime Created { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }

        public Boolean IsRead { get; set; }
        public virtual Household Household { get; set; }
    }
}