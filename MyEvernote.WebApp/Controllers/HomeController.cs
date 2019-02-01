using MyEvernote.BusinessLayer2;
using MyEvernote.Entities2;
using MyEvernote.Entities2.ValueObjects;
using MyEvernote.Entities2.Messages;
using MyEvernote.WebApp.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using MyEvernote.WebApp.ViewModels;
using System.Runtime.Remoting.Messaging;
using MyEvernote.BusinessLayer2.Results;

namespace MyEvernote.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        //category managerdeab categoryManager isimli bir nesne tanımladık
       private CategoryManager categoryManager = new CategoryManager();
        /*.Enitities2 nin içinde bulunan categorylerde liste çekmiştik o listeledeki İdleri almış olduk.
            category managerdaki GetCategoryById methodunun içinde bulunan Find özelliği sayesinde tıkladığımız id ile 
            listedeki id tutuyorsa işlemi gerçekleştirdik*/
     private   EvernoteUserManager EvernoteUserManager = new EvernoteUserManager();
        // GET: Home
        public ActionResult Index()
        {
            //category controller üzerinden gelen view talebi
           /* if (TempData["mm"] != null)
            {
                return View(TempData["mm"] as List<Note>);
            }*/
           
            //notları bu alıyoruz
          return View(noteManager.ListIQueryable().OrderByDescending(x=>x.ModifiedOn).ToList());
             //sqlden çekmeye yarıyoe bu kod
            //return View(nm.GetAllNoteQueryable().OrderByDescending(x => x.ModifiedOn).ToList());

        }
        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           



            Category cat = categoryManager.Find(x => x.Id==id.Value);
            if (cat == null)
            {
                return HttpNotFound();

            }
            return View("Index", cat.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }
        public ActionResult MostLiked()
        {
            return View("Index", noteManager.ListIQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }
        public ActionResult About()
        {
            return View();

        }
        
        public ActionResult ShowProfile()
        {
            EvernoteUser currentUser = Session["login"] as EvernoteUser;
           
            BusinessLayerResult<EvernoteUser> res = EvernoteUserManager.GetUserById(currentUser.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }
            return View(res.Result);
        }
        public ActionResult EditProfile()
        {
            EvernoteUser currentUser = Session["login"] as EvernoteUser;
            
            BusinessLayerResult<EvernoteUser> res = EvernoteUserManager.GetUserById(currentUser.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }
            return View(res.Result);
        }
        [HttpPost]
        public ActionResult EditProfile(EvernoteUser model,HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&

                  (ProfileImage.ContentType == "image/jpeg" ||
                      ProfileImage.ContentType == "image/jpg" ||
                      ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";
                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileİmageFilename = filename;


                }

             
                BusinessLayerResult<EvernoteUser> res = EvernoteUserManager.UpdateProfile(model);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi",
                        IsRedirectingUrl = "/Home/EditProfile"
                    };
                    return View("Error", errorNotifyObj);
                }

                Session["login"] = res.Result; //profil güncellendiği için sesion güncellendi
                return RedirectToAction("ShowProfile");

            }
            else
            {
                return View(model);
            }


        }
        public ActionResult DeleteProfile()
        {
           EvernoteUser currentUser=Session["login"] as EvernoteUser;
            
            BusinessLayerResult<EvernoteUser> res = EvernoteUserManager.RemoveUserById(currentUser.Id);
            if (res.Errors.Count>0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profile Silinemedi",
                    IsRedirectingUrl = "/Home/ShowProfile"
                };
                return View("Error" ,errorNotifyObj);
            }
            Session.Clear();
            return RedirectToAction("Index");
        }
       
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            //bu kısımda UI a özgü kodlar yer almalı(araştır)
            if (ModelState.IsValid)
            {
                
                BusinessLayerResult<EvernoteUser> res = EvernoteUserManager.LoginUser(model);

                if (res.Errors.Count>0)
                {
                    if (res.Errors.Find(x=>x.Code ==ErrorMessageCode.UserIsNotActive)!=null)
                    {
                        ViewBag.SetLink = "http://Home/Activate/1234-4567-7905";
                    }
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                Session["login"] = res.Result;
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
         //IsValid modelde hata olmadığını buluyor ve içerdeki işlemlere başlıyor
            if (ModelState.IsValid)
            {
                
                BusinessLayerResult<EvernoteUser> res = EvernoteUserManager.RegisterUser(model);
              

                if (res.Errors.Count > 0)
                {
                    //tüm errors listesinde dön hangi String(aşşığıdaki x onu belirtir)
                    //yani hata yakalandıysa model error e ekle 
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    IsRedirectingUrl = "/Home/Login",
                };
                notifyObj.Items.Add(" Hesabınıza ativasyon linki yolladık o linki tıklayarak aktif duruma geçip işlemlerinize devam edebilirsiniz");

                return View("Ok",notifyObj);

                
            }

            return View(model);
        }
    
        public ActionResult UserActive(Guid id)
        {

            BusinessLayerResult<EvernoteUser> res = EvernoteUserManager.ActivateUser(id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors
                };
                
                return View("Error",errorNotifyObj);
            }
            OkViewModel okNotifyObj = new OkViewModel()
            {
               Title="Hesap aktifleştirildi",
                IsRedirectingUrl="/Home/Login"
            };
            okNotifyObj.Items.Add("  Hesabınıza ativasyon işlemi tamammlanmıştır.Artık not paylaşabilirsiniz");
            return View("Ok",okNotifyObj);
        }
        
        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Index");

        }

    }
}