namespace TrybeHotel.Dto
{
    public class UserDto {
        public int userId { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string userType { get; set; }
    }

    public class UserDtoInsert {
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }

    public class LoginDto {
      public string email { get; set; }
      public string password { get; set; }
    }
}