using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiniBankApp.Models;
using Twilio;
using Twilio.Rest.Verify.V2.Service;
using System.Configuration;

namespace MiniBankApp.Controllers
{
    public class BankController : Controller
    {
        // ✅ Twilio settings from Web.config (recommended)
        private readonly string accountSid = ConfigurationManager.AppSettings["TwilioAccountSid"];
        private readonly string authToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
        private readonly string verifyServiceSid = ConfigurationManager.AppSettings["TwilioVerifyServiceSid"];

        public BankController()
        {
            TwilioClient.Init(accountSid, authToken);
        }

        public ActionResult Index() => View();
        public ActionResult AccountType() => View();
        public ActionResult RequiredDoc() => View();
        public ActionResult PanandAadhar() => View();
        public ActionResult ReggistredAdd() => View();
        public ActionResult AddFund() => View();
        public ActionResult sampleotp() => View();
        public ActionResult Transaction() => View();

        public ActionResult RegisteredAddress() => View();
        [HttpPost]
        public ActionResult RegisteredAddress(FormCollection form) => RedirectToAction("PermanentAdd");

        public ActionResult PermanentAdd() => View();
        [HttpPost]
        public ActionResult PermanentAdd(FormCollection form) => RedirectToAction("BranchSelection");

        public ActionResult BranchSelection() => View();

        [HttpPost]
        public ActionResult SelectAccountType(string accountType)
        {
            using (var db = new MyDbContext())
            {
                var selection = new AccountSelection
                {
                    AccountType = accountType,
                    SubmittedOn = DateTime.Now
                };
                db.AccountSelections.Add(selection);
                db.SaveChanges();
                Session["SelectedAccountType"] = accountType;
            }

            return RedirectToAction("RequiredDoc", "Bank");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KYCverification(KYCInfo model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new MyDbContext())
                {
                    db.KYCInfos.Add(model);
                    db.SaveChanges();
                }
                return RedirectToAction("PanandAadhar");
            }
            return View(model);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult KYCverification() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PanandAadhar(PanAadhaarInfo model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new MyDbContext())
                {
                    db.PanAadhaarInfos.Add(model);
                    db.SaveChanges();
                }
                TempData["CustomerName"] = model.Name;
                return RedirectToAction("Individual");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Individual() => View();

        [HttpPost]
        public ActionResult Individual(IndividualProfile profile)
        {
            if (ModelState.IsValid)
            {
                using (var db = new MyDbContext())
                {
                    db.IndividualProfiles.Add(profile);
                    db.SaveChanges();
                }
                return RedirectToAction("Business");
            }
            return View(profile);
        }

        [HttpGet]
        public ActionResult Business() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Business(BusinessProof model, HttpPostedFileBase GSTDocument, HttpPostedFileBase UDYAMDocument, HttpPostedFileBase AdditionalDocument)
        {
            if (ModelState.IsValid)
            {
                string uploadPath = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                if (GSTDocument != null && GSTDocument.ContentLength > 0)
                {
                    string gstFile = Path.GetFileName(GSTDocument.FileName);
                    GSTDocument.SaveAs(Path.Combine(uploadPath, gstFile));
                    model.GSTFilePath = "/Uploads/" + gstFile;
                }

                if (UDYAMDocument != null && UDYAMDocument.ContentLength > 0)
                {
                    string udyamFile = Path.GetFileName(UDYAMDocument.FileName);
                    UDYAMDocument.SaveAs(Path.Combine(uploadPath, udyamFile));
                    model.UDYAMFilePath = "/Uploads/" + udyamFile;
                }

                if (AdditionalDocument != null && AdditionalDocument.ContentLength > 0)
                {
                    string additionalFile = Path.GetFileName(AdditionalDocument.FileName);
                    AdditionalDocument.SaveAs(Path.Combine(uploadPath, additionalFile));
                    model.AdditionalFilePath = "/Uploads/" + additionalFile;
                }

                using (var db = new MyDbContext())
                {
                    db.BusinessProofs.Add(model);
                    db.SaveChanges();
                }

                string selectedType = Session["SelectedAccountType"]?.ToString();
                if (selectedType == "Individual")
                    return RedirectToAction("ReggistredAdd");
                else if (selectedType == "Sole Proprietorship")
                    return RedirectToAction("EntityProfile");
                else
                    return RedirectToAction("CompanyProfile");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult EntityProfile() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EntityProfile(EntityProfile model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new MyDbContext())
                {
                    db.EntityProfiles.Add(model);
                    db.SaveChanges();
                }
                return RedirectToAction("CompanyProfile");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult CompanyProfile() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompanyProfile(CompanyProfile model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new MyDbContext())
                {
                    db.CompanyProfiles.Add(model);
                    db.SaveChanges();
                }
                return RedirectToAction("ReggistredAdd");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BranchSelection(BranchBankDetails model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new MyDbContext())
                {
                    db.BranchBankDetails.Add(model);
                    db.SaveChanges();
                }
                return RedirectToAction("AddFund");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFund(FundTransaction model)
        {
            if (ModelState.IsValid)
            {
                model.TransactionDate = DateTime.Now;
                string accountNumber = "ACC" + new Random().Next(10000000, 99999999);
                string accountType = Session["SelectedAccountType"]?.ToString() ?? "Unknown";
                string customerName = "Customer";

                using (var db = new MyDbContext())
                {
                    var latestPan = db.PanAadhaarInfos.OrderByDescending(p => p.Id).FirstOrDefault();
                    if (latestPan != null) customerName = latestPan.Name;

                    db.FundTransactions.Add(model);
                    var account = new CustomerAccount
                    {
                        CustomerName = customerName,
                        AccountNumber = accountNumber,
                        AccountType = accountType,
                        Balance = model.Amount,
                        CreatedOn = DateTime.Now
                    };
                    db.CustomerAccounts.Add(account);
                    db.SaveChanges();
                }

                TempData["AccountNumber"] = accountNumber;
                TempData["CustomerName"] = customerName;
                TempData["AccountType"] = accountType;
                TempData["Balance"] = model.Amount;

                return RedirectToAction("PaymentSuccess");
            }
            return View(model);
        }

        public ActionResult PaymentSuccess() => View();

        // ✅ TWILIO OTP SEND
        [HttpPost]
        public JsonResult SendOtp(OtpRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Mobile))
            {
                return Json(new { success = false, message = "Mobile number is required." });
            }

            try
            {
                string formattedMobile = request.Mobile.StartsWith("+91") ? request.Mobile : "+91" + request.Mobile;
                Session["KycMobileNumber"] = formattedMobile;

                var verification = VerificationResource.Create(
                    to: formattedMobile,
                    channel: "sms",
                    pathServiceSid: verifyServiceSid
                );

                return Json(new { success = true, message = "OTP sent successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error sending OTP: " + ex.Message });
            }
        }

        // ✅ TWILIO OTP VERIFY
        [HttpPost]
        public JsonResult VerifyOtp(string otp)
        {
            string mobile = Session["KycMobileNumber"]?.ToString();
            if (string.IsNullOrEmpty(mobile))
            {
                return Json(new { success = false, message = "Mobile number not found in session." });
            }

            try
            {
                var verificationCheck = VerificationCheckResource.Create(
                    to: mobile,
                    code: otp,
                    pathServiceSid: verifyServiceSid
                );

                if (verificationCheck.Status == "approved")
                {
                    Session["KYC_OTP_Verified"] = true;
                    return Json(new { success = true, message = "OTP verified successfully!" });
                }

                return Json(new { success = false, message = "Invalid OTP. Please try again." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error verifying OTP: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult SendEmailOtp(string email)
        {
            try
            {
                var otp = new Random().Next(100000, 999999).ToString();
                Session["KYC_EmailOTP"] = otp;
                Session["KYC_Email"] = email;

                // ✅ Send via SMTP (or SendGrid/MailKit/etc.)
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.To.Add(email);
                message.Subject = "Your Email OTP for KYC Verification";
                message.Body = $"Your OTP is: {otp}";
                message.From = new System.Net.Mail.MailAddress("yourbank@example.com");

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.yourhost.com");
                smtp.Credentials = new System.Net.NetworkCredential("your_email", "your_password");
                smtp.EnableSsl = true;
                smtp.Port = 587; // or 465
                smtp.Send(message);

                return Json(new { success = true, message = "Email OTP sent!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult VerifyEmailOtp(string otp)
        {
            string sessionOtp = Session["KYC_EmailOTP"]?.ToString();
            if (otp == sessionOtp)
            {
                Session["KYC_EmailOTP_Verified"] = true;
                return Json(new { success = true, message = "Email OTP verified successfully." });
            }

            return Json(new { success = false, message = "Invalid Email OTP." });
        }

    }
}
