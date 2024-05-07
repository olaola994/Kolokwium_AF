using System.Data;
using System.Data.SqlClient;
using Kolokwium.DTOs;

namespace Kolokwium.Repositories;

public class BooksRepository : IBookRepository
{
    private readonly IConfiguration _configuration;
    public BooksRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<BooksAuthorsDTO> GetInformationAboutAuthors(int id)
    {
        var query = "SELECT b.PK, b.title, a.first_name, a.last_name from books b join authors a on b.PK = a.PK where b.PK = @id";
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);
        await connection.OpenAsync();
        var result = command.ExecuteReader();
        var Book = new BooksAuthorsDTO();
        Book.Authors = new List<AuthorDTO>();
        while (result.Read())
        {
            Book.Id = result.GetInt32(0);
            Book.Title = result.GetString(1);
            if (!result.IsDBNull(2))
            {
                Book.Authors.Add(new AuthorDTO()
                {
                    FirstName = result.GetString(2),
                    LastName = result.GetString(3)
                });
            }
        }

        return Book;
    }

    public async Task<int> AddBook(AddBookDTO request)
    {
        var query = @"INSERT INTO books (Title) VALUES (@Title); SELECT SCOPE_IDENTITY();";
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand(query, connection);
        
        command.Parameters.AddWithValue("@Title", request.Title);
        await connection.OpenAsync();
        int bookId = Convert.ToInt32(await command.ExecuteScalarAsync());

        if (request.Authors != null)
        {
            foreach (var author in request.Authors)
            {
                command.Parameters.Clear();
                command.CommandText = "INSERT INTO books_authors (FK_book, FK_author) VALUES (@book, @author)";
                command.Parameters.AddWithValue("@book", bookId);
                command.Parameters.AddWithValue("@author", author.ID);
                command.ExecuteNonQuery();
            }
        }

        return bookId;
    }
    
}