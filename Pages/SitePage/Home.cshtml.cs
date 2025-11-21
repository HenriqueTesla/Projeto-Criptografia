using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projeto_Criptografia.Services;

namespace Projeto_Criptografia.Pages.SitePage
{
    public class HomeModel : PageModel
    {
        private readonly UserService _userService;

        public HomeModel(UserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        public string? NewName { get; set; }

        public string Message { get; set; } = string.Empty;

        public void OnGet()
        {
            Username = Request.Cookies["username"] ?? "Usuário";
        }

        public IActionResult OnPostEdit(string newName)
        {
            string loggedUser = Request.Cookies["username"] ?? "";

            if (string.IsNullOrWhiteSpace(loggedUser))
            {
                Message = "Erro: usuário não identificado.";
                return Page();
            }

            if (_userService.UpdateUsername(loggedUser, newName))
            {
                Response.Cookies.Append("username", newName);
                Message = "Nome atualizado com sucesso!";
            }
            else
            {
                Message = "Não foi possível atualizar. Nome já existe?";
            }

            Username = Request.Cookies["username"] ?? loggedUser;
            return Page();
        }

        public IActionResult OnPostDelete()
        {
            string loggedUser = Request.Cookies["username"] ?? "";

            if (_userService.DeleteUser(loggedUser))
            {
                Response.Cookies.Delete("username");
                return RedirectToPage("/SitePage/Login");
            }

            Message = "Erro ao excluir usuário.";
            return Page();
        }
    }
}
