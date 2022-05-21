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

        public List<DirectoryEntery> Directory_table = new List<DirectoryEntery>();
        public List<FileEntery> File_table = new List<FileEntery>();
        directory parent;
        
        VirtualDisk virtualDisk = new VirtualDisk();
        public int searchDir(String name)
        {
            //readDirectory();
            char[] na = new char[11];
            for (int i = 0;i<name.Length;i++) {
                na[i] = name[i];
            }

            for (int i = 0; i < this.Directory_table.Count; i++)
            {
                String str = new String(this.Directory_table[i].fileName);
                
                
                if (String.Equals(str, new string(na)))
                {
                    return i;
                }
            }
            return -1;
        }
        public int searchFile(String name)
        {
            //readDirectory();
            
            char[] na = new char[11];
            char[] na2 = new char[11];
            for (int i = 0; i < name.Length; i++)
            {
                na[i] = name[i];
            }
            

            for (int i = 0; i < this.File_table.Count; i++)
            {
                String str = new String(this.File_table[i].fileName);
                for (int x = 0; x < str.Length; x++)
                {
                    na2[x] = str[x];
                }
              

                if (String.Equals(new string(na2), new string(na)))
                {
                    return i;
                }
            }
            return -1;
        }
        public void UpdateContent(int index, DirectoryEntery dirEntery)
        {
            
            //readDirectory();
            
                Directory_table.RemoveAt(index);
                Directory_table.Insert(index, dirEntery);
           
        }
        public void deleteDirectory(DirectoryEntery d)
        {
            if (FatTable.getNext(searchDir(new String(d.fileName)) + 5) == -1)
            {
                FatTable.setNext(searchDir(new String(d.fileName)) + 4, -1);
            }
            
            FatTable.setNext(searchDir(new String(d.fileName))+5, 0);
            FatTable.writeFat();
            /*int index, next;
            if (d.firstCluster != 0)
            {
                index = d.firstCluster;
                next = FatTable.getNext(index);


                do
                {
                    
                    index = next;
                    if (index != -1)
                    {
                        next = FatTable.getNext(index);
                    }
                } while (index != -1);
            }
                FatTable.writeFat();
            */
        }



        
        public void readDirectory()
        {
            FatTable.fat_table=FatTable.getFat_table();
            //List <DirectoryEntery> Directory_talble2 = new List<DirectoryEntery>();
            List<byte> ls = new List<byte>();
            int fatIndex = 0;
            int next;
            if (Directory_table != null) Directory_table.Clear();

            if (this.firstCluster != 0 && FatTable.getNext(this.firstCluster) != 0)
            {
                fatIndex = this.firstCluster;
            do
            {
               
                
                    
                    
                    if (fatIndex != -1)
                    {
                        next = FatTable.getNext(fatIndex);
                    }
                    next = FatTable.getNext(fatIndex);
                    ls.AddRange(virtualDisk.getBlock(fatIndex));
                    fatIndex = next;
                    


                


            } while (fatIndex != -1);

            }
            else
            {
                Console.WriteLine("there is no folders in here");
                
            }
            bool n = false;
            for (int i = 0; i < ls.Count / 32; i++)
            {
                byte[] by = new byte[32];
                if (n) break;
                for (int j = 0; j < 32; j++)
                {
                    if (Convert.ToChar(ls[j + (i * 32)]) == '#') {
                        n = true;
                        break;
                    }
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
            int aval = FatTable.getavilableBlocks();
            if (aval >= numberOfRequiredBlocks)
            {

                List<byte[]> blocks = new List<byte[]>();
                for (int i = 0; i < numberOfFullSizeBlocks; i++)
                {
                    blocks[i] = new byte[1024];
                    for (int j =0; j<1024; j ++) {
                        blocks[i][j] = DirTablebytes[j+(i*1024)];
                    }
                    
                }
                byte[] reminderBlock = new byte[reminder];
                for (int j = 0; j < reminder; j++)
                {
                    reminderBlock[j] = DirTablebytes[j + (numberOfFullSizeBlocks  * 1024)];
                }
                
                
                int fatIndex;
                int lastIndex = -1;

                // this statement used to defind if the directory is a root directory or not if true then we write the file in a spacific place
                if (firstCluster != 0)
                {
                    fatIndex = firstCluster;
                }
                else
                {

                    fatIndex = FatTable.getAvilableBlock();
                    firstCluster = fatIndex;
                }
                // write the directory table as a blocks on the file and manage the fat table data
                for (int i = 0; i < numberOfFullSizeBlocks; i++)
                {
                    virtualDisk.writeBlock(blocks[i], fatIndex);
                    FatTable.setNext(fatIndex, -1);
                    if (lastIndex != -1)
                    {
                        FatTable.setNext(lastIndex, fatIndex);
                    }
                    lastIndex = fatIndex;
                    fatIndex = FatTable.getAvilableBlock();
                    FatTable.writeFat();
                }
                if (reminderBlock.Length > 0)
                {
                    fatIndex = FatTable.getAvilableBlock();
                    if (numberOfFullSizeBlocks == 0 && this.firstCluster == 0) { this.firstCluster = fatIndex; } 
                    
                    lastIndex = fatIndex-1;
                    virtualDisk.writeBlock(reminderBlock, fatIndex);
                    FatTable.setNext(fatIndex, -1);
                    FatTable.setNext(lastIndex, fatIndex);
                    FatTable.writeFat();
                }
            }
            else { Console.WriteLine("there is no enough space"); }

        }





        //------------------------------------------------------------------------------------




    
    }
}
