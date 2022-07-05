using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieApp.Web.Data;
using MovieApp.Web.Entity;
using MovieApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Web.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MovieContext _context;

        public MoviesController(MovieContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
       
        public IActionResult List(int? id,string q )
        {


            var movies = MovieRepository.Movies;
            if (id != null)
            {
                movies = movies.Where(m => m.GenreId == id).ToList(); 
            }
            if(!String.IsNullOrEmpty(q))
            {
                movies = movies.Where(i =>
                 i.Title.ToLower().Contains(q.ToLower()) ||
                 i.Description.ToLower().Contains(q.ToLower())).ToList();
            }



            var model= new MoviesViewModel()
            {
                
                Movies = movies
             
            };
            return View("Movies", model);
        }


            //localhost:5000/movies/details/1

           public IActionResult Details(int id)
            {
            return View(MovieRepository.GetById(id));
            }
        
        
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Genres = new SelectList(GenreRepository.Genres, "GenreId", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Movie m)
        {

           if (ModelState.IsValid)
            {

                //MovieRepository.Add(m);
                _context.Movies.Add(m);
                _context.SaveChanges();

                TempData["Message"] = $"{m.Title} isimli film eklendi.";
             return RedirectToAction("List");
            }
            ViewBag.Genres = new SelectList(GenreRepository.Genres, "GenreId", "Name");
            return View();
          
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {

            ViewBag.Genres = new SelectList(GenreRepository.Genres, "GenreId", "Name");
            return View(MovieRepository.GetById(id));
        }

        [HttpPost]
        public IActionResult Edit(Movie m)
        {
            if (ModelState.IsValid)
            {
                MovieRepository.Edit(m);
              

                return RedirectToAction("Details", "Movies", new { @id = m.MovieId });
            }

            ViewBag.Genres = new SelectList(GenreRepository.Genres, "GenreId", "Name");
            return View(m);
        }
        [HttpPost]
        public IActionResult Delete(int MovieId, string Title)
        {
            MovieRepository.Delete(MovieId);
            TempData["Message"] = $"{Title} isimli film silindi.";
            return RedirectToAction("List");
        }
    }
    }


