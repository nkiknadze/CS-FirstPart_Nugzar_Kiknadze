using System;
using System.Collections.Generic;
using System.IO;

namespace ATM_ConsoleApp
{

    //გადარიცხვის ნაწილი რომ დავამატო. ლოგში შევუშა. მიმღები უნდა გვყავდეს იუზერების ნაწილშ და შევხედოთ ID ით.
    class Program
    {
        // filebis paths
        private const string userDataFile = "C:\\Users\\nkiknadze\\Desktop\\C# Course\\TestN1\\ATM_Users.txt";
        private const string logFilePath = "C:\\Users\\nkiknadze\\Desktop\\C# Course\\TestN1\\ATM_Log.txt";

        static void Main(string[] args)
        {
            EnsureDataFilesExist();
            MainMenu();
        }

        // sachiro failebis shemowmeba
        private static void EnsureDataFilesExist()
        {
            if (!File.Exists(userDataFile))
                File.WriteAllText(userDataFile, "");

            if (!File.Exists(logFilePath))
                File.WriteAllText(logFilePath, "");
        }

        // Main Menu
        private static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("ATMMachine");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                Console.Write("airchie: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Register();
                        break;
                    case "2":
                        Login();
                        break;
                    case "3":
                        Console.WriteLine("ByBy!");
                        return;
                    default:
                        Console.WriteLine("shecdoma. airchie swori mnishvneloba.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        // axali user
        private static void Register()
        {
            Console.Clear();
            Console.WriteLine("Register");

            Console.Write("name: ");
            string name = Console.ReadLine();

            Console.Write("password: ");
            string password = Console.ReadLine();

            string userId = Guid.NewGuid().ToString().Substring(0, 8);

            // sawyisi balansi
            decimal balance = 0;

            // user monacemebis shenaxva
            File.AppendAllText(userDataFile, $"{userId}|{name}|{password}|{balance}\n");

            Console.WriteLine($"warmatebit daregistrirda User ID aris: {userId} da Name aris {name}");
            Console.ReadLine();
        }

        // Login
        private static void Login()
        {
            Console.Clear();
            Console.WriteLine("Login");

            Console.Write("sheiyvane UserName: ");
            string name = Console.ReadLine();

            Console.Write("sheiyvane password: ");
            string password = Console.ReadLine();

            string[] users = File.ReadAllLines(userDataFile);
            foreach (var user in users)
            {
                string[] details = user.Split('|');
                if (details[1] == name && details[2] == password)
                {
                    Console.WriteLine("warmatebulia!");
                    UserMenu(name, details[0], decimal.Parse(details[3]));
                    return;
                }
            }

            Console.WriteLine("araswori momxmarebeli an paroli!!!");
            Console.ReadLine();
        }
        private static void TransferAmount(string senderId, string senderName, ref decimal senderBalance)
        {
            Console.Write("sheiyvane mimgebis User ID: ");
            string recipientId = Console.ReadLine();

            string[] users = File.ReadAllLines(userDataFile);

            // მოძებნე მიმღები ტექსტურ ფაილში
            string recipientUser = Array.Find(users, user => user.Split('|')[0] == recipientId);

            if (recipientUser == null)
            {
                Console.WriteLine("mimgebi ar moidzebna!");
                return;
            }

            string[] recipientDetails = recipientUser.Split('|');
            decimal recipientBalance = decimal.Parse(recipientDetails[3]);

            Console.Write("sheiyvanet gadasaricxi tanxa: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal transferAmount) && transferAmount > 0)
            {
                if (transferAmount > senderBalance)
                {
                    Console.WriteLine("arasakmarisi nashti!");
                    return;
                }

                // განახლება ანგარიშებზე
                senderBalance -= transferAmount;
                recipientBalance += transferAmount;

                // განახლება ფაილში
                for (int i = 0; i < users.Length; i++)
                {
                    string[] details = users[i].Split('|');
                    if (details[0] == senderId)
                    {
                        users[i] = $"{details[0]}|{details[1]}|{details[2]}|{senderBalance}";
                    }
                    else if (details[0] == recipientId)
                    {
                        users[i] = $"{details[0]}|{details[1]}|{details[2]}|{recipientBalance}";
                    }
                }

                File.WriteAllLines(userDataFile, users);

                Console.WriteLine($"warmatebit gadairicxa ${transferAmount:F2} {recipientDetails[1]} stan (ID: {recipientId}).");

                // ლოგირება
                LogTransaction(senderId, senderName, $"gadaricxulia ${transferAmount:F2} to {recipientDetails[1]} (ID: {recipientId}).");
                LogTransaction(recipientId, recipientDetails[1], $"migebulia ${transferAmount:F2} from {senderName} (ID: {senderId}).");
            }
            else
            {
                Console.WriteLine("araswori tanxa!");
            }
        }
        // User menu for operations
        private static void UserMenu(string userId, string name, decimal balance)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Hi, {name} (ID: {userId})");
                Console.WriteLine("1. balansisi shemowmeba");
                Console.WriteLine("2. tanxis shemotana");
                Console.WriteLine("3. tanxis gatana");
                Console.WriteLine("4. gadaricxva momxmareblebshorisi");
                Console.WriteLine("5. gamosvla");
                Console.Write("airchie: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine($"mimdinare balansi aris: ${balance:F2}");
                        LogTransaction(userId, name, "balance");
                        break;

                    case "2":
                        Console.Write("sheiyvanet shesatani tanxis moculoba: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal depositAmount) && depositAmount > 0)
                        {
                            balance += depositAmount;
                            Console.WriteLine($"shetanilia ${depositAmount:F2}. axali balansi aris ${balance:F2}.");
                            LogTransaction(userId, name, $"shetanilia ${depositAmount:F2}");
                        }
                        else
                        {
                            Console.WriteLine("araswori tanxa.");
                        }
                        break;

                    case "3":
                        Console.Write("sheiyvanet gasatani tanxis moculoba: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal withdrawAmount) && withdrawAmount > 0)
                        {
                            if (withdrawAmount <= balance)
                            {
                                balance -= withdrawAmount;
                                Console.WriteLine($"gatanilia ${withdrawAmount:F2}. axali balansi aris ${balance:F2}.");
                                LogTransaction(userId, name, $"gatanilia ${withdrawAmount:F2}");
                            }
                            else
                            {
                                Console.WriteLine("arasakmarisi nasshti.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("araswori tanxa.");
                        }
                        break;
                    case "4":
                        TransferAmount(userId, name, ref balance);
                        break;
                    case "5":
                        UpdateBalance(userId, balance);
                        Console.WriteLine("Logged out!");
                        return;

                    default:
                        Console.WriteLine("airchie swori mnishvneloba.");
                        break;
                }

                Console.ReadLine();
            }
        }

        // balansebis ganaxleba failshi

        private static void UpdateBalance(string userId, decimal balance)
        {
            string[] users = File.ReadAllLines(userDataFile);
            for (int i = 0; i < users.Length; i++)
            {
                string[] details = users[i].Split('|');
                if (details[0] == userId)
                {
                    users[i] = $"{details[0]}|{details[1]}|{details[2]}|{balance}";
                    break;
                }
            }

            File.WriteAllLines(userDataFile, users);
        }

        // transakciebis logi
        private static void LogTransaction(string userId, string user, string message)
        {
            string logMessage = $"{DateTime.Now:G} | User ID: {userId} - {user} | {message}";
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }
    }
}