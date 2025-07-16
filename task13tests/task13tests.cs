using Xunit;
using task13;

public class StudentSerializerTests
{
    [Fact]
    public void Serialize_ValidStudent_ShouldReturnCorrectStudent()
    {
        var student = new Student
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            BirthDate = new DateTime(2006, 1, 1),
            Grades = new List<Subject>
            {
                new Subject { Name = "Geometry", Grade = 5 },
                new Subject { Name = "Algebra", Grade = 4 }
            }
        };

        var json = StudentSerializer.Serialize(student);

        Assert.Contains("Ivan", json);
        Assert.Contains("Ivanov", json);
        Assert.Contains("01.01.2006", json);
        Assert.Contains("Geometry", json);
        Assert.Contains("5", json);
        Assert.Contains("Algebra", json);
        Assert.Contains("4", json);
    }

    [Fact]
    public void Serialize_NullProperties_ShouldIgnoreNullValues()
    {
        var student = new Student
        {
            FirstName = null,
            LastName = "Ivanov",
            BirthDate = new DateTime(2006, 1, 1),
            Grades = null
        };

        string json = StudentSerializer.Serialize(student);

        Assert.DoesNotContain("FirstName", json);
        Assert.Contains("Ivanov", json);
        Assert.Contains("01.01.2006", json);
        Assert.DoesNotContain("Grades", json);
    }

    [Fact]
    public void Deserialize_ValidJson_ShouldReturnCorrectStudent()
    {
        var json = @"
        {
            ""FirstName"": ""Ivan"",
            ""LastName"": ""Ivanov"",
            ""BirthDate"": ""01.01.2006"",
            ""Grades"": [
                { ""Name"": ""Geometry"", ""Grade"": 5 },
                { ""Name"": ""Algebra"", ""Grade"": 4}
            ]
        }";

        var student = StudentSerializer.Deserialize(json);

        Assert.Equal("Ivan", student.FirstName);
        Assert.Equal("Ivanov", student.LastName);
        Assert.Equal(new DateTime(2006, 1, 1), student.BirthDate);
        Assert.NotNull(student.Grades);
        Assert.Equal("Geometry", student.Grades[0].Name);
        Assert.Equal(5, student.Grades[0].Grade);
        Assert.Equal("Algebra", student.Grades[1].Name);
        Assert.Equal(4, student.Grades[1].Grade);
    }

    [Fact]
    public void Deserialize_NullProperties_ShouldThrowInvalidDataExceptionError()
    {
        var json = @"
        {
            ""FirstName"": null,
            ""LastName"": ""Ivanov"",
            ""BirthDate"": ""01.01.2006"",
            ""Grades"": null
        }";

        Assert.Throws<InvalidDataException>(() => StudentSerializer.Deserialize(json));
    }

    [Fact]
    public void FileOperations_SaveAndLoad_ShouldWorkCorrectly()
    {
        var student = new Student
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            BirthDate = new DateTime(2006, 1, 1),
            Grades = new List<Subject>
            {
                new Subject { Name = "Geometry", Grade = 5 },
                new Subject { Name = "Algebra", Grade = 4 }
            }
        };

        var path = "student.json";

        StudentSerializer.SaveToFile(path, student);
        var loaded = StudentSerializer.LoadFromFile(path);

        Assert.Equal(student.FirstName, loaded.FirstName);
        Assert.Equal(student.LastName, loaded.LastName);
        Assert.Equal(student.BirthDate, loaded.BirthDate);
        Assert.NotNull(loaded.Grades);
        Assert.Equal(student.Grades.Count, loaded.Grades.Count);

        File.Delete(path);
    }
}
