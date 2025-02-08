using System;
using System.Collections.Generic;

namespace LibraryDomain.Model;

public partial class Worker: Entity
{
    public string FullName { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
