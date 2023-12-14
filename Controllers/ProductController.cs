using Microsoft.AspNetCore.Mvc;
using ecommerce_web.Models;
using ecommerce_web.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ecommerce_web.Controllers;
[Actions]
[ExceptionFilter]
public class ProductController:Controller{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _hostEnvironment;
    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _hostEnvironment = hostEnvironment;
    }

    [HttpGet]
    public IActionResult Index(){
        if(HttpContext.Session.GetString("Customer") == "")
        {
            TempData["Session"] = "";    
        }
        else{
            TempData["Session"] = Request.Cookies["User"];
        }
        IEnumerable<Inventory> objProductList = _unitOfWork.Inventory.GetAll();
        return View(objProductList);
    }
    [HttpGet]
    public IActionResult ProductPage(string? categoryType=null, string? priceRange=null){
        Console.WriteLine(categoryType);
        Console.WriteLine(priceRange);
        if(HttpContext.Session.GetString("Customer") == "")
        {
            TempData["Session"] = "";    
        }
        else{
            TempData["Session"] = Request.Cookies["User"];
        }
        IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(
            u => new SelectListItem{
                Text=u.CategoryName,
                Value=u.CategoryId
            }
        );
        ViewBag.CategoryList = CategoryList;        
        IEnumerable<Inventory> products = _unitOfWork.Inventory.GetAll();
        return View(products);
    }
    [HttpGet]
    public IActionResult AdminProductView(){
        ViewBag.userCount = DataConnection.getUserCount();
        // TempData["Session"] = Request.Cookies["User"];
        return View();
    }
    [HttpGet]
    public IActionResult AdminProductInsert(){
        ViewBag.userCount = DataConnection.getUserCount();
        // TempData["Session"] = Request.Cookies["User"];
        Inventory upsertProduct = new();
        IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(
            u => new SelectListItem{
                Text=u.CategoryName,
                Value=u.CategoryId
            }
        );
        ViewBag.CategoryList = CategoryList;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AdminProductInsert(Inventory product, IFormFile file){
        if(ModelState.IsValid){
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if(file != null){
                string filename = Guid.NewGuid().ToString();
                var upload = Path.Combine(wwwRootPath,@"images\");
                var extension = Path.GetExtension(file.FileName);

                using(var fileStreams = new FileStream(Path.Combine(upload,filename+extension),FileMode.Create)){
                    file.CopyTo(fileStreams);
                }
                product.Image=@"\images\"+filename+extension;
            }
            _unitOfWork.Inventory.Add(product);
            _unitOfWork.Save();        
            return RedirectToAction("AdminProductView");
        }
        return View(product);
    }

    [HttpGet]
    public IActionResult AdminProductUpdate(int? id){
        ViewBag.userCount = DataConnection.getUserCount();
        // TempData["Session"] = Request.Cookies["User"];
        // Inventory upsertProduct = new();
        Inventory product = _unitOfWork.Inventory.GetFirstOrDefault(u => u.Id == id);
        IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(
            u => new SelectListItem{
                Text=u.CategoryName,
                Value=u.CategoryId
            }
        );
        ViewBag.CategoryList = CategoryList;
        return View(product);
    }   

    [HttpPost]
    public IActionResult AdminProductUpdate(Inventory product, IFormFile file){
        Console.WriteLine("Reached function");
        // Console.WriteLine(product.Id);
        if(ModelState.IsValid){
            Console.WriteLine("Is valid");
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if(file != null){
                string filename = Guid.NewGuid().ToString();
                var upload = Path.Combine(wwwRootPath,@"images\");
                var extension = Path.GetExtension(file.FileName);

                using(var fileStreams = new FileStream(Path.Combine(upload,filename+extension),FileMode.Create)){
                    file.CopyTo(fileStreams);
                }
                product.Image=@"\images\"+filename+extension;
            }
            // _unitOfWork.Inventory.Update(product);
            // _unitOfWork.Save();        
            // return RedirectToAction("AdminProductView");
        }
        else{
            Console.WriteLine(product.Id);
            DataConnection.AdminProductUpdate(product);
            // return RedirectToAction("AdminProductView");
        }
        return RedirectToAction("AdminProductView");
    }

    [HttpGet]
    public IActionResult AdminProductDelete(int? id){
        ViewBag.userCount = DataConnection.getUserCount();
        // TempData["Session"] = Request.Cookies["User"];
        Inventory product = _unitOfWork.Inventory.GetFirstOrDefault(u => u.Id == id);
        IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(
            u => new SelectListItem{
                Text=u.CategoryName,
                Value=u.CategoryId
            }
        );
        ViewBag.CategoryList = CategoryList;
        return View(product);        
    }
    [HttpPost]
    public IActionResult AdminProductDelete(Inventory product){
        _unitOfWork.Inventory.Remove(product);
        _unitOfWork.Save();        
        return RedirectToAction("AdminProductView");
    }
    [HttpGet]
    public IActionResult ProductView(int? productId){
        ViewBag.userCount = DataConnection.getUserCount();
        if(HttpContext.Session.GetString("Customer") == "")
        {
            TempData["Session"] = "";    
        }
        else{
            TempData["Session"] = Request.Cookies["User"];
        }
        var product = _unitOfWork.Inventory.GetFirstOrDefault(u => u.Id == productId);
        return View(product);
    }

#region API CALLS
[HttpGet]
public IActionResult GetAll(){
    var productList = _unitOfWork.Inventory.GetAll(includeProperties:"Category");
    return Json(new{data=productList});
}

public IActionResult GetSmartPhones(){
    var productList = _unitOfWork.Inventory.GetSome(u => u.Category.CategoryName == "Smartphone",includeProperties:"Category");
    return Json(new{data=productList});
}
#endregion
}

