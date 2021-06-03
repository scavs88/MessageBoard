using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Slacker.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Slacker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        SlackerContext context = new SlackerContext();
        

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        List<Messages> messages = new List<Messages>();
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }
        [Authorize, HttpPost]
        public IActionResult Messages(string message)
        {
            using(SlackerContext context = new SlackerContext())
            {
                Messages newMessage = new Messages();

                newMessage.UserId = User.Identity.Name;
                newMessage.PostedTime = DateTime.Now;
                newMessage.Updated = false;
                newMessage.Message = message;
                context.Add(newMessage);
                context.SaveChanges();
            }
            return Redirect("Messages");
        }
        [Authorize]
        public IActionResult Messages()
        {
            List<Messages> messageList = new List<Messages>();
            using(SlackerContext context = new SlackerContext())
            {
                messageList = context.Messages.ToList();
            }
            return View(messageList);
        }


        [Authorize]
        public IActionResult Edit(int id)
        {
            Messages newMessage = new Messages();
            using(SlackerContext context = new SlackerContext())
            {
                newMessage = context.Messages.ToList().Find(m => m.Id == id);
                return View(newMessage);
            }           
        }


        [Authorize,HttpPost]
        public IActionResult Edits(string messages, int id)
        {
            Messages newMessage = new Messages();
            using(SlackerContext context = new SlackerContext())
            {
                newMessage = context.Messages.ToList().Find(m => m.Id == id);
                newMessage.Updated = true;
                newMessage.Message = messages;
                context.SaveChanges();
            }
            return RedirectToAction("Messages");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
