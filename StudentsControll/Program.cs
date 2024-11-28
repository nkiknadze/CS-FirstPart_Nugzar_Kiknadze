namespace StudentsControll
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;

    class Student
    {
        public string Name { get; set; }
        public int ListNumber { get; set; }
        public char Grade { get; set; }

        public Student(string name, int listNumber, char grade)
        {
            Name = name;
            ListNumber = listNumber;
            Grade = grade;
        }

        public void Display()
        {
            Console.WriteLine($"Name: {Name}, ListNumber: {ListNumber}, Grade: {Grade}");
        }
    }

    class Program
    {
        static List<Student> students = new List<Student>();
        static string filePath = "C:\\Users\\nkiknadze\\Desktop\\C# Course\\TestN1\\Studens.txt";

        static void Main()
        {
            // Load data from the file at startup
            LoadDataFromFile();

            while (true)
            {
                ShowMenu();

                Console.Write("airchie moqmedeba: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        StudentisDamateba();
                        break;
                    case "2":
                        YvelaStudentisNaxva();
                        break;
                    case "3":
                        StudentisDzebna();
                        break;
                    case "4":
                        StudentisShefasebisGanaxleba();
                        break;
                    case "5":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("sheiyvanet swori mnishvneloba!");
                        break;
                }
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("\n Student Controll System");
            Console.WriteLine("1. axali studentis damateba");
            Console.WriteLine("2. yvela studentis naxva");
            Console.WriteLine("3. studentis dzebna siis nomrit");
            Console.WriteLine("4. studentistvis shefasebis ganaxleba");
            Console.WriteLine("5. gamosvla");
        }

        static void StudentisDamateba()
        {
            Console.Write("sheiyvanet saxeli: ");
            string name = Console.ReadLine();

            Console.Write("sheiyvanes siis nomeri: ");
            if (!int.TryParse(Console.ReadLine(), out int listNumber))
            {
                Console.WriteLine("araswori mnishvneloba, sheiyvanet tavidan.");
                return;
            }
            if (students.Any(student => student.ListNumber == listNumber))
            {
                Console.WriteLine($"studenti siis nomrit {listNumber} ukve arsebobs! sheiyvanet sxva nomeri.");
                return;
            }

            Console.Write("sheiyvanet shefasebis qula (1-5): ");
            char grade;
            if (!char.TryParse(Console.ReadLine(), out grade) || !"12345".Contains(grade))
            {
                Console.WriteLine("araswori mnishvneloba, sheiyvanet tavidan.");
                return;
            }

            students.Add(new Student(name, listNumber, grade));
            Console.WriteLine($"studenti - {name} nomrit - {listNumber}  damatebulia!");

            
            SaveDataToFile();
        }

        static void YvelaStudentisNaxva()
        {
            if (students.Count == 0)
            {
                Console.WriteLine("studentebi ar aris sheyvanili.");
                return;
            }

            Console.WriteLine("\n sia");
            foreach (var student in students)
            {
                student.Display();
            }
        }

        static void StudentisDzebna()
        {
            Console.Write("sheiyvanet studentis siis nomeri: ");
            if (!int.TryParse(Console.ReadLine(), out int rollNumber))
            {
                Console.WriteLine("araswori mnishvneloba, sheiyvanet tavidan.");
                return;
            }

            var student = students
                .Where(s => s.ListNumber == rollNumber)
                .FirstOrDefault();

            if (student != null)
            {
                student.Display();
            }
            else
            {
                Console.WriteLine("studenti ar moidzebna.");
            }
        }

        static void StudentisShefasebisGanaxleba()
        {
            Console.Write("sheiyvanet studentis siis nomeri shefasebis shesacvlelad: ");
            if (!int.TryParse(Console.ReadLine(), out int rollNumber))
            {
                Console.WriteLine("araswori mnishvneloba, sheiyvanet tavidan.");
                return;
            }

            var student = students
                .FirstOrDefault(s => s.ListNumber == rollNumber);

            if (student != null)
            {
                Console.Write("sheiyvanet axali qula (1-5): ");
                char grade;
                if (!char.TryParse(Console.ReadLine().ToUpper(), out grade) || !"12345".Contains(grade))
                {
                    Console.WriteLine("araswori mnishvneloba, sheiyvanet tavidan.");
                    return;
                }

                student.Grade = grade;
                Console.WriteLine("studentis qula sheicvala!");

                SaveDataToFile();
            }
            else
            {
                Console.WriteLine("studenti ar moidzebna.");
            }
        }


        static void SaveDataToFile()
        {
            using (StreamWriter sw = new StreamWriter(filePath, false))
            {
                foreach (var student in students)
                {
                    sw.WriteLine($"name - {student.Name},listnumber - {student.ListNumber}, studentGrate - {student.Grade}");
                }
            }
        }

        static void LoadDataFromFile()
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 3)
                    {
                        string name = parts[0];
                        int listNumber = int.Parse(parts[1]);
                        char grade = char.Parse(parts[2]);

                        students.Add(new Student(name, listNumber, grade));
                    }
                }
            }
        }
    }
}
