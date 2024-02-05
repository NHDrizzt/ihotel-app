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
            throw new NotImplementedException();
        }

        public UserDto Login(LoginDto login)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == login.email && u.Password == login.password);
            if (user == null) {
                throw new InvalidOperationException("Incorrect e-mail or password"); 
            }
            return new UserDto {
                userId = user.UserId,
                name = user.Name,
                email = user.Email,
                userType = user.UserType
            };
        }
        public UserDto Add(UserDtoInsert user)
        {
            var updateUser = new User {
                Name = user.name,
                Email = user.email,
                Password = user.password,
                UserType = "client"
            };
            var userExists = _context.Users.FirstOrDefault(u => u.Email == updateUser.Email);
            if (userExists != null)
            {
                throw new InvalidOperationException("User email already exists");
            }
            _context.Users.Add(updateUser);
            _context.SaveChanges();
            return new UserDto {
                userId = updateUser.UserId,
                name = updateUser.Name,
                email = updateUser.Email,
                userType = updateUser.UserType
            };
        }

        public UserDto GetUserByEmail(string userEmail)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null) {
                return null;
            }
            return new UserDto {
                userId = user.UserId,
                name = user.Name,
                email = user.Email,
                userType = user.UserType
            };
        }

        public IEnumerable<UserDto> GetUsers()
        {
            var users = _context.Users.ToList();
            return users.Select(u => new UserDto {
                userId = u.UserId,
                name = u.Name,
                email = u.Email,
                userType = u.UserType
            });
        }

    }
}