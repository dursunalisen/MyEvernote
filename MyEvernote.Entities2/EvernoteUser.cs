using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities2
{
    [Table("EvernoteUsers")]
    public class EvernoteUser : MyEntityBase
    {
        [DisplayName("İsim"), StringLength(25, ErrorMessage = "{0} max {1} karakter olmalıdır")]
        public string Name { get; set; }
        [DisplayName("Soyisim"), StringLength(25)]
        public string Surname { get; set; }
        [DisplayName("Kullanıcı Adı"), Required(ErrorMessage ="{0} boş geçilemez"), StringLength(25)]
        public string Username { get; set; }
        [DisplayName("E-posta"), Required(ErrorMessage = "{0}  boş geçilemez"), StringLength(70)]

        public string Email { get; set; }
        [DisplayName("Sifre"), Required(ErrorMessage = "{0}  boş geçilemez"), StringLength(25)]
        public string Password { get; set; }
        [StringLength(30),ScaffoldColumn(false)]
        public string ProfileİmageFilename { get; set; }
        [DisplayName("IsActive")]
        public bool IsActive { get; set; }
        [Required,ScaffoldColumn(false)]
        public Guid ActivateGuid { get; set; }
        [Required]
        [DisplayName("IsAdmin")]
        public bool IsAdmin { get; set; }
        public virtual List<Note> Notes { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public virtual List<Liked> Likes { get; set; }
    }

}
