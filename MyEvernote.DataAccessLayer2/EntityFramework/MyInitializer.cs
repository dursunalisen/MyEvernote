using MyEvernote.Entities2;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer2.EntityFramework
{
   public class MyInitializer:CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            //admin user ekleme 
            EvernoteUser admin = new EvernoteUser()
            {

                Name = "Dursun",
                Surname = "ŞEN",
                Email="dursunalisen@gmail.com",
                ActivateGuid=Guid.NewGuid(),
                IsActive=true,
                IsAdmin=true,
                Username="dursunalisen",
                ProfileİmageFilename="user_boy.png",
                Password="123456",
                CretedOn=DateTime.Now,
                ModifiedOn=DateTime.Now.AddMinutes(5),
                ModifiedUsername="dursunalisen"
          
            };
            //standart user ekleme 
            EvernoteUser standartUser = new EvernoteUser()
            {

                Name = "Ali",
                Surname = "ŞEN",
                Email = "alisen@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "alisen",
                ProfileİmageFilename = "user_boy.png",
                Password = "654321",
                CretedOn = DateTime.Now.AddHours(1),
                ModifiedOn = DateTime.Now.AddMinutes(65),
                ModifiedUsername = "dursunalisen"

            };
            context.EvernoteUsers.Add(admin);
            context.EvernoteUsers.Add(standartUser);
            for (int i = 0; i < 8; i++)
            {
                EvernoteUser user = new EvernoteUser()
                {

                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    // bu şekilde değişken adı kullanılablir 
                    Username = $"user{i}",
                    ProfileİmageFilename = "user_boy.png",
                    Password = "123",
                    CretedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = $"user{i}"

                };
                context.EvernoteUsers.Add(user);


            }
            context.SaveChanges();
            //user list for using
            List<EvernoteUser> userlist = context.EvernoteUsers.ToList();

            //fake kategori oluşturuyoruz
            for (int i = 0; i < 10; i++)
            {
                Category cat = new Category()
                {

                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CretedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUsername = "dursunalisen",
                };
                context.Categories.Add(cat);
                //note eklemek
                for (int k = 0; k < FakeData.NumberData.GetNumber(5,9); k++)
                {
                    EvernoteUser owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];
                    Note note = new Note()
                    {

                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1, 9),
                        Owner = owner,
                        CretedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = owner.Username,
                    };
                    cat.Notes.Add(note);
                    //Comments eklemek
                    for (int j = 0; j < FakeData.NumberData.GetNumber(3,5); j++)
                    {
                        EvernoteUser comment_owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];
                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                          
                            Owner =comment_owner,
                            CretedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUsername = comment_owner.Username
                        };
                        note.Comments.Add(comment);
                        //like ekleme
                        for (int m = 0; m < note.LikeCount; m++)
                        {
                            Liked liked = new Liked()
                            {
                                LikedUser = userlist[m]

                            };
                            note.Likes.Add(liked);
                        }
                    }

                }
                context.SaveChanges();
            }

        }
    }
}
