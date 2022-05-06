using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    class DirectoryEntery
    {
        public char[] fileName = new char[11];     // size of this variable is 11 byte
        public byte fileAttr;                      // 0x0 means file or 0x10 means folder
        public int[] fileEmpty = new int[3];       // size is 12 byte  
        public int fileSize;
        public int firstCluster;
        
        public DirectoryEntery(char[] fileN, byte att, int firstC)
        {
            this.fileAttr = att;
            this.fileName = fileN;
            this.firstCluster = firstC;
        }
        public byte[] getBytes (){                // this methode returns the directory Entery as an array of Bytes
            byte[] bytes = new byte[32];
            Buffer.BlockCopy(Encoding.ASCII.GetBytes(fileName), 0, bytes, 0, 10);
            bytes[11] = fileAttr;
            Buffer.BlockCopy(fileEmpty, 0, bytes, 12, 23);
            byte[] fSize = BitConverter.GetBytes(fileSize);
            byte[] fstCluster = BitConverter.GetBytes(firstCluster);
            Buffer.BlockCopy(fSize, 0, bytes, 24, 27);
            Buffer.BlockCopy(fstCluster, 0, bytes, 28, 31);
            return bytes;
        }

        public DirectoryEntery getDiroctryEntry(byte[] arr) {
         char[] fileName = new char[11];
            Buffer.BlockCopy(arr, 0, fileName, 0, 10);
            int firstCluster = BitConverter.ToInt32(arr, 10);
            byte att=arr[11];
       
        DirectoryEntery DirEntery = new DirectoryEntery(fileName, att, firstCluster);

            //  this one need to be done
            // take the value from the array and past it on the atteriputes of the class
            return DirEntery;
        } 

    }
}
