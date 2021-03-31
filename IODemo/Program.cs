// 该程序演示了System.IO类中的一些特性
// 1、二进制流的使用
// 2、字节流的使用
// 3、目录结构与操作
// 4、驱动器信息
// 5、文件的操作以及文件流
// 6、文件路径的操作
// 该网站展示了一些通用的IO操作的Demo如有需要可以查阅https://docs.microsoft.com/zh-cn/dotnet/standard/io/common-i-o-tasks
using System;
using System.IO;
using System.Text;
namespace IODemo
{
    class Program
    {
        static void Main(string[] args)
        {

            ////------------------1、二进制流的使用------------------//
            //BinaryWriter bw = new BinaryWriter(new FileStream("test.txt", FileMode.OpenOrCreate), Encoding.UTF8);
            //bw.BaseStream.Seek(0, SeekOrigin.End);
            //bw.Write("wo");
            //bw.Close();
            //BinaryReader br = new BinaryReader(new FileStream("test.txt", FileMode.Open), Encoding.UTF8);
            //String content1 = br.ReadString();
            //String content2 = br.ReadString();
            //Console.WriteLine(content1 + " " + content2);
            //br.Close();


            ////------------------2、二进制流的使用------------------//

            ////------------------3、目录结构与操作------------------//
            //// Directory提供了目录的典型操作，复制、移动、重命名、创建和删除目录
            //// DirectoryInfo提供了与上述Directory一样的功能但是使用它需要实例化
            //try
            //{
            //    // 检索目录移动文件
            //    string sourceDirectory = @"D:\src";
            //    string archiveDirectory = @"D:\des";
            //    var txtFiles = Directory.EnumerateFiles(sourceDirectory, "*.txt");

            //    foreach (string currentFile in txtFiles)
            //    {
            //        string fileName = currentFile.Substring(sourceDirectory.Length + 1);
            //        Directory.Move(currentFile, Path.Combine(archiveDirectory, fileName));
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            ////------------------4、驱动器信息------------------//
            //DriveInfo[] allDrives = DriveInfo.GetDrives();
            //foreach (DriveInfo d in allDrives)
            //{
            //    Console.WriteLine("Drive {0}", d.Name);
            //    Console.WriteLine("  Drive type: {0}", d.DriveType);
            //    if (d.IsReady == true)
            //    {
            //        Console.WriteLine("  Volume label: {0}", d.VolumeLabel);
            //        Console.WriteLine("  File system: {0}", d.DriveFormat);
            //        Console.WriteLine(
            //            "  Available space to current user:{0, 15} bytes",
            //            d.AvailableFreeSpace);

            //        Console.WriteLine(
            //            "  Total available space:          {0, 15} bytes",
            //            d.TotalFreeSpace);

            //        Console.WriteLine(
            //            "  Total size of drive:            {0, 15} bytes ",
            //            d.TotalSize);
            //    }
            //}


            //------------------5、文件的操作以及文件流------------------//
            // 使用 File 类执行典型操作，例如一次复制、移动、重命名、创建、打开、删除和追加到单个文件。
            // 如果对同一文件执行多个操作，则使用 FileInfo 实例方法而不是类的相应静态方法会更有效， File 因为并不总是需要安全检查。
            // 你还可以使用 File 类来获取和设置文件特性或 DateTime 与文件的创建、访问和写入相关的信息。
            // 如果要对多个文件执行操作，请参阅 Directory.GetFiles 或 DirectoryInfo.GetFiles 。
            // FileAccess 指定对文件的读取和写入访问权限。
            // FileShare 指定已在使用的文件所允许的访问级别。
            // FileMode 指定是保留还是覆盖现有文件的内容，以及创建现有文件的请求是否会引发异常。
            // FileStream fs = new FileStream(name, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 使用 FileStream 类来读取、写入、打开和关闭文件系统上的文件，以及操作与文件相关的其他操作系统句柄，包括管道、标准输入和标准输出。 您可以使用 Read 、 Write 、 CopyTo 和 Flush 方法执行同步操作，或者使用 ReadAsync 、 WriteAsync 、 CopyToAsync 和 FlushAsync 方法执行异步操作。 使用异步方法来执行占用大量资源的文件操作，而不会阻止主线程。

            string path = @"d:\des\234.txt";
            if (!File.Exists(path))
            {   
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("Hello");
                    sw.WriteLine("And");
                    sw.WriteLine("Welcome");
                }
            }

            // Open the file to read from.
            using (StreamReader sr = File.OpenText(path))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
            }

            // 同步文件流
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (FileStream fs  = File.Create(path))
            {
                byte[] info1 = new UTF8Encoding(true).GetBytes("This is some text");
                byte[] info2 = new UTF8Encoding(true).GetBytes("This is some more text,");
                byte[] info3 = new UTF8Encoding(true).GetBytes("\r\nand this is on a new line");
                byte[] info4 = new UTF8Encoding(true).GetBytes("\r\n\r\nThe following is a subset of characters:\r\n");

          
                fs.Write(info1, 0, info1.Length);
                fs.Write(info2, 0, info1.Length);
                fs.Write(info3, 0, info1.Length);
                fs.Write(info4, 0, info1.Length);

            }
            using (FileStream fs = File.OpenRead(path))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                while (fs.Read(b, 0, b.Length) > 0)
                {
                    Console.WriteLine(temp.GetString(b));
                }
            }
            //异步文件流
            AsyncFileWrite();
            //------------------6、文件路径的操作------------------//
            //路径是提供文件或目录的位置的字符串。 路径不一定指向磁盘上的位置;
            //例如，路径可能映射到内存中或设备上的某个位置。 路径的准确格式由当前平台决定。
            //例如，在某些系统中，路径可以从驱动器或卷号开始，而此元素在其他系统中不存在。
            //在某些系统上，文件路径可以包含扩展，这表示文件中存储的信息的类型。
            //文件扩展名的格式依赖于平台;例如，某些系统限制了三个字符的扩展 (例如，在光学媒体) 上使用的较小闪存存储和较旧版本的 ISO 9660 上通常使用 FAT16，而其他系统则不限制。
            //当前平台还决定了用于分隔路径中的元素的字符集，以及指定路径时不能使用的字符集。
            //由于这些差异，类的字段以及 Path 类的某些成员的确切行为 Path 取决于平台。
            string path1 = @"c:\temp\MyTest.txt";
            string path2 = @"c:\temp\MyTest";
            string path3 = @"temp";
            if (Path.HasExtension(path1))
            {
                Console.WriteLine("{0} has an extension.", path1);
            }

            if (!Path.HasExtension(path2))
            {
                Console.WriteLine("{0} has no extension.", path2);
            }

            if (!Path.IsPathRooted(path3))
            {
                Console.WriteLine("The string {0} contains no root information.", path3);
            }

            Console.WriteLine("The full path of {0} is {1}.", path3, Path.GetFullPath(path3));
            Console.WriteLine("{0} is the location for temporary files.", Path.GetTempPath());
            Console.WriteLine("{0} is a file available for use.", Path.GetTempFileName());


            Console.ReadKey();
        }

        public async static void AsyncFileWrite()
        {
            //异步文件流
            UnicodeEncoding uniencoding = new UnicodeEncoding();
            string filename = @"d:\des\async.txt";

            byte[] result = uniencoding.GetBytes("Async Info");

            using (FileStream SourceStream = File.Open(filename, FileMode.OpenOrCreate))
            {
                SourceStream.Seek(0, SeekOrigin.End);
                await SourceStream.WriteAsync(result, 0, result.Length);
            }

        }
    }
}
