using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projeto_Criptografia.Services;

namespace Projeto_Criptografia.Pages.SitePage
{
    public class RegisterModel : PageModel
    {
        private readonly UserService _userService;

        [BindProperty]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string? Message { get; set; }

        public RegisterModel(UserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            bool success = _userService.Register(Username, Password);

            if (success)
            {
                return RedirectToPage("/SitePage/Login");
            }

            Message = "Usuário já existe.";
            return Page();
        }
    }
}
