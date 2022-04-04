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
    }
}
