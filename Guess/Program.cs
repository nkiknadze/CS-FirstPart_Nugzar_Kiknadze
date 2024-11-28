using System;
using System.IO;

namespace GuessNum
{
    class GuessNum
    {
        static void Main()
        {
            int Num1 = 1;
            int Num2 = 10;

            Random random = new Random();
            int targetNumber = random.Next(Num1, Num2 + 1);

            int attempts = 0;
            const int maxAttempts = 5;

            string logFilePath = "C:\\Users\\nkiknadze\\Desktop\\C# Course\\TestN1\\GuessNum.txt";

            // Check if the log file exists, if not create it
            if (!File.Exists(logFilePath))
            {
                using (StreamWriter sw = File.CreateText(logFilePath))
                {
                    sw.WriteLine("GuessNUMLog");
                    sw.WriteLine("=======================");
                }
            }

            Console.WriteLine($"start!");
            Console.WriteLine($"gamoicani ricxvi {Num1}- dan - {Num2} - mde. gaqvs {maxAttempts} cda gamosacnobad!");

            while (attempts < maxAttempts)
            {
                Console.Write("sheiyvane sheni ricxvi: ");
                string userInput = Console.ReadLine();

                if (int.TryParse(userInput, out int userGuess))
                {
                    
                    using (StreamWriter sw = File.AppendText(logFilePath))
                    {
                        sw.WriteLine($"motamashe: {userGuess}, cda: {attempts + 1}, Date: {DateTime.Now}");
                    }

                    
                    if (userGuess < Num1 || userGuess > Num2)
                    {
                        Console.WriteLine($"sheni ricxvi {Num1} dan {Num2}-shi aris!");
                        
                        using (StreamWriter sw = File.AppendText(logFilePath))
                        {
                            sw.WriteLine($"araswori reinji: {userGuess}. Date: {DateTime.Now}");
                        }
                        continue;
                    }

                    attempts++;

                    if (userGuess == targetNumber)
                    {
                        Console.WriteLine($"sworia: dashifruli ricxvi iyo {targetNumber}.");
                        
                        using (StreamWriter sw = File.AppendText(logFilePath))
                        {
                            sw.WriteLine($"sworia: {userGuess} cda -  {attempts}. Date: {DateTime.Now}");
                        }
                        break;
                    }
                    else if (userGuess < targetNumber)
                    {
                        Console.WriteLine("arasworia, MAGLA!");
                    }
                    else
                    {
                        Console.WriteLine("arasworia, DABLA!");
                    }

                    Console.WriteLine($"dagrcha {maxAttempts - attempts} - cda");
                }
                else
                {
                    Console.WriteLine("araswori mnishvneloba. sheiwyvane ricxvi!!!!");
                    
                    using (StreamWriter sw = File.AppendText(logFilePath))
                    {
                        sw.WriteLine($"araswori mnishvneloba: {userInput}. Date: {DateTime.Now}");
                    }
                }

                if (attempts == maxAttempts && userInput != targetNumber.ToString())
                {
                    Console.WriteLine($"ver gamoicani, daxarje {maxAttempts} - ve cda. swori ricxvi iyo {targetNumber}.");
                    // Log the game over
                    using (StreamWriter sw = File.AppendText(logFilePath))
                    {
                        sw.WriteLine($"Game Over. Date: {DateTime.Now}");
                    }
                }
                if (attempts != maxAttempts && userInput == targetNumber.ToString())
                {
                    Console.WriteLine("CONGRATS (O_o)");
                }
                else
                {
                    Console.WriteLine("GAME OVER :(");
                }
            }   
        }
    }
}
