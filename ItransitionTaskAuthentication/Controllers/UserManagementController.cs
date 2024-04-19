using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ItransitionTaskAuthentication.Data;
using ItransitionTaskAuthentication.Models;
using Microsoft.EntityFrameworkCore;

namespace ItransitionTaskAuthentication.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Получаем всех пользователей из базы данных
            var users = _context.Users.ToList();
            return View(users);
        }

        [HttpPost]
        public IActionResult ManageUsers(int[] selectedUsers, string action)
        {
            if (selectedUsers == null || selectedUsers.Length == 0)
            {
                // Пользователь не выбрал ни одного пользователя
                ModelState.AddModelError("", "Please select at least one user.");
                return RedirectToAction("Index");
            }

            foreach (var userId in selectedUsers)
            {
                var user = _context.Users.Find(userId);

                if (user == null)
                {
                    // Пользователь с заданным идентификатором не найден
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
                        user.IsDeleted = true;
                        break;
                    default:
                        // Действие не распознано
                        ModelState.AddModelError("", "Invalid action.");
                        return RedirectToAction("Index");
                }

            }

            // Сохраняем изменения в базе данных
            _context.SaveChanges();

            // Перенаправление на страницу с пользователями после выполнения действия
            return RedirectToAction("Index");
        }
    }
}
