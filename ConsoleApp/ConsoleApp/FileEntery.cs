using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApp
{
    class FileEntery : DirectoryEntery
    {
        directory parent;
      
        public String content1;
        DirectoryEntery Directory_table;

        FatTable fat = new FatTable();
        VirtualDisk virtualDisk = new VirtualDisk();

        public FileEntery(char[] fileN, byte att, int firstC, directory parent, int filSize, String content) : base(fileN, att, firstC, filSize)
        {
            this.content1 = content;
            this.parent = parent;

        }


        public List<byte> getBytes()
        {                // this methode returns the directory Entery as an array of Bytes
            byte[] bytes = new byte[32];
            List<byte> content = new List<byte>();
            byte[] name = new byte[11];
            for (int i = 0; i < Encoding.ASCII.GetBytes(fileName).Length; i++)
            {
                bytes[i] = Encoding.ASCII.GetBytes(fileName)[i];
            }
            bytes[11] = fileAttr;
            for (int i = 24; i < 28; i++)
            {
                bytes[i] = BitConverter.GetBytes(fileSize)[i - 24];
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    bytes[j + (j * i)] = BitConverter.GetBytes(fileEmpty[i])[j];
                }
            }
            for (int i = 28; i < 32; i++)
            {
                bytes[i] = BitConverter.GetBytes(firstCluster)[i - 28];
            }
            content.AddRange(bytes);


                content.AddRange( Encoding.ASCII.GetBytes(this.content1));
            


            return content;
        }

        public FileEntery getFileEntry(List<byte> arr)
        {
            byte[] fileBytes = new byte[32];
            byte[] content = new byte[arr.Count-32];
            for (int i = 32; i < arr.Count; i++)
            {
                content[i] = arr[i];
            }
            for (int i = 0; i < 32; i++)
            {
                fileBytes[i] = arr[i];
            }
            byte[] fileName = new byte[11];
            for (int i = 0; i < 11; i++)
            {

                fileBytes[i] = fileName[i];
            }


            int firstCluster = BitConverter.ToInt32(fileBytes, 10);
            byte att = fileBytes[11];

            FileEntery DirEntery = new FileEntery(Encoding.ASCII.GetChars(fileName), att, firstCluster, this.parent,
                content.Length, new String(Encoding.ASCII.GetChars(content)));

            //  this one need to be done
            // take the value from the array and pass it on the atteriputes of the class
            return DirEntery;



        }




        public void writeFile()
        {  // we take the data from the list which repreasents the directory table and put them in a big array to write it in 
           //the file

            List<byte> fileData = getBytes();
// we use this part to find the avaliable blockes and write the data

            // int numberOfRequiredBlocks =Math.Ceiling(Convert.ToDecimal(DirTablebytes.Length / 1024));
            int numberOfRequiredBlocks;
            if (fileData.Count % 1024 == 0)
            {
                numberOfRequiredBlocks = fileData.Count / 1024;
            }
            else
            {
                numberOfRequiredBlocks = (fileData.Count / 1024) + 1;
            }

            int numberOfFullSizeBlocks = fileData.Count / 1024; // here we takes the floor of the number (it is the floor by the default as it is integer) 

            int reminder = fileData.Count % 1024;
            // taking the seel of the number to define the required blocks to save the data
            int aval = FatTable.getavilableBlocks();
            if (aval >= numberOfRequiredBlocks)
            {

                List<byte[]> blocks = new List<byte[]>();
                for (int i = 0; i < numberOfFullSizeBlocks; i++)
                {
                    blocks[i] = new byte[1024];
                    for (int j = 0; j < 1024; j++)
                    {
                        blocks[i][j] = fileData[j + (i * 1024)];
                    }

                }
                byte[] reminderBlock = new byte[reminder];
                for (int j = 0; j < reminder; j++)
                {
                    reminderBlock[j] = fileData[j + (numberOfFullSizeBlocks * 1024)];
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
                    
                    lastIndex = fatIndex - 1;
                    virtualDisk.writeBlock(reminderBlock, fatIndex);
                    FatTable.setNext(fatIndex, -1);
                    FatTable.setNext(lastIndex, fatIndex);
                    FatTable.writeFat();
                }
            }
            else { Console.WriteLine("there is no enough space"); }

        }


        public void importFile(String path,String name)
        {
             

            
            this.content1 =File.ReadAllText(Path.GetFullPath(path));
            using (FileStream file = File.OpenRead(path.ToString()))
            {


                this.fileName= name.ToCharArray();
                this.fileSize = file.Name.ToCharArray().Length ;


            }
        }
        public void exportFile(String path)
        {

            using (FileStream file = File.Create(path))
            {
                
            }
            File.WriteAllText(path, content1);
        }
        public void readContent()
        {
            FatTable.fat_table = FatTable.getFat_table();
            //List <DirectoryEntery> Directory_talble2 = new List<DirectoryEntery>();
            List<byte> ls = new List<byte>();
            int fatIndex = 0;
            int next;
            
            this.content1 = "";
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
            byte[] cont = new byte[ls.Count];
            for (int i = 0; i < ls.Count ; i++)
            {
                
                  if (Convert.ToChar(ls[i]) == '#')
                    {
                        
                        break;
                    }
                    cont[i] = ls[i];
          
            }
            this.content1=new string(Encoding.ASCII.GetChars(cont));
        }







    }
}

