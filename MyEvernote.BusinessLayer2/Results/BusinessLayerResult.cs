using MyEvernote.Entities2.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer2.Results
{
    public class BusinessLayerResult<T> where T : class
    {
        //list türünden tanımladık ki birden çok hatayı içinde barındıra bilsin 

        public List<ErrorMessageObj> Errors { get; set; }
        public T Result { get; set; }

        public BusinessLayerResult()
        {
            //ilk oluşmada errors list kesin oluşsun diye new ile liste oluşturduk
            Errors = new List<ErrorMessageObj>();
        }
        public void AddError(ErrorMessageCode code,string message)
        {
            Errors.Add(new ErrorMessageObj() { Code=code,Message=message});
        }
    }
}
