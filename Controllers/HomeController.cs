  using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Frender.Models;
// Other using statements
namespace Frender.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;

        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {

            return View();
        }

        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            // get the user Id in session
            int? userId = HttpContext.Session.GetInt32("userId");

            // grab the current user
            var UserInDb = _context.Users.FirstOrDefault(user => user.UserId == userId);
            ViewBag.SpecificUser = UserInDb;

            // grab the current user
            var notUserInDb = _context.Users.Where(user => user.UserId != userId);
            ViewBag.notSpecificUser = notUserInDb;

            // 
            return View();
        }

        // [HttpGet("user/friend/{friendWithId}")]
        // public IActionResult AddFriend(int friendWithId)
        // {
        //     // DO NOT TRUST DATA FROM THE INTERNET!
        //     // check that friendId is valid; Check that it isn't the current user
        //     int? userId = HttpContext.Session.GetInt32("userId");

        //     Friendship friendship = new Friendship()
        //     {
        //         FriendId =  (int)userId,
        //         FriendWithId = friendWithId
        //     };

        //     _context.Add(friendship);
        //     _context.SaveChanges();

        //     return RedirectToAction("Dashboard");
        // }

        // goes to logged in users profile
        [HttpGet("UserProfile/{userId}")]
        public IActionResult UserProfile(int userId)
        {
            // gets the selected hobby from DB
            ViewBag.UserProfile = _context.Users
                .FirstOrDefault(user => user.UserId == userId);


            return View();
        }




        // show login page
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View("Login");
        }


        // -----------------------------------------------

        // Post Actions

        // Register Post
        [HttpPost("RegisterUser")]
        public IActionResult RegisterUser(User user)
        {
            // Check initial ModelState
            if (ModelState.IsValid)
            {
                // If a User exists with provided userName
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    // Manually add a ModelState error to the Username field, with provided
                    // error message
                    ModelState.AddModelError("Email", "Email already in use!");

                    // You may consider returning to the View at this point
                    return View("Index");
                }
                // if everything is okay save the user to the DB
                // Initializing a PasswordHasher object, providing our User class as its type
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                _context.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            // other code
            return View("Index");
        }

        // Login Post 
        [HttpPost("LoginPost")]
        public IActionResult LoginPost(LoginUser userSubmission)
        {
            if (ModelState.IsValid)
            {
                // If inital ModelState is valid, query for a user with provided email
                var userInDb = _context.Users.FirstOrDefault(u => u.Email == userSubmission.Email);
                // If no user exists with provided email
                if (userInDb == null)
                {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("UserName", "Invalid UserName");
                    return View("Login");
                }

                // Initialize hasher object
                var hasher = new PasswordHasher<LoginUser>();

                // verify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);

                // result can be compared to 0 for failure
                if (result == 0)
                {
                    // handle failure (this should be similar to how "existing email" is handled)
                    ModelState.AddModelError("Password", "Not the right password cops are being called!");
                    return View("Login");
                }
                // assign user ID to sessions userId
                HttpContext.Session.SetInt32("userId", userInDb.UserId);
                // If everything is good go to the Dashboard view page 
                // pass in the user we found in the db into it
                return RedirectToAction("Dashboard");
            }
            // go back to login if fails
            return View("Login");
        }

    }
}