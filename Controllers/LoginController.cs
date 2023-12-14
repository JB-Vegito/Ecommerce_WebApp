using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using ecommerce_web.Models;
using Microsoft.AspNetCore.Http;

namespace ecommerce_web.Controllers{
    public class LoginController:Controller{
        private CookieOptions? userInfo;
        [HttpGet]
        public IActionResult LoginPage(){
        if(HttpContext.Session.GetString("Customer") == "")
        {
            TempData["Session"] = "";    
        }
        else{
            TempData["Session"] = Request.Cookies["User"];
        }
            return View();
        }
        [HttpGet]
        public IActionResult Signup(){
            return View();
        }
        [HttpGet]
        public IActionResult ForgotPassword(){
            return View();
        }
        [HttpPost]
        public IActionResult LoginPage(Users user){
            userInfo = new CookieOptions();
            int status = DataConnection.UserLogin(user);
            if(status==1){
                ModelState.AddModelError("CustomError","Username or Password is incorrect !!!");            
            }
            
            if(ModelState.IsValid){
                HttpContext.Session.SetString("Customer",user.username);
                Response.Cookies.Append("User",HttpContext.Session.GetString("Customer"),userInfo);
                return RedirectToAction("Index","Home");
            }
            return View();            
        }
        [HttpPost]
        public IActionResult Signup(Users user,string confirmPassword){
            userInfo = new CookieOptions();
            if(user.password != confirmPassword){
                Console.WriteLine("Password checked");
                Console.WriteLine(confirmPassword);
                ModelState.AddModelError("CustomError","Password does not match !!!");
            }
            if(ModelState.IsValid){
                DataConnection.UserRegistration(user);
                HttpContext.Session.SetString("Customer",user.username);
                Response.Cookies.Append("User",HttpContext.Session.GetString("Customer"),userInfo);
                return RedirectToAction("Index","Home");
            }            
            return View();
        }
        [HttpGet]
        public IActionResult Logout(){
            HttpContext.Session.SetString("Customer","");
            Response.Cookies.Delete("User");                
            return RedirectToAction("Index","Home");
        }
    }
}