using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ConsoleApp

{
                

class VirtualDisk
    {
        string path = @"F:\college\third_year\second semester\compilers\project\disk.txt";

      

   public void initialize()
        {

            char[] fat= new char[1024*4];
            for (int i = 0; i < 1024 * 4; i++)
            {
                fat[i] = '*';
            }
            byte[] superBlock = new byte[1024];

            for (int i = 0; i < 1024; i++)
            {
                superBlock[i] = 0;
            }
            
            char[] data = new char[1024 * 1019];
            for (int i = 0; i < 1024 * 1019; i++)
            {
                data[i] = '#';
            }
            byte[] Fat = Encoding.UTF8.GetBytes(fat);
            byte[] Data = Encoding.UTF8.GetBytes(data);

            using (FileStream file = File.Create(path)) {

                
                file.Write(superBlock, 0, superBlock.Length);
                file.Write(Fat,0,Fat.Length);
                
                file.Write(Data, 0, Data.Length);
            }



        }
    }

}
