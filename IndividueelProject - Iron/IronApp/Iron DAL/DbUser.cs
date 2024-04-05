using System.Security.Cryptography;
using System.Text;
using Iron_DAL.DTO;
using Microsoft.Data.SqlClient;

namespace Iron_DAL;

public class DbUser
{
    private string _db = "Server=localhost\\SQLEXPRESS;Database=iron;Trusted_Connection=True;Encrypt=False;";
    
    
    /// <summary>
    /// Returns user on successful registration, null otherwise.
    /// </summary>
    /// <returns>User</returns>
    public int AddUser(UserDto? user)
    {
        SqlConnection conn;
        SqlCommand cmd;
        int id = -1;
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
        
        // Create sha256 hash
        using (SHA256 sha256Hash = SHA256.Create())
        {
            Byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes((string)user.PasswordHash));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            user.PasswordHash = builder.ToString();
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
        // Retrieve user from db
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd =
            new SqlCommand(
                "SELECT * FROM users WHERE (UserName = @name OR LOWER(Email) = LOWER(@email)) AND PasswordHash = @password",
                conn);
        cmd.Parameters.AddWithValue("@name", user.UserName ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@email", user.Email ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@password", user.PasswordHash);
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            user.Id = (int)reader["UserID"];
            user.DateOfBirth = reader["BirthDate"].ToString();
            user.Email = reader["Email"].ToString();
            user.Weight = (decimal)reader["BodyWeight"];
            reader.Close();

            return user;
        }
        return null;
    }
    
    
}