using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ecommerce_web.Models;
using ecommerce_web.Data;
using ecommerce_web.Repository.IRepository;
using ecommerce_web.Repository;


namespace ecommerce_web.Controllers;
public class CategoryController:Controller{
    private readonly IUnitOfWork _unitOfWork;
    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public IActionResult ProductList(){ 
        if(HttpContext.Session.GetString("Customer") == "")
        {
            TempData["Session"] = "";    
        }
        else{
            TempData["Session"] = Request.Cookies["User"];
        }     
        IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
        var SmartPhones = _unitOfWork.Inventory.GetSome(u=>u.Category.CategoryName == "SmartPhone");
        var Cameras = _unitOfWork.Inventory.GetSome(u=>u.Category.CategoryName == "Camera");
        var Laptops = _unitOfWork.Inventory.GetSome(u=>u.Category.CategoryName == "Laptop");
        var Headphones = _unitOfWork.Inventory.GetSome(u=>u.Category.CategoryName == "Headphone");

        var Details = new ProductView();
        Details.Categories = objCategoryList;
        Details.Smartphones = SmartPhones;
        Details.Cameras = Cameras;
        Details.Laptops = Laptops;
        Details.Headphones = Headphones;
        
        return View(Details);
    }
    [HttpPost]
    public IActionResult ProductList(int productId){
        Console.WriteLine("Reached");
        Console.WriteLine(productId);
        return RedirectToAction("Index","Product");
    }
    public IActionResult AdminCategoryView(){
        ViewBag.userCount = DataConnection.getUserCount();
        ViewBag.visitCount = Request.Cookies["Visit"];
        IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
        return View(objCategoryList);
    }
    [HttpGet]
    public IActionResult AdminCategoryAdd(){
        ViewBag.userCount = DataConnection.getUserCount();
        ViewBag.visitCount = Request.Cookies["Visit"];
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AdminCategoryAdd(Category categoryObject){
        Console.WriteLine(categoryObject.CategoryName);
        if(categoryObject.CategoryId == null){
            ModelState.AddModelError("CategoryId","The Category Id field is required");
        }
        
        if(ModelState.IsValid){
            _unitOfWork.Category.Add(categoryObject);
            _unitOfWork.Save();
            return RedirectToAction("AdminCategoryView");
        }
        return View();
    }

    [HttpGet]
    public IActionResult AdminCategoryUpdate(string? categoryid){
        ViewBag.userCount = DataConnection.getUserCount();
        ViewBag.visitCount = Request.Cookies["Visit"];
        if(categoryid == null){
            return NotFound();
        }
        var categoryObject = _unitOfWork.Category.GetFirstOrDefault(u => u.CategoryId == categoryid);
        if(categoryObject==null){
            return NotFound();
        }
        return View(categoryObject);
    }
    [HttpPost]
    public IActionResult AdminCategoryUpdate(Category category){
        if(ModelState.IsValid){
            // _unitOfWork.Category.Update(category);
            // _unitOfWork.Save();
            DataConnection.AdminCategoryUpdate(category);
            return RedirectToAction("AdminCategoryView");
        }
        return View(category);
    }

    [HttpGet]
    public IActionResult AdminCategoryDelete(string? categoryid){
        ViewBag.userCount = DataConnection.getUserCount();
        ViewBag.visitCount = Request.Cookies["Visit"];
        if(categoryid == null){
            return NotFound();
        }
        var categoryObject = _unitOfWork.Category.GetFirstOrDefault(u => u.CategoryId == categoryid);
        return View(categoryObject);
    }
    [HttpPost]
    public IActionResult AdminCategoryDelete(Category category){
        ViewBag.userCount = DataConnection.getUserCount();
        _unitOfWork.Category.Remove(category);
        _unitOfWork.Save();
        return RedirectToAction("AdminCategoryView");
    }    
}

