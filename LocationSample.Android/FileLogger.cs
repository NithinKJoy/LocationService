using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LocationSample.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(FileLogger))]
namespace LocationSample.Droid
{
    public class FileLogger : IFileLogger
    { 
        public void LogInformation(string value)
        {
            //try
            //{
            //    string directoryPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            //    var path = Path.Combine(directoryPath, "LocationSampleLog   ");
            //    string filename = Path.Combine(path, "LogFile.txt");
            //    if (!Directory.Exists(path))
            //    {
            //        Directory.CreateDirectory(path);
            //    }

            //    var fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            //    // Set stream position to end-of-file
            //    fs.Seek(0, SeekOrigin.End);

            //    using (StreamWriter objStreamWriter = new StreamWriter(fs, Encoding.UTF8))
            //    {
            //        objStreamWriter.WriteLine(value);
            //        objStreamWriter.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //}
        }
    }
}
