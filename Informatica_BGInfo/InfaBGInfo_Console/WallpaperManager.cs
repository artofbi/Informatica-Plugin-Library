// This source file is part of WallpaperControl, a tool for automatic changing 
// of the desktop wallpaper.
// 
// Visit http://www.wallpaper-control.xe.cx/ and 
// http://wpcontrol.codeplex.com/ for more.
//
// Copyright (c) 2009 - 2010 Niklas Kroll
//
// Licensed under the GPLv2.
// See http://www.gnu.org/licenses/gpl-2.0.html for details.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using Microsoft.Win32;

namespace InfaBGInfo_Console
{


    public struct CurrentDesktopDimensions
    {
        public int DesktopWidth;
        public int DesktopHeight;
    }

    /// <summary>
    /// Manages the windows wallpaper including setting the next, the previous and
    /// the selected wallpaper as desktop background.
    /// </summary>
    class WallpaperManager
    {

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(
            int uAction, int uParam, string lpvParam, int fuWinIni);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int GetSystemMetrics(int nIndex);


        /// <summary>
        /// Represents that the wallpaper should be changed.
        /// </summary>
        /// <remarks>Used as first argument in SystemParametersInfo()</remarks>
        const int SPI_SETDESKWALLPAPER = 20;
        /// <summary>
        /// Specifies if the INI file should be updated.
        /// </summary>
        const int SPIF_UPDATEINIFILE = 0x01;
        /// <summary>
        /// Specifies if it should be broadcasted that the INI file has been changed.
        /// </summary>
        const int SPIF_SENDWININICHANGE = 0x02;

        /// <summary>
        /// Sets the windows wallpaper.
        /// </summary>
        /// <param name="path">The path to the image file.</param>
        /// <param name="style">The syle how the wallpaper should be displayed if it doesn't fit the screen.</param>
        /// <exception cref="System.IO.FileNotFoundException">Thrown if the specified file is not existing.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown if the param 'path' is null or an empty string.</exception>
        public static void SetWallpaper(string imageFullFilePath, Wallpaper.Style style)
        {
            // Make sure the path is set
            if (imageFullFilePath == null || imageFullFilePath.Length == 0)
            {
                throw new ArgumentNullException("path", "The path to the wallpaper is not set!");
            }

            // Check if the specified file exists
            if (File.Exists(imageFullFilePath))
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", true);

                // Determin the zoom style of the wallpaper and save it to registry
                switch (style)
                {
                    case Wallpaper.Style.Stretched:
                        key.SetValue(@"WallpaperStyle", "2");
                        key.SetValue(@"TileWallpaper", "0");
                        break;
                    case Wallpaper.Style.Centered:
                        key.SetValue(@"WallpaperStyle", "1");
                        key.SetValue(@"TileWallpaper", "0");
                        break;
                    case Wallpaper.Style.Tiled:
                        key.SetValue(@"WallpaperStyle", "1");
                        key.SetValue(@"TileWallpaper", "1");
                        break;
                }
                // Change wallpaper with SystemParametersInfo from Win API
                SafeNativeMethods.SystemParametersInfo(SPI_SETDESKWALLPAPER
                    , 0, imageFullFilePath, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }


        public static void Set(string imageFullFilePath, Wallpaper.Style style)
        {
            //System.IO.Stream s = new WebClient().OpenRead(uri.ToString());

            System.Drawing.Image img = Bitmap.FromFile(imageFullFilePath);
            img.Save(imageFullFilePath, System.Drawing.Imaging.ImageFormat.Bmp);

            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            if (style == Wallpaper.Style.Stretched)
            {
                key.SetValue(@"WallpaperStyle", 2.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }

            if (style == Wallpaper.Style.Centered)
            {
                key.SetValue(@"WallpaperStyle", 1.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }

            if (style == Wallpaper.Style.Tiled)
            {
                key.SetValue(@"WallpaperStyle", 1.ToString());
                key.SetValue(@"TileWallpaper", 1.ToString());
            }

            SystemParametersInfo(SPI_SETDESKWALLPAPER,
                0,
                imageFullFilePath,
                SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }

        private void ChangeWallPaper(string imageFullFilePath, Wallpaper.Style style)
        {
            //String imageFullFilePath = "C:\\_Dev\\FyghtSoft_InfaBGInfo.bmp";

            if (imageFullFilePath == null)
                return;

            const int SPI_SETDESKWALLPAPER = 20;
            const int SPIF_UPDATEINIFILE = 0x01;
            const int SPIF_SENDWININICHANGE = 0x02;

            const int SM_CXSCREEN = 0;
            const int SM_CYSCREEN = 1;

            Bitmap img = new Bitmap(imageFullFilePath);

            Bitmap newImg = new Bitmap(img, GetSystemMetrics(SM_CXSCREEN),
                GetSystemMetrics(SM_CYSCREEN));


            newImg.Save(imageFullFilePath, System.Drawing.Imaging.ImageFormat.Bmp);

            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0,
                imageFullFilePath,
                SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }


        public static void ViewDimensions()
        {
            const int SM_CXSCREEN = 0;
            const int SM_CYSCREEN = 1;

            Console.WriteLine(GetSystemMetrics(SM_CXSCREEN).ToString() + " "
                + GetSystemMetrics(SM_CYSCREEN).ToString());
        }

        public static CurrentDesktopDimensions GetDesktopDimensions()
        {
            const int SM_CXSCREEN = 0;
            const int SM_CYSCREEN = 1;

            CurrentDesktopDimensions oCDD = new CurrentDesktopDimensions();
            oCDD.DesktopHeight = GetSystemMetrics(SM_CYSCREEN);
            oCDD.DesktopWidth = GetSystemMetrics(SM_CXSCREEN);

            return oCDD;
        }

    }


    [Serializable]
    public class WallpaperFileException : Exception
    {
        public WallpaperFileException()
        {
        }

        public WallpaperFileException(string message)
            : base(message)
        {
        }

        public WallpaperFileException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected WallpaperFileException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    public class WallpaperPathException : Exception
    {
        public WallpaperPathException()
        {
        }

        public WallpaperPathException(string message)
            : base(message)
        {
        }

        public WallpaperPathException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected WallpaperPathException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    public class ImageFormatException : Exception
    {
        public ImageFormatException()
        {
        }

        public ImageFormatException(string message)
            : base(message)
        {
        }

        public ImageFormatException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ImageFormatException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }


}
