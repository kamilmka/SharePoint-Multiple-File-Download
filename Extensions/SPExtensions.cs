using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkDownload.Extensions
{
    public static class SPExtensions
    {
        public static bool IsFolder(this SPListItem item)
        {
            return (item.Folder != null);
        }

        public static bool IsDocumentLibrary(this SPList list)
        {
            return (list.BaseType == SPBaseType.DocumentLibrary);
        }
    }
}
