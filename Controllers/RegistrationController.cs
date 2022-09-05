using cubichi.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

[ApiController]
[Route("")]
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

    [HttpPost, Route("register")]
    public IActionResult Register(UserReg user)
    {
        try
        {
            var command = new NpgsqlCommand($"SELECT 1 FROM \"users\" WHERE username = '{user.UserName}'", _connection);
            if (command.ExecuteScalar() != null)
                return BadRequest("User already exists");

            var insertCommand = new NpgsqlCommand($"INSERT INTO \"users\"(username, password) VALUES ('{user.UserName}', encode(digest('{user.Password}', 'sha256'), 'hex'));", _connection);
            insertCommand.ExecuteNonQuery();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost, Route("UploadSkin")]
    public IActionResult UploadSkin(SkinUploading request)
    {
        try
        {
            var command = new NpgsqlCommand($"SELECT 1 FROM \"users\" WHERE username = '{request.UserName}' AND password = encode(digest('{request.Password}', 'sha256'), 'hex')", _connection);
            if (command.ExecuteScalar() == null)
                return BadRequest("Wrong username or password");

            using (var stream = new FileStream(Path.Combine(@"C:\\Users\\drtta\\Documents\\Programming\\cubichi", "kekis" + ".png"), FileMode.Create))
            {
                request.File.CopyTo(stream);
            }

            return Ok("Skin Uploaded Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}