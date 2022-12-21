
namespace TaskPlanner.BLL.DTO.User
{
    public class UpdateUserDto
    {
        public int UserID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string UserName { get; set; }
        public string Passwords { get; set; }
    }
}
