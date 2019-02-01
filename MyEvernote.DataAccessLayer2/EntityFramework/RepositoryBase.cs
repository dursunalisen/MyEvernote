using MyEvernote.DataAccessLayer2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer2.EntityFramework
{
    public class RepositoryBase
    {
        //protected alanlar miras alınarak gelirler 
        protected static DatabaseContext context;
        private static object _lockSync = new object();
    
        //cunstructor'ın protected olması demek sadece miras alan new yapabilir anlamı taşır
        protected RepositoryBase()
        {
            //miras aldığımız için db=new.DatabaseContext(); satırına gidereke newleme yapacak
            CreateContext();
        }
        //newleme  işlemini bunun içinde yapıyoruz  singleton pattern diye geçer bu 
        private static void CreateContext()
        {
            /*böyle yapmamızın sebebi kontrol edip null ise
            bir kere databasecontexti dönmemizi sağlayacak
                eğer null değilse var olan databasecontext dönülecek*/
            
            if (context==null)
            {
                //çoklu projelerde sıraya koyar ortamı kitler önce bunu çalıştır 
                //Kilitleme yapılırsa nesne örneği kilitli olacağından,
                //oluşturulan ilk örneğin işleminin bitmesini bekler ve ikinci bir istek yapıldığında oluşturulan ilk örneği kullanır.
                lock (_lockSync)
                {
                        if (context==null)
                        {
                          context = new DatabaseContext();
                        }
                }
                
            }
        }
          
    }
}
