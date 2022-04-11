﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyNotes.BusinessLayer;
using MyNotes.BusinessLayer.Models;
using MyNotes.BusinessLayer.ValueObject;
using MyNotes.DataAccessLayer;
using MyNotes.EntityLayer;
using MyNotes.EntityLayer.Messages;
using MyNotes.MVC.ViewModel;


namespace MyNotes.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyNotesUserManager mum = new MyNotesUserManager();
        private readonly NoteManager nm = new NoteManager();
        private BusinessLayerResult<MyNotesUser> res;


        public ActionResult ByCategoryId(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            List<Note> notes=nm.QList().Where(s=>s.Category.Id==id && s.IsDraft == false).OrderByDescending(s => s.ModifiedOn).ToList();

            return View("Index",notes);
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                res = mum.LoginUser(model);
                if (res.Errors.Count>0)
                {
                    if (res.Errors.Find(x=>x.Code==ErrorMessageCode.UserIsNotActive)!=null)
                    {
                        ViewBag.SetLink = "http://Home/UserActivate/1234-2345-3456789";
                    }
                    res.Errors.ForEach(s=>ModelState.AddModelError("",s.Message));
                    return View(model);
                }

                //Session["Login"] = res.Result;
                CurrentSession.Set("Login", res.Result);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                MyNotesUserManager mum = new MyNotesUserManager();
                BusinessLayerResult<MyNotesUser> res = mum.RegisterUser(model);

                if (res.Errors.Count>0)
                {
                    res.Errors.ForEach(s=>ModelState.AddModelError("",s.Message));
                    return View(model);
                }
                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login"
                };
                notifyObj.Items.Add("Lütfen e-posta adresinize gönderdiğimiz aktivasyon link'ine tıklayarak hesabınızı aktive ediniz.");
                
                return View("Ok", notifyObj);
                //return RedirectToAction("Login");
            }

            return View(model);
        }
        public ActionResult RegisterOk()
        {
            return View();
        }

        public ActionResult UserActivate(Guid id)
        {
            res = mum.ActivateUser(id);
            if (res.Errors.Count>0)
            {
                TempData["errors"] = res.Errors;
                return RedirectToAction("UserActivateCancel");
            }
            return RedirectToAction("UserActivateOk");
        }
        public ActionResult UserActivateOk()
        {
            return View();
        }
        public ActionResult UserActivateCancel()
        {
            List<ErrorMessageObj> errors = null;
            if (TempData["errors"]!=null)
            {
                errors = TempData["errors"] as List<ErrorMessageObj>;
            }
            return View(errors);
        }

        public ActionResult ShowProfile()
        {
            if (CurrentSession.User is MyNotesUser currentUser) res = mum.GetUserById(currentUser.Id);
            if (res.Errors.Count>0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items=res.Errors
                };
                return View("Error",errorNotifyObj);
            }
             return View(res.Result);
        }

        public ActionResult Index()
        {
            //Test test = new Test();

            return View(nm.QList().OrderByDescending(s => s.ModifiedOn).ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult LogOut()
        {
            //Session.Clear();
            CurrentSession.Clear();
            return RedirectToAction("Index");
        }
    }
}