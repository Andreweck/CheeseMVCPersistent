using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseMVC.ViewModels
{
    public class AddMenuItemViewModel
    {
        public Menu Menu { get; set; }
        

        public int MenuID { get; set; }

        public List<SelectListItem> Cheeses { get; set; }
        public int CheeseID { get; set; }

        public AddMenuItemViewModel()
        {
            Cheeses = new List<SelectListItem>();
        }

        public AddMenuItemViewModel(Menu menu, List<Cheese> cheeses) : this()
        {
            Menu = menu;

            foreach (Cheese cheese in cheeses)
            {
                Cheeses.Add(new SelectListItem
                {
                    Value = cheese.ID.ToString(),
                    Text = cheese.Name.ToString()
                });
                
            }
        }
        
    }
}
