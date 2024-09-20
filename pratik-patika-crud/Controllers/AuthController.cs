using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using pratik_patika_crud.Entities;
using pratik_patika_crud.Models;
using System.Security.Claims;

namespace pratik_patika_crud.Controllers
{
    public class AuthController : Controller
    {

        private static List<UserEntity> _users = new List<UserEntity>()
        {
            new UserEntity(){ Id=1, Email = "." , Password = ", "}
        };

        // Sadece constructorda  tanımlanabilen readonly bir değişken tanımladık
        private readonly IDataProtector _dataProtector;


        // bir class içerisindeki metotları başka bir class içerisinde kullanmak istersem 
        // burada kendi değişkenimle oluşturduğumuz robotu bağlıyoruz.
        public AuthController(IDataProtectionProvider dataProtectionProvider)
        {
            // hangi amaçla çağırdığımızı yazabiliriz. security
            _dataProtector = dataProtectionProvider.CreateProtector("security");
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(SignUpViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View(formData);
            }

            var user = _users.FirstOrDefault(x => x.Email.ToLower() == formData.Email.ToLower());

            if (user is not null)
            {
                ViewBag.Error = "Kullanıcı Mevcut";
                return View(formData);
            }

            var newUser = new UserEntity()
            {
                Id = _users.Max(x => x.Id) + 1,
                Email = formData.Email.ToLower(),
                // buradaki işlem kriptolu bir şekilde passwordu kaydediyor. örneğin 1234 olan şifreyi ^'dsf+^tdsgv4509*6klşgSADFa gibi bir şifre ile eşleştirip öyle kaydediyor. Tamamen güvenlik ile alakalı.
                Password = _dataProtector.Protect(formData.Password)
            };
            _users.Add(newUser);
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel formData)
        {
            var user = _users.FirstOrDefault(x => x.Email.ToLower() == formData.Email.ToLower());

            if (user is null)
            {
                ViewBag.Error = "Kullanıcı adı veya şifre hatalı";
                return View(formData);
            }

            var rawPassword = _dataProtector.Unprotect(user.Password);
            //şifrede büyültme küçültme olmamalı
            if (rawPassword == formData.Password)
            {
                var claims = new List<Claim>();

                //browserında hangi bilgilerin tutulacaksa ona göre alan oluşturur. aşağıdakilere göre.email/firstname/lastname gibi..
                claims.Add(new Claim("email", user.Email));
                claims.Add(new Claim("id",user.Id.ToString()));


                //kimliğin tanımlanması. claimsin içindekilerle oturum açılacağı için claimside tanımladık..
                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                //Girişin özelliklerini tanımlıyoruz.
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true, // yenilebilir oturum
                    ExpiresUtc = new DateTimeOffset(DateTime.Now.AddHours(48)) // oturum açıldıktan sonra 48 saat geçerli..
                };

                // internet hızı bilgisayar hızı gibi faktörler olduğundan dolayı bir asenkron işlemler vardır.
                // AWAIT => eş zamanlı yapılan işlemlerin birbirlerini beklemesi için kullanılıyor. bu kullanıldığında metotun türü Task< > olmalıdır. 
                //Asekron metotlar geriye promise döner..
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimIdentity),authProperties);

            }
            // eğer şartlar uymuyorsa geriye kullanıcı adı veya şifre hatalı mesajı döndür
            else
            {
                ViewBag.Error = "Kullanıcı adı veya şifre hatalı";
                return View(formData);
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
