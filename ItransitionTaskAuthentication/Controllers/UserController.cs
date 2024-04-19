using ItransitionTaskAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using ItransitionTaskAuthentication.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace YourProjectNamespace.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /User/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserModel model)
        {
            if (ModelState.IsValid)
            {
                // Создаем новый экземпляр UserModel на основе данных из формы
                var newUser = new UserModel
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password,
                    RegistrationDate = DateTime.Now,
                    LastLoginDate = DateTime.Now,
                    IsBlocked = false,
                    IsDeleted = false
                };

                // Добавляем нового пользователя в контекст базы данных и сохраняем изменения
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home"); // Перенаправляем пользователя на главную страницу
            }

            // Если модель недействительна, возвращаем пользователю представление с сообщениями об ошибках
            return View(model);
        }

                // GET: /User/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserModel model)
        {
            
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == model.Password);
            if (user != null)
            {
                // Успешная аутентификация, установите аутентификационные куки
                var claims = new List<Claim>
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Идентификатор пользователя
                new Claim(ClaimTypes.Name, user.Username) // Имя пользователя
                // Другие дополнительные утверждения (например, роли пользователя)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                user.LastLoginDate = DateTime.Now;
                _context.SaveChanges();
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Перенаправление на главную страницу или другую страницу
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Неверное имя пользователя или пароль.");
                return View(model);
            }
            
        }
        public async Task<IActionResult> Logout()
        {
      
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

    }
}
