using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ShopApp.WebUI.Models
{
	public class UserEditViewModel
	{
			public string name { get; set; }
			public string surname { get; set; }
			[Required]
			public string password { get; set; }
			public string confirmpassword { get; set; }
			public string phonenumber { get; set; }
			public string mail { get; set; }

	}
}
