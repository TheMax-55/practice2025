using System.Text.Json;
using System.Text.Json.Serialization;
namespace task13;

public class Subject
{
    public string? Name { get; set; }
    public int Grade { get; set; }
}

public class Student
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public List<Subject>? Grades { get; set; }
}

public class DateConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.Parse(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("dd.MM.yyyy"));
    }
}

public static class StudentSerializer
{
    static readonly JsonSerializerOptions options = new JsonSerializerOptions
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new DateConverter() }
    };

    public static string Serialize(Student student)
        => JsonSerializer.Serialize(student, options);

    public static Student Deserialize(string json)
    {
        var student = JsonSerializer.Deserialize<Student>(json, options);
        if (student == null
        || string.IsNullOrWhiteSpace(student.FirstName)
        || string.IsNullOrWhiteSpace(student.LastName)
        || student.BirthDate == default
        || student.Grades == null)
            throw new InvalidDataException("Объект с некорректными данными.");
        return student;
    }

    public static void SaveToFile(string path, Student student)
        => File.WriteAllText(path, Serialize(student));

    public static Student LoadFromFile(string path)
        => Deserialize(File.ReadAllText(path));
}
