using SharedLibrary.Model;
using SharedLibrary.DTOs;
namespace SharedLibrary.Mappers
{
    public static class UserMapper
    {
        public static UserDTO EntityToDto(ApplicationUser user)
        {
            return new UserDTO
            {
                
                FullName = user.FullName,
                username = user.UserName,
                EmailAddress=user.Email,
                Department=user.Department
            };
        }

        public static ApplicationUser DtoToEntity(UserDTO userDto)
        {
            return new ApplicationUser
            {
               
                FullName=userDto.FullName,
                Email=userDto.EmailAddress,
                UserName=userDto.username,
                Department=userDto.Department
            };
        }
    }
}
