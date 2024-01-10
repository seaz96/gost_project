namespace Gost_Project.Data.Models;

public class ChangeUserRoleModel
{
    public long UserId { get; set; }
    
    public bool IsAdmin { get; set; }
}