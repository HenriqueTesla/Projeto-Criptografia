using System.Text.Json;
using Isopoh.Cryptography.Argon2;
using Projeto_Criptografia.Models;

namespace Projeto_Criptografia.Services
{
    public class UserService
    {
        private readonly string _filePath;

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
            catch { }
        }

        public bool Register(string username, string password)
        {
            username = username.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            var users = LoadUsers();

            if (users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                return false;

            string hash = Argon2.Hash(password);

            users.Add(new User
            {
                Username = username,
                PasswordHash = hash
            });

            SaveUsers(users);
            return true;
        }

        public bool Login(string username, string password)
        {
            username = username.Trim();

            var users = LoadUsers();
            var user = users.FirstOrDefault(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user == null)
                return false;

            return Argon2.Verify(user.PasswordHash, password);
        }

        public bool UpdateUsername(string oldUsername, string newUsername)
        {
            var users = LoadUsers();

            var user = users.FirstOrDefault(u =>
                u.Username.Equals(oldUsername, StringComparison.OrdinalIgnoreCase));

            if (user == null)
                return false;

            if (users.Any(u =>
                u.Username.Equals(newUsername, StringComparison.OrdinalIgnoreCase)))
                return false;

            user.Username = newUsername.Trim();
            SaveUsers(users);

            return true;
        }

        public bool DeleteUser(string username)
        {
            var users = LoadUsers();

            var user = users.FirstOrDefault(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user == null)
                return false;

            users.Remove(user);
            SaveUsers(users);

            return true;
        }
    }
}
