using System;
namespace CapstoneProject.Areas.Users.Models.LoginModel.Schemas
{
	public class LoginInfo
	{
		public LoginInfo()
		{
		}
		/// <summary>
		/// UserName login
		/// </summary>
		public string Username { get; set; }
		/// <summary>
		/// Password login
		/// </summary>
		public string Password { get; set; }
	}
}

