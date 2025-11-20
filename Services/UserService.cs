using System.Text.Json;
using Microsoft.AspNetCore.Identity;

namespace Projeto_Criptografia.Services
{
    public class User
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }

    public class UserService
    {
        private readonly string _filePath;
        private readonly PasswordHasher<User> _hasher = new();

        public UserService()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            var projectDir = Path.Combine(appData, "Projeto-Criptografia");

            if (!Directory.Exists(projectDir))
            {
                Console.WriteLine("[DEBUG] Criando pasta do projeto em AppData: " + projectDir);
                Directory.CreateDirectory(projectDir);
            }

            _filePath = Path.Combine(projectDir, "users.json");
            Console.WriteLine("[DEBUG] Caminho REAL do users.json: " + _filePath);

            try
            {
                if (!File.Exists(_filePath))
                {
                    Console.WriteLine("[DEBUG] Criando arquivo vazio users.json");
                    File.WriteAllText(_filePath, "[]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] Erro ao preparar arquivo users.json: " + ex);
                throw;
            }
        }

        private List<User> LoadUsers()
        {
            try
            {
                var json = File.ReadAllText(_filePath);
                var users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
                Console.WriteLine($"[DEBUG] LoadUsers carregou {users.Count} usuários.");
                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] LoadUsers falhou: " + ex);
                return new List<User>();
            }
        }

        private void SaveUsers(List<User> users)
        {
            try
            {
                var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
                Console.WriteLine("[DEBUG] Usuários salvos com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] SaveUsers falhou: " + ex);
                throw;
            }
        }

        public bool Register(string username, string password)
        {
            Console.WriteLine($"[DEBUG] Register chamado para '{username}'");

            var users = LoadUsers();
            Console.WriteLine("[DEBUG] Quantidade de usuários antes: " + users.Count);

            if (users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("[DEBUG] Usuário já existe.");
                return false;
            }

            var user = new User { Username = username };
            user.PasswordHash = _hasher.HashPassword(user, password);

            users.Add(user);
            SaveUsers(users);

            Console.WriteLine("[DEBUG] Novo usuário salvo com sucesso.");
            return true;
        }

        public bool Login(string username, string password)
        {
            Console.WriteLine($"[DEBUG] Login chamado para '{username}'");

            var users = LoadUsers();
            var user = users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                Console.WriteLine("[DEBUG] Usuário não encontrado.");
                return false;
            }

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);

            Console.WriteLine("[DEBUG] Verificação de senha: " + result);

            return result == PasswordVerificationResult.Success;
        }
    }
}
