using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Carreno_Financial_Portal.Models;
using Carreno_Financial_Portal.ViewModels;
using Microsoft.AspNet.Identity;

namespace Carreno_Financial_Portal.Controllers
{
    [Authorize(Roles = "Owner")]
    public class InvitationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Invitations
        public ActionResult Index()
        {
            var invitations = db.Invitations.Include(i => i.Household);
            return View(invitations.ToList());
        }

        // GET: Invitations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            return View(invitation);
        }

        // GET: Invitations/Create
        public ActionResult Create()
        {
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name");
            return View();
        }

        // POST: Invitations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Invitation invitation)
        {
            var householdId = db.Users.Find(User.Identity.GetUserId()).HouseholdId;
            if (ModelState.IsValid)
            {
                invitation.Created = DateTime.Now;
                invitation.HouseholdId = (int)householdId;
                invitation.Code = Guid.NewGuid();
                invitation.Valid = true;

                db.Invitations.Add(invitation);
                db.SaveChanges();

                //Fires off Invitation through email
                var callbackUrl = Url.Action("AcceptInvite", "Invitations", new { email = invitation.RecipientEmail, code = invitation.Code }, protocol: Request.Url.Scheme);
       
                try
                {
                    //Spin up an instance of the EmailService Class and then call its SendEmailAsync method
                    var emailAddress = WebConfigurationManager.AppSettings["EmailFrom"];
                    var emailFrom = $"Carreno Financial Portal <{emailAddress}>";

                    var email = new MailMessage(emailFrom, invitation.RecipientEmail)
                    {
                        Subject = invitation.Subject,
                        Body = $"{invitation.Body} <hr/> Please accept this invitation by clicking <a href =\"{callbackUrl}\">here</a>",
                        IsBodyHtml = true
                    };

                    //build the Mail message that the SendEmailAsync method takes 
                    var emailSvc = new EmailService();
                    await emailSvc.SendAsync(email);



                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }



                return RedirectToAction("Dashboard", "Households");

            }

            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", invitation.HouseholdId);
            return View(invitation);
        }

        // GET: Invitations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", invitation.HouseholdId);
            return View(invitation);
        }

        // POST: Invitations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseholdId,Created,RecipientEmail,Code,Valid")] Invitation invitation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invitation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", invitation.HouseholdId);
            return View(invitation);
        }

        // GET: Invitations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            return View(invitation);
        }

        // POST: Invitations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invitation invitation = db.Invitations.Find(id);
            db.Invitations.Remove(invitation);
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
