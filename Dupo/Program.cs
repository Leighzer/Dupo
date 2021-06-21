using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dupo
{
    // interesting starter code getting all files (and files in subdirectories) in a directory
    // https://stackoverflow.com/questions/929276/how-to-recursively-list-all-the-files-in-a-directory-in-c/929418#929418
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                string path = args[0];
                if (Directory.Exists(path))
                {
                    var test = GetFiles(path).ToList();
                    foreach (string file in GetFiles(path))
                    {
                        Console.WriteLine(file);
                    }
                }
                else
                {
                    Console.Write($"Directory does not exist at {path}");
                }
            }
            else
            {
                Console.WriteLine($"No arguments received. Please specify a path to a directory.");
            }
        }

        public static IEnumerable<string> GetFiles(string path)
        {
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(path);
            while (queue.Count > 0)
            {
                path = queue.Dequeue();
                try
                {
                    foreach (string subDir in Directory.GetDirectories(path))
                    {
                        queue.Enqueue(subDir);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
                string[] files = null;
                try
                {
                    files = Directory.GetFiles(path);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
                if (files != null)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        yield return files[i];
                    }
                }
            }
        }
    }
}
