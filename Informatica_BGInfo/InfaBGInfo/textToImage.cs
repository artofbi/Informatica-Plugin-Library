/*

// The current wallpaper path is stored in the registry at HKEY_CURRENT_USER\\Control Panel\\Desktop\\WallPaper

RegistryKey rkWallPaper = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", false);
string WallpaperPath = rkWallPaper.GetValue("WallPaper").ToString();

rkWallPaper.Close();
// Return the current wallpaper path
return WallpaperPath;
 
 */
using System;
using System.Collections.Generic;

using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;

namespace InfaBGInfo
{
    class textToImage
    {

        public void SaveImageToFile(string sImageText, Color backgroundColor
            , Color foregroundColor, int fontSize, FontStyle fontStyle, string sOutputFile
            , CurrentDesktopDimensions oCDD, string sLayerOnImageFullFilePath
            )
        {
            // set some properties
            Bitmap objBmpImage = null;
            System.Drawing.Image original = null;

            if (String.IsNullOrEmpty(sLayerOnImageFullFilePath))
            {
                // Create a background image of size and color basic bkg
                objBmpImage = new Bitmap(oCDD.DesktopWidth, oCDD.DesktopHeight);

                // Create a graphics object to measure the text's width and height.
                Graphics objGraphics = Graphics.FromImage(objBmpImage);

                // Set Background color
                objGraphics.Clear(backgroundColor);
                objGraphics.Flush();

                original = objBmpImage;
            }
            else
            {
                //objBmpImage = new Bitmap(sLayerOnImageFullFilePath);
                objBmpImage = new Bitmap(Image.FromFile(sLayerOnImageFullFilePath)
                    , oCDD.DesktopWidth
                    , oCDD.DesktopHeight);
                original = objBmpImage;
            }

            // Construct a bitmap from the button image resource 
            // to create the main textToString image
            Bitmap bmp1 = CreateBitmapImage(sImageText, fontSize
                , fontStyle, foregroundColor);


            Graphics gra = null;
            try
            {
                gra = Graphics.FromImage(original);
            }
            catch (Exception)
            {
                /*
                On 2010/07/30 I couldn't figure out how to prevent a bad .GIF file from causing an Exception.
                The goal here was to some how convert the image to a renderable format that the text could be
                superimposed on.  I did not get this working.  instead I am posting basic code for a blank 
                solid background image.
                 */
                ////////Bitmap org = (Bitmap)Image.FromFile(sLayerOnImageFullFilePath);
                ////////Bitmap bm = new Bitmap(org.Width, org.Height); // Can specify optional pixel format but defaults to 32bppArgb
                ////////gra=Graphics.FromImage(bm);

                //////////bm.Save(sOutputFile, System.Drawing.Imaging.ImageFormat.Bmp);
                //////////gra.DrawImage(org,0,0);

                ////////original = bm;
                // Create a background image of size and color basic bkg
                objBmpImage = new Bitmap(oCDD.DesktopWidth, oCDD.DesktopHeight);

                // Create a graphics object to measure the text's width and height.
                Graphics objGraphics = Graphics.FromImage(objBmpImage);

                // Set Background color
                objGraphics.Clear(backgroundColor);
                objGraphics.Flush();


                // These two objects must availble before proceeding.
                original = objBmpImage;
                gra = Graphics.FromImage(original);

            }


            Bitmap logo = new Bitmap(bmp1);
            gra.DrawImage(logo, new Point(5, 5));

            original.Save(sOutputFile, System.Drawing.Imaging.ImageFormat.Bmp);

            // Dispose of the image file.
            bmp1.Dispose();
            gra.Dispose();
        }

        private Bitmap CreateBitmapImage(string sImageText, int fontSize
            , FontStyle fontStyle, Color foregroundColor)
        {

            Bitmap objBmpImage = new Bitmap(1, 1);
            int intWidth = 0;
            int intHeight = 0;



            // Create the Font object for the image text drawing.
            Font objFont = new Font("Arial", fontSize, fontStyle
                , System.Drawing.GraphicsUnit.Pixel);



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
