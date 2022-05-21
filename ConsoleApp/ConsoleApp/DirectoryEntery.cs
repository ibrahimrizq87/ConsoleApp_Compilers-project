using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    class DirectoryEntery
    {
        public char[] fileName = new char[11];     // size of this variable is 11 byte
        public byte fileAttr;                      // 0x0 means file or 0x10 means folder
        public int[] fileEmpty = { 0,0,0};       // size is 12 byte  
        public int fileSize;
        public int firstCluster;

        public DirectoryEntery(char[] fileN, byte att, int firstC,int filSize)
        {
            int i = 0;

            if (fileN.Length > 11)
            {
                for (i = 0; i < 11; i++)
                {
                    this.fileName[i] = fileN[i];
                }
            }
            else {
                for (i = 0; i < fileN.Length; i++)
                {
                    this.fileName[i] = fileN[i];
                }
            }
            
            this.fileSize = filSize;
            this.fileAttr = att;
            
            this.firstCluster = firstC;
        }
        public byte[] getBytes()
        {                // this methode returns the directory Entery as an array of Bytes
            byte[] bytes = new byte[32];
            byte[] name = new byte[11];
            for (int i=0;i<11;i++) {
                bytes[i] = Encoding.ASCII.GetBytes(fileName)[i];
            }
            bytes[11] = fileAttr;
            for (int i = 24; i < 28; i++)
            {
                bytes[i] = BitConverter.GetBytes(fileSize)[i-24];
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++) {
                    bytes[j +(j*i)] = BitConverter.GetBytes(fileEmpty[i])[j];
                } 
            }
            for (int i = 28; i < 32; i++)
            {
                bytes[i] = BitConverter.GetBytes(firstCluster)[i-28];
            }

          
            return bytes;
        }

        public DirectoryEntery getDiroctryEntry(byte[] arr)
        {

           byte [] fileName = new byte[11];
            for (int i = 0; i < 11; i++)
            {
                
                arr[i] = fileName[i];
            }

            
            int firstCluster = BitConverter.ToInt32(arr, 10);
            byte att = arr[11];

            DirectoryEntery DirEntery = new DirectoryEntery(Encoding.ASCII.GetChars(fileName), att, firstCluster,0);

            //  this one need to be done
            // take the value from the array and pass it on the atteriputes of the class
            return DirEntery;
        
        
        
        }

    }
}
