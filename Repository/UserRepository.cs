using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly ITrybeHotelContext _context;
        public UserRepository(ITrybeHotelContext context)
        {
            _context = context;
        }
        public UserDto GetUserById(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId) ?? throw new Exception("User not found");
            return new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                UserType = user.UserType
            };
        }

        public UserDto Login(LoginDto login)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == login.Email && u.Password == login.Password) ?? throw new Exception("Invalid email or password");
            return new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                UserType = user.UserType
            };
        }
        public UserDto Add(UserDtoInsert user)
        {
            var hasThisUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);

            if (hasThisUser != null)
            {
                throw new Exception("User email already exists");
            }

            _context.Users.Add(new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                UserType = "client"
            });
            _context.SaveChanges();

            var addedUser = _context.Users.FirstOrDefault(u => u.Email == user.Email) ?? throw new Exception("Error trying to get the new added User.");
            return new UserDto
            {
                UserId = addedUser.UserId,
                Name = addedUser.Name,
                Email = addedUser.Email,
                Password = addedUser.Password,
                UserType = addedUser.UserType
            };
        }

        public UserDto GetUserByEmail(string userEmail)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail) ?? throw new Exception("User not found");
            return new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                UserType = user.UserType
            };
        }

        public IEnumerable<UserDto> GetUsers()
        {
            var users = _context.Users.ToList();
            return users.Select(u => new UserDto
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email,
                Password = u.Password,
                UserType = u.UserType
            });
        }

    }
}