using System;
namespace CapstoneProject.Areas.Users.Models.LoginModel.Schemas
{
	public class LoginInfo
	{
		public LoginInfo()
		{
			Type = 1;
        }
		/// <summary>
		/// UserName login
		/// </summary>
		public string Username { get; set; }
		/// <summary>
		/// Password login
		/// </summary>
		public string Password { get; set; }
		public int? Type { get; set; }
	}
}

