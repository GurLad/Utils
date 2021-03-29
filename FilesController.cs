using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace Utils
{
    /*
     * This class helps using files without having to worry about a path and missing files.
     * Feel free to use it.
     * 
     * This code is pretty old by now, but still my go-to file system, mostly out of habit.
     * 
     * Version 2.1, by Gur Ladizhinsky, 29.03.2021
     */

    public class FilesController
    {
        public string Path { get; set; } = Directory.GetCurrentDirectory();
        public string DefultFileFormat { get; set; } = ".txt";
        public string DefultImageFileFormat { get; set; } = ".png";
        public char Seperator { get; set; } = @"\"[0]; //Mainly for other OS's.
        public Encoding TextEncoding = Encoding.Default;
        public FilesController()
        {
            //Fixes bugs when launching from start
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            Path = Environment.CurrentDirectory;
        }
        public FilesController(string BasePath)
        {
            //Fixes bugs when launching from start
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            Path = Environment.CurrentDirectory;
            CreateDirectory(BasePath, true);
        }
        /// <summary>
        /// Checks whether a file already exists.
        /// </summary>
        /// <param name="Name">The name of the file to check.</param>
        /// <param name="Format">The format of the file (defaults to "", not default format).</param>
        /// <returns>True if it exists, false otherwise.</returns>
        public bool CheckFileExist(string Name, string Format = null)
        {
            return File.Exists(Path + Seperator + Name + (Format ?? ""));
        }
        /// <summary>
        /// Creates a new directory, and modifies the path to enter it if NavigateThere is true.
        /// </summary>
        /// <param name="Name">The name of the directory to create.</param>
        /// <param name="NavigateThere">If true, the Path will alter to include this directory.</param>
        public void CreateDirectory(string Name, bool NavigateThere = false)
        {
            Directory.CreateDirectory(Path + Seperator + Name);
            if (NavigateThere) Path += Seperator + Name;
        }
        /// <summary>
        /// Checks whether a directory already exists.
        /// </summary>
        /// <param name="Name">The name of the directory to check.</param>
        public bool CheckDirectoryExists(string Name)
        {
            return Directory.Exists(Path + Seperator + Name);
        }
        /// <summary>
        /// Deletes a directory.
        /// </summary>
        /// <param name="Name">The name of the directory to delete.</param>
        public void DeleteDirectory(string Name)
        {
            Directory.Delete(Path + Seperator + Name);
        }
        /// <summary>
        /// Resets Path to its defult value.
        /// </summary>
        public void ResetPath()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            Path = Environment.CurrentDirectory;
        }
        /// <summary>
        /// Saves a text file.
        /// </summary>
        /// <param name="Name">The name of the file.</param>
        /// <param name="Content">The content of the file.</param>
        /// <param name="Format">The format of the file (a string, ex. ".txt").</param>
        public void SaveFile(string Name, string Content, string Format = null)
        {
            if (Format == null)
            {
                Format = DefultFileFormat;
            }
            File.WriteAllText(Path + Seperator + Name + Format, Content, TextEncoding);
        }
        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="Name">The name of the file to delete.</param>
        /// <param name="Format">The format of the file (a string, ex. ".txt").</param>
        public void DeleteFile(string Name, string Format = null)
        {
            if (Format == null)
            {
                Format = DefultFileFormat;
            }
            File.Delete(Path + Seperator + Name + Format);
        }
        /// <summary>
        /// Reads a text file and returns its contents.
        /// </summary>
        /// <param name="Name">The name of the file to read.</param>
        /// <param name="DefultValue">If the file doesn't exist, create a new file with this value.</param>
        /// <param name="Format">The format of the file (a string, ex. ".txt").</param>
        /// <returns></returns>
        public string LoadFile(string Name, string DefultValue = "", string Format = null)
        {
            if (Format == null)
            {
                Format = DefultFileFormat;
            }
            if (!CheckFileExist(Name + Format)) SaveFile(Name, DefultValue, Format);
            return File.ReadAllText(Path + Seperator + Name + Format, TextEncoding);
        }
        /// <summary>
        /// Appeands data to a text file (basically a combination between save and load file).
        /// </summary>
        /// <param name="Name">The name of the file.</param>
        /// <param name="Content">The content to appeand (at the end of the file).</param>
        /// <param name="DefultValue">If the file doesn't exist, create a new file with this value.</param>
        /// <param name="Format">The format of the file (a string, ex. ".txt").</param>
        public void AppendFile(string Name, string Content, string DefultValue = "", string Format = null)
        {
            if (Format == null)
            {
                Format = DefultFileFormat;
            }
            SaveFile(Name, LoadFile(Name, DefultValue, Format) + Content, Format);
        }
        /// <summary>
        /// Saves an image file.
        /// </summary>
        /// <param name="Name">The name of the file.</param>
        /// <param name="TheImage">The image to save.</param>
        /// <param name="Format">The format of the image (a string, ex. ".png").</param>
        public void SaveImage(string Name, Image TheImage, string Format = null)
        {
            if (Format == null)
            {
                Format = DefultImageFileFormat;
            }
            if (!Directory.Exists(Path + Seperator + "Images")) Directory.CreateDirectory(Path + Seperator + "Images");
            if (File.Exists(Path + Seperator + "Images" + Seperator + Name + Format)) File.Delete(Path + Seperator + "Images" + Seperator + Name + Format);
            // MessageBox.Show(Path + @"\Images\" + Name + Format); // Debug
            TheImage.Save(Path + Seperator + "Images" + Seperator + Name + Format);
        }
        /// <summary>
        /// Loads an image file.
        /// </summary>
        /// <param name="Name">The name of the image to load.</param>
        /// <param name="Format">The format of the image (a string, ex. ".png").</param>
        /// <param name="DisposeMode">Use Stream rather than Image.FromFile. If unsure, leave it as true.</param>
        /// <returns></returns>
        public Image LoadImage(string Name, string Format = null, bool DisposeMode = true)
        {
            if (Format == null)
            {
                Format = DefultImageFileFormat;
            }
            try
            {
                if (!Directory.Exists(Path + @"\Images")) Directory.CreateDirectory(Path + @"\Images");
                if (DisposeMode)
                {
                    Image image;
                    using (Stream stream = File.OpenRead(Path + @"\Images\" + Name + Format))
                    {
                        image = Image.FromStream(stream);
                        stream.Dispose();
                    }
                    return image;
                }
                else
                {
                    return Image.FromFile(Path + @"\Images\" + Name + Format);
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Returns all files in Path ( + AdditionalPath).
        /// </summary>
        /// <param name="IncludePath">If true, all files will include their full path (ex. "C:\Folder\Name.png" vs. "Name.png").</param>
        /// <param name="IncludeFormat">If true, all files will include their format (ex. "Name.png" vs. "Name"). You should disable this only if you know all files are the same format.</param>
        /// <param name="AdditionalPath">Check in this path inside Path (ex. Path is "C:\", addition is "Folder\", will return all files in "C:\Folder\"). This is unsafe, and you really shouldn't use it.</param>
        /// <returns></returns>
        public string[] AllFiles(bool IncludePath = false, bool IncludeFormat = true, string AdditionalPath = "")
        {
            if (!Directory.Exists(Path + AdditionalPath)) Directory.CreateDirectory(Path + AdditionalPath);
            if (IncludePath) return Directory.GetFiles(Path + AdditionalPath);
            else
            {
                string[] Files = Directory.GetFiles(Path + AdditionalPath);
                for (int i = 0; i < Files.Length; i++)
                {
                    string[] Temp = Files[i].Split(@"\"[0]);
                    Files[i] = Temp[Temp.Length - 1];
                    if (!IncludeFormat && Files[i].LastIndexOf('.') >= 0)
                    {
                        Files[i] = Files[i].Substring(0, Files[i].LastIndexOf('.'));
                    }
                }
                return Files;
            }
        }
        /// <summary>
        /// Returns all directories in Path ( + AdditionalPath).
        /// </summary>
        /// <param name="IncludePath">If true, all files will include their full path (ex. "C:\Folder\Name.png" vs. "Name.png").</param>
        /// <param name="AdditionalPath">Check in this path inside Path (ex. Path is "C:\", addition is "Folder\", will return all files in "C:\Folder\"). This is unsafe, and you really shouldn't use it.</param>
        /// <returns></returns>
        public string[] AllDirectories(bool IncludePath = false, string AdditionalPath = "")
        {
            if (!Directory.Exists(Path + AdditionalPath)) Directory.CreateDirectory(Path + AdditionalPath);
            if (IncludePath) return Directory.GetDirectories(Path + AdditionalPath);
            else
            {
                string[] Files = Directory.GetDirectories(Path + AdditionalPath);
                for (int i = 0; i < Files.Length; i++)
                {
                    string[] Temp = Files[i].Split(@"\"[0]);
                    Files[i] = Temp[Temp.Length - 1];
                }
                return Files;
            }
        }
    }
}
