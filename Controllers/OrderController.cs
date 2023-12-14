using Microsoft.AspNetCore.Mvc;
using ecommerce_web.Models;
using ecommerce_web.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data;

namespace ecommerce_web.Controllers;
[Actions]
[ExceptionFilter]
public class OrderController:Controller{
    private CookieOptions? orderInfo;
    private readonly IUnitOfWork _unitOfWork;
    public OrderController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    [HttpGet]
    public IActionResult Index(){
        if(HttpContext.Session.GetString("Customer") == "")
        {
            TempData["Session"] = "";  
            return RedirectToAction("LoginPage","Login");
        }
        int price = DataConnection.getCartPrice(HttpContext.Session.GetString("Customer"));
        TempData["Session"] = Request.Cookies["User"];
        ViewBag.price = price;
        IEnumerable<string> states = new List<String> {"Kerala","TamilNadu","Karnataka"};
        IEnumerable<SelectListItem> stateItem = states.Select(s => new SelectListItem{
            Text = s,
            Value=s
        });
        IEnumerable<string> city = new List<String> {"Kochi","Trivandrum","Thrissur","Chennai","Coimbatore","Madurai","Bengaluru","Mysuru","Udupi"};        
        IEnumerable<SelectListItem> cityItem = city.Select(s => new SelectListItem{
            Text = s,
            Value=s
        });
        IEnumerable<string> kerala = new List<String> {"Kochi","Trivandrum","Thrissur"};  
        IEnumerable<SelectListItem> KeralaCity = kerala.Select(s => new SelectListItem{
            Text = s,
            Value=s
        });     

        IEnumerable<string> tamilnadu = new List<String> {"Chennai","Coimbatore","Madurai"};  
        IEnumerable<SelectListItem> TamilNaduCity = tamilnadu.Select(s => new SelectListItem{
            Text = s,
            Value=s
        });

        IEnumerable<string> karnataka = new List<String> {"Bengaluru","Mysuru","Udupi"};  
        IEnumerable<SelectListItem> KarnatakaCity = karnataka.Select(s => new SelectListItem{
            Text = s,
            Value=s
        });                
        ViewBag.states = stateItem;
        ViewBag.defaultCities = cityItem;
        ViewBag.Kerala = KeralaCity;
        ViewBag.TamilNadu = TamilNaduCity;
        ViewBag.Karnataka = KarnatakaCity;

        // var cartList = _unitOfWork.Cart.GetSome(u => u.Username == HttpContext.Session.GetString("Customer"),includeProperties:"Inventory");
        // string uniqueId = Guid.NewGuid().ToString();
        // foreach(var item in cartList){
        //     Orders order = new Orders();
        //     order.Username = HttpContext.Session.GetString("Customer");
        //     order.Product_Id = item.Product_Id;
        //     order.Item_Quantity = item.Item_Quantity;
        //     order.OrderId = uniqueId;

        //     _unitOfWork.Order.Add(order);
        //     _unitOfWork.Save();
        // }        
        return View();
    }    

    [HttpPost]
    public IActionResult Index(OrderDetails details,string stateValue){
        if(HttpContext.Session.GetString("Customer") == ""){
            return NotFound();
        }
        Console.WriteLine("Setting state");
        if(details.City == "Kochi" || details.City == "Trivandrum" || details.City=="Thrissur"){
            details.State = "Kerala";
        }
        else if(details.City == "Chennai" || details.City == "Coimbatore" || details.City=="Madurai"){
            details.State = "TamilNadu";
        }  
        else if(details.City == "Bengaluru" || details.City == "Udupi" || details.City=="Mysuru"){
            details.State = "Karnataka";
        }              

        details.OrderDate = DateTime.UtcNow;

            Console.WriteLine("Entered");
            orderInfo = new CookieOptions();
            var jsonString = JsonConvert.SerializeObject(details);
            Response.Cookies.Append("Details",jsonString,orderInfo);
            TempData["Username"]=HttpContext.Session.GetString("Customer");
            return RedirectToAction("Payment","Payment",details);            

        // return View();
    }

    [HttpGet]
    public IActionResult AdminOrderView(){
        IEnumerable<OrderDetails> details = _unitOfWork.Shipping.GetSome(u => u.Shipping == null);
        if(details!=null){
            List<string> ShippingStatus = new List<string>{"Not shipped","Shipped"};
            ViewBag.ShippingStatus = new SelectList(ShippingStatus);
            return View(details);
        }
        return NotFound();
    }
    
        public IActionResult AdminOrderUpdate(string? orderid){
        Console.WriteLine("REDIRECTED TO ADMIN ORDER");
        Console.WriteLine(orderid);
        OrderDetails details = _unitOfWork.Shipping.GetFirstOrDefault(u => u.OrderId == orderid);
        if(details!=null){
            details.Shipping = "Shipped";
            DataConnection.updateOrder(details);
            return RedirectToAction("AdminOrderView");
        }
        return NotFound();
    }

    [HttpGet]
    public IActionResult AdminShippedOrders(){          
        IEnumerable<OrderDetails> details = _unitOfWork.Shipping.GetSome(u => u.Shipping != null);
        if(details!=null){
            List<string> ShippingStatus = new List<string>{"Not shipped","Shipped"};
            ViewBag.ShippingStatus = new SelectList(ShippingStatus);
            return View(details);
        }
        return NotFound();
    }
    [HttpGet]    
    public IActionResult ViewOrderlist(string? order){
        if(HttpContext.Session.GetString("Customer") == "")
        {
            TempData["Session"] = "";  
            return RedirectToAction("LoginPage","Login");
        }   
        else{
            TempData["Session"] = Request.Cookies["User"];
        }               
        Console.WriteLine("Reached Orderlist");
        if(order==null) return NotFound();

        IEnumerable<Orders> orders = _unitOfWork.Order.GetSome(u => u.OrderId == order, includeProperties:"Inventory");

        foreach(var item in orders){
            Console.WriteLine(item.Inventory.Name);
            Console.WriteLine(item.Item_Quantity);
        }

        return View(orders);
    }
    [HttpGet]
    public IActionResult UserOrders(){
        if(HttpContext.Session.GetString("Customer") == "")
        {
            TempData["Session"] = "";  
            return RedirectToAction("LoginPage","Login");
        } 
        else{
            TempData["Session"] = Request.Cookies["User"];
        }       
        DataSet order = new DataSet();
        order = DataConnection.UserOrder(HttpContext.Session.GetString("Customer"));
        return View(order);
    }
}