namespace htdc_api.Models;

public class UserProfile : BaseModel
{
    public string AspNetUserId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Avatar { get; set; }

    public DateTime? LastLogin { get; set; }

    public int? BranchId { get; set; } = 0;
}