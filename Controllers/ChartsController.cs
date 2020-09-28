using Carreno_Financial_Portal.Models;
using Carreno_Financial_Portal.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Carreno_Financial_Portal.Controllers
{
    public class ChartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Charts
        public JsonResult GetCategoryChartData()
        {
            var dataVM = new CatDataVM();

            var householdId = db.Users.Find(User.Identity.GetUserId()).HouseholdId;

            foreach (var category in db.Categories.Where(c => c.HouseholdId == householdId).ToList())
            {
                //Now that I have a Category I need all the category Items
                var catData = new BarChartData
                {
                    Name = category.Name,
                    Target = category.TargetAmount,
               

                };

           
              
                foreach (var item in db.Categories.Find(category.Id).CategoryItems.ToList())
                {
                    var transactions = db.Transactions.Where(t => t.CategoryItemId == item.Id).ToList();

                    if (transactions.Count > 0)
                        catData.Actual += transactions.Sum(t => t.Amount);
                }

                dataVM.Data.Add(catData);
             
            }
            dataVM.Labels.AddRange(new List<string> { "Name", "Target", "Actual" });
            return Json(dataVM);
        }
    }
}


//  if ($('$normal-bar-graph').length) {
//                        $.ajax({
//    type: "POST",
//                            url: '@Url.Action("GetCategoryChartData", "Charts")',
//                            dataType: "json"
//                        }).then(function (response) {
//    Morris.Bar({
//        element: 'normal-bar-graph',
//                                data: response.Data,
//                                xkey: 'Name',
//                                ykeys:['Name', 'Target', 'Actual'],
//                                labels: response.labels

//                            });
//});
//                    }