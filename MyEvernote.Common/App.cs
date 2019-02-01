using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Common
{//common ve core ortak kullanım alanın diğer kajetler için
    public static class App
    {
        public static ICommon Common = new DefaultCommon();
    }
}
