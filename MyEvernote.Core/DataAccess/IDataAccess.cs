using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Core.DataAccess
{
   public interface IDataAccess<T>
    {
        /*
        Bu alanı havuz gibi düşün diğer projelerde referens ettiğimiz için
        Diğer projelerin(BusinessLayer-DataAccessLAyer vb) içinde bu Class olmasa bile refere ederek ulaşbilir hala getirdik.
        UI katmanı yanı WebApp da Busines layer refere edildiği için Web App da Busines Layer sayesinde IDataAccess'e Ulaşmış oluyor
             
        */
         List<T> List();
        IQueryable<T> ListIQueryable();
         List<T> List(Expression<Func<T, bool>> where);

         int Insert(T obj);

         int Update(T obj);

         int Delete(T obj);

         int Save();

        T Find(Expression<Func<T, bool>> where);
        
    }
}
