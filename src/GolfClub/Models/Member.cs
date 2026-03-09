using System.ComponentModel.DataAnnotations;

namespace GolfClub.Models;

public enum Gender { Male, Female, Other }

public class Member
{
    public int MemberId { get; set; }

    [Required, StringLength(20)]
    public string MembershipNumber { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public Gender Gender { get; set; }

    [Required, Range(0, 54)]
    public int Handicap { get; set; }
}
