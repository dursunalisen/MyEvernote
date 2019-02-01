using MyEvernote.Entities2;
using MyEvernote.DataAccessLayer2.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEvernote.BusinessLayer2.Abstract;

namespace MyEvernote.BusinessLayer2
{
    public  class CategoryManager:ManagerBase<Category>

    {
        /*Generic bir class tanımladığımız için T yerine Category gelecek
        ManagerBase clasında ona görede alanlar doldurulacak Category'nin içinde ne varsa*/
       
       
    
       /* public override int Delete(Category obj)
        {
            //Note managerın içinden bir özellikle işlem yapacağımız için burada bir nesneye aldık NoteManager'
            NoteManager noteManager = new NoteManager();
            //likelarla ilgili işlem yapacağımız için like için bir nesne tanımladık
            LikedManager likedManager = new LikedManager();
            CommentManager commentManager = new CommentManager();
            foreach (Note note  in obj.Notes.ToList())
            {
                //likes tablo ismi likdein içindeki 
                foreach (Liked like in note.Likes.ToList())
                {
                    likedManager.Delete(like);   
                }
                foreach (Comment comment in note.Comments.ToList())
                {
                    commentManager.Delete(comment);
                }

                noteManager.Delete(note);
                
            }
            return base.Delete(obj);

        }*/
            
        //bu işlemi burada yapmamızın sebebi BusinesLayer iş class'ı genel bir class windowsta modilde
        //kullanıcağı zaman Business a bakıp uygun şekle bürünsün diye burada yapıyoruz*/
        
    }
}
