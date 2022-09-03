// Controller to register new users.

using CubichiReg.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

[ApiController]
[Route("[controller]")]
public class RegistrationController : ControllerBase
{
    private readonly NpgsqlConnection _connection;

    public RegistrationController()
    {
        string connectionString = $"Host=localhost;Username=cubich;Password=timoha;Database=cubich";
        _connection = new NpgsqlConnection(connectionString);
        _connection.Open();
    }

    ~RegistrationController()
    {
        _connection.Close();
    }

    [HttpPost, Route("register/{Username}/{Password}")]
    public IActionResult Register(string Username, string Password)
    {
        try
        {
            var command = new NpgsqlCommand($"SELECT 1 FROM \"users\" WHERE username = '{Username}'", _connection);
            if (command.ExecuteScalar() != null)
                return BadRequest("User already exists");

            var insertCommand = new NpgsqlCommand($"INSERT INTO \"users\"(username, password) VALUES ('{Username}', encode(digest('{Password}', 'sha256'), 'hex'));", _connection);
            insertCommand.ExecuteNonQuery();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}