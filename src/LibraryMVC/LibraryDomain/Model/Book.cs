using System;
using System.Collections.Generic;

namespace LibraryDomain.Model;

public partial class Book: Entity
{
    public string Title { get; set; } = null!;

    public int PublisherId { get; set; }

    public int AddedBy { get; set; }

    public virtual Worker AddedByNavigation { get; set; } = null!;

    public virtual ICollection<AuthorsBook> AuthorsBooks { get; set; } = new List<AuthorsBook>();

    public virtual ICollection<ClientsBook> ClientsBooks { get; set; } = new List<ClientsBook>();

    public virtual ICollection<GenresBook> GenresBooks { get; set; } = new List<GenresBook>();

    public virtual Publisher Publisher { get; set; } = null!;
}
