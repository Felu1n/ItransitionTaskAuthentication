using ItransitionTaskAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using ItransitionTaskAuthentication.Data;


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
            
            // Поиск пользователя в базе данных по имени пользователя и паролю
            var user = _context.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
            if (user != null)
            {
                // Успешная аутентификация, выполните необходимые действия
                // Например, можно сохранить информацию о пользователе в сессии или установить куки
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Неверное имя пользователя или пароль.");
                return View(model);
            }
           

            return View(model);
        }
    }
}
