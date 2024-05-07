namespace Kolokwium.DTOs;

public class BooksAuthorsDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<AuthorDTO> Authors { get; set; }
}