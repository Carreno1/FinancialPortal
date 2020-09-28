using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Carreno_Financial_Portal.ViewModels
{
    public class BarChartData
    {
        public string Name { get; set; }
        public decimal Target { get; set; }
        public decimal Actual { get; set; }

    }

    public class CatDataVM
    {
            public List<BarChartData> Data = new List<BarChartData>();
            public List<string> Labels = new List<string>();
    }


    

}