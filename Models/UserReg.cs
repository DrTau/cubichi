using System.ComponentModel.DataAnnotations;

namespace CubichiReg.Models;

public class UserReg
{
    [Required, MinLength(3)]
    public string UserName { get; set; } = string.Empty;
    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;
}