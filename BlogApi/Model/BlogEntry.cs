using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Model
{
    [Table("blog_entry")]
    public partial class BlogEntry
    {
        [Key]
        [Column("bet_id")]
        public int BetId { get; set; }
        [Column("bet_add_date")]
        public DateTime? BetAddDate { get; set; }
        [Column("bet_deleted")]
        [DefaultValue("false")]
        public bool? BetDeleted { get; set; }
        [Column("bet_title")]
        public string BetTitle { get; set; } = null!;
        [Column("bet_content")]
        public string? BetContent { get; set; }
        [Column("bet_autor")]
        public string? BetAutor { get; set; }
        [Column("bet_publications_date")]
        public DateTime? BetPublicationDate { get; set; }
        [Column("bet_cat_id")]
        public int BetCatId { get; set; }
        public virtual Category? BetCat { get; set; }
        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();

    }
}
