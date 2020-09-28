using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Carreno_Financial_Portal.Helpers;
using Carreno_Financial_Portal.Models;
using Microsoft.AspNet.Identity;
using Carreno_Financial_Portal.Extensions;
using System.Threading.Tasks;

namespace Carreno_Financial_Portal.Controllers
{
    [Authorize]
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper(); 

        
        public ActionResult Dashboard()
        {
            var houseId = db.Users.Find(User.Identity.GetUserId()).HouseholdId;
            var house = db.Households.Include(h => h.BankAccounts).Include(h => h.Notifications).FirstOrDefault(h => h.Id == houseId);



            //Setup the neccessary data for wizard
            ViewBag.AccountType = new SelectList(db.BankAccountTypes, "Id", "Type");
            




            return View(house);
        }

        // GET: Households
        public ActionResult Index()
        {
            return View(db.Households.ToList());
        }

        // GET: Households/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // GET: Households/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string houseName, string greeting, string bankName, int accountType, string startingBalance, string warningBalance, string categoryName, string itemName)
        {
            //var newHouse = new Household
            //{
            //    Created = DateTime.Now,
            //};
            //if (ModelState.IsValid)
            //{
            //    db.Households.Add(household);
            //    db.SaveChanges();



            //}

            var newHouse = new Household 
            { 
                Created = DateTime.Now,
                Name = houseName, 
                Greeting = greeting 
            };

            db.Households.Add(newHouse);
            db.SaveChanges();
            //After creating a new House we need to do the following actions
            //Update the Users record to include the new household Id
            var user = db.Users.Find(User.Identity.GetUserId());
            user.HouseholdId = newHouse.Id;

            //Assign this user to the role of owner
            roleHelper.AddUserToRole(user.Id, "Owner");

            //Call the programmatic rauthorize extension method
            await HttpContextBaseExtension.RefreshAuthentication(HttpContext, user);

            var newBank = new BankAccount
            {
                HouseholdId = newHouse.Id,
                BankAccountTypeId = accountType,
                Created = DateTime.Now,
                StartingBalance = Convert.ToDecimal(startingBalance),
                CurrentBalance = Convert.ToDecimal(startingBalance),
                LowBalanceLevel = Convert.ToDecimal(warningBalance),
                Name = bankName,
                OwnerId = User.Identity.GetUserId()


            };
            db.BankAccounts.Add(newBank);
            db.SaveChanges();

            var newCategory = new Category
            {
                HouseholdId = newHouse.Id,
                Name = categoryName,
                Description = "Should have entered description in wizard",
                TargetAmount = 600
                
            };
            db.Categories.Add(newCategory);
            db.SaveChanges();

            var newItem = new CategoryItem
            {
                CategoryId = newCategory.Id,
                Name = itemName,
                Description = "",

            };
            db.CategoryItems.Add(newItem);
            db.SaveChanges();

            //Redirect to Dashboard
            return RedirectToAction("Dashboard");

        }

        // GET: Households/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Created,Name,Greeting")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(household);
        }

        // GET: Households/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}


    //$.ajax({
    //type: "Post",
    //        url: '@Url.Action("GetCategoryChartData", "Charts")',
    //        dataType: "json"

    //    }).then(function (response) {
    //new Morris.Bar({
    //            element: 'cat1Chart',
    //            data: [response.Data],
    //            xkey: 0,
    //            ykeys: response.YKeys,
    //            labels: response.Labels,
    //            barColors: response.BarColors,
    //            gridTextSize: 11,
    //            hideHover: 'auto',
    //            resize: true
    //        });

    //    });