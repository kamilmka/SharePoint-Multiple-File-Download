using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkDownload.Extensions
{
    public class ZipFileBuilder : IDisposable
    {
        private bool disposed = false;

        ZipOutputStream zipStream = null;
        protected ZipOutputStream ZipStream
        {
            get { return zipStream; }

        }

        ZipEntryFactory factory = null;
        private ZipEntryFactory Factory
        {
            get { return factory; }
        }


        public ZipFileBuilder(Stream outStream)
        {
            zipStream = new ZipOutputStream(outStream);
            zipStream.SetLevel(9); //best compression

            factory = new ZipEntryFactory(DateTime.Now);
        }

        public void Add(string fileName, Stream fileStream)
        {
            //create a new zip entry            
            ZipEntry entry = factory.MakeFileEntry(fileName);
            entry.DateTime = DateTime.Now;
            ZipStream.PutNextEntry(entry);

            byte[] buffer = new byte[65536];

            int sourceBytes;
            do
            {
                sourceBytes = fileStream.Read(buffer, 0, buffer.Length);
                ZipStream.Write(buffer, 0, sourceBytes);
            }
            while (sourceBytes > 0);


        }

        public void AddDirectory(string directoryName)
        {
            ZipEntry entry = factory.MakeDirectoryEntry(directoryName);
            ZipStream.PutNextEntry(entry);
        }

        public void Finish()
        {
            if (!ZipStream.IsFinished)
            {
                ZipStream.Finish();
            }
        }

        public void Close()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            this.Close();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (ZipStream != null)
                        ZipStream.Dispose();
                }
            }

            disposed = true;
        }
    }
}
