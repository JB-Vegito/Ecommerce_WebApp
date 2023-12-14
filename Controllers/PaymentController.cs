using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Razorpay.Api;
using ecommerce_web.Repository;
using ecommerce_web.Models;
using Newtonsoft.Json;

namespace ecommerce_web.Controllers
{
    public class PaymentController : Controller
    {
        private const string _key = "rzp_test_pEYVuE1azCpLML";
        private const string _secret = "ZZ2hCfPNF6925AnmUQyeLZiD";    
        private readonly IUnitOfWork _unitOfWork;
        public PaymentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }            

        // public ViewResult Registration()
        // {
        //     var model = new RegistrationModel() { Amount = 2 };
        //     return View(model);
        // }

        public ViewResult Payment(OrderDetails details)
        {
        if(HttpContext.Session.GetString("Customer") == "")
        {
            TempData["Session"] = "";    
        }
        else{
            TempData["Session"] = Request.Cookies["User"];
        }            
            OrderModel order = new OrderModel()
            {
                OrderAmount = details.FinalAmount/100,
                Currency = "INR",
                Payment_Capture = 1,    // 0 - Manual capture, 1 - Auto capture
                Notes = new Dictionary<string, string>()
                {
                    { "note 1", "first note while creating order" }, { "note 2", "you can add max 15 notes" },
                    { "note for account 1", "this is a linked note for account 1" }, { "note 2 for second transfer", "it's another note for 2nd account" }
                }
            };
            // var orderId = CreateOrder(order);
            var orderId = CreateTransfersViaOrder(order);
            Console.WriteLine(_key);
            Console.WriteLine(order.OrderAmount);
            Console.WriteLine(order.Currency);
            Console.WriteLine(orderId);
            Console.WriteLine(TempData["Username"].ToString());
            RazorPayOptionsModel razorPayOptions = new RazorPayOptionsModel()
            {
                Key = _key,
                AmountInSubUnits = order.OrderAmountInSubUnits,
                Currency = order.Currency,
                Name = "Skynet",
                Description = "for Dotnet",
                ImageLogUrl = "",
                OrderId = orderId,
                ProfileName = TempData["Username"].ToString(),
                // ProfileContact = registration.Mobile,
                // ProfileEmail = registration.Email,
                Notes = new Dictionary<string, string>()
                {
                    { "note 1", "this is a payment note" }, { "note 2", "here also, you can add max 15 notes" }
                }
            };
            return View(razorPayOptions);
        }

        private string CreateOrder(OrderModel order)
        {
        if(HttpContext.Session.GetString("Customer") == "")
        {
            TempData["Session"] = "";    
        }
        else{
            TempData["Session"] = Request.Cookies["User"];
        }            
            try
            {
                RazorpayClient client = new RazorpayClient(_key, _secret);
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", order.OrderAmountInSubUnits);
                options.Add("currency", order.Currency);
                options.Add("payment_capture", order.Payment_Capture);
                options.Add("notes", order.Notes);

                Order orderResponse = client.Order.Create(options);
                var orderId = orderResponse.Attributes["id"].ToString();
                return orderId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string CreateTransfersViaOrder(OrderModel order)
        {
        if(HttpContext.Session.GetString("Customer") == "")
        {
            TempData["Session"] = "";    
        }
        else{
            TempData["Session"] = Request.Cookies["User"];
        }            
            try
            {
                RazorpayClient client = new RazorpayClient(_key, _secret);
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", order.OrderAmountInSubUnits);
                options.Add("currency", order.Currency);
                options.Add("payment_capture", order.Payment_Capture);
                options.Add("notes", order.Notes);

                List<Dictionary<string, object>> transfers = new List<Dictionary<string, object>>();

                // Tranfer to Account 1
                Dictionary<string, object> transfer = new Dictionary<string, object>();
                transfer.Add("account", "acc_FrZdKIHffMifPl");              // account 1
                transfer.Add("amount", order.OrderAmountInSubUnits / 2);    // 50% amount of the total amount
                transfer.Add("currency", "INR");
                transfer.Add("notes", order.Notes);
                List<string> linkedAccountNotes = new List<string>();
                linkedAccountNotes.Add("note for account 1");
                transfer.Add("linked_account_notes", linkedAccountNotes);
                transfers.Add(transfer);

                // Transfer to Account 2
                transfer = new Dictionary<string, object>();
                transfer.Add("account", "acc_FrZbSTT96Jfp6n");              // account 2
                transfer.Add("amount", order.OrderAmountInSubUnits / 2);    // 50% amount of the total amount
                transfer.Add("currency", "INR");
                transfer.Add("notes", order.Notes);
                linkedAccountNotes = new List<string>();
                linkedAccountNotes.Add("note 2 for second transfer");
                transfer.Add("linked_account_notes", linkedAccountNotes);
                transfers.Add(transfer);

                // Add transfers to options object
                options.Add("transfers", transfers);

                Order orderResponse = client.Order.Create(options);
                var orderId = orderResponse.Attributes["id"].ToString();
                return orderId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ViewResult AfterPayment()
        {
        if(HttpContext.Session.GetString("Customer") == "")
        {
            TempData["Session"] = "";    
        }
        else{
            TempData["Session"] = Request.Cookies["User"];
        }            
            var paymentStatus = Request.Form["paymentstatus"].ToString();
            if (paymentStatus == "Fail")
                return View("Fail");

           
            else 
            {
                var cartList = _unitOfWork.Cart.GetSome(u => u.Username == HttpContext.Session.GetString("Customer"),includeProperties:"Inventory");
                string uniqueId = Guid.NewGuid().ToString();
                foreach(var item in cartList){
                    Orders order = new Orders();
                    order.Username = HttpContext.Session.GetString("Customer");
                    order.Product_Id = item.Product_Id;
                    order.Item_Quantity = item.Item_Quantity;
                    order.OrderId = uniqueId;

                    _unitOfWork.Order.Add(order);
                    _unitOfWork.Save();
                }                
                DataConnection.ClearCart(HttpContext.Session.GetString("Customer"));
                var cookie = Request.Cookies["Details"];
                if(cookie!=null){
                    var jsonString = cookie;
                    OrderDetails details = JsonConvert.DeserializeObject<OrderDetails>(jsonString);
                    details.OrderId = uniqueId;

                    Console.WriteLine(details.OrderId);
                    Console.WriteLine(details.Address);
                    Console.WriteLine(details.State);
                    Console.WriteLine(details.City);
                    Console.WriteLine(details.PinCode);
                    Console.WriteLine(details.FinalAmount);

                    _unitOfWork.Shipping.Add(details);
                    _unitOfWork.Save();
                    
                }

                var orderId = Request.Form["orderid"].ToString();
                var paymentId = Request.Form["paymentid"].ToString();
                var signature = Request.Form["signature"].ToString();

                var validSignature = CompareSignatures(orderId, paymentId, signature);
                ViewBag.Message = "Congratulations!! Your payment was successful";
                return View("Success");
            }
           
        }

        private bool CompareSignatures(string orderId, string paymentId, string razorPaySignature)
        {
        if(HttpContext.Session.GetString("Customer") == "")
        {
            TempData["Session"] = "";    
        }
        else{
            TempData["Session"] = Request.Cookies["User"];
        }            
            var text = orderId + "|" + paymentId;
            var secret = _secret;
            var generatedSignature = CalculateSHA256(text, secret);
            if (generatedSignature == razorPaySignature)
                return true;
            else
                return false;
        }

        private string CalculateSHA256(string text, string secret)
        {
        if(HttpContext.Session.GetString("Customer") == "")
        {
            TempData["Session"] = "";    
        }
        else{
            TempData["Session"] = Request.Cookies["User"];
        }            
            string result = "";
            var enc = Encoding.Default;
            byte[]
            baText2BeHashed = enc.GetBytes(text),
            baSalt = enc.GetBytes(secret);
            System.Security.Cryptography.HMACSHA256 hasher = new HMACSHA256(baSalt);
            byte[] baHashedText = hasher.ComputeHash(baText2BeHashed);
            result = string.Join("", baHashedText.ToList().Select(b => b.ToString("x2")).ToArray());
            return result;
        }

        public ViewResult Capture()
        {
        if(HttpContext.Session.GetString("Customer") == "")
        {
            TempData["Session"] = "";    
        }
        else{
            TempData["Session"] = Request.Cookies["User"];
        }            
            return View();
        }

        public ViewResult CapturePayment(string paymentId)
        {
        if(HttpContext.Session.GetString("Customer") == "")
        {
            TempData["Session"] = "";    
        }
        else{
            TempData["Session"] = Request.Cookies["User"];
        }            
            RazorpayClient client = new RazorpayClient(_key, _secret);
            Razorpay.Api.Payment payment = client.Payment.Fetch(paymentId);
            var amount = payment.Attributes["amount"];
            var currency = payment.Attributes["currency"];

            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", amount);
            options.Add("currency", currency);
            Razorpay.Api.Payment paymentCaptured = payment.Capture(options);
            ViewBag.Message = "Payment capatured!";
            return View("Success");
        }
    }
}