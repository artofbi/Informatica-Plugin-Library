using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;

namespace InfaBGInfo_Console
{
    class textToImage
    {

        public void SaveImageToFile(string sImageText, Color backgroundColor
            , Color foregroundColor, string sOutputFile, CurrentDesktopDimensions oCDD)
        {

            // Create a background image of size and color basic bkg
            Bitmap objBmpImage = new Bitmap(oCDD.DesktopWidth, oCDD.DesktopHeight);

            // Create a graphics object to measure the text's width and height.
            Graphics objGraphics = Graphics.FromImage(objBmpImage);
            objGraphics = Graphics.FromImage(objBmpImage);

            // Set Background color
            objGraphics.Clear(backgroundColor);
            objGraphics.Flush();


            // Construct a bitmap from the button image resource.
            Bitmap bmp1 = CreateBitmapImage(sImageText, foregroundColor);
            //bmp1.Save("c:\\_Dev\\testImage.gif", System.Drawing.Imaging.ImageFormat.Gif);


            //string mainBkg = @"C:\Documents and Settings\Administrator\Desktop\fs_logo_for_wallpaper.png";

            System.Drawing.Image original = objBmpImage;
            
            Graphics gra = Graphics.FromImage(original);
            Bitmap logo = new Bitmap(bmp1);
            gra.DrawImage(logo, new Point(5, 5));



            // Save the image as a GIF.
            //bmp1.Save("c:\\_Dev\\testImage.gif", System.Drawing.Imaging.ImageFormat.Gif);

            original.Save(sOutputFile, System.Drawing.Imaging.ImageFormat.Bmp);

            // Dispose of the image file.
            bmp1.Dispose();
        }

        private Bitmap CreateBitmapImage(string sImageText, Color foregroundColor)
        {

            Bitmap objBmpImage = new Bitmap(1, 1);
            int intWidth = 0;
            int intHeight = 0;



            // Create the Font object for the image text drawing.
            Font objFont = new Font("Arial", 20, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);



            // Create a graphics object to measure the text's width and height.
            Graphics objGraphics = Graphics.FromImage(objBmpImage);



            // This is where the bitmap size is determined.
            intWidth = (int)objGraphics.MeasureString(sImageText, objFont).Width;
            intHeight = (int)objGraphics.MeasureString(sImageText, objFont).Height;

            // Create the bmpImage again with the correct size for the text and font.
            objBmpImage = new Bitmap(objBmpImage, new Size(intWidth, intHeight));



            // Add the colors to the new bitmap.
            objGraphics = Graphics.FromImage(objBmpImage);



            // Set Background color
            //objGraphics.Clear(Color.);
            objGraphics.SmoothingMode = SmoothingMode.HighQuality;
            objGraphics.TextRenderingHint = TextRenderingHint.SystemDefault;
            objGraphics.DrawString(sImageText, objFont, new SolidBrush(foregroundColor), 0, 0);
            objGraphics.Flush();



            return (objBmpImage);
        }
    }
}
