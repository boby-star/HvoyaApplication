namespace HvoyaApplication.Models
{
    public class Category
    {
        public int CategoryId {  get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        public List<Dessert> Desserts { get; set; } = new List<Dessert>();
    }
}
