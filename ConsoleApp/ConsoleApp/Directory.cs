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

    }
}
