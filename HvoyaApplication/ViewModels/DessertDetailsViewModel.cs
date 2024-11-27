using HvoyaApplication.Models;
using System.Collections.Generic;

namespace HvoyaApplication.ViewModels
{
    public class DessertDetailsViewModel
    {
        public int DessertId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public List<Review> Reviews { get; set; }
        public bool IsAvailable {  get; set; }
    }
}
