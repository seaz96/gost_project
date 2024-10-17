namespace GostStorage.Models.Accounts;

public class ChangeUserRoleModel
{
    public long UserId { get; set; }

    public bool IsAdmin { get; set; }
}