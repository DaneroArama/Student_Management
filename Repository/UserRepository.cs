using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Student_Management.Model;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Data;

namespace Student_Management.Repository
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public void Add(UserModel userModel)
        {
            throw new NotImplementedException();
        }
        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser = false;

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT COUNT(*) FROM [User] WHERE Username = @Username AND Password = @Password";

                // Correctly add the parameters for username and password
                command.Parameters.AddWithValue("@Username", credential.UserName);
                command.Parameters.AddWithValue("@Password", credential.Password);

                // Execute the command and check if the count is greater than 0
                int userCount = (int)command.ExecuteScalar();

                // If a matching user is found, userCount will be > 0
                validUser = userCount > 0;
            }

            return validUser;
        }

        public void Edit(UserModel userModel)
        {
            throw new NotImplementedException();
        }
        public UserModel GetByUsername(string username)
        {
            UserModel user = null; // Initialize the user variable

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT Id, Username, Password, Email FROM [User] WHERE Username = @Username";
                command.Parameters.AddWithValue("@Username", SqlDbType.NVarChar).Value = username;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel()
                        {
                            Id = reader["Id"].ToString(),
                            Username = reader["Username"].ToString(),
                            Password = String.Empty, // Do not return the password
                            Email = reader["Email"].ToString(),
                        };
                    }
                }
            }
            return user;
        }
        public IEnumerable<UserModel> GetByAll()
        {
            throw new NotImplementedException();
        }
        public UserModel GetById(int id)
        {
            throw new NotImplementedException();
        }
        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}