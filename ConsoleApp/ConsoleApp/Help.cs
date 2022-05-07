using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApp
{
    class Help
    {
        public String Dir() {
            return @"Displays a list of files and subdirectories in a directory.

DIR[drive:][path][filename][/ A[[:]attributes]] [/ B] [/ C] [/ D] [/ L] [/ N]
  [/ O[[:]sortorder]] [/ P] [/ Q] [/ R] [/ S] [/ T[[:]timefield]] [/ W] [/ X] [/ 4]

  [drive:][path][filename]
            Specifies drive, directory, and/ or files to list.


 / A          Displays files with specified attributes.
  attributes D  Directories R  Read - only files
H  Hidden files               A Files ready for archiving
S  System files               I  Not content indexed files

L  Reparse Points             O  Offline files
- Prefix meaning not
/ B          Uses bare format(no heading information or summary).

/ C          Display the thousand separator in file sizes.This is the

default.Use / -C to disable display of separator.

/ D          Same as wide but files are list sorted by column.

/ L          Uses lowercase.

/ N          New long list format where filenames are on the far right.

/ O          List by files in sorted order.

sortorder    N  By name(alphabetic)       S  By size(smallest first)

E  By extension(alphabetic)  D  By date / time(oldest first)

G  Group directories first - Prefix to reverse order
/ P          Pauses after each screenful of information.

/ Q          Display the owner of the file.

/ R          Display alternate data streams of the file.

/ S          Displays files in specified directory and all subdirectories.

/ T          Controls which time field displayed or used for sorting
timefield   C  Creation

A  Last Access

W  Last Written
/ W          Uses wide list format.

/ X          This displays the short names generated for non - 8dot3 file

names.The format is that of / N with the short name inserted

before the long name.If no short name is present, blanks are

displayed in its place.

/ 4          Displays four - digit years


Switches may be preset in the DIRCMD environment variable.Override
preset switches by prefixing any switch with - (hyphen)--for example, / -W.";

        }
        public String Cls()
        {
            return @"Clears the screen.

CLS
";
        }
        public String Md()
        {
            return @"Creates a directory.

MKDIR [drive:]path
MD [drive:]path

If Command Extensions are enabled MKDIR changes as follows:

MKDIR creates any intermediate directories in the path, if needed.
For example, assume \a does not exist then:

    mkdir \a\b\c\d

is the same as:

    mkdir \a
    chdir \a
    mkdir b
    chdir b
    mkdir c
    chdir c
    mkdir d

which is what you would have to type if extensions were disabled.
";
        }
        public String Rd()
        {
            return @"Removes (deletes) a directory.

RMDIR [/S] [/Q] [drive:]path
RD [/S] [/Q] [drive:]path

    /S      Removes all directories and files in the specified directory
            in addition to the directory itself.  Used to remove a directory
            tree.

    /Q      Quiet mode, do not ask if ok to remove a directory tree with /S
";
        }
        public String Cd()
        {
            return @"Displays the name of or changes the current directory.

CHDIR [/D] [drive:][path]
CHDIR [..]
CD [/D] [drive:][path]
CD [..]

  ..   Specifies that you want to change to the parent directory.

Type CD drive: to display the current directory in the specified drive.
Type CD without parameters to display the current drive and directory.

Use the /D switch to change current drive in addition to changing current
directory for a drive.

If Command Extensions are enabled CHDIR changes as follows:

The current directory string is converted to use the same case as
the on disk names.  So CD C:\TEMP would actually set the current
directory to C:\Temp if that is the case on disk.

CHDIR command does not treat spaces as delimiters, so it is possible to
CD into a subdirectory name that contains a space without surrounding
the name with quotes.  For example:

    cd \winnt\profiles\username\programs\start menu

is the same as:

    cd ""\winnt\profiles\username\programs\start menu""

which is what you would have to type if extensions were disabled.
";
        }
       public String help() {
            return File.ReadAllText(@"F:\college\third_year\second semester\compilers\project\help.txt");
        }
    }

}
