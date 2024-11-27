using System.ComponentModel.DataAnnotations;

namespace HvoyaApplication.Models
{
    public class Review
    {
        public int ReviewId {  get; set; }
        public string Comment { get; set; }
        [Range(1, 5)]
        public int Rating {  get; set; }

        public int DessertId {  get; set; }
        public Dessert Dessert { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
