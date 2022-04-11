using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.EntityLayer
{
    [Table("tblNotes")]
    public class Note:BaseEntity
    {
        [StringLength(160),Required]
        public string Title { get; set; }
        [StringLength(2000), Required]
        public string Text { get; set; }
        public bool IsDraft { get; set; }
        public int LikeCount { get; set; }
        public int? CategoryId { get; set; }

        public virtual MyNotesUser Owner { get; set; }

        public virtual Category Category { get; set; }

        public virtual List<Comment> Comments { get; set; }= new List<Comment>();
        public virtual List<Liked> Likes { get; set; } = new List<Liked>();


        //eager loading: olusturacagim nesnede alt baglamlar var ise hepsini getirir.

        //ogranciler ve siniflar tablom var bunlarin arasinda foreign key baglantim var

        //ogrenciler listesi aldigimda eger eager loading kullanarak aliyorsam liste
        /*
         * ad
         * soyad
         * sinif[
         *          {
         *              sinifadi
         *              kat
         *              egitmen
         *          }
         *
         * ]
         *
         */

        //lazy loading : olusturulan nesene getirilir ilgili baglamlar yuklenmez
        //virtual olarak isaretlenirse lazy loading yapilmis olur
    }
}
