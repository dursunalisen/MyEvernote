using MyEvernote.DataAccessLayer2.EntityFramework;
using MyEvernote.Entities2;
using MyEvernote.Entities2.ValueObjects;
using MyEvernote.Entities2.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEvernote.Common.Helpers;
using System.Net.Mail;
using MyEvernote.BusinessLayer2.Results;
using MyEvernote.BusinessLayer2.Abstract;

namespace MyEvernote.BusinessLayer2
{
    public class EvernoteUserManager:ManagerBase<EvernoteUser>
    {
        public BusinessLayerResult<EvernoteUser> RegisterUser(RegisterViewModel data)
        {
            EvernoteUser user = Find(x => x.Username == data.Username || x.Email == data.Email);

            BusinessLayerResult<EvernoteUser> LayerResult = new BusinessLayerResult<EvernoteUser>();

            if (user != null)
            {
                if (user.Username==data.Username)
                {
                    LayerResult.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı");
                }
                if (user.Email==data.Email)
                {
                    LayerResult.AddError(ErrorMessageCode.EmailAlreadyExists, "E -mail adresi kayıtlı");
                }
                
            }
            else
            {
                //miras aldığın Insert'ü kullan dedik base.Insert yaparak
                int dbResult = base.Insert(new EvernoteUser()
                {
                    Username = data.Username,
                    Email = data.Email,
                    ProfileİmageFilename="user_boy.png",
                    Password = data.Password,
                    ActivateGuid = Guid.NewGuid(),
                    IsActive=false,
                    IsAdmin=false


                });

                if (dbResult >0)
                {   //busineslayerresult ın içindeki Result özelliğini aşşğıdaki işlem ile doldurduk
                    LayerResult.Result = Find(x => x.Username == data.Username && x.Email == data.Email);
                    //TODO : Aktivasyon Mail i atılacak 

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{LayerResult.Result.ActivateGuid}";
                    string body = $"Merhaba {LayerResult.Result.Username} ;<br> <br> Hesabınızı aktifleştirmek için <a href='{activateUri}'target='_blank'>tıklayınız </a>";

                    MailHelper.SendMail(body, LayerResult.Result.Email,"MyEvernote hesap aktifleştirildi");
                }
            }

            return LayerResult;
        }

        public BusinessLayerResult<EvernoteUser> GetUserById(int id)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = Find(x => x.Id==id);
            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound,"Kullanıcı bulunamadı");
            }

            return res;
        }

        public BusinessLayerResult<EvernoteUser> LoginUser(LoginViewModel data)
        {
            //business da çalışacak kodlar her yerde çalışır şekilde olmalı web windows vs lerde

            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = Find(x => x.Username == data.Username && (x.Password == data.Password));

            if (res.Result != null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı aktifleştirilmemiş.");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "E mail i kontrol et");

                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı adı ya da şifre hatalı");
            }
            return res;
        }

        public BusinessLayerResult<EvernoteUser> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = Find(x => x.ActivateGuid == activateId);

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExists, "Kullanıcı Zaten aktif edilememiştir.");
            }

            if (res.Result.IsActive)
            {
                res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı Zaten aktif edilmiştir.");
                return res;
            }
            if(res.Result !=null && !res.Result.IsActive)
            {
                Update(res.Result);
            }
            return res;
        }

        public BusinessLayerResult<EvernoteUser> UpdateProfile(EvernoteUser data)
        {
            EvernoteUser db_user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            if (db_user != null && db_user.Id != data.Id )
            {
                if (db_user.Username==data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "KKullanıcı adı kayıtlı");
                }
                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adı kayıtlı");
                }
                return res;
            }
            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;

            if (string.IsNullOrEmpty(data.ProfileİmageFilename)==false)
            {
                res.Result.ProfileİmageFilename = data.ProfileİmageFilename;

            }

            if (base.Update(res.Result)==0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil Güncelennemedi");
            }
            return res;
        }

        public BusinessLayerResult<EvernoteUser> RemoveUserById(int id)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            EvernoteUser user = Find(x => x.Id == id);
            if (user !=null)
            {
                if (Delete(user) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı Silinemedi");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı Bulunamadı");
            }
            return res;
        }

        //new ile Insert Methodunu gizledik -managerbasede de ınsert vardı onu ezmiş olduk override ile de ezebilirdik
        //fakat int türünden başk bir türe çeviremiyoruz override'ı Method hidding yaptık 
        public new BusinessLayerResult<EvernoteUser> Insert(EvernoteUser data)
        {
            EvernoteUser user = Find(x => x.Username == data.Username || x.Email == data.Email);

            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = data;

            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E -mail adresi kayıtlı");
                }

            }
            else
            {
                res.Result.ProfileİmageFilename = "ser_boy.png";
                res.Result.ActivateGuid = Guid.NewGuid();
                //miras aldığın Insert'ü kullan dedik base.Insert yaparak
                if (base.Insert(res.Result)==0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanıcı Eklenemedi");
    
                }


            }

            return res;
        }

        public new BusinessLayerResult<EvernoteUser> Update(EvernoteUser data)
        {

            EvernoteUser db_user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = data;
            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "KKullanıcı adı kayıtlı");
                }
                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adı kayıtlı");
                }
                return res;
            }
            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı Güncelennemedi");
            }
            return res;

        }
    }
}
