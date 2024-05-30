using System;
using System.Collections.Generic;

// Interfaces
public interface IPersonService
{
    int CalculateAge();
    decimal CalculateSalary();
    List<string> GetAddresses();
}

public interface IStudentService : IPersonService
{
    void EnrollInCourse(Course course);
    double CalculateGPA();
}

public interface IInstructorService : IPersonService
{
    void AssignDepartment(Department department);
}

// Abstract Base Class
public abstract class Person : IPersonService
{
    public string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    private List<string> Addresses { get; set; } = new List<string>();
    public decimal BaseSalary { get; set; }

    public int CalculateAge()
    {
        return DateTime.Now.Year - DateOfBirth.Year;
    }

    public virtual decimal CalculateSalary()
    {
        if (BaseSalary < 0)
            throw new ArgumentException("Salary cannot be negative.");
        return BaseSalary;
    }

    public void AddAddress(string address)
    {
        Addresses.Add(address);
    }

    public List<string> GetAddresses()
    {
        return Addresses;
    }
}

// Derived Classes
public class Student : Person, IStudentService
{
    private List<Course> Courses { get; set; } = new List<Course>();
    private Dictionary<Course, char> Grades { get; set; } = new Dictionary<Course, char>();

    public void EnrollInCourse(Course course)
    {
        Courses.Add(course);
        course.AddStudent(this);
    }

    public void AddGrade(Course course, char grade)
    {
        Grades[course] = grade;
    }

    public double CalculateGPA()
    {
        if (Grades.Count == 0)
            return 0.0;

        double totalPoints = 0;
        foreach (var grade in Grades.Values)
        {
            totalPoints += GradeToPoints(grade);
        }
        return totalPoints / Grades.Count;
    }

    private double GradeToPoints(char grade)
    {
        switch (grade)
        {
            case 'A': return 4.0;
            case 'B': return 3.0;
            case 'C': return 2.0;
            case 'D': return 1.0;
            case 'F': return 0.0;
            default: return 0.0;
        }
    }
}

public class Instructor : Person, IInstructorService
{
    public Department Department { get; private set; }
    public DateTime JoinDate { get; set; }
    public decimal Bonus { get; set; }

    public void AssignDepartment(Department department)
    {
        Department = department;
    }

    public override decimal CalculateSalary()
    {
        return base.CalculateSalary() + CalculateExperienceBonus();
    }

    private decimal CalculateExperienceBonus()
    {
        int yearsOfExperience = DateTime.Now.Year - JoinDate.Year;
        return yearsOfExperience * Bonus;
    }
}

// Course and Department Classes
public class Course
{
    public string Name { get; set; }
    private List<Student> EnrolledStudents { get; set; } = new List<Student>();

    public void AddStudent(Student student)
    {
        EnrolledStudents.Add(student);
    }

    public List<Student> GetStudents()
    {
        return EnrolledStudents;
    }
}

public class Department
{
    public string Name { get; set; }
    public Instructor Head { get; set; }
    public decimal Budget { get; set; }
    public List<Course> Courses { get; set; } = new List<Course>();
}

// Color and Ball Classes
public class Color
{
    public int Red { get; set; }
    public int Green { get; set; }
    public int Blue { get; set; }
    public int Alpha { get; set; }

    public Color(int red, int green, int blue, int alpha = 255)
    {
        Red = red;
        Green = green;
        Blue = blue;
        Alpha = alpha;
    }

    public int GetGrayScale()
    {
        return (Red + Green + Blue) / 3;
    }
}

public class Ball
{
    public int Size { get; private set; }
    public Color Color { get; private set; }
    private int throwCount;

    public Ball(int size, Color color)
    {
        Size = size;
        Color = color;
        throwCount = 0;
    }

    public void Pop()
    {
        Size = 0;
    }

    public void Throw()
    {
        if (Size > 0)
        {
            throwCount++;
        }
    }

    public int GetThrowCount()
    {
        return throwCount;
    }
}

// Main Program
class Program3
{
    static void Main(string[] args)
    {
        // Test Person, Student, and Instructor
        Student student = new Student { Name = "John Doe", DateOfBirth = new DateTime(2000, 1, 1), BaseSalary = 0 };
        Instructor instructor = new Instructor { Name = "Dr. Smith", DateOfBirth = new DateTime(1980, 1, 1), BaseSalary = 50000, JoinDate = new DateTime(2010, 1, 1), Bonus = 1000 };

        Department department = new Department { Name = "Computer Science", Head = instructor, Budget = 1000000 };
        instructor.AssignDepartment(department);

        Course course = new Course { Name = "Programming 101" };
        student.EnrollInCourse(course);
        student.AddGrade(course, 'A');

        Console.WriteLine($"{student.Name}'s GPA: {student.CalculateGPA()}");
        Console.WriteLine($"{instructor.Name}'s Salary: {instructor.CalculateSalary()}");

        // Test Color and Ball
        Color redColor = new Color(255, 0, 0);
        Ball ball = new Ball(5, redColor);
        ball.Throw();
        ball.Throw();
        ball.Pop();
        ball.Throw(); // This should not increase the throw count

        Console.WriteLine($"Ball throw count: {ball.GetThrowCount()}");
    }
}
