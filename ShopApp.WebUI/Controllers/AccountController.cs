using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Models;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Controllers
{
	[AutoValidateAntiforgeryToken]
	public class AccountController : Controller
	{
		private UserManager<ApplicationUser> _userManager;
		private SignInManager<ApplicationUser> _signInManager;
		private IEmailSender _emailSender;
		private ICartService _cartService;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, ICartService cartService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_emailSender = emailSender;
			_cartService = cartService;
		}
		[HttpGet]
		public IActionResult Register()
		{
			return View(new RegisterModel());
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var user = new ApplicationUser
			{
				UserName = model.UserName,
				Email = model.Email,
				FullName = model.FullName
			};

			var result = await _userManager.CreateAsync(user, model.Password);
			if (result.Succeeded)
			{
				//token and email
				//var tokenCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
				//var callbackUrl = Url.Action("ConfirmEmail", "Account", new
				//{
				//userId = user.Id,
				//token = tokenCode
				//});

				//await _emailSender.SendEmailAsync(model.Email, "Hesabınızı Onaylayınız", $"Lütfen mail hesabınızı onaylamak için linke <a href='http://localhost:61903{callbackUrl}'>tıklayınız</a>.");
				_cartService.InitializeCart(user.Id);
				return RedirectToAction("Login", "Account");
			}
			ModelState.AddModelError("", "Bir hata oluştu tekrar deneyiniz");
			return View(model);

		}

		[HttpGet]
		public IActionResult Login(string ReturnUrl = null)
		{
			return View(new LoginModel()
			{
				ReturnUrl = ReturnUrl
			});
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginModel model)
		{

			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null)
			{
				ModelState.AddModelError("", "Böyle bir hesap yok");
				return View(model);
			}

			//if (!await _userManager.IsEmailConfirmedAsync(user))
			//{
			//	ModelState.AddModelError("", "Hesabınızı onaylayınız");
			//	return View(model);
			//}


			var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
			if (result.Succeeded)
			{
				return Redirect(model.ReturnUrl ?? "~/");
			}
			ModelState.AddModelError("", "Email veya parola yanlış");
			return View(model);
		}

		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return Redirect("~/");
		}

		//public async Task<IActionResult> ConfirmEmail(string userId, string token)
		//{
		//	if (userId == null || token ==null)
		//	{
		//		TempData["message"] = "Geçersiz token";
		//		return View();
		//	}
		//	var user = await _userManager.FindByIdAsync(userId);
		//	if (user != null)
		//	{
		//		var result = await _userManager.ConfirmEmailAsync(user, token);
		//		if (result.Succeeded)
		//		{
		//			TempData["message"] = "Hesabınız onaylandı";
		//			return View();
		//		}
		//	}
		//	TempData["message"] = "Hesap onaylanmadı";
		//	return View();
		//}

		public IActionResult ResetPassword()
		{
			return View();
		}

		public IActionResult AccessDenied()
		{
			return View();
		}
	}
}
