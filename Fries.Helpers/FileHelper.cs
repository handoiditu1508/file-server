using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fries.Helpers
{
    public static class FileHelper
    {
        public static bool IsDirectory(string path)
        {
            FileAttributes attr = File.GetAttributes(path);

            return attr.HasFlag(FileAttributes.Directory);
        }
    }
}
