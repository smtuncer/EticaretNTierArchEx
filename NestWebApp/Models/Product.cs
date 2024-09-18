using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NestWebApp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Grammage { get; set; }
        public string? PiecesInBox { get; set; }
        public string? Image { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsCampaing { get; set; }
        public bool IsDeleted { get; set; }
        public int? ProductCategoryId { get; set; }
        [ForeignKey("ProductCategoryId")]
        public ProductCategory? ProductCategory { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
    }
}
