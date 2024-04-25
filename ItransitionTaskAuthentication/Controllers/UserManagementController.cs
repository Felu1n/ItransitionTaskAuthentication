using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ItransitionTaskAuthentication.Data;
using ItransitionTaskAuthentication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ItransitionTaskAuthentication.Controllers
{
    [Authorize]
    public class UserManagementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        [HttpPost]
        public IActionResult ManageUsers(int[] selectedUsers, string action)
        {
            if (selectedUsers == null || selectedUsers.Length == 0)
            {
                ModelState.AddModelError("", "Please select at least one user.");
                return RedirectToAction("Index");
            }

            foreach (var userId in selectedUsers)
            {
                var user = _context.Users.Find(userId);

                if (user == null)
                {
                    ModelState.AddModelError("", $"User with ID {userId} not found.");
                    return RedirectToAction("Index");
                }

                switch (action)
                {
                    case "Block":
                        user.IsBlocked = true;
                        break;
                    case "Unblock":
                        user.IsBlocked = false;
                        break;
                    case "Delete":
                        _context.Users.Remove(user);
                        break;
                    default:
                        ModelState.AddModelError("", "Invalid action.");
                        return RedirectToAction("Index");
                }
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
