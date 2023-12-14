using Microsoft.AspNetCore.Mvc;
using ecommerce_web.Models;
using ecommerce_web.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ecommerce_web.Controllers;
[Actions]
public class CartController:Controller{
    private static int id = 1;
    private readonly IUnitOfWork _unitOfWork;
    public CartController(IUnitOfWork unitOfWork)
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
        ViewBag.price = DataConnection.getCartPrice(HttpContext.Session.GetString("Customer"));
        // Console.WriteLine(price);
        TempData["Session"] = Request.Cookies["User"];
        return View();
    }
    
    [HttpGet]
    public IActionResult CartInsert(int? itemId){
        // Console.WriteLine("Customer: " + HttpContext.Session.GetString("Customer").GetType());
        if(HttpContext.Session.GetString("Customer") == null)
        {            
            TempData["Session"] = "";  
            return RedirectToAction("LoginPage","Login");
        }
        else{
            TempData["Session"] = Request.Cookies["User"];
        }        
        Inventory product = _unitOfWork.Inventory.GetFirstOrDefault(u=>u.Id==itemId);
        Console.WriteLine(HttpContext.Session.GetString("Customer"));
        Console.WriteLine(product.Id);
        if(HttpContext.Session.GetString("Customer")!=""){
         Cart cartItem = new Cart();
        // cartItem.Cart_Id = 1;
        cartItem.Username = HttpContext.Session.GetString("Customer");
        cartItem.Product_Id = product.Id;
        cartItem.Item_Quantity = 1;
        _unitOfWork.Cart.Add(cartItem);
        _unitOfWork.Save();       
        }
        ViewBag.price = DataConnection.getCartPrice(HttpContext.Session.GetString("Customer"));
        return RedirectToAction("Index");        
    }

    [HttpGet]
    public IActionResult CartUpdateUp(int id){
        if(HttpContext.Session.GetString("Customer") == "")
        {
            TempData["Session"] = "";  
            return RedirectToAction("LoginPage","Login");
        }
        else{
            TempData["Session"] = Request.Cookies["User"];
        }         
        // Cart cartItem = _unitOfWork.Cart.GetFirstOrDefault(u => u.Cart_Id == id);
        // _unitOfWork.Cart.Update(cartItem);
        // _unitOfWork.Save();
        DataConnection.updateQuantityUp(id);
        ViewBag.price = DataConnection.getCartPrice(HttpContext.Session.GetString("Customer"));
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult CartUpdateDown(int id){
        if(HttpContext.Session.GetString("Customer") == "")
        {
            TempData["Session"] = "";  
            return RedirectToAction("LoginPage","Login");
        }
        else{
            TempData["Session"] = Request.Cookies["User"];
        }         
        Cart cartItem = _unitOfWork.Cart.GetFirstOrDefault(u => u.Cart_Id == id);
        if(cartItem.Item_Quantity==1){
        _unitOfWork.Cart.Remove(cartItem);
        _unitOfWork.Save();            
        }
        // _unitOfWork.Cart.Update(cartItem);
        // _unitOfWork.Save();
        DataConnection.updateQuantityDown(id);
        ViewBag.price = DataConnection.getCartPrice(HttpContext.Session.GetString("Customer"));
        return RedirectToAction("Index");
    }    
    [HttpGet]
    public IActionResult CartDelete(int? id){
        if(HttpContext.Session.GetString("Customer") == "")
        {
            TempData["Session"] = "";  
            return RedirectToAction("LoginPage","Login");
        }
        else{
            TempData["Session"] = Request.Cookies["User"];
        }         
        Cart cartItem = _unitOfWork.Cart.GetFirstOrDefault(u => u.Cart_Id == id);                        
        _unitOfWork.Cart.Remove(cartItem);
        _unitOfWork.Save();
        ViewBag.price = DataConnection.getCartPrice(HttpContext.Session.GetString("Customer"));
        return RedirectToAction("Index");
    }


    #region API calls
    public IActionResult GetCart(){
        var cartList = _unitOfWork.Cart.GetSome(u => u.Username == HttpContext.Session.GetString("Customer"),includeProperties:"Inventory");
        return Json(new{data=cartList});
    }    
    #endregion
}