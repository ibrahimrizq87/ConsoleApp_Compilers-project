using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    class DirectoryEntery
    {
        protected char[] fileName = new char[11];
        protected byte fileAttr;
        protected int fileSize;
        protected int firstCluster;
        //fileEmpty
        public DirectoryEntery(char[] fileN, int firstC, byte att)
        {
            this.fileAttr = att;
            this.fileName = fileN;
            this.firstCluster = firstC;
        }
        public byte[] getBytes (){
            byte[] bytes = new byte[32];
            Buffer.BlockCopy(fileName, 0, bytes, 0, 10);
            bytes[11] = fileAttr;
            byte[] size = BitConverter.GetBytes(fileSize);
            byte[] f = BitConverter.GetBytes(firstCluster);
            // file empty ------------------
            Buffer.BlockCopy(size, 0, bytes, 24, 28);
            Buffer.BlockCopy(f, 0, bytes, 28, 32);

            return bytes;
        }
        public DirectoryEntery getDiroctryEntry(byte[] arr) {
         char[] fileName = new char[11];
            Buffer.BlockCopy(arr, 0, fileName, 0, 10);
            int i = BitConverter.ToInt32(arr, 10);
            byte att=arr[11];
       
        DirectoryEntery DI = new DirectoryEntery(fileName,i,att);
            //  this one need to be done
            // take the value from the array and past it on the atteriputes of the class
            return DI ;
        } 

    }
}
