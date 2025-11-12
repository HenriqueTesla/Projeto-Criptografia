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
            var baseDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
            _filePath = Path.Combine(baseDir, "AppData", "users.json");
            Console.WriteLine("[DEBUG] Caminho do arquivo de usuários: " + _filePath);

            try
            {
                var dir = Path.GetDirectoryName(_filePath)!;
                if (!Directory.Exists(dir))
                {
                    Console.WriteLine("[DEBUG] Criando diretório: " + dir);
                    Directory.CreateDirectory(dir);
                }

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
                return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] LoadUsers failed: " + ex);
                return new List<User>();
            }
        }

        private void SaveUsers(List<User> users)
        {
            try
            {
                var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
                Console.WriteLine("[DEBUG] SaveUsers wrote file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] SaveUsers failed: " + ex);
                throw;
            }
        }

        public bool Register(string username, string password)
        {
            Console.WriteLine($"[DEBUG] Register called for '{username}'");
            var users = LoadUsers();
            Console.WriteLine("[DEBUG] Users count before: " + users.Count);

            if (users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("[DEBUG] Username already exists.");
                return false;
            }

            var user = new User { Username = username };
            user.PasswordHash = _hasher.HashPassword(user, password);

            users.Add(user);
            SaveUsers(users);
            Console.WriteLine("[DEBUG] New user added and saved.");
            return true;
        }

        public bool Login(string username, string password)
        {
            var users = LoadUsers();
            var user = users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (user == null) return false;
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
