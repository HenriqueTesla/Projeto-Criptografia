using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projeto_Criptografia.Services;

namespace Projeto_Criptografia.Pages.SitePage
{
    public class LoginModel : PageModel
    {
        private readonly UserService _userService;

        [BindProperty]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public LoginModel(UserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            if (_userService.Login(Username, Password))
                Message = "Login realizado com sucesso!";
            else
                Message = "Usuário ou senha incorretos.";
        }
    }
}
