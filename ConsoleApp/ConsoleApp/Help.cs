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
        public String exit()
        {
            return (@"Quits the CMD.EXE program (command interpreter) or the current batch
script.

EXIT [/B] [exitCode]

  /B          specifies to exit the current batch script instead of
              CMD.EXE.  If executed from outside a batch script, it
              will quit CMD.EXE

  exitCode    specifies a numeric number.  if /B is specified, sets
              ERRORLEVEL that number.  If quitting CMD.EXE, sets the process
              exit code with that number.
");
        }
        public String copy()
        {
            return @"Copies one or more files to another location.

COPY [/D] [/V] [/N] [/Y | /-Y] [/Z] [/L] [/A | /B ] source [/A | /B]
     [+ source [/A | /B] [+ ...]] [destination [/A | /B]]

  source       Specifies the file or files to be copied.
  /A           Indicates an ASCII text file.
  /B           Indicates a binary file.
  /D           Allow the destination file to be created decrypted
  destination  Specifies the directory and/or filename for the new file(s).
  /V           Verifies that new files are written correctly.
  /N           Uses short filename, if available, when copying a file with a
               non-8dot3 name.
  /Y           Suppresses prompting to confirm you want to overwrite an
               existing destination file.
  /-Y          Causes prompting to confirm you want to overwrite an
               existing destination file.
  /Z           Copies networked files in restartable mode.
  /L           If the source is a symbolic link, copy the link to the target
               instead of the actual file the source link points to.

The switch /Y may be preset in the COPYCMD environment variable.
This may be overridden with /-Y on the command line.  Default is
to prompt on overwrites unless COPY command is being executed from
within a batch script.

To append files, specify a single file for destination, but multiple files
for source (using wildcards or file1+file2+file3 format).
";
        }
        public String del()
        {
            return @"Deletes one or more files.

DEL [/P] [/F] [/S] [/Q] [/A[[:]attributes]] names
ERASE [/P] [/F] [/S] [/Q] [/A[[:]attributes]] names

  names         Specifies a list of one or more files or directories.
                Wildcards may be used to delete multiple files. If a
                directory is specified, all files within the directory
                will be deleted.

  /P            Prompts for confirmation before deleting each file.
  /F            Force deleting of read-only files.
  /S            Delete specified files from all subdirectories.
  /Q            Quiet mode, do not ask if ok to delete on global wildcard
  /A            Selects files to delete based on attributes
  attributes    R  Read-only files            S  System files
                H  Hidden files               A  Files ready for archiving
                I  Not content indexed Files  L  Reparse Points
                O  Offline files              -  Prefix meaning not

If Command Extensions are enabled DEL and ERASE change as follows:

The display semantics of the /S switch are reversed in that it shows
you only the files that are deleted, not the ones it could not find.
";
        }
        public String rename()
        {
            return @"Renames a file or files.

RENAME [drive:][path]filename1 filename2.
REN [drive:][path]filename1 filename2.

Note that you cannot specify a new drive or path for your destination file.
";
        }
          
    }

}
