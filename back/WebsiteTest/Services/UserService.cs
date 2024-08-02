using AnalyticSoftware.Database;
using AnalyticSoftware.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Net.WebSockets;

namespace AnalyticSoftware.Services
{
    public class UserService
    {

        private readonly IMongoCollection<User> _users;
        private readonly SecurityService _securityService;

        public UserService(DatabaseContext dbContext, SecurityService securityService)
        {
            _users = dbContext.Users;
            _securityService = securityService;
        }

        public async Task RegisterUser(User superUser, string email, string password, string role, string s3Bucket)
        {
            if (superUser == null)
            {
                // Make this Json response in the future
                throw new Exception("Only Superuser can add accounts"); 
            }

            var userExists = await _users.Find(user => user.Email == email).FirstOrDefaultAsync();

            if (userExists != null)
            {
                // Make this Json response
                throw new Exception("User already exists");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var newUser = new User
            {
                Email = email,
                PasswordHash = passwordHash,
                Role = role,
                S3Bucket = s3Bucket
            };
            await _users.InsertOneAsync(newUser);
        }

        public async Task<User> CreateSuperUserAsync(string email, string password)
        {
            var existingUser = await _users.Find(user => user.Email == email).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                throw new InvalidOperationException("Super user already exists.");
            }

            var passwordHash = _securityService.HashPassword(password);
            var superUser = new User
            {
                Email = email,
                Role = "SuperUser",
                PasswordHash = passwordHash
            };

            await _users.InsertOneAsync(superUser);
            return superUser;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _users.Find(user => user.Email == email).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> GetUserById(ObjectId id)
        {
            var user = await _users.Find(user => user.Id.Equals(id)).FirstOrDefaultAsync(); ;
            return user;
        }
    }
}
