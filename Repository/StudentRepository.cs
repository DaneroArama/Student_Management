using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using Student_Management.Model;
using System.Collections.Generic;

namespace Student_Management.Repository
{
    public class StudentRepository
    {
        private readonly string _connectionString;

        public StudentRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["StudentDB"].ConnectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public ObservableCollection<ClassModel> GetClassesByYearAndSemester(string yearText, string semesterText)
        {
            var classes = new ObservableCollection<ClassModel>();
            
            // Convert year text to number
            int year = ConvertYearTextToNumber(yearText);
            // Convert semester text to number
            int semester = semesterText.StartsWith("First") ? 1 : 2;
            
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(
                    @"SELECT ClassID, ClassName, Year, Semester 
                      FROM Classes 
                      WHERE Year = @Year 
                      AND Semester = @Semester
                      ORDER BY ClassName", connection);

                command.Parameters.AddWithValue("@Year", year);
                command.Parameters.AddWithValue("@Semester", semester);
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        classes.Add(new ClassModel
                        {
                            ClassID = Convert.ToInt32(reader["ClassID"]),
                            ClassName = reader["ClassName"].ToString(),
                            Year = Convert.ToInt32(reader["Year"]),
                            Semester = Convert.ToInt32(reader["Semester"])
                        });
                    }
                }
            }
            return classes;
        }

        public ObservableCollection<string> GetAllYears()
        {
            // Return a fixed list of all possible years
            return new ObservableCollection<string>
            {
                "First Year",
                "Second Year",
                "Third Year",
                "Fourth Year",
                "First Year (Honors)",
                "Second Year (Honors)",
                "Third Year (Honors)",
                "Fourth Year (Honors)"
            };
        }

        public ObservableCollection<string> GetAllSemesters()
        {
            return new ObservableCollection<string>
            {
                "First Semester",
                "Second Semester"
            };
        }

        public ObservableCollection<StudentModel> GetStudentsByYearAndSemester(int year, int semester, DateTime date)
        {
            var students = new ObservableCollection<StudentModel>();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    @"SELECT s.ID, s.RollNo, s.Name, s.Year, s.Semester, s.Phone, s.Address, s.ProfilePicture,
                             CASE WHEN a.Status = 'Present' THEN 1 ELSE 0 END as IsPresent
                      FROM Students s
                      LEFT JOIN Attendance a ON s.ID = a.StudentID 
                           AND a.AttendanceDate = @Date
                      WHERE s.Year = @Year 
                      AND s.Semester = @Semester
                      ORDER BY s.Name", connection);

                command.Parameters.AddWithValue("@Year", year);
                command.Parameters.AddWithValue("@Semester", semester);
                command.Parameters.AddWithValue("@Date", date.Date);
                
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
                            ProfilePicture = reader["ProfilePicture"]?.ToString(),
                            IsPresent = Convert.ToBoolean(reader["IsPresent"])
                        });
                    }
                }
            }
            return students;
        }

        public void SaveAttendance(int studentId, int classId, DateTime date, string status)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                // First check if attendance record already exists
                var checkCommand = new SqlCommand(
                    @"SELECT AttendanceID 
                      FROM Attendance 
                      WHERE StudentID = @StudentID 
                      AND ClassID = @ClassID 
                      AND AttendanceDate = @Date", connection);
                
                checkCommand.Parameters.AddWithValue("@StudentID", studentId);
                checkCommand.Parameters.AddWithValue("@ClassID", classId);
                checkCommand.Parameters.AddWithValue("@Date", date.Date);
                
                var existingId = checkCommand.ExecuteScalar();
                
                if (existingId != null)
                {
                    // Update existing record
                    var updateCommand = new SqlCommand(
                        @"UPDATE Attendance 
                          SET Status = @Status 
                          WHERE AttendanceID = @AttendanceID", connection);
                    
                    updateCommand.Parameters.AddWithValue("@Status", status);
                    updateCommand.Parameters.AddWithValue("@AttendanceID", existingId);
                    updateCommand.ExecuteNonQuery();
                }
                else
                {
                    // Insert new record
                    var insertCommand = new SqlCommand(
                        @"INSERT INTO Attendance (StudentID, ClassID, AttendanceDate, Status) 
                          VALUES (@StudentID, @ClassID, @Date, @Status)", connection);
                    
                    insertCommand.Parameters.AddWithValue("@StudentID", studentId);
                    insertCommand.Parameters.AddWithValue("@ClassID", classId);
                    insertCommand.Parameters.AddWithValue("@Date", date.Date);
                    insertCommand.Parameters.AddWithValue("@Status", status);
                    insertCommand.ExecuteNonQuery();
                }
            }
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

        public ObservableCollection<StudentModel> GetAllStudents()
        {
            var students = new ObservableCollection<StudentModel>();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    @"SELECT ID, RollNo, Name, Year, Semester, Phone, Address, ProfilePicture
                      FROM Students 
                      ORDER BY Name", connection);
                
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
                            ProfilePicture = reader["ProfilePicture"]?.ToString(),
                            IsPresent = false // Default value for attendance
                        });
                    }
                }
            }
            return students;
        }

        private int ConvertYearTextToNumber(string yearText)
        {
            switch (yearText)
            {
                case "First Year": return 1;
                case "Second Year": return 2;
                case "Third Year": return 3;
                case "Fourth Year": return 4;
                case "First Year (Honors)": return 5;
                case "Second Year (Honors)": return 6;
                case "Third Year (Honors)": return 7;
                case "Fourth Year (Honors)": return 8;
                default: return 1;
            }
        }

        public List<YearConfig> GetAllYearConfigurations()
        {
            var years = new List<YearConfig>();
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(
                    "SELECT Year, YearName, NumberOfSemesters FROM Years", 
                    connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            years.Add(new YearConfig
                            {
                                Id = Convert.ToInt32(reader["Year"]),
                                YearName = reader["YearName"].ToString(),
                                NumberOfSemesters = Convert.ToInt32(reader["NumberOfSemesters"])
                            });
                        }
                    }
                }
            }
            return years;
        }

        public List<ClassModel> GetClassesForYear(int yearId)
        {
            var classes = new List<ClassModel>();
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(
                    "SELECT ClassID, ClassName, Year, Semester FROM Classes WHERE Year = @YearId", 
                    connection))
                {
                    command.Parameters.AddWithValue("@YearId", yearId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            classes.Add(new ClassModel
                            {
                                ClassID = Convert.ToInt32(reader["ClassID"]),
                                ClassName = reader["ClassName"].ToString(),
                                Year = Convert.ToInt32(reader["Year"]),
                                Semester = Convert.ToInt32(reader["Semester"])
                            });
                        }
                    }
                }
            }
            return classes;
        }

        public void AddClass(ClassModel classModel)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(
                    @"INSERT INTO Classes (ClassName, Year, Semester) 
                      VALUES (@ClassName, @Year, @Semester); 
                      SELECT SCOPE_IDENTITY()", 
                    connection))
                {
                    command.Parameters.AddWithValue("@ClassName", classModel.ClassName);
                    command.Parameters.AddWithValue("@Year", classModel.Year);
                    command.Parameters.AddWithValue("@Semester", classModel.Semester);
                    classModel.ClassID = Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public void DeleteClass(int classId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // First delete attendance records for this class
                        using (var command = new SqlCommand(
                            "DELETE FROM Attendance WHERE ClassID = @ClassId", 
                            connection, transaction))
                        {
                            command.Parameters.AddWithValue("@ClassId", classId);
                            command.ExecuteNonQuery();
                        }

                        // Then delete the class
                        using (var command = new SqlCommand(
                            "DELETE FROM Classes WHERE ClassID = @ClassId", 
                            connection, transaction))
                        {
                            command.Parameters.AddWithValue("@ClassId", classId);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void AddYear(YearConfig year)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(
                    @"INSERT INTO Years (YearName, NumberOfSemesters) 
                      VALUES (@YearName, @NumberOfSemesters); 
                      SELECT SCOPE_IDENTITY()", 
                    connection))
                {
                    command.Parameters.AddWithValue("@YearName", year.YearName);
                    command.Parameters.AddWithValue("@NumberOfSemesters", year.NumberOfSemesters);
                    year.Id = Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public void DeleteYear(int yearId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // First delete attendance records for all classes in this year
                        using (var command = new SqlCommand(
                            @"DELETE FROM Attendance 
                              WHERE ClassID IN (SELECT ClassID FROM Classes WHERE Year = @YearId)", 
                            connection, transaction))
                        {
                            command.Parameters.AddWithValue("@YearId", yearId);
                            command.ExecuteNonQuery();
                        }

                        // Then delete the classes
                        using (var command = new SqlCommand(
                            "DELETE FROM Classes WHERE Year = @YearId", 
                            connection, transaction))
                        {
                            command.Parameters.AddWithValue("@YearId", yearId);
                            command.ExecuteNonQuery();
                        }

                        // Finally delete the year
                        using (var command = new SqlCommand(
                            "DELETE FROM Years WHERE Year = @YearId", 
                            connection, transaction))
                        {
                            command.Parameters.AddWithValue("@YearId", yearId);
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void UpdateClass(ClassModel classModel)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(
                    @"UPDATE Classes 
                      SET ClassName = @ClassName, 
                          Year = @Year, 
                          Semester = @Semester 
                      WHERE ClassID = @ClassID", connection))
                {
                    command.Parameters.AddWithValue("@ClassID", classModel.ClassID);
                    command.Parameters.AddWithValue("@ClassName", classModel.ClassName);
                    command.Parameters.AddWithValue("@Year", classModel.Year);
                    command.Parameters.AddWithValue("@Semester", classModel.Semester);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateYear(YearConfig year)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(
                    @"UPDATE Years 
                      SET YearName = @YearName, 
                          NumberOfSemesters = @NumberOfSemesters 
                      WHERE Year = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", year.Id);
                    command.Parameters.AddWithValue("@YearName", year.YearName);
                    command.Parameters.AddWithValue("@NumberOfSemesters", year.NumberOfSemesters);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}