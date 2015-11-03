using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.IO;
using BulkDownload.Extensions;

namespace BulkDownload.Layouts.BulkDownload
{
    public partial class BulkDownload : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string listId = Request.Params["listId"];
            if (string.IsNullOrEmpty(listId)) return;

            SPList list = SPContext.Current.Web.Lists.GetList(new Guid(listId), false);

            if (!list.IsDocumentLibrary()) return;

            string pItemIds = Request.Params["itemIDs"];
            if (string.IsNullOrEmpty(pItemIds)) return;

            SPDocumentLibrary library = (SPDocumentLibrary)list;

            string[] sItemIds = pItemIds.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            int[] itemsIDs = new int[sItemIds.Length];
            for (int i = 0; i < sItemIds.Length; i++)
            {
                itemsIDs[i] = Convert.ToInt32(sItemIds[i]);
            }

            if (itemsIDs.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (ZipFileBuilder builder = new ZipFileBuilder(ms))
                    {
                        foreach (int id in itemsIDs)
                        {
                            SPListItem item = library.GetItemById(id);
                            if (item.IsFolder())
                                AddFolder(builder, item.Folder, string.Empty);
                            else
                                AddFile(builder, item.File, string.Empty);
                        }

                        builder.Finish();
                        WriteStreamToResponse(ms);
                    }
                }
            }

        }

        private static void AddFile(ZipFileBuilder builder, SPFile file, string folder)
        {
            using (Stream fileStream = file.OpenBinaryStream())
            {
                builder.Add(folder + "\\" + file.Name, fileStream);
                fileStream.Close();
            }
        }

        private void AddFolder(ZipFileBuilder builder, SPFolder folder, string parentFolder)
        {
            string folderPath = parentFolder == string.Empty ? folder.Name : parentFolder + "\\" + folder.Name;
            builder.AddDirectory(folderPath);

            foreach (SPFile file in folder.Files)
            {
                AddFile(builder, file, folderPath);
            }

            foreach (SPFolder subFolder in folder.SubFolders)
            {
                AddFolder(builder, subFolder, folderPath);
            }
        }

        private void WriteStreamToResponse(MemoryStream ms)
        {
            if (ms.Length > 0)
            {
                string filename = DateTime.Now.ToFileTime().ToString() + ".zip";
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("Content-Length", ms.Length.ToString());
                Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/octet-stream";

                byte[] buffer = new byte[65536];
                ms.Position = 0;
                int num;
                do
                {
                    num = ms.Read(buffer, 0, buffer.Length);
                    Response.OutputStream.Write(buffer, 0, num);
                }

                while (num > 0);

                Response.Flush();
            }
        }
    }
}
