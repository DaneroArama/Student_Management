using System.Configuration;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using Student_Management.Model;

namespace Student_Management.Repository
{
    public class StudentRepository
    {
        private readonly string _connectionString;

        public StudentRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["StudentDB"].ConnectionString;
        }

        [Obsolete]
        public ObservableCollection<StudentModel> GetAllStudents()
        {
            var students = new ObservableCollection<StudentModel>();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Students", connection);
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(new StudentModel
                        {
                            Id = reader["ID"].ToString(),
                            RollNo = reader["RollNo"].ToString(),
                            Name = reader["Name"].ToString(),
                            Year = reader["Year"].ToString(),
                            Semester = reader["Semester"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Address = reader["Address"].ToString(),
                            ProfilePicture = reader["ProfilePicture"]?.ToString()
                        });
                    }
                }
            }
            return students;
        }

        public void UpdateStudent(StudentModel student)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    @"UPDATE Students 
                      SET RollNo = @RollNo, 
                          Name = @Name, 
                          Year = @Year, 
                          Semester = @Semester, 
                          Phone = @Phone, 
                          Address = @Address,
                          ProfilePicture = @ProfilePicture 
                      WHERE ID = @Id", connection);

                command.Parameters.AddWithValue("@Id", student.Id);
                command.Parameters.AddWithValue("@RollNo", student.RollNo ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Name", student.Name ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Year", student.Year ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Semester", student.Semester ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Phone", student.Phone ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Address", student.Address ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ProfilePicture", student.ProfilePicture ?? (object)DBNull.Value);

                command.ExecuteNonQuery();
            }
        }

        public void AddStudent(StudentModel student)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    @"INSERT INTO Students (RollNo, Name, Year, Semester, Phone, Address, ProfilePicture)
                      VALUES (@RollNo, @Name, @Year, @Semester, @Phone, @Address, @ProfilePicture)", connection);

                command.Parameters.AddWithValue("@RollNo", student.RollNo ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Name", student.Name ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Year", student.Year ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Semester", student.Semester ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Phone", student.Phone ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Address", student.Address ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ProfilePicture", student.ProfilePicture ?? (object)DBNull.Value);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteStudent(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SqlCommand(
                        "DELETE FROM Students WHERE ID = @Id", connection);

                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error deleting student: {ex.Message}", ex);
                }
            }
        }
    }
}