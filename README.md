# SharePoint-Multiple-File-Download
This is a custom action for SharePoint 2013 that enables multiple file download as a zip file (Bulk file download)

This is based on an existing SharePoint 2010 multi file download solution. Original solution can be found at <a href="http://www.deviantpoint.com/post/2010/05/08/sharepoint-2010-download-as-zip-file-custom-ribbon-action.aspx">SharePoint 2010 Download as Zip File Custom Ribbon Action</a>. Credit goes to the original developer Bart X. Tubalinal for his solution.

This solution will work only on SharePoint 2013. (Will not work in SharePoint 2010)


#How to Deploy
Deploy the "BulkDownload.wsp" included above and activate the Site Level (web) feature "Bulk Download". This will include a "Download as Zip" button to the SharePoint ribbon control next to "Download a Copy" button. This button is visible only in document libraries.

If you want to make modifications please use the Visual Studio 2012 solution and make modifications as necessary.