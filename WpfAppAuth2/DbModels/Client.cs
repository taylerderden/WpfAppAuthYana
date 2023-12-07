using System;
using System.Collections.Generic;

namespace WpfAppAuth2.DbModels;

public partial class Client
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Patronymic { get; set; }

    public DateOnly Birthday { get; set; }

    public DateTime RegistrationDate { get; set; }

    public string? Email { get; set; }

    public string Phone { get; set; } = null!;

    public string GenderCode { get; set; } = null!;

    public byte[]? Photo { get; set; }

    public virtual ICollection<ClientService> ClientServices { get; set; } = new List<ClientService>();

    public virtual Gender GenderCodeNavigation { get; set; } = null!;

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
