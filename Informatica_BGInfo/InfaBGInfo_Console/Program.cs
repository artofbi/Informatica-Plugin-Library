using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace InfaBGInfo_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            textToImage testRun = new textToImage();
            //testRun.SaveImageToFile("This is just how it is\nDon't you understand?", "");
            //InfaWSHMetaDataInfo testWSH = new InfaWSHMetaDataInfo();
            //testWSH.ConductWSHMetaDataTrial();


            InfaWSHMetaDataInfo oGo = new InfaWSHMetaDataInfo("localhost", 7333
                , "", "Domain_virtualxp", "sdk_rep", "Administrator", "Administrator"
                , "Domain_virtualxp", "");

            //Console.WriteLine(oGo.GatherStringDataValues());


            string imageFullFilePath = "C:\\_Dev\\FyghtSoft_InfaBGInfo.bmp";
            WindowsSystemInfo oWSI = new WindowsSystemInfo();


            StringBuilder sb = new StringBuilder();


            // provide options as a struct and passed to wallpaper settings
            ShowOutputInfoOptions oBkgOptions = new ShowOutputInfoOptions();
            oBkgOptions.showOSInformation = true;

            sb.Append(oGo.GatherStringDataValues(oBkgOptions).ToString());
            //sb.AppendLine("Number of Folders: " + oGo.numOfFoldersInRepository.ToString());
            //sb.AppendLine("");
            sb.Append(oWSI.GetSystemInfoString());


            testRun.SaveImageToFile(sb.ToString()
                , Color.White, Color.Violet, imageFullFilePath
                , WallpaperManager.GetDesktopDimensions());



            WallpaperManager.ViewDimensions();

            //swap wallpaper
            WallpaperManager.SetWallpaper(imageFullFilePath, Wallpaper.Style.Centered);

            





            #region MyRegion
            //WindowsSystemInfo testRun2 = new WindowsSystemInfo();

            ////testRun2.windowsSystemInfo();
            //testRun2.GetStuff("Win32_OperatingSystem");
            ////testRun2.GetStuff("Win32_NetworkAdapterConfiguration WHERE IPEnabled = 'TRUE'");
            //testRun2.GetIPAddress();

            //HTMLToImage hc = new HTMLToImage();
            //hc.HtmlImageCapture +=
            //   new HTMLToImage.HTMLToImageEvent(hc_HtmlImageCapture);
            //hc.Create("http://www.codeproject.com");
            // Generate thumbnail of a webpage at 1024x768 resolution
            //Bitmap thumbnail = hc.GenerateScreenshot("http://www.artofbi.com", 1024, 768);

            // Generate thumbnail of a webpage at the webpage's full size (height and width)
            //thumbnail = GenerateScreenshot("http://pietschsoft.com");

            // Display Thumbnail in PictureBox control
            //pictureBox1.Image = thumbnail;


            // Save Thumbnail to a File
            //thumbnail.Save(@"C:\_Dev\thumbnail.png", System.Drawing.Imaging.ImageFormat.Png); 
            #endregion
            
        }

        static public void hc_HtmlImageCapture(object sender, Uri url, Bitmap image)
        {
            image.Save("C:\\_Dev\\FyghtSoft_InfaBGInfo.bmp");
        }
    }
}
