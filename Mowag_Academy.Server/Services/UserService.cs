using MongoDB.Driver;
using Mowag_Academy.Server.Models;
using Mowag_Academy.Shared.DTOs;

namespace Mowag_Academy.Server.Services
{
    public class UsersService
    {
        private readonly IMongoCollection<MongoUser> _usersCollection;

        public UsersService(IMongoDatabase database)
        {
            _usersCollection = database.GetCollection<MongoUser>("users");
        }

        // Get all users, mapping MongoUser to UserDTO
        public async Task<List<UserDto>> GetAsync()
        {
            var mongoUsers = await _usersCollection.Find(_ => true).ToListAsync();
            return mongoUsers.ConvertAll(ToUserDTO);
        }

        // Get a single user by ID
        public async Task<UserDto> GetAsync(string id)
        {
            var mongoUser = await _usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
            return mongoUser == null ? null : ToUserDTO(mongoUser);
        }

        // Create a new user
        public async Task CreateAsync(UserDto userDto)
        {
            var mongoUser = ToMongoUser(userDto);
            await _usersCollection.InsertOneAsync(mongoUser);
        }

        // Update an existing user
        public async Task UpdateAsync(string id, UserDto updatedUser)
        {
            var mongoUser = ToMongoUser(updatedUser);
            mongoUser.Id = id; // Ensure the ID is consistent
            await _usersCollection.ReplaceOneAsync(u => u.Id == id, mongoUser);
        }

        // Delete a user by ID
        public async Task RemoveAsync(string id)
        {
            await _usersCollection.DeleteOneAsync(u => u.Id == id);
        }

        // Mapping method: MongoUser to UserDTO
        private UserDto ToUserDTO(MongoUser mongoUser)
        {
            return new UserDto
            {
                Id = mongoUser.Id,
                Username = mongoUser.Username,
                Firstname = mongoUser.Firstname,
                Lastname = mongoUser.Lastname,
                CourseDate = mongoUser.CourseDate,
                PossibleStartingYear = mongoUser.PossibleStartingYear,
                Password = mongoUser.PasswordHash,
                Role = mongoUser.Role   
            };
        }

        // Mapping method: UserDTO to MongoUser
        private static MongoUser ToMongoUser(UserDto userDto)
        {
            return new MongoUser
            {
                Id = userDto.Id,
                Username = userDto.Username,
                Firstname = userDto.Firstname,
                Lastname = userDto.Lastname,
                CourseDate = userDto.CourseDate,
                PossibleStartingYear = userDto.PossibleStartingYear,
                PasswordHash = userDto.Password,
                Role = userDto.Role
            };
        }
    }
}
