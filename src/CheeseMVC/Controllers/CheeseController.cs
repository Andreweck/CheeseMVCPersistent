using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Models;
using System.Collections.Generic;
using CheeseMVC.ViewModels;
using CheeseMVC.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheeseMVC.Controllers
{
    public class CheeseController : Controller
    {
        private readonly CheeseDbContext context;

        public CheeseController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            IList<Cheese> cheeses = context.Cheeses.Include(c => c.Category).ToList();

            return View(cheeses);
        }

        public IActionResult Add()
        {
            List<CheeseCategory> CategoryList = context.Categories.ToList();

            ViewBag.Categories = new List<CheeseCategory>();

            AddCheeseViewModel addCheeseViewModel = 
                new AddCheeseViewModel(CategoryList);

            return View(addCheeseViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddCheeseViewModel addCheeseViewModel)
        {
            if (ModelState.IsValid)
            {
                CheeseCategory newCategory =
                    context.Categories.Single(c => c.ID == addCheeseViewModel.CategoryID);
                
                // Add the new cheese to my existing cheeses
                Cheese newCheese = new Cheese
                {
                    Name = addCheeseViewModel.Name,
                    Description = addCheeseViewModel.Description,
                    Category = newCategory
                };

                context.Cheeses.Add(newCheese);
                context.SaveChanges();

                return Redirect("/Cheese");
            }

            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Items = new List<SelectListItem>();
            foreach (var cat in ViewBag.Categories)
            {
                ViewBag.Items.Add(new SelectListItem()
                {
                    Value = cat.ID.ToString(),
                    Text = cat.Name.ToString()

                });
            }
                return View(addCheeseViewModel);
        }

        public IActionResult Remove()
        {
            ViewBag.title = "Remove Cheeses";
            ViewBag.cheeses = context.Cheeses.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Remove(int[] cheeseIds)
        {
            foreach (int cheeseId in cheeseIds)
            {
                Cheese theCheese = context.Cheeses.Single(c => c.ID == cheeseId);
                context.Cheeses.Remove(theCheese);
            }

            context.SaveChanges();

            return Redirect("/");
        }

        public IActionResult Category(int id)
        {
            if (id == 0)
            {
                return Redirect("/Category");
            }
            CheeseCategory category = context.Categories
                .Include(c => c.Cheeses)
                .Single(c => c.ID == id);

            ViewBag.title = "Cheeses in category: " + category.Name;

            return View("Index", category.Cheeses);

        }
    }
}
