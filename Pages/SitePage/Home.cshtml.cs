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

        [BindProperty]
        public string? Action { get; set; }

        public string Message { get; set; } = string.Empty;

        public void OnGet()
        {
            Username = Request.Cookies["username"] ?? "Usuário";
        }

        public IActionResult OnPost()
        {
            string loggedUser = Request.Cookies["username"] ?? "";

            if (string.IsNullOrEmpty(loggedUser))
            {
                Message = "Erro: usuário não identificado.";
                return Page();
            }

            if (Action == "Edit")
            {
                if (_userService.UpdateUsername(loggedUser, NewName!))
                {
                    Response.Cookies.Append("username", NewName!);
                    Message = "Nome atualizado com sucesso!";
                }
                else
                {
                    Message = "Não foi possível atualizar (nome já existe?).";
                }
            }
            else if (Action == "Delete")
            {
                if (_userService.DeleteUser(loggedUser))
                {
                    Response.Cookies.Delete("username");
                    return RedirectToPage("/SitePage/Login");
                }
                else
                {
                    Message = "Erro ao excluir usuário.";
                }
            }

            Username = Request.Cookies["username"] ?? loggedUser;
            return Page();
        }
    }
}
