using System.Collections.Generic;
using HvoyaApplication.Models;
using HvoyaApplication.ViewModels;

namespace HvoyaApplication.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<DessertDetailsViewModel> Desserts { get; set; }
        public string CurrentCategory {  get; set; }
        public string SearchQuery { get; set; }

        public HomeViewModel()
        {
            Desserts = new List<DessertDetailsViewModel>();
        }
    }
}
