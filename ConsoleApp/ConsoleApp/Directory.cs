using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    class directory :DirectoryEntery
    {
public directory(char[] fileN, int firstC, byte att) :base(fileN, firstC, att) {
          
        }
        
        List<DirectoryEntery> Directory_table;
        FatTable fat = new FatTable();
        VirtualDisk VD = new VirtualDisk();
        public void readDirectory() {
            List<DirectoryEntery> DTs;
            List<byte> ls;
            int fatIndex=0;
            int next;
            do
            {
                if (firstCluster != 0)
                {
                    fatIndex = firstCluster;
                    next = fat.getNext(fatIndex);
                    ls.AddRange(VD.getBlock(fatIndex));
                    fatIndex = next;
                    if (fatIndex != -1) { next = fat.getNext(fatIndex); }


                }
                

            } while (fatIndex != -1);
            for (int i = 0; i < ls.Count; i++)
            {
                byte[] by = new byte[32];
                for (int j=0;j<32;j++) {
                    by[j] = ls[i];
                }
                DTs[i]= getDiroctryEntry(by);

            }
        }
        public void writeDirectory() {
       
            byte[] DTbytes = new byte[32* Directory_table.Count];
            for (int i=0;i< Directory_table.Count;i++) {
                byte[] Dibytes = new byte[32];
                Dibytes = Directory_table[i].getBytes();
                for (int j = 0; j < 32; j++) {
                    DTbytes[j+(i*32)]= Dibytes[j];
                }
            }
            int numberOfRequiredBlocks;
            int numberOfFullSizeBlocks = DTbytes.Length / 1024;
            int reminder = DTbytes.Length % 1024;
            if (DTbytes.Length % 1024 == 0)
            {
                 numberOfRequiredBlocks = DTbytes.Length / 1024;
            }
            else {
                numberOfRequiredBlocks = (DTbytes.Length / 1024)+1;

            }
            if (fat.getavilableBlocks() <= numberOfRequiredBlocks)
            {
                List<byte[]> block;
                int fatIndex;
                int lastIndex = -1;
                if (firstCluster != 0)
                {
                    fatIndex = firstCluster;
                }
                else {

                    fatIndex=fat.getAvilableBlock();
                    firstCluster = fatIndex;
                }
                for (int i = 0; i < numberOfFullSizeBlocks; i++) {
                    VD.writeBlock(block[i],fatIndex);
                    fat.setNext(fatIndex,-1);
                    fat.setNext(lastIndex, fatIndex);
                    lastIndex = fatIndex;
                    fatIndex = fat.getAvilableBlock();
                    // write the reminder
                    fat.writeFat();
                }

            }
            else { Console.WriteLine("there is no enogh space"); }

        }
    }
}
