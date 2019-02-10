using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace bugtracker.Helpers
{
    public class iconHelper
    {
        
        public static string GetAssociatedIcon(string fileName)
        {
            var icon = "";
            var fileExtension = Path.GetExtension(fileName);

            switch (fileName)
            {
                case ".pdf":
                    icon = "fa fa-file-pdf-o";
                    break;
                case ".img":
                    icon = "fa fa-file-pdf-o";
                    break;
                case ".doc":
                    icon = "fa fa-file-pdf-o";
                    break;
                case ".txt":
                    icon = "fa fa-file-pdf-o";
                    break;
                case ".docx":
                    icon = "fa fa-file-pdf-o";
                    break;
                case ".xls":
                    icon = "fa fa-file-pdf-o";
                    break;
                case ".xlsx":
                    icon = "fa fa-file-pdf-o";
                    break;
                case ".jpg":
                case ".jpeg":
                case ".gif":
                case ".tiff":
                case ".bmp":
                case ".png":
                    icon = "fa fa-file-image-o";
                    break;
                default:
                    icon = "fa fa-file-code-o";
                    break;
                    

            }
            return icon;
        }
    }
}