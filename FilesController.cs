using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
namespace Utils
{
    /*
     * This class helps using files without having to worry about a path and missing files.
     * Feel free to use it.
     * 
     * By Gur Ladizhinsky, 01.10.2018
     */

    public class FilesController
    {
        public string Path { get; set; } = Directory.GetCurrentDirectory();
        public FilesController() { }
        public FilesController(string BasePath)
        {
            CreateDirectory(BasePath, true);
        }
        public bool CheckFileExist(string Name)
        {
            return File.Exists(Path + @"\" + Name);
        }
        public void CreateDirectory(string Name, bool NavigateThere = false)
        {
            Directory.CreateDirectory(Path + @"\" + Name);
            if (NavigateThere) Path += @"\" + Name;
        }
        public void SaveFile(string Name, string Content)
        {
            // MessageBox.Show(Path + @"\" + Name); //Debug
            File.WriteAllText(Path + @"\" + Name, Content);
        }
        public void DeleteFile(string Name, bool ConfirmDialog = true)
        {
            if (!ConfirmDialog) File.Delete(Path + @"\" + Name);
            else if (MessageBox.Show("Are you sure you want to delete this?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes) File.Delete(Path + @"\" + Name);
        }
        public string LoadFile(string Name, string DefultValue = "")
        {
            // MessageBox.Show(Path + @"\" + Name); // Debug
            if (!File.Exists(Path + @"\" + Name)) SaveFile(Name, DefultValue);
            return File.ReadAllText(Path + @"\" + Name);
        }
        public void SaveImage(string Name, Image TheImage, string Format = ".png")
        {
            if (!Directory.Exists(Path + @"\Images")) Directory.CreateDirectory(Path + @"\Images");
            if (File.Exists(Path + @"\Images\" + Name + Format)) File.Delete(Path + @"\Images\" + Name + Format);
            // MessageBox.Show(Path + @"\Images\" + Name + Format); // Debug
            TheImage.Save(Path + @"\Images\" + Name + Format);
        }
        public Image LoadImage(string Name, bool DisposeMode = true, string Format = ".png")
        {
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
            catch { return null; }
        }
        public string[] AllFiles(string AdditionalPath = "")
        {
            if (!Directory.Exists(Path + AdditionalPath)) Directory.CreateDirectory(Path + AdditionalPath);
            return Directory.GetFiles(Path + AdditionalPath);
        }
    }
}
