using Kolokwium.DTOs;

namespace Kolokwium.Repositories;

public interface IBookRepository
{
    Task<BooksAuthorsDTO> GetInformationAboutAuthors(int id);
    Task<int> AddBook(AddBookDTO request);
}