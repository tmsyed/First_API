using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using First_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace First_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IConfiguration configuration;

        public BookController(IConfiguration iConfig)
        {
            configuration = iConfig;
        }

        // GET: api/Book
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            IEnumerable<Book> queryResult;

            var sb = new StringBuilder();
            sb.Append(@"SELECT * FROM Books");

            string connectionString = configuration.GetConnectionString("test_database");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                queryResult = connection.Query<Book>(sb.ToString());
            }

            return Ok(queryResult.ToList());
        }

        // GET: api/Book/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            IEnumerable<Book> queryResult;

            var sb = new StringBuilder();
            sb.Append(@"SELECT * FROM Books WHERE Id=@Id");

            using (var connection = new SqlConnection(configuration.GetConnectionString("test_database")))
            {
                connection.Open();

                queryResult = connection.Query<Book>(sb.ToString(), new { Id = id });
            }

            return Ok(queryResult.ToList());
        }

        //POST: api/Book
        [HttpPost]
        public IActionResult Post([FromBody] Book bookDto)
        {
            if (bookDto == null)
            {
                return BadRequest();
            }

            var sb = new StringBuilder();
            sb.Append("SET IDENTITY_INSERT dbo.Books ON ");
            sb.Append(@"INSERT INTO [dbo].[Books] ([ID], [BookName], [ISBN]) VALUES (@ID, @bookName, @isbn)");

            using (var connection = new SqlConnection(configuration.GetConnectionString("test_database")))
            {
                connection.Open();

                int count = connection.Execute(sb.ToString(), bookDto);

                if(count <= 0)
                {
                    return BadRequest("Already exists");
                }
            }

            return Ok(bookDto);
        }

        // PUT: api/Book/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Book bookDto)
        {
            if (bookDto == null)
            {
                return BadRequest();
            }

            var sb = new StringBuilder();
            sb.Append(@"UPDATE Books SET [BookName] = @BookName, ISBN = @ISBN WHERE ID = @ID");

            using (var connection = new SqlConnection(configuration.GetConnectionString("test_database")))
            {
                connection.Open();

                int count = connection.Execute(sb.ToString(), bookDto);

                if (count <= 0)
                {
                    return BadRequest("Already exists");
                }
            }

            return Ok(bookDto);
        }

      

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            
            var sb = new StringBuilder();
            sb.Append(@"DELETE FROM Books WHERE Id=@Id");

            using (var connection = new SqlConnection(configuration.GetConnectionString("test_database")))
            {
                connection.Open();

                int count = connection.Execute(sb.ToString(), new { Id = id});

                if (count <= 0)
                {
                    return BadRequest("Resource could not be found for deletion");
                }
            }

            return Ok("Resource deleted");
        }
    }
}
