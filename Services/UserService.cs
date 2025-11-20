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

        public UserService(IWebHostEnvironment env)
        {
            var dataDir = Path.Combine(env.WebRootPath, "data");

            if (!Directory.Exists(dataDir))
                Directory.CreateDirectory(dataDir);

            _filePath = Path.Combine(dataDir, "users.json");

            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath, "[]");
        }

        private List<User> LoadUsers()
        {
            try
            {
                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            catch
            {
                return new List<User>();
            }
        }

        private void SaveUsers(List<User> users)
        {
            try
            {
                string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
            catch
            {
            }
        }

        public bool Register(string username, string password)
        {
            username = username.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            var users = LoadUsers();

            if (users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                return false;

            var user = new User { Username = username };
            user.PasswordHash = _hasher.HashPassword(user, password);

            users.Add(user);
            SaveUsers(users);

            return true;
        }

        public bool Login(string username, string password)
        {
            username = username.Trim();

            var users = LoadUsers();
            var user = users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user == null)
                return false;

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
