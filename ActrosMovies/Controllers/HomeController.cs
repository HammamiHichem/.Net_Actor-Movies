using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using ActrosMovies.Models;
using Microsoft.AspNetCore.Http;

namespace ActrosMovies.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyContext _context;

        public HomeController(ILogger<HomeController> logger, MyContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
{
    // Initialiser `ViewBag.IsLoggedIn` à false par défaut
    ViewBag.IsLoggedIn = HttpContext.Session.GetInt32("UserId") != null;
    return View();
}


        // Enregistrement des utilisateurs
        [HttpPost("user/create")]
        public IActionResult CreateUser(User newUser)
        {
            if (ModelState.IsValid)
            {
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                newUser.Password = hasher.HashPassword(newUser, newUser.Password);
                _context.Add(newUser);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("UserId", newUser.UserId);
                return RedirectToAction("Index");
            }
            else
            {
                return View("Index");
            }
        }

        // Connexion des utilisateurs
        [HttpPost("user/login")]
        public IActionResult LoginUser(LoginUser loginUser)
        {
            if (ModelState.IsValid)
            {
                User userInDb = _context.Users.FirstOrDefault(a => a.Email == loginUser.LogEmail);
                if (userInDb == null)
                {
                    ModelState.AddModelError("LogEmail", "Invalid Email or Password");
                    return View("Index");
                }

                PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(loginUser, userInDb.Password, loginUser.LogPassword);

                if (result == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError("LogPassword", "Invalid Email or Password");
                    return View("Index");
                }

                HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                return RedirectToAction("Success");
            }
            else
            {
                return View("Index");
            }
        }

        // Page après connexion réussie
        [HttpGet("success")]
        public IActionResult Success()
        {
            // Vérifie si l'utilisateur est connecté
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // Déconnexion des utilisateurs
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet("movie/add")]
        public IActionResult Movie()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }

            var model = new MyViewModel
            {
                AllMovies = _context.Movies.ToList()
            };
            return View(model);
        }

        [HttpPost("movie/add")]
        public IActionResult AddMovie(Movie newMovie)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                _context.Add(newMovie);
                _context.SaveChanges();
                return RedirectToAction("Movie");
            }
            else
            {
                MyViewModel model = new MyViewModel
                {
                    AllMovies = _context.Movies.ToList()
                };
                return View("Movie", model);
            }
        }

        [HttpPost("movies/{movieId}/destroy")]
        public IActionResult DestroyMovie(int movieId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }

            Movie? movieToDestroy = _context.Movies.SingleOrDefault(a => a.MovieId == movieId);
            if (movieToDestroy != null)
            {
                _context.Movies.Remove(movieToDestroy);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpGet("actor/add")]
        public IActionResult Actor()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }

            var model = new MyViewModel
            {
                AllActors = _context.Actors.ToList()
            };
            return View(model);
        }

        [HttpPost("actor/add")]
        public IActionResult AddActor(Actor newActor)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                _context.Add(newActor);
                _context.SaveChanges();
                return RedirectToAction("Actor");
            }
            else
            {
                MyViewModel model = new MyViewModel
                {
                    AllActors = _context.Actors.ToList()
                };
                return View("Actor", model);
            }
        }

        [HttpPost("actors/{actorId}/destroy")]
        public IActionResult DestroyActor(int actorId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }

            Actor? actorToDestroy = _context.Actors.SingleOrDefault(a => a.ActorId == actorId);
            if (actorToDestroy != null)
            {
                _context.Actors.Remove(actorToDestroy);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpGet("/association")]
        public IActionResult Association()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.AllMovies = _context.Movies.ToList();
            ViewBag.AllActors = _context.Actors.ToList();
    
            var allAssociations = _context.Associations
                .Include(a => a.Actor)
                .Include(a => a.Movie)
                .ToList();

            ViewBag.AllAssociations = allAssociations;
            return View();
        }

        [HttpPost("/association/add")]
        public IActionResult AddAssociation(Association newAssociation)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                _context.Add(newAssociation);
                _context.SaveChanges();

                ViewBag.AllActors = _context.Actors.ToList();
                ViewBag.AllMovies = _context.Movies.ToList();
                ViewBag.AllAssociations = _context.Associations
                    .Include(a => a.Actor)
                    .Include(a => a.Movie)
                    .ToList();

                return View("Association");
            }

            ViewBag.AllActors = _context.Actors.ToList();
            ViewBag.AllMovies = _context.Movies.ToList();
            ViewBag.AllAssociations = _context.Associations
                .Include(a => a.Actor)
                .Include(a => a.Movie)
                .ToList();

            return View("Association");
        }
        [HttpPost]
    public IActionResult DeleteAssociation(int associationId)
    {
        // Rechercher l'association par son ID
        var association = _context.Associations.Find(associationId);
        
        if (association != null)
        {
            // Supprimer l'association
            _context.Associations.Remove(association);
            _context.SaveChanges();
        }

        // Rediriger vers la vue appropriée après suppression
        return RedirectToAction("Association"); // Remplacez "Index" par la vue que vous souhaitez afficher après suppression
    }

        [HttpGet("actors/{actorId}/edit")]
        public IActionResult EditActor(int actorId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }

            var actor = _context.Actors.SingleOrDefault(a => a.ActorId == actorId);
            if (actor == null)
            {
                return NotFound();
            }

            var model = new MyViewModel
            {
                Actor = actor,
                AllActors = _context.Actors.ToList()
            };

            return View("Actor", model);
        }

        [HttpPost("actors/{actorId}/update")]
        public IActionResult UpdateActor(int actorId, Actor updatedActor)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }

            var actorInDb = _context.Actors.SingleOrDefault(a => a.ActorId == actorId);
            if (actorInDb == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                actorInDb.Name = updatedActor.Name;
                actorInDb.Image = updatedActor.Image;
                _context.SaveChanges();
                return RedirectToAction("Actor");
            }

            var model = new MyViewModel
            {
                Actor = updatedActor,
                AllActors = _context.Actors.ToList()
            };

            return View("Actor", model);
        }

        [HttpGet("movies/{movieId}/edit")]
        public IActionResult EditMovie(int movieId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }

            var movie = _context.Movies.SingleOrDefault(m => m.MovieId == movieId);
            if (movie == null)
            {
                return NotFound();
            }

            var model = new MyViewModel
            {
                Movie = movie,
                AllMovies = _context.Movies.ToList()
            };

            return View("Movie", model);
        }

        [HttpPost("movies/{movieId}/update")]
        public IActionResult UpdateMovie(int movieId, Movie updatedMovie)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }

            var movieInDb = _context.Movies.SingleOrDefault(m => m.MovieId == movieId);
            if (movieInDb == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                movieInDb.Title = updatedMovie.Title;
                movieInDb.Description = updatedMovie.Description;
                movieInDb.MovieImage = updatedMovie.MovieImage;
                _context.SaveChanges();
                return RedirectToAction("Movie");
            }

            var model = new MyViewModel
            {
                Movie = updatedMovie,
                AllMovies = _context.Movies.ToList()
            };

            return View("Movie", model);
        }

        public IActionResult Privacy()
        {
            var movies = _context.Movies.ToList(); // Assurez-vous que _context.Movies est bien défini
            return View(movies);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
        }
    }
}
