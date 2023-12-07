using System;
using System.Collections.Generic;

namespace WpfAppAuth2.DbModels;

public partial class ProductPhoto
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string PhotoPath { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
