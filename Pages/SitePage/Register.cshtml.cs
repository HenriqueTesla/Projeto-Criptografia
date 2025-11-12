using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projeto_Criptografia.Services;

namespace Projeto_Criptografia.Pages.SitePage
{
    public class RegisterModel : PageModel
    {
        private readonly UserService _userService;
        private readonly ILogger<RegisterModel> _logger;

        [BindProperty]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string? Message { get; set; }

        public RegisterModel(UserService userService, ILogger<RegisterModel> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation("[DEBUG] GET /SitePage/Register");
        }

        public IActionResult OnPost()
        {
            bool success = _userService.Register(Username, Password);

            if (success)
            {
                Message = "Usuário registrado com sucesso!";
                return RedirectToPage("/SitePage/Login");
            }
            else
            {
                Message = "Usuário já existe ou erro ao salvar.";
                return Page();
            }
        }
    }
}
