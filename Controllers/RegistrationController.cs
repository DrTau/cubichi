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
        string connectionString = $"Host=localhost;Username=cubics;Password=timoha;Database=cubich";
        _connection = new NpgsqlConnection(connectionString);
        _connection.Open();
    }

    ~RegistrationController()
    {
        _connection.Close();
    }

    [HttpPost]
    public IActionResult Register(UserReg user)
    {
        try
        {
            var command = new NpgsqlCommand($"SELECT 1 FROM users WHERE username = '{user.UserName}'", _connection);
            if (command.ExecuteScalar() != null)
                return BadRequest("User already exists");

            command = new NpgsqlCommand($"INSERT INTO users(username, password) VALUES ('{user.UserName}', ''encode(digest('{user.Password}', 'sha256'), 'hex'));");
            command.ExecuteNonQuery();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}