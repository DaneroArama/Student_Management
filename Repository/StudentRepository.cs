using System;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using Student_Management.Model;
using System.Collections.Generic;
using System.Windows;
using System.Diagnostics;

namespace Student_Management.Repository
{
    public class StudentRepository
    {
        private readonly string _connectionString;

        public StudentRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["StudentDB"].ConnectionString + ";TrustServerCertificate=True;Encrypt=False";
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
                try
                {
                    connection.Open();
                    var command = new SqlCommand(
                        @"SELECT c.ClassID, c.ClassName, c.YearId, c.Semester,
                                 y.YearLevel, y.YearType 
                          FROM Classes c
                          JOIN Years y ON c.YearId = y.YearId
                          WHERE c.YearId = @YearId 
                          AND c.Semester = @Semester
                          ORDER BY c.ClassName", connection);

                    command.Parameters.AddWithValue("@YearId", year);
                    command.Parameters.AddWithValue("@Semester", semester);
                    
                    System.Diagnostics.Debug.WriteLine($"Executing GetClassesByYearAndSemester - Year: {year}, Semester: {semester}");
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var classModel = new ClassModel
                            {
                                ClassID = Convert.ToInt32(reader["ClassID"]),
                                ClassName = reader["ClassName"].ToString(),
                                YearId = Convert.ToInt32(reader["YearId"]),
                                Semester = Convert.ToInt32(reader["Semester"]),
                                YearLevel = reader["YearLevel"].ToString(),
                                YearType = reader["YearType"].ToString()
                            };
                            classes.Add(classModel);
                        }
                    }
                    
                    System.Diagnostics.Debug.WriteLine($"Found {classes.Count} classes");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in GetClassesByYearAndSemester: {ex.Message}");
                    throw;
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

        public ObservableCollection<StudentModel> GetStudentsByYearAndSemester(int year, int semester, DateTime date, int classId)
        {
            var students = new ObservableCollection<StudentModel>();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    
                    // Debug information
                    System.Diagnostics.Debug.WriteLine($"Executing SQL query for Year: {year}, Semester: {semester}, Date: {date.ToShortDateString()}, ClassID: {classId}");
                    
                    var command = new SqlCommand(
                        @"SELECT s.ID, s.RollNo, s.Name, s.YearId, s.Semester, s.Phone, s.Address, s.ProfilePicture,
                                 CASE WHEN a.Status = 'Present' THEN 1 ELSE 0 END as IsPresent
                          FROM Students s
                          LEFT JOIN Attendance a ON s.ID = a.StudentID 
                               AND a.AttendanceDate = @Date
                               AND a.ClassID = @ClassID
                          WHERE s.YearId = @YearId 
                          AND s.Semester = @Semester
                          ORDER BY s.Name", connection);

                    command.Parameters.AddWithValue("@YearId", year);
                    command.Parameters.AddWithValue("@Semester", semester);
                    command.Parameters.AddWithValue("@Date", date.Date);
                    command.Parameters.AddWithValue("@ClassID", classId);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(new StudentModel
                            {
                                Id = reader["ID"].ToString(),
                                RollNo = reader["RollNo"].ToString(),
                                Name = reader["Name"].ToString(),
                                YearId = Convert.ToInt32(reader["YearId"]),
                                Semester = Convert.ToInt32(reader["Semester"]),
                                Phone = reader["Phone"].ToString(),
                                Address = reader["Address"].ToString(),
                                ProfilePicture = reader["ProfilePicture"]?.ToString(),
                                IsPresent = Convert.ToBoolean(reader["IsPresent"])
                            });
                        }
                    }
                    
                    // Debug information
                    System.Diagnostics.Debug.WriteLine($"Found {students.Count} students in the database");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Database error: {ex.Message}");
                    throw;
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
                          YearId = @Year, 
                          Semester = @Semester, 
                          Phone = @Phone, 
                          Address = @Address,
                          ProfilePicture = @ProfilePicture 
                      WHERE ID = @Id", connection);

                command.Parameters.AddWithValue("@Id", student.Id);
                command.Parameters.AddWithValue("@RollNo", student.RollNo ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Name", student.Name ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Year", student.YearId);
                command.Parameters.AddWithValue("@Semester", student.Semester);
                command.Parameters.AddWithValue("@Phone", student.Phone ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Address", student.Address ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ProfilePicture", student.ProfilePicture ?? (object)DBNull.Value);

                command.ExecuteNonQuery();
            }
        }

        public void AddStudent(StudentModel student)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    // Debug the student data before saving
                    Debug.WriteLine($"Adding student: YearId={student.YearId}, YearLevel={student.YearLevel}, YearType={student.YearType}");
                    
                    // Make sure we have the correct YearId based on YearType
                    UpdateStudentYearId(student);
                    
                    string query = @"INSERT INTO Students (RollNo, Name, YearId, Semester, Phone, Address, ProfilePicture) 
                                    VALUES (@RollNo, @Name, @YearId, @Semester, @Phone, @Address, @ProfilePicture);
                                    SELECT SCOPE_IDENTITY();";
                    
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RollNo", student.RollNo);
                        command.Parameters.AddWithValue("@Name", student.Name);
                        command.Parameters.AddWithValue("@YearId", student.YearId);
                        command.Parameters.AddWithValue("@Semester", student.Semester);
                        command.Parameters.AddWithValue("@Phone", string.IsNullOrEmpty(student.Phone) ? DBNull.Value : (object)student.Phone);
                        command.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(student.Address) ? DBNull.Value : (object)student.Address);
                        command.Parameters.AddWithValue("@ProfilePicture", student.ProfilePicture == null ? DBNull.Value : (object)student.ProfilePicture);
                        
                        // Get the ID of the newly inserted student
                        var result = command.ExecuteScalar();
                        student.Id = result.ToString();
                        
                        Debug.WriteLine($"Added student with ID: {student.Id}, YearId: {student.YearId}, YearType: {student.YearType}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding student: {ex}");
                throw;
            }
        }

        // Add this new method to ensure the correct YearId is set based on YearType
        private void UpdateStudentYearId(StudentModel student)
        {
            try
            {
                // If YearId is already set and YearType is empty, we need to get the YearType
                if (student.YearId > 0 && string.IsNullOrEmpty(student.YearType))
                {
                    var years = GetAllYearConfigurations();
                    var year = years.FirstOrDefault(y => y.YearId == student.YearId);
                    if (year != null)
                    {
                        student.YearType = year.YearType;
                        student.YearLevel = year.YearLevel;
                        Debug.WriteLine($"Set YearType to {student.YearType} based on YearId {student.YearId}");
                    }
                }
                // If YearType is set but YearId doesn't match, we need to update YearId
                else if (!string.IsNullOrEmpty(student.YearType))
                {
                    var years = GetAllYearConfigurations();
                    
                    // Try to find a match based on YearLevel and YearType
                    var year = years.FirstOrDefault(y => 
                        y.YearLevel == student.YearLevel && 
                        y.YearType == student.YearType);
                    
                    // If not found, try to find by first word of YearLevel and YearType
                    if (year == null && !string.IsNullOrEmpty(student.YearLevel))
                    {
                        string firstWord = student.YearLevel.Split(' ')[0]; // Get "First", "Second", etc.
                        year = years.FirstOrDefault(y => 
                            y.YearLevel.StartsWith(firstWord) && 
                            y.YearType == student.YearType);
                        
                        Debug.WriteLine($"Trying to match with first word: {firstWord} and type: {student.YearType}");
                    }
                    
                    if (year != null && year.YearId != student.YearId)
                    {
                        student.YearId = year.YearId;
                        Debug.WriteLine($"Updated YearId to {student.YearId} based on YearType {student.YearType}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in UpdateStudentYearId: {ex}");
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
                    @"SELECT s.ID, s.RollNo, s.Name, s.YearId, s.Semester, s.Phone, s.Address, s.ProfilePicture,
                             y.YearLevel, y.YearType
                      FROM Students s
                      LEFT JOIN Years y ON s.YearId = y.YearId
                      ORDER BY s.Name", connection);
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var student = new StudentModel
                        {
                            Id = reader["ID"].ToString(),
                            RollNo = reader["RollNo"].ToString(),
                            Name = reader["Name"].ToString(),
                            YearId = Convert.ToInt32(reader["YearId"]),
                            YearLevel = reader["YearLevel"].ToString(),
                            YearType = reader["YearType"].ToString(), // Get YearType directly from database
                            Semester = Convert.ToInt32(reader["Semester"]),
                            Phone = reader["Phone"].ToString(),
                            Address = reader["Address"].ToString(),
                            ProfilePicture = reader["ProfilePicture"]?.ToString(),
                            IsPresent = false // Default value for attendance
                        };
                        
                        // If YearType is empty, try to get it from the YearId
                        if (string.IsNullOrEmpty(student.YearType))
                        {
                            UpdateStudentYearId(student);
                        }
                        
                        Debug.WriteLine($"Loaded student: ID={student.Id}, YearId={student.YearId}, YearLevel={student.YearLevel}, YearType={student.YearType}");
                        students.Add(student);
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

        public ObservableCollection<YearConfig> GetAllYearConfigurations()
        {
            var years = new ObservableCollection<YearConfig>();
            
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    string query = "SELECT YearId, YearLevel, YearType, NumberOfSemesters FROM Years ORDER BY YearId";
                    
                    using (var command = new SqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var year = new YearConfig
                            {
                                YearId = Convert.ToInt32(reader["YearId"]),
                                YearLevel = reader["YearLevel"].ToString(),
                                YearType = reader["YearType"].ToString(),
                                NumberOfSemesters = Convert.ToInt32(reader["NumberOfSemesters"])
                            };
                            
                            System.Diagnostics.Debug.WriteLine($"Loaded year: ID={year.YearId}, Level={year.YearLevel}, Type={year.YearType}, Semesters={year.NumberOfSemesters}");
                            years.Add(year);
                        }
                    }
                }
                
                System.Diagnostics.Debug.WriteLine($"Total years loaded: {years.Count}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading years: {ex.Message}");
                MessageBox.Show($"Error loading years: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    "SELECT ClassID, ClassName, YearId, Semester FROM Classes WHERE YearId = @YearId", 
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
                                YearId = Convert.ToInt32(reader["YearId"]),
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
                    @"INSERT INTO Classes (ClassName, YearId, Semester) 
                      VALUES (@ClassName, @Year, @Semester); 
                      SELECT SCOPE_IDENTITY()", 
                    connection))
                {
                    command.Parameters.AddWithValue("@ClassName", classModel.ClassName);
                    command.Parameters.AddWithValue("@Year", classModel.YearId);
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
                    @"INSERT INTO Years (YearLevel, NumberOfSemesters) 
                      VALUES (@YearLevel, @NumberOfSemesters); 
                      SELECT SCOPE_IDENTITY()", 
                    connection))
                {
                    command.Parameters.AddWithValue("@YearLevel", year.YearLevel);
                    command.Parameters.AddWithValue("@NumberOfSemesters", year.NumberOfSemesters);
                    year.YearId = Convert.ToInt32(command.ExecuteScalar());
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
                              WHERE ClassID IN (SELECT ClassID FROM Classes WHERE YearId = @YearId)", 
                            connection, transaction))
                        {
                            command.Parameters.AddWithValue("@YearId", yearId);
                            command.ExecuteNonQuery();
                        }

                        // Then delete the classes
                        using (var command = new SqlCommand(
                            "DELETE FROM Classes WHERE YearId = @YearId", 
                            connection, transaction))
                        {
                            command.Parameters.AddWithValue("@YearId", yearId);
                            command.ExecuteNonQuery();
                        }

                        // Finally delete the year
                        using (var command = new SqlCommand(
                            "DELETE FROM Years WHERE YearId = @YearId", 
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
                          YearId = @Year, 
                          Semester = @Semester 
                      WHERE ClassID = @ClassID", connection))
                {
                    command.Parameters.AddWithValue("@ClassID", classModel.ClassID);
                    command.Parameters.AddWithValue("@ClassName", classModel.ClassName);
                    command.Parameters.AddWithValue("@Year", classModel.YearId);
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
                      SET YearLevel = @YearLevel, 
                          NumberOfSemesters = @NumberOfSemesters 
                      WHERE YearId = @YearId", connection))
                {
                    command.Parameters.AddWithValue("@YearId", year.YearId);
                    command.Parameters.AddWithValue("@YearLevel", year.YearLevel);
                    command.Parameters.AddWithValue("@NumberOfSemesters", year.NumberOfSemesters);
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool StudentsExistForYearAndSemester(int yearId, int semester)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SqlCommand(
                        @"SELECT COUNT(*) FROM Students 
                          WHERE YearId = @YearId AND Semester = @Semester", 
                        connection);
                    
                    command.Parameters.AddWithValue("@YearId", yearId);
                    command.Parameters.AddWithValue("@Semester", semester);
                    
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error checking students: {ex.Message}");
                    return false;
                }
            }
        }

        public List<AttendanceModel> GetAttendanceForDate(DateTime date)
        {
            var attendanceList = new List<AttendanceModel>();
            
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    var command = new SqlCommand(@"
                        SELECT a.*, s.Name as StudentName, c.ClassName 
                        FROM Attendance a
                        INNER JOIN Students s ON a.StudentID = s.ID
                        INNER JOIN Classes c ON a.ClassID = c.ClassID
                        WHERE CONVERT(date, a.AttendanceDate) = @Date", connection);
                    
                    command.Parameters.AddWithValue("@Date", date.Date);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            attendanceList.Add(new AttendanceModel
                            {
                                AttendanceID = reader.GetInt32(reader.GetOrdinal("AttendanceID")),
                                StudentID = reader.GetInt32(reader.GetOrdinal("StudentID")),
                                ClassID = reader.GetInt32(reader.GetOrdinal("ClassID")),
                                AttendanceDate = reader.GetDateTime(reader.GetOrdinal("AttendanceDate")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                Remarks = reader.IsDBNull(reader.GetOrdinal("Remarks")) ? null : reader.GetString(reader.GetOrdinal("Remarks"))
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting attendance for date: {ex.Message}");
                throw;
            }

            return attendanceList;
        }

        public List<AttendanceModel> GetAttendanceForDateRange(DateTime startDate, DateTime endDate)
        {
            var attendanceList = new List<AttendanceModel>();
            
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    var command = new SqlCommand(@"
                        SELECT a.*, s.Name as StudentName, c.ClassName 
                        FROM Attendance a
                        INNER JOIN Students s ON a.StudentID = s.ID
                        INNER JOIN Classes c ON a.ClassID = c.ClassID
                        WHERE CONVERT(date, a.AttendanceDate) BETWEEN @StartDate AND @EndDate", connection);
                    
                    command.Parameters.AddWithValue("@StartDate", startDate.Date);
                    command.Parameters.AddWithValue("@EndDate", endDate.Date);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            attendanceList.Add(new AttendanceModel
                            {
                                AttendanceID = reader.GetInt32(reader.GetOrdinal("AttendanceID")),
                                StudentID = reader.GetInt32(reader.GetOrdinal("StudentID")),
                                ClassID = reader.GetInt32(reader.GetOrdinal("ClassID")),
                                AttendanceDate = reader.GetDateTime(reader.GetOrdinal("AttendanceDate")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                Remarks = reader.IsDBNull(reader.GetOrdinal("Remarks")) ? null : reader.GetString(reader.GetOrdinal("Remarks"))
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting attendance for date range: {ex.Message}");
                throw;
            }

            return attendanceList;
        }

        public AttendanceModel GetAttendanceForStudentAndClass(int studentId, int classId, DateTime date)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    var command = new SqlCommand(@"
                        SELECT AttendanceID, StudentID, ClassID, AttendanceDate, Status, Remarks
                        FROM Attendance 
                        WHERE StudentID = @StudentID 
                        AND ClassID = @ClassID 
                        AND CONVERT(date, AttendanceDate) = @Date", connection);

                    command.Parameters.AddWithValue("@StudentID", studentId);
                    command.Parameters.AddWithValue("@ClassID", classId);
                    command.Parameters.AddWithValue("@Date", date.Date);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new AttendanceModel
                            {
                                AttendanceID = reader.GetInt32(reader.GetOrdinal("AttendanceID")),
                                StudentID = reader.GetInt32(reader.GetOrdinal("StudentID")),
                                ClassID = reader.GetInt32(reader.GetOrdinal("ClassID")),
                                AttendanceDate = reader.GetDateTime(reader.GetOrdinal("AttendanceDate")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                Remarks = reader.IsDBNull(reader.GetOrdinal("Remarks")) ? null : reader.GetString(reader.GetOrdinal("Remarks"))
                            };
                        }
                    }
                }
                return null; // Return null if no attendance record found
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting attendance for student {studentId} in class {classId}: {ex.Message}");
                throw;
            }
        }
    }
}