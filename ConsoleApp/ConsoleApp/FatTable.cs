using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace ConsoleApp
{
    class FatTable
    {
        static string path = @"F:\college\third_year\second semester\compilers\project\disk.txt";

       public static int[] fat_table = new int[1024];
       public FatTable() {
            

        }
        public static void initializeFat() {
            for (int i = 0; i < 1024; i++) {
                if (i < 4)
                {
                    fat_table[i] = i + 1;

                }
                else if (i == 4)
                {
                    fat_table[i] = -1;

                }
                else {
                    fat_table[i] = 0;
                }
            }

        }

        public static void writeFat() {
            using (FileStream file = File.OpenWrite(path))
            {

                file.Seek(1024, SeekOrigin.Begin);
                byte[] newFat = new byte[1024 * 4];
                Buffer.BlockCopy(fat_table, 0, newFat, 0, newFat.Length);
                file.Write(newFat, 0, newFat.Length);

            }
        }
        public static int[] getFat_table() {
            int[] bytesAsInts = new int[1024];
            using (FileStream file = File.OpenRead(path))
            {

                
                 file.Seek(1024, SeekOrigin.Begin);

                byte[] newFat = new byte[1024 * 4];
                file.Read(newFat, 0, 1024 * 4);
                Buffer.BlockCopy(newFat, 0, bytesAsInts, 0, bytesAsInts.Length);
            
            }
            fat_table = bytesAsInts;
            return bytesAsInts;
        }
        public static void displayFat_table()
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

        public static int getNext(int index) {

            return fat_table[index];
        }
        public static void setNext(int index, int value)
        {

             fat_table[index] = value;
        }
        public static int getAvilableBlock(){
            for (int i = 0; i < 1024; i++) {
                if (fat_table[i] == 0)
                    return i;
            }
            return -1;
        }
        public int getFreeSpace() {
            return getavilableBlocks()*1024;
        }

        public static int getavilableBlocks() {
            int count = 0;
            for (int i = 0; i < 1024; i++) {
                if (fat_table[i] == 0) {
                    count = count + 1;

                }
            }
            return count;
        }
    
    }


    }

