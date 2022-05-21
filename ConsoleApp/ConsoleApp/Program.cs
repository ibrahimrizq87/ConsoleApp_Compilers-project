using System;
using System.IO;


namespace ConsoleApp
{
    class Program
    {


        static void Main(string[] args)
        {
            Help help = new Help();
            
            VirtualDisk v = new VirtualDisk();
            v.initialize();

            char[] fname = { 'H' };
            directory currentDir = new directory(fname, 0x10, 5, null,0);
            String path = fname.ToString();
            FatTable fat = new FatTable();
            currentDir.readDirectory();
            
           
            

            String command = "";
            String commandText;
            String name;
            String name2;
            Boolean hasName;
            Boolean hasName2;

            while (true)
            {
                name2 = "";
                   name = "";
                hasName = false;
                hasName2 = false;
      
                command = "";
                path = new String(currentDir.fileName);
                Console.Write(path + ">");
                commandText = Console.ReadLine();
                command.ToLower();
                string[] sep = { " " };

                string[] wo = commandText.Split(sep, System.StringSplitOptions.RemoveEmptyEntries);
                if (commandText != "")
                {


                    command = wo[0].Trim();
                    if (wo.Length > 1)
                    {
                        name = wo[1];
                        hasName = true;
                    }
                    if (wo.Length > 2)
                    {
                        name2 = wo[2];
                        hasName2 = true;
                       
                    }

                }



                if (commandText == "")
                    continue;
                else if (command == "exit" && !hasName) { 
                    Environment.Exit(0);

                    currentDir.writeDirectory();
                }

                else if (command == "md" && hasName && !hasName2)
                {
                    if (currentDir.searchDir(name) == -1)
                    {

                        char[] na = name.ToCharArray();
                        DirectoryEntery dir = new DirectoryEntery(na, 0x10, 0, 0);
                        currentDir.Directory_table.Add(dir);

                        currentDir.writeDirectory();
                    }
                    else {
                        Console.WriteLine("there is a directroy has the same name");
                    }
                }
                else if (command == "help" && !hasName )
                {
                    Console.WriteLine(help.help());
                }
                else if (command == "help" && hasName && !hasName2)
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
                else if (command == "rd" && hasName && !hasName2)
                {
                    if (currentDir.searchDir(name) != -1)
                    {
                        currentDir.deleteDirectory(currentDir.Directory_table[currentDir.searchDir(name)]);
                        currentDir.Directory_table.Remove(currentDir.Directory_table[currentDir.searchDir(name)]);
                    }
                    else
                    {
                        Console.WriteLine("The system cannot find the path specified.");
                    }
                }
                else if (command == "cd" && hasName && !hasName2)
                {
                    if (currentDir.searchDir(name) != -1)
                    {
                        DirectoryEntery d = currentDir.Directory_table[currentDir.searchDir(name)];
                        directory dir = new directory(d.fileName, d.fileAttr, d.firstCluster, currentDir, 0);
                        currentDir = dir;
                        currentDir.readDirectory();
                        //directory dir = new directory(d.fileName, d.fileAttr, d.firstCluster, currentDir, d.fileSize);
                        // currentDir = dir;
                    }
                    else if (name == "..") {
                        char[] fName = { 'H' };
                        directory currentDIR = new directory(fName, 0x10, 5, null, 0);
                        currentDir= currentDIR;
                        currentDir.readDirectory();
                    }
                    else
                    {
                        Console.WriteLine("The system cannot find the path specified.");
                    }


                }
                else if (command == "rename" && hasName && hasName2 )
                {
                    if (currentDir.searchDir(name) != -1)
                    {
                        DirectoryEntery d = currentDir.Directory_table[currentDir.searchDir(name)];
                        char[] n = name2.ToCharArray();
                        DirectoryEntery dNew= new DirectoryEntery(n, 0x10,d.firstCluster,0);

                        currentDir.UpdateContent(currentDir.searchDir(name),dNew);
                        //directory dir = new directory(name2.ToCharArray(), d.fileAttr, d.firstCluster, currentDir, d.fileSize);
                        // currentDir.UpdateContent(name, dir);
                    }
                    else
                    {
                        Console.WriteLine("The system cannot find the path specified.");
                    }
                }
                else if (command == "copy" && hasName && hasName2 )
                {
                    if (currentDir.searchFile(name) != -1 && currentDir.searchDir(name2) != -1)
                    {
                        currentDir.File_table[currentDir.searchFile(name2)].content1 = currentDir.File_table[currentDir.searchFile(name2)].content1;
                    }
                    else
                    {
                        Console.WriteLine("The system cannot find the path specified.");
                    }
                }
                else if (command == "del" && hasName && !hasName2)
                {

                    if (currentDir.searchFile(name) != -1)
                    {
                        currentDir.File_table.RemoveAt(currentDir.searchFile(name));
                        if (currentDir.searchDir(name) != -1) currentDir.Directory_table.RemoveAt(currentDir.searchDir(name));

                    }
                    else
                    {
                        Console.WriteLine("The system cannot find the path specified.");
                    }
                }
                else if (command == "type" && hasName && !hasName2)
                {

                    if (currentDir.searchFile(name) != -1)
                    {
                        Console.WriteLine(currentDir.File_table[currentDir.searchFile(name)].content1);
                    }
                    else
                    {
                        Console.WriteLine("The system cannot find the path specified.");
                    }
                }
                else if (command == "dir" && !hasName )
                {
                    int fileN = 0;
                    int dN = 0;
                    int size = 0;
                    Console.WriteLine("Directorys in"+path+":");

                    for (int i = 0;i<currentDir.Directory_table.Count;i++) {
                    
                        if (currentDir.Directory_table[i].fileAttr == 0x0)
                        {
                            

                            Console.WriteLine("      " + currentDir.Directory_table[i].fileSize + " "+ new String(currentDir.Directory_table[i].fileName));
                            fileN++;
                            size += currentDir.Directory_table[i].fileSize;
                        }
                        else {
                            Console.WriteLine("      <DIR>" + "       "+new String (currentDir.Directory_table[i].fileName));


                            dN++;
                        }


             

                    }
                    Console.WriteLine(fileN + " File(s)              " + size + "bytes");
                    Console.WriteLine(dN + " Dir(s)              " + fat.getFreeSpace() + "free bytes");


                }
                else if (command == "cls" && !hasName )
                {
                    Console.Clear();
                    
                }
                else if (command == "import" && hasName && hasName2)
                {

                    char[] na = { 'n' };
                    FileEntery F = new FileEntery(na, 0x0, 0, currentDir, 0, "");
                    
                    F.importFile(name, name2);
                    F.writeFile();
                    currentDir.File_table.Add(F);
                    DirectoryEntery entery = new DirectoryEntery(F.fileName, F.fileAttr, F.firstCluster, F.fileSize);
                    currentDir.Directory_table.Add(entery);
                }
                else { Console.Write("'" + commandText + "'" + "is not recognized as an internal or external command,\n operable program or batch file\n"); }


            }
        }





    }
}
