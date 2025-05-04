using System.ComponentModel.DataAnnotations.Schema;

[Table("UserAccount")]
public class UserAccountModel
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = "Default Username";
    public string Email { get; set; } = "Default Email";
}