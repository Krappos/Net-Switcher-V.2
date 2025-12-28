using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace netSwitcherBackend
{
    public class FileWorker
    {
        string IsrName = Environment.UserName;


        //for basic use
        string FolderPath => $@"C:\\Users\\{IsrName}\\Testing";


        //for IT department
        //string FolderPath => $@"C:\\WebSwitcher";


        string FilePath => Path.Combine(FolderPath, "path.txt");

        public void CreateFiles()
        {
            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);
            if (!File.Exists(FilePath))
                File.WriteAllText(FilePath, string.Empty);
        }
        public void WriteScript(string script)
        {
            File.WriteAllText(FilePath, script);

        }
        public String ReadScript()
        {
            if (File.Exists(FilePath))
                return File.ReadAllText(FilePath);
            else
                return string.Empty;
        }
    }
}
