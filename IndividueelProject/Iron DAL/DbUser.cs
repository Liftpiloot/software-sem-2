using System.Security.Cryptography;
using System.Text;
using Iron_DAL.DTO;
using Iron_Interface;
using Microsoft.Data.SqlClient;

namespace Iron_DAL;

public class DbUser : IDbUser
{
    private readonly string _db = "Server=localhost\\SQLEXPRESS;Database=iron;Trusted_Connection=True;Encrypt=False;";
    
    
    /// <summary>
    /// Returns userId on successful registration, 0 if user already exists, -1 on error.
    /// </summary>
    /// <returns>User</returns>
    public int AddUser(UserDto user)
    {
        SqlConnection conn;
        SqlCommand cmd;
        int id;
        try
        {
            // check if username or email already exists
            conn = new SqlConnection(_db);
            conn.Open();
            cmd =
                new SqlCommand("SELECT * FROM users WHERE username = @username OR LOWER(email) = LOWER(@email)", conn);
            cmd.Parameters.AddWithValue("@username", user.UserName);
            cmd.Parameters.AddWithValue("@email", user.Email);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                reader.Close();
                return 0; // User already exists
            }
            reader.Close();
        }
        catch (Exception e)
        {
            return -1;
        }
        
        // Insert user into db
        try
        {
            conn = new SqlConnection(_db);
            conn.Open();
            cmd = new SqlCommand(
                "INSERT INTO users (UserName, PasswordHash, Email, BirthDate, BodyWeight) VALUES (@username, @password, LOWER(@email), @dateOfBirth, @weight); SELECT SCOPE_IDENTITY();",
                conn);
            cmd.Parameters.AddWithValue("@username", user.UserName);
            cmd.Parameters.AddWithValue("@password", user.PasswordHash);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@dateOfBirth", user.DateOfBirth);
            cmd.Parameters.AddWithValue("@weight", user.Weight);
            id = Convert.ToInt32((object?)cmd.ExecuteScalar());
            conn.Close();
        }
        catch (Exception e)
        {
            return -1;
        }

        return id;
    }
    
    /// <summary>
    /// Returns user on successful login, null otherwise. Either username or email must be set.
    /// </summary>
    /// <returns>User</returns>
    public UserDto? Login(UserDto user)
    {
        try
        {
            SqlConnection conn = new SqlConnection(_db);
            conn.Open();
            SqlCommand cmd =
                new SqlCommand(
                    "SELECT * FROM users WHERE (UserName = @name OR LOWER(Email) = LOWER(@email)) AND PasswordHash = @password",
                    conn);
            cmd.Parameters.AddWithValue("@name", user.UserName);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@password", user.PasswordHash);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                user.Id = (int)reader["UserID"];
                user.DateOfBirth = (DateTime)reader["BirthDate"];
                user.Email = reader["Email"].ToString() ?? string.Empty;
                user.Weight = (decimal)reader["BodyWeight"];
                reader.Close();

                return user;
            }
            return null;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public bool EditWeight(int userId, decimal result)
    {
        try
        {
            SqlConnection conn = new SqlConnection(_db);
            conn.Open();
            SqlCommand cmd = new SqlCommand("UPDATE users SET BodyWeight = @weight WHERE UserID = @id", conn);
            cmd.Parameters.AddWithValue("@weight", result);
            cmd.Parameters.AddWithValue("@id", userId);
            int rows = cmd.ExecuteNonQuery();
            conn.Close();
            return rows > 0;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public bool ChangePassword(int userId, string modelNewPassword)
    {
        try
        {
            SqlConnection conn = new SqlConnection(_db);
            conn.Open();
            SqlCommand cmd = new SqlCommand("UPDATE users SET PasswordHash = @password WHERE UserID = @id", conn);
            cmd.Parameters.AddWithValue("@password", modelNewPassword);
            cmd.Parameters.AddWithValue("@id", userId);
            int rows = cmd.ExecuteNonQuery();
            conn.Close();
            return rows > 0;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public UserDto? GetUser(int userId)
    {
        try
        {
            SqlConnection conn = new SqlConnection(_db);
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM users WHERE UserID = @id", conn);
            cmd.Parameters.AddWithValue("@id", userId);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                UserDto user = new UserDto
                {
                    Id = (int)reader["UserID"],
                    UserName = reader["UserName"].ToString() ?? string.Empty,
                    Email = reader["Email"].ToString() ?? string.Empty,
                    DateOfBirth = (DateTime)reader["BirthDate"],
                    Weight = (decimal)reader["BodyWeight"]
                };
                reader.Close();
                return user;
            }
            return null;
        }
        catch (Exception e)
        {
            return null;
        }
    }
}