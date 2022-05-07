using System;
using System.IO;


namespace ConsoleApp
{
    class Program
    {


        static void Main(string[] args)
        {
            Help help = new Help();
            String path = "";
            VirtualDisk v = new VirtualDisk();
            v.initialize();

            char[] fname = { 'H' };
            directory currentDir = new directory(fname, 0x10, 5, null,0);

            currentDir.readDirectory();

            /*
                       VirtualDisk v = new VirtualDisk();
                       v.initialize();

                       char[] name = { 'H' };
                       directory currentDir = new directory(name, 0x10, 5, null);

                       currentDir.readDirectory();


                       FatTable f = new FatTable();



                       f.initializeFat();
                       f.writeFat();
                       f.displayFat_table();

                        */



            String command = "";
            String commandText;
            String attr = "";
            String name;
            String name2;
            Boolean hasAttr;
            Boolean hasName;
            Boolean hasName2;

            while (true)
            {
                name2 = "";
                   name = "";
                hasName = false;
                hasName2 = false;
                hasAttr = false;
                attr = "";
                command = "";
                path = (currentDir.fileName).ToString();
                Console.Write(path + ">");
                commandText = Console.ReadLine();

                string[] separatingStrings = { "-" };
                string[] words = commandText.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);

                if (commandText != "")
                {
                    command = words[0].ToLower().Trim();
                    if (words.Length > 1)
                    {
                        attr = words[1];
                        hasAttr = true;
                    }
                    command.ToLower();
                    string[] sep = { " " };

                    string[] wo = commandText.Split(sep, System.StringSplitOptions.RemoveEmptyEntries);
                    command = wo[0];
                    if (wo.Length > 0)
                    {
                        name = wo[1];
                        hasName = true;
                    }
                    if (wo.Length > 1)
                    {
                        name2 = wo[2];
                        hasName2 = true;
                    }

                }



                /* foreach (char ch in commandText) {
                    if (ch == '-')
                        arg += commandText[i + 1];
                    else if (ch != ' ')
                        command += ch;
                    i++;
                }*/


                if (commandText == "")
                    continue;
                else if (command == "exit" && !hasName && !hasAttr)
                    Environment.Exit(0);

                else if (command == "md" && hasName)
                {
                    char[] na = name.ToCharArray();


                    directory dir = new directory(na, 0x10, 0, currentDir, 0);
                    currentDir.Directory_table.Add(dir);
                    currentDir.writeDirectory();

                }
                else if (command == "help" && !hasName)
                {
                    Console.WriteLine(help.help());
                }
                else if (command == "help" && hasName)
                {
                    if (name == "md")
                    {
                        Console.WriteLine(help.Md());

                    }
                    else if (name == "cd")
                    {
                        Console.WriteLine(help.Cd());

                    }
                    else if (name == "dir")
                    {
                        Console.WriteLine(help.Dir());

                    }
                    else if (name == "cls")
                    {
                        Console.WriteLine(help.Cls());

                    }
                    else if (name == "rd")
                    {
                        Console.WriteLine(help.Rd());

                    }
                    else if (name == "md")
                    {
                        Console.WriteLine(help.Md());

                    }
                    else if (name == "exit")
                    {
                        Console.WriteLine(help.exit());

                    }
                    else if(name == "copy")
                    {
                        Console.WriteLine(help.copy());

                    }
                    else if (name == "rename")
                    {
                        Console.WriteLine(help.rename());

                    }
                    else if (name == "del")
                    {
                        Console.WriteLine(help.del());

                    }

                }
                else if (command == "rd" && hasName)
                {
                    if (currentDir.searchDir(name) != -1)
                    {
                        DirectoryEntery d = currentDir.Directory_table[currentDir.searchDir(name)];

                        directory dir = new directory(d.fileName, d.fileAttr, d.firstCluster, currentDir, d.fileSize);
                        currentDir.deleteDirectory(dir);
                    }
                    else
                    {
                        Console.WriteLine("The system cannot find the path specified.");
                    }
                }
                else if (command == "cd" && hasName)
                {
                    if (currentDir.searchDir(name) != -1)
                    {
                        DirectoryEntery d = currentDir.Directory_table[currentDir.searchDir(name)];

                        directory dir = new directory(d.fileName, d.fileAttr, d.firstCluster, currentDir, d.fileSize);
                        currentDir = dir;
                    }
                    else
                    {
                        Console.WriteLine("The system cannot find the path specified.");
                    }


                }
                else if (command == "rename" && hasName && hasName2)
                {
                    if (currentDir.searchDir(name) != -1)
                    {
                        DirectoryEntery d = currentDir.Directory_table[currentDir.searchDir(name)];
                        directory dir = new directory(name2.ToCharArray(), d.fileAttr, d.firstCluster, currentDir, d.fileSize);
                        currentDir.UpdateContent(name, dir);
                    }
                    else
                    {
                        Console.WriteLine("The system cannot find the path specified.");
                    }
                }
                else if (command == "copy" && hasName && hasName2)
                {
                    if (currentDir.searchDir(name) != -1 && currentDir.searchDir(name2) != -1)
                    {
                        DirectoryEntery d = currentDir.Directory_table[currentDir.searchDir(name)];
                        DirectoryEntery d2 = currentDir.Directory_table[currentDir.searchDir(name)];
                        FileEntery fil = new FileEntery(name.ToCharArray(), d.fileAttr, d.firstCluster, currentDir, d.fileSize);
                        FileEntery fil2 = new FileEntery(name2.ToCharArray(), d.fileAttr, d.firstCluster, currentDir, d.fileSize);
                        fil2.content = fil.content;

                    }
                    else
                    {
                        Console.WriteLine("The system cannot find the path specified.");
                    }
                }
                else if (command == "del" && hasName)
                {

                    if (currentDir.searchDir(name) != -1)
                    {
                        DirectoryEntery d = currentDir.Directory_table[currentDir.searchDir(name)];

                        FileEntery fil = new FileEntery(d.fileName, d.fileAttr, d.firstCluster, currentDir, d.fileSize);
                        fil.deleteFile();
                    }
                    else
                    {
                        Console.WriteLine("The system cannot find the path specified.");
                    }
                }
                else if (command == "types" && hasName)
                {

                    if (currentDir.searchDir(name) != -1)
                    {
                        DirectoryEntery d = currentDir.Directory_table[currentDir.searchDir(name)];

                        FileEntery fil = new FileEntery(d.fileName, d.fileAttr, d.firstCluster, currentDir, d.fileSize);
                        Console.WriteLine(fil.content);
                    }
                    else
                    {
                        Console.WriteLine("The system cannot find the path specified.");
                    }
                }
                else if (command == "import")
                {
                    //-------------------------
                }
                else if (command == "export")
                {
                    //-------------------------
                }
                else if (command == "cls" && !hasName && !hasAttr)
                {
                    Console.Clear();
                    Console.WriteLine(attr);
                }
                else if (command == "help" && !hasName)
                { Console.WriteLine(File.ReadAllText(@"F:\college\third_year\second semester\compilers\project\help.txt")); }

                else { Console.Write("'" + commandText + "'" + "is not recognized as an internal or external command,\n operable program or batch file\n"); }


            }
        }





    }
}
