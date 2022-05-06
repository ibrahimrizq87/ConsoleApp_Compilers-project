using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    class FileEntery : DirectoryEntery 
    {
        directory parent;
        String content;
        DirectoryEntery Directory_table ;
      
        FatTable fat = new FatTable();
        VirtualDisk virtualDisk = new VirtualDisk();

        public FileEntery(char[] fileN, byte att, int firstC, directory parent, String content) : base(fileN, att, firstC) {
            this.parent = parent;
            this.content = content;
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

        //-----------------------------------------------------------------------------------------------------







        public int searchDir(String name)
        {
            readDirectory();

            for (int i = 0; i < parent.Directory_table.Count; i++)
            {
                String str = new String(parent.Directory_table[i].fileName);
                if (String.Equals(str, name) && fileAttr == 0x0)
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
                parent.Directory_table.RemoveAt(index);
                parent.Directory_table.Insert(index, dirEntery);
            }
        }

        //-----------------------------------------------------------------------------------------------








        public void readDirectory()
        {
            //List <DirectoryEntery> Directory_talble2 = new List<DirectoryEntery>();
            List<byte> ls = new List<byte>();
            int fatIndex = 0;
            int next;
           
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
            byte[] by = new byte[32];

            for (int i = 0; i <   32; i++)
            {
                by[i] = ls[i];

            }
            Directory_table = getDiroctryEntry(by);
            byte[] contentbyte = new byte[(ls.Count)-32];

            for (int i = 32; i < ls.Count; i++)
            {
                contentbyte[i] = ls[i];

            }
            content= Convert.ToBase64String(contentbyte);

        }


        /*
                byte[] by = new byte[32];
                for (int j = 0; j < 32; j++)
                {
                    by[j] = ls[j + (i * 32)];
                }
                //Directory_talble2.Add(getDiroctryEntry(by));
                Directory_table.Add(getDiroctryEntry(by));
            */



        //-----------------------------------------------------------------------------------------------






        public void writeDirectory()
        {  // we take the data from the list which repreasents the directory table and put them in a big array to write it in 
           //the file

            byte[] FileEnterybytes = new byte[32];

            FileEnterybytes = Directory_table.getBytes();
                
            
            // we use
            // this part to find the avaliable blockes and write the data

            // int numberOfRequiredBlocks =Math.Ceiling(Convert.ToDecimal(DirTablebytes.Length / 1024));
            int numberOfRequiredBlocks;
            byte[] contentBytes =Encoding.ASCII.GetBytes(content);
            if (contentBytes.Length % 1024 == 0)
            {
                numberOfRequiredBlocks = contentBytes.Length / 1024;
            }
            else
            {
                numberOfRequiredBlocks = (contentBytes.Length / 1024) + 1;
            }

            int numberOfFullSizeBlocks = contentBytes.Length / 1024; // here we takes the floor of the number (it is the floor by the default as it is integer) 

            int reminder = contentBytes.Length % 1024;
            // taking the seel of the number to define the required blocks to save the data

            if (fat.getavilableBlocks() >= numberOfRequiredBlocks)
            {

                List<byte[]> blocks = new List<byte[]>();
                for (int i = 0; i < numberOfFullSizeBlocks; i++)
                {
                    blocks[i] = new byte[1024];
                    Buffer.BlockCopy(contentBytes, i * 1024, blocks[i], 0, 1024);

                }
                Byte[] reminderBlock = new byte[reminder];
                Buffer.BlockCopy(contentBytes, numberOfFullSizeBlocks * 1024, reminderBlock, 0, reminder);

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
                virtualDisk.writeBlock(FileEnterybytes, fatIndex);
                fat.setNext(fatIndex, -1);
                lastIndex = fatIndex;
                fatIndex = fat.getAvilableBlock();
                fat.writeFat();
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

