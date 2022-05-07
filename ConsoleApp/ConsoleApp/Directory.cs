using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    class directory : DirectoryEntery
    {
        public directory(char[] fileN, byte att, int firstC, directory parent,int filSize) : base(fileN, att, firstC, filSize)
        {
            if (parent != null)
            {
                this.parent = parent;
            }
        }

        public List<DirectoryEntery> Directory_table;
        directory parent;
        FatTable fat = new FatTable();
        VirtualDisk virtualDisk = new VirtualDisk();
        public int searchDir(String name)
        {
            readDirectory();

            for (int i = 0; i < Directory_table.Count; i++)
            {
                String str = new String(Directory_table[i].fileName);
                if (String.Equals(str, name))
                {
                    return i;
                }
            }
            return -1;
        }
        public void UpdateContent(DirectoryEntery dirEntery)
        {
            int index = searchDir(new String(dirEntery.fileName));
            readDirectory();
            if (index != -1)
            {
                Directory_table.RemoveAt(index);
                Directory_table.Insert(index, dirEntery);
            }
        }
        public void deleteDirectory()
        {
            if (firstCluster != 0)
            {
                int index = firstCluster;
                int next = fat.getNext(index);
                do
                {
                    fat.setNext(index, 0);
                    index = next;
                    if (index != -1) { next = fat.getNext(index); }

                } while (index != -1);
                if (parent != null)
                {
                    parent.readDirectory();
                    int row = parent.searchDir(new String(fileName));
                    if (row != 0)
                    {
                        parent.Directory_table.RemoveAt(row);
                        parent.writeDirectory();
                        fat.writeFat();
                    }
                }
            }
        }
        public void readDirectory()
        {
            //List <DirectoryEntery> Directory_talble2 = new List<DirectoryEntery>();
            List<byte> ls = new List<byte>();
            int fatIndex = 0;
            int next;
            Directory_table.Clear();
            do
            {
                if (firstCluster != 0)
                {
                    fatIndex = firstCluster;
                    next = fat.getNext(fatIndex);
                    ls.AddRange(virtualDisk.getBlock(fatIndex));
                    fatIndex = next;
                    if (fatIndex != -1)
                    {
                        next = fat.getNext(fatIndex);
                    }


                }


            } while (fatIndex != -1);

            for (int i = 0; i < ls.Count / 32; i++)
            {
                byte[] by = new byte[32];
                for (int j = 0; j < 32; j++)
                {
                    by[j] = ls[j + (i * 32)];
                }
                //Directory_talble2.Add(getDiroctryEntry(by));
                Directory_table.Add(getDiroctryEntry(by));
            }
        }


        public void writeDirectory()
        {  // we take the data from the list which repreasents the directory table and put them in a big array to write it in 
           //the file

            byte[] DirTablebytes = new byte[32 * Directory_table.Count];

            for (int i = 0; i < Directory_table.Count; i++)
            {
                byte[] DirEnterybytes = new byte[32];
                DirEnterybytes = Directory_table[i].getBytes();
                for (int j = 0; j < 32; j++)
                {
                    DirTablebytes[j + (i * 32)] = DirEnterybytes[j];
                }
            }
            // we use this part to find the avaliable blockes and write the data

            // int numberOfRequiredBlocks =Math.Ceiling(Convert.ToDecimal(DirTablebytes.Length / 1024));
            int numberOfRequiredBlocks;
            if (DirTablebytes.Length % 1024 == 0)
            {
                numberOfRequiredBlocks = DirTablebytes.Length / 1024;
            }
            else
            {
                numberOfRequiredBlocks = (DirTablebytes.Length / 1024) + 1;
            }

            int numberOfFullSizeBlocks = DirTablebytes.Length / 1024; // here we takes the floor of the number (it is the floor by the default as it is integer) 

            int reminder = DirTablebytes.Length % 1024;
            // taking the seel of the number to define the required blocks to save the data

            if (fat.getavilableBlocks() >= numberOfRequiredBlocks)
            {

                List<byte[]> blocks = new List<byte[]>();
                for (int i = 0; i < numberOfFullSizeBlocks; i++)
                {
                    blocks[i] = new byte[1024];
                    Buffer.BlockCopy(DirTablebytes, i * 1024, blocks[i], 0, 1024);

                }
                Byte[] reminderBlock = new byte[reminder];
                Buffer.BlockCopy(DirTablebytes, numberOfFullSizeBlocks * 1024, reminderBlock, 0, reminder);

                int fatIndex;
                int lastIndex = -1;

                // this statement used to defind if the directory is a root directory or not if true then we write the file in a spacific place
                if (firstCluster != 0)
                {
                    fatIndex = firstCluster;
                }
                else
                {

                    fatIndex = fat.getAvilableBlock();
                    firstCluster = fatIndex;
                }
                // write the directory table as a blocks on the file and manage the fat table data
                for (int i = 0; i < numberOfFullSizeBlocks; i++)
                {
                    virtualDisk.writeBlock(blocks[i], fatIndex);
                    fat.setNext(fatIndex, -1);
                    if (lastIndex != -1)
                    {
                        fat.setNext(lastIndex, fatIndex);
                    }
                    lastIndex = fatIndex;
                    fatIndex = fat.getAvilableBlock();
                    fat.writeFat();
                }
                if (reminderBlock.Length > 0)
                {
                    fatIndex = fat.getAvilableBlock();
                    virtualDisk.writeBlock(reminderBlock, fatIndex);
                    fat.setNext(fatIndex, -1);
                    fat.setNext(lastIndex, fatIndex);
                    fat.writeFat();
                }
            }
            else { Console.WriteLine("there is no enough space"); }

        }
    }
}
