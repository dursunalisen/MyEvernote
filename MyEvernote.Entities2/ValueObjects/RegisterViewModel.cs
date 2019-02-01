using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEvernote.Entities2.ValueObjects
{
    //EvernoteUserdan miras almamamızın sebebi bu sayfada RePassword olması 
    //veri tabanınada gereksiz yere bir kolon açaçak ama şifre tekrar girilme işlemi sadece bu sayfaya özgü olmalı
    public class RegisterViewModel
    {
        [DisplayName("Kullanıcı adı"),
            Required(ErrorMessage = "{0} alanı boş geçilemez"), 
            StringLength(25, ErrorMessage = "{0} max. {1} karakter olmalı")]

        public string Username { get; set; }
        [DisplayName("E posta"), 
            Required(ErrorMessage = "{0} alanı boş geçilemez"),
            StringLength(70, ErrorMessage = "{0} max. {1} karakter olmalı"),
            EmailAddress(ErrorMessage ="{0} alanı için geçerli bir mail girin")]
         
        public string Email { get; set; }
        [DisplayName("Sifre"),
             Required(ErrorMessage = "{0} alanı boş geçilemez"), 
             StringLength(25, ErrorMessage = "{0} max. {1} karakter olmalı")]

        public string Password { get; set; }
        [DisplayName("Sifre Tekrarı"),
            Required(ErrorMessage = "{0} alanı boş geçilemez"),
            StringLength(25, ErrorMessage = "{0} max. {1} karakter olmalı"),
            Compare("Password",ErrorMessage ="{0} ile {1} uyuşmuyor")]
        //yukarıdaki açıklamanın içindeki {} işlemler alan adlarını alır 
        public string RePassword { get; set; }
    }
}