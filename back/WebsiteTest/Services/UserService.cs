using AnalyticSoftware.Database;
using AnalyticSoftware.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AnalyticSoftware.Services
{
    public class UserService
    {

        private readonly IMongoCollection<User> _users;

        public UserService(DatabaseContext dbContext)
        {
            _users = dbContext.Users;
        }

        public async Task RegisterUser(string superUserId, string email, string password, string role, string s3Bucket)
        {
            var superUser = await _users.Find(user => user.Id == new ObjectId(superUserId) && user.Role == "superuser").FirstOrDefaultAsync();

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
    }
}
