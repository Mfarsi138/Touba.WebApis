using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Touba.WebApis.DataLayer.Models.Product
{
    public class ProductTest
    {
        [Key]
        [MaxLength(100)]
        public string Id { get; set; }
        [Required]
        [MaxLength(100)]
        
        [Column(TypeName = "nvarchar(MAX)")]
        public string Description { get; set; }

        

        //common fields
        [MaxLength(100)]
        public string SysId { get; set; }

        public DateTime CreateDateTime { get; set; }

        [Required]
        public string CreatorUserId { get; set; }


        public bool IsDeleted { get; set; }

        public int State { get; set; }

        //public static explicit operator ProductTest(System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<IEnumerable<ProductTest>>> v)
        //{
        //    throw new NotImplementedException();
        //}

        public string StateTitle { get; set; }
    }
}
