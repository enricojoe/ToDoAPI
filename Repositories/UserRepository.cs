using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Security.Cryptography;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            }
        }
        public async Task<string> AuthenticateUserAsync(User user)
        {
            using (IDbConnection connection = Connection)
            {

                var userResult = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM [user] WHERE Username = @Username",
                                                                             new { user.Username });
                // return null if user not found
                if (userResult == null) return "User tidak ditemukan";

                byte[] salt = userResult.Salt;
                if (!VerifyPassword(user.PasswordHash, userResult.PasswordHash, salt))
                {
                    return "Password salah";
                }

                // authentication successful so generate jwt token
                var token = await generateJwtToken(user);

                return token;
            }
        }
        public async Task<string> AddUserAsync(User user)
        {
            using (IDbConnection connection = Connection)
            {
                try
                {
                    byte[] salt;
                    var password = HashPasword(user.PasswordHash, out salt);
                    user.PasswordHash = password;
                    user.Salt = salt;
                    var query = "INSERT INTO [user] (username, passwordHash, salt) VALUES(@username, @passwordHash, @salt)";
                    var result = await connection.ExecuteAsync(query, user);
                    if (result > 0)
                    {
                        return "User berhasil didaftarkan.";
                    }
                    else
                    {
                        return "Gagal mendaftarkan user.";
                    }
                }
                catch (SqlException ex)
                {
                    return "Username sudah pernah digunakan";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred: {ex.Message}");
                    return "Terdapat error";
                }
            }
        }
        public async Task<List<User>> GetAll()
        {
            using (IDbConnection connection = Connection)
            {
                var result = await connection.QueryAsync<User>("SELECT * FROM [user]");
                return result.ToList();
            }
        }
        public async Task<User?> GetById(int id)
        {
            using (IDbConnection connection = Connection)
            {
                var result = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM [user] WHERE id = @id", new { id });
                return result;
            }
        }
        public async Task<User?> GetByName(string Username)
        {
            using (IDbConnection connection = Connection)
            {
                var result = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM [user] WHERE Username = @Username", new { Username });
                return result;
            }
        }

        private const int keySize = 64;
        private const int iterations = 5;
        private HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        private string HashPasword(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(keySize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);

            return Convert.ToHexString(hash);
        }

        private bool VerifyPassword(string password, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }

        private async Task<string> generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = await Task.Run(() =>
            {
                var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:Secret"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { 
                        new Claim("id", user.Id.ToString()),
                        new Claim("iss", "ToDoServer"), 
                        new Claim("aud", "ToDoApp"),
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                return tokenHandler.CreateToken(tokenDescriptor);
            });

            return tokenHandler.WriteToken(token);
        }
    }
}


//public async Task<User?> AddAndUpdateUser(User user)
//{
//    using (IDbConnection connection = Connection)
//    {
//        bool isSuccess = false;
//        if (user.Id > 0)
//        {
//            var findUser = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM [user] WHERE Id = @id",
//                                                                           new { user.Id });

//            if (findUser != null)
//            {
//                // obj.Address = userObj.Address;
//                obj.FirstName = userObj.FirstName;
//                obj.LastName = userObj.LastName;
//                db.Users.Update(obj);
//                isSuccess = await db.SaveChangesAsync() > 0;
//            }
//        }
//        else
//        {
//            var query = "INSERT INTO [user] (name, urgent, done) VALUES(@name, @urgent, @done)";
//            var result = await connection.ExecuteAsync(query, todo);

//            await db.Users.AddAsync(userObj);
//            isSuccess = await db.SaveChangesAsync() > 0;
//        }

//        return isSuccess ? user : null;
//    }
//}