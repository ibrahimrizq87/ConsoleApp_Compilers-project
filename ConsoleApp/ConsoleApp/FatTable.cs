using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace ConsoleApp
{
    class FatTable
    {
        string path = @"F:\college\third_year\second semester\compilers\project\disk.txt";

        int[] fat_table;
       public FatTable() {
            fat_table = new int[1024];

        }
        public void initializeFat() {
            for (int i = 0; i < 1024; i++) {
                if (i < 5)
                {
                    fat_table[i] = -1;
                }
                else {     
             fat_table[i] = 0;
            }
            }

        }

        public void writeFat() {
            using (FileStream file = File.OpenWrite(path))
            {

                file.Seek(1024, SeekOrigin.Begin);
                byte[] newFat = new byte[1024 * 4];
                Buffer.BlockCopy(fat_table, 0, newFat, 0, newFat.Length);
                file.Write(newFat, 0, newFat.Length);

            }
        }
        public int[] getFat_table() {
            int[] bytesAsInts = new int[1024];
            using (FileStream file = File.OpenRead(path))
            {

                
                 file.Seek(1024, SeekOrigin.Begin);

                byte[] newFat = new byte[1024 * 4];
                file.Read(newFat, 0, 1024 * 4);
                Buffer.BlockCopy(newFat, 0, bytesAsInts, 0, bytesAsInts.Length);
            }

            return bytesAsInts;
        }
        public void displayFat_table()
        {
            int[] bytesAsInts = new int[1024];
            using (FileStream file = File.OpenRead(path))
            {


                file.Seek(1024, SeekOrigin.Begin);

                byte[] newFat = new byte[1024 * 4];
              
                file.Read(newFat, 0, 1024 * 4);
                Buffer.BlockCopy(newFat, 0, bytesAsInts, 0, bytesAsInts.Length);
                for (int i =0;i< 1024;i++) {
                    Console.WriteLine(i + "| "+bytesAsInts[i]);
                }
            }

            
        }

        public int getNext(int index) {

            return fat_table[index];
        }
        public void setNext(int index, int value)
        {

             fat_table[index] = value;
        }
        public int getAvilableBlock(){
            for (int i = 0; i < 1024; i++) {
                if (fat_table[i] == 0)
                    return i;
            }
            return -1;
        }


    }

    }

