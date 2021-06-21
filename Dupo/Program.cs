using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Dupo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                string path = args[0];
                if (Directory.Exists(path))
                {
                    IEnumerable<string> files = GetFiles(path);

                    Console.WriteLine($"{files.Count()} file(s) found.");

                    Dictionary<string, List<string>> hashLookup = new Dictionary<string, List<string>>();

                    using (var md5 = MD5.Create())
                    {
                        foreach (var file in files)
                        {
                            using (FileStream stream = File.OpenRead(file))
                            {
                                byte[] hash = md5.ComputeHash(stream);
                                string hashString = BytesToString(hash);

                                if (!hashLookup.ContainsKey(hashString))
                                {
                                    hashLookup.Add(hashString, new List<string>() { file });
                                }
                                else
                                {
                                    hashLookup[hashString].Add(file);
                                }
                            }
                        }
                    }

                    int dupeCount = hashLookup.Where(x => x.Value.Count > 1).Count();

                    Console.WriteLine($"{dupeCount} set(s) of files with same contents found. (md5 hash of contents match)" + Environment.NewLine);

                    var orderedKeyValues = hashLookup.OrderByDescending(x => x.Value.Count).ToList();

                    foreach (var pair in orderedKeyValues)
                    {
                        Console.WriteLine($"{pair.Value.Count} {pair.Key}");
                        foreach (var file in pair.Value)
                        {
                            Console.WriteLine(file);
                        }
                        Console.WriteLine(Environment.NewLine);
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

        // adapted from: http://csharphelper.com/blog/2018/04/calculate-hash-codes-for-a-file-in-c/
        public static string BytesToString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            string result = sb.ToString();
            return result;
        }

        // interesting code getting all files (and files in subdirectories) in a directory
        // https://stackoverflow.com/questions/929276/how-to-recursively-list-all-the-files-in-a-directory-in-c/929418#929418
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
