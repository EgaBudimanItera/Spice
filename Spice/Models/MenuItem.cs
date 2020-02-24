using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Spicyness { get; set; }
        public enum ESpicy {NA=0,NotSPicy=1,Spicy=2,VerySpicy=3 }
        public string Images { get; set; }
        [Display(Name="Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        [Display(Name = "SubCategory")]
        public int SubCategoryId { get; set; }
        [ForeignKey("SubCategoryId")]
        public virtual SubCategory SubCategory { get; set; }
        [Range(1000,int.MaxValue,ErrorMessage ="Harganya Jangan Murah2 Lah")]
        public double Price { get; set; }

    }
}
