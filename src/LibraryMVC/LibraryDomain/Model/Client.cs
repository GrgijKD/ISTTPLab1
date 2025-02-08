using System;
using System.Collections.Generic;

namespace LibraryDomain.Model;

public partial class Client: Entity
{
    public string FullName { get; set; } = null!;

    public virtual ICollection<ClientsBook> ClientsBooks { get; set; } = new List<ClientsBook>();
}
