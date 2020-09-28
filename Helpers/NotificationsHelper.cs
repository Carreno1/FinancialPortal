using Carreno_Financial_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carreno_Financial_Portal.Helpers
{
    public class NotificationsHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void ManageNotifications(BankAccount bank, Transaction trans)
        {
            ManageNotification(bank, trans);

        }

        private void ManageNotification(BankAccount bank, Transaction trans)
        {
            var overdraft = bank.CurrentBalance < 0;
            var warning = bank.CurrentBalance < bank.LowBalanceLevel;



            if (overdraft || warning)
            {


                var variableText = bank.CurrentBalance < 0 ? "an overdraft" : "a low balance level breach";
                var message = $"your recent transaction in the amount of {trans.Amount} for {trans.Memo} has resulted in {variableText}";
                GenerateNotification(message, bank.HouseholdId);

            }




        }

        private void GenerateNotification(string msg, int householdId)
        {
            var newNotification = new Notification
            {
                HouseholdId = householdId,
                Created = DateTime.Now,
                Subject = "Heads up something interesting happened in your account",
                Body = msg,
                IsRead = false
            };

            db.Notifications.Add(newNotification);
            db.SaveChanges();


        }
    }
}