using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEvernote.WebApp.ViewModels
{
    public class NotifyViewModelBase<T>
    {
        public List<T> Items { get; set; }
        public string Header { get; set; }
        public string Title { get; set; }
        public bool IsRedirecting { get; set; }
        public string IsRedirectingUrl { get; set; }
        public int IsRedirectingTimeout { get; set; }

        public NotifyViewModelBase()
        {
            Header = "Yönlendiriliyorsunuz";
            Title = "Geçersiz İlem";
            IsRedirecting = true;
            IsRedirectingUrl = "/Home/Index";
            IsRedirectingTimeout = 10000;
            Items = new List<T>();
        }
    }
}