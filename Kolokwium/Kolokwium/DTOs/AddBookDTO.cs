namespace Kolokwium.DTOs;

public class AddBookDTO
{
   public string Title { get; set; }
   public List<AddAuthorDTO> Authors { get; set; }
   
}