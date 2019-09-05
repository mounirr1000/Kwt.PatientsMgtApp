using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kwt.PatientsMgtApp.WebUI.Models
{
    public class FileViewModel
    {
        public string PatientCid { get; set; }

        public string PatientName { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public bool[] IsFolderEmpty { get; set; }
        public string[] FilesName { get; set; }

        public string[] FilesPath { get; set; }

        public string FolderName { get; set; }
        public string FolderPath { get; set; }

        public string[] FoldersPath { get; set; }

        public string[] FoldersName { get; set; }
    }
}