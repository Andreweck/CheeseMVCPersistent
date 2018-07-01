using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Models;
using System.Collections.Generic;
using CheeseMVC.ViewModels;
using CheeseMVC.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        private readonly CheeseDbContext context;

        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            List<Menu> cheeses = context.Menus.ToList();

            return View(cheeses);
        }

        public IActionResult Add()
        {
            AddMenuViewModel addMenuViewModel = 
                new AddMenuViewModel();
            return View(addMenuViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (ModelState.IsValid)
            {
                Menu newMenu =
                    new Menu
                    {
                        Name = addMenuViewModel.Name
                    };
                
                context.Menus.Add(newMenu);
                context.SaveChanges();

                return Redirect("/Menu");
            }

            return View(addMenuViewModel);
        }

        
        public IActionResult ViewMenu(int id)
        {
            List<CheeseMenu> items = context
                .CheeseMenus
                .Include(item => item.Cheese)
                .Where(cm => cm.MenuID == id)
                .ToList();

            Menu menu = context.Menus.Single(m => m.ID == id);

            ViewMenuViewModel viewModel =
                new ViewMenuViewModel
                {
                    Menu = menu,
                    Items = items
                };

            return View(viewModel);
        }

        public IActionResult AddItem(int id)
        {
            Menu menu = context.Menus.Single(m => m.ID == id);
            
            List<Cheese> Cheeses = context.Cheeses.ToList();

            ViewBag.Title = "Add Item to Menu: " + menu.Name;

            AddMenuItemViewModel addItemViewModel = 
                new AddMenuItemViewModel(menu, Cheeses);

            

            return View(addItemViewModel);


        }
        [HttpPost]
        public IActionResult AddItem(int id, AddMenuViewModel menuViewModel)
        {
            
                int cheeseID = int.Parse(Request.Form["CheeseID"]);
                var menuID = menuViewModel.MenuID;

                IList<CheeseMenu> existingItems = context.CheeseMenus
                    .Where(cm => cm.CheeseID == cheeseID)
                    .Where(cm => cm.MenuID == id).ToList();

                if (existingItems.Count == 0)
                {
                    CheeseMenu menuItem = new CheeseMenu
                    {
                        Cheese = context.Cheeses.Single(c => c.ID == cheeseID),
                        Menu = context.Menus.Single(m => m.ID == id)

                    };
                    context.CheeseMenus.Add(menuItem);
                    context.SaveChanges();
                }
                

            
            return Redirect(string.Format("/Menu/ViewMenu/{0}", id));
        }
    }
}
