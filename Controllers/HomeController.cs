using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wall_Assign.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace Wall_Assign.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context {get; set;}
        public HomeController(MyContext context)
        {
            _context = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User registered_user)
        {  
            
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u=> u.Email == registered_user.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                    return View("Index");
                }
                else{
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    registered_user.Password = Hasher.HashPassword(registered_user, registered_user.Password);
                    _context.Users.Add(registered_user);
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("UserId", registered_user.UserId);
                    return RedirectToAction("Wall");
                }

            }  
            return View("Index");

        }
        [HttpGet("Wall")]
        public IActionResult Wall()
        {
            User userIndb = _context.Users.FirstOrDefault(u=>u.UserId == (int)HttpContext.Session.GetInt32("UserId"));
            List<Message> all_messages = _context.Messages.Include(m=>m.Comments).ThenInclude(m=>m.Navuser).ToList();            
            
            if (userIndb == null){
                return View("Index");
            }
            else{
                ViewBag.User = userIndb;
                return View(all_messages);
            }
        }


        [HttpPost("Login")]
        public IActionResult Login(LoginUser log_user)
        {
            if (ModelState.IsValid){
                User userInDb = _context.Users.FirstOrDefault(user => user.Email ==log_user.LoginEmail);
                
                if (userInDb == null)
                {
                    ModelState.AddModelError("LoginEmail", "Email does not exist.");
                    return View("Index");
                }
                else 
                {
                    var hash = new PasswordHasher<LoginUser>();
                    var result = hash.VerifyHashedPassword(log_user, userInDb.Password, log_user.LoginPassword);
                    HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                    if (result == 0)
                    {
                        ModelState.AddModelError("LoginPassword","Password does not match"); 
                        return View("Index");
                    }
                return RedirectToAction("Wall");
                }  
            }
            else{
                return View("Index");
            }   
        }

        [HttpPost("make_message")]
        public IActionResult Make_Message(Message new_message)
        {
            new_message.UserId = (int)HttpContext.Session.GetInt32("UserId");
            _context.Messages.Add(new_message);
            _context.SaveChanges();
            return Redirect("/Wall");

        }

        [HttpPost("make_comment")]
        public IActionResult Make_Comment(Comment new_comment)
        {
            new_comment.UserId = (int)HttpContext.Session.GetInt32("UserId");
            _context.Comments.Add(new_comment);
            _context.SaveChanges();
            return Redirect("/Wall");
        }

    


        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
