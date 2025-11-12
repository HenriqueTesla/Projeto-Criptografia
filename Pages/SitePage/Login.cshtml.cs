using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projeto_Criptografia.Services;

namespace Projeto_Criptografia.Pages.SitePage
{
    public class LoginModel : PageModel
    {
        private readonly UserService _userService;

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public LoginModel()
        {
            _userService = new UserService();
        }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            if (_userService.Login(Email, Password))
                Message = "Login realizado com sucesso!";
            else
                Message = "Email ou senha incorretos.";
        }
    }
}
