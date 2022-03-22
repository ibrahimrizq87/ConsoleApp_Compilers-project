using System;
using System.IO;


namespace ConsoleApp
{
    class Program
    {


        static void Main(string[] args)
        {
            
            
            string path = Directory.GetCurrentDirectory();
            String command="";
            String commandText;
            String arg ="";
            int i = 0;
            while (true)
            {
                arg = "";
                command = "";
                Console.Write(path + ">");
                commandText = Console.ReadLine();
                
                string[] separatingStrings = { " -"};
                 
                string[] words = commandText.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
                
                if (commandText != "")
                    command = words[0].ToLower();
                

                /* foreach (char ch in commandText) {
                    if (ch == '-')
                        arg += commandText[i + 1];
                    else if (ch != ' ')
                        command += ch;
                    i++;
                }*/


                if (commandText == "")
                    continue;
                else if (command == "exit")
                    Environment.Exit(0);
                else if (command == "cls") { 
                    Console.Clear();
                    Console.WriteLine(arg);
                }
                else if (command == "help")
                    Console.WriteLine(File.ReadAllText(@"F:\college\third_year\second semester\compilers\project\help.txt"));
                else Console.Write("'" + commandText + "'" + "is not recognized as an internal or external command,\n operable program or batch file\n");


            }
        }
    }
}
