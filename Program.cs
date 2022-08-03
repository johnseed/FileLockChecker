// See https://aka.ms/new-console-template for more information

using X.Common.Helper;

bool isLocked = IsFileLocked(new FileInfo(args[0]));
if(isLocked)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("File is locked");
    Console.ResetColor();
}
else
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("File is not locked");
    Console.ResetColor();
}
Console.WriteLine("Hello, World!");

bool IsFileLocked(FileInfo file)
{
    if(!string.IsNullOrWhiteSpace(file.DirectoryName) && file.Exists)
    {
        // fcntl ?
        string result = CommandHelper.Execute("lsof", $" {file.FullName}");
        bool isLocked = !string.IsNullOrWhiteSpace(result);
        return isLocked;
    }
    return false;
}

// not working on linux
bool IsFileLocked2(FileInfo file)
{
    try
    {
        using FileStream stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        stream.Lock(0, file.Length);
        stream.Close();
    }
    catch (IOException)
    {
        //the file is unavailable because it is:
        //still being written to
        //or being processed by another thread
        //or does not exist (has already been processed)
        return true;
    }

    //file is not locked
    return false;
}
