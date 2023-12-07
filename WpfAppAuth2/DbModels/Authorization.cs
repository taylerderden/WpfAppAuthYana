using System;
using System.Collections.Generic;

namespace WpfAppAuth2.DbModels;

public partial class Authorization
{
    public int IdAuthorization { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;
}
