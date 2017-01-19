// This source file is part of WallpaperControl 0.4, a tool for automatic changing 
// of the desktop wallpaper.
// 
// Visit http://www.wallpaper-control.xe.cx/ and 
// http://wpcontrol.codeplex.com/ for more info.
//
// Copyright (c) 2009 - 2010 Niklas Kroll
// Original code copyright (c) Sean Campbell
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
//
// This code is taken from Sean Campbells blog article about setting the windows 
// wallpaper on Coding4Fun. 
// Link: http://blogs.msdn.com/coding4fun/archive/2006/10/31/912569.aspx

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Microsoft.Win32;
using System.Drawing.Imaging;

namespace InfaBGInfo
{
    /// <summary>
    /// Represents a wallpaper that can be set as desktop background.
    /// </summary>
    internal class Wallpaper
    {

        string[] allowedExts = { "jpg", "gif", "bmp", "png" };

        private bool isImageSupported(string ext)
        {
            foreach (string s in this.allowedExts)
            {
                if (s == ext) return true;
            }

            return false;
        }

        /// <summary>
        /// Creates a new wallpaper object.
        /// </summary>
        /// <param name="path">The absolute path to the image.</param>
        /// <exception cref="WallpaperFileException">Thrown if path is invalid.</exception>
        /// <exception cref="ImageFormatException">Thrown if the image file is not supported.</exception>
        public Wallpaper(string path)
        {
            // Check if path is set and correct.
            if (!String.IsNullOrEmpty(path) && File.Exists(path))
            {
                if (isImageSupported(System.IO.Path.GetExtension(path).ToLowerInvariant()))
                {
                    // Apply path
                    this.Path = path;
                    // Apply standard zoom style
                    this.ZoomStyle = Wallpaper.Style.Stretched;
                    // Get image format from file and apply it
                    Image img = Image.FromFile(path);
                    this.Format = img.RawFormat;
                }
                else
                {
                    throw new ImageFormatException("The specified image is not supported!");
                }
            }
            else
            {
                throw new WallpaperFileException("Unable to access file at specified path!");
            }
        }

        /// <summary>
        /// Creates a new wallpaper object.
        /// </summary>
        /// <param name="path">The absolute path to the image.</param>
        /// <param name="zoomStyle">Defines how the image is zoomed if it doesn't
        /// fit to the screen size.</param>
        /// <exception cref="ImageFormatException">Thrown if the image file is not supported.</exception>
        /// <exception cref="WallpaperFileException">Thrown if path is invalid.</exception>
        public Wallpaper(string path, Wallpaper.Style zoomStyle)
        {
            // Check that path param is valid
            if (!String.IsNullOrEmpty(path) && File.Exists(path))
            {
                // Check if image file is supported
                if (isImageSupported(System.IO.Path.GetExtension(path)))
                {
                    // Apply path
                    this.Path = path;
                    // Apply zoom style
                    this.ZoomStyle = zoomStyle;
                    // Get image format
                    Image img = Image.FromFile(path);
                    // Apply image format
                    this.Format = img.RawFormat;
                }
                else
                {
                    throw new ImageFormatException("The specified image is not supported!");
                }
            }
            else
            {
                throw new WallpaperFileException("Unable to file access specified path!");
            }
        }

        /// <summary>
        /// Creates a new wallpaper object.
        /// </summary>
        /// <param name="path">The absolute path to the image.</param>
        /// <param name="zoomStyle">
        /// Defines how the image is zoomed if it doesn't fit to the screen size.
        /// </param>
        /// <param name="title">The title of the wallpaper.</param>
        /// <exception cref="ImageFormatException">Thrown if the image file is not supported.</exception>
        /// <exception cref="WallpaperFileException">Thrown if path is invalid.</exception>
        public Wallpaper(string path, Wallpaper.Style zoomStyle, string title)
        {
            // Check that path param is valid
            if (String.IsNullOrEmpty(path) && File.Exists(path))
            {
                // Check if image file is supported
                if (isImageSupported(System.IO.Path.GetExtension(path)))
                {
                    // Apply path
                    this.Path = path;
                    // Apply zoom style
                    this.ZoomStyle = zoomStyle;
                    // Apply title
                    this.Title = title;
                    // Get image format
                    Image img = Image.FromFile(path);
                    // Apply image format
                    this.Format = img.RawFormat;
                }
                else
                {
                    throw new ImageFormatException("The specified image is not supported!");
                }
            }
            else
            {
                throw new WallpaperFileException("Unable to file access specified path!");
            }
        }

        /// <summary>
        /// The path to the image file.
        /// </summary>
        private string _path;

        /// <summary>
        /// The ZoomStyle of the wallpaper. Defines how the image is zoomed if it
        /// doesn't fit to the screen.
        /// </summary>
        private Wallpaper.Style _zoomStyle;

        /// <summary>
        /// The title of the wallpaper.
        /// </summary>
        private string _title;

        /// <summary>
        /// Stores the format of the wallpaper.
        /// </summary>
        private ImageFormat _format;

        /// <summary>
        /// Gets or sets the path to the image file.
        /// </summary>
        internal string Path
        {
            get
            {
                return this._path;
            }
            private set
            {
                if (String.IsNullOrEmpty(value))
                    return;

                this._path = value;
            }
        }

        /// <summary>
        /// Gets or sets the ZoomStyle of the wallpaper.
        /// </summary>
        internal Wallpaper.Style ZoomStyle
        {
            get
            {
                return this._zoomStyle;
            }
            set
            {
                this._zoomStyle = value;
            }
        }

        /// <summary>
        /// Gets or sets the title of the wallpaper.
        /// </summary>
        internal string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
            }
        }

        /// <summary>
        /// Gets or sets the format of the wallpaper.
        /// </summary>
        internal ImageFormat Format
        {
            get
            {
                return this._format;
            }
            private set
            {
                this._format = value;
            }
        }

        /// <summary>
        /// The styles how a wallpaper can be set if it doesn't fit the screen.
        /// </summary>
        internal enum Style : int
        {
            ///// <summary>
            ///// The selected image is shown as created and it fits the screen.
            ///// </summary>
            //None = 0,
            ///// <summary>
            /// The selected image is stretched until it fits the screen.
            /// </summary>
            Stretched,
            /// <summary>
            /// The image is displayed multiple times until the screen is filled.
            /// </summary>
            Tiled,
            /// <summary>
            /// The single image is displayed. It is positioned in the center of the screen.
            /// </summary>
            Centered
        }

        /// <summary>
        /// Converts the wallpaper to another image format.
        /// </summary>
        /// <param name="newFormat">The new image format.</param>
        /// <returns>The converted wallpaper.</returns>
        internal Wallpaper Convert(ImageFormat newFormat)
        {
            Image imageFile = Image.FromFile(this.Path);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(this.Path);
            string newPath = "cache\\" + fileName;
            imageFile.Save(newPath, newFormat);
            return new Wallpaper(newPath, this.ZoomStyle, this.Title);
        }
    }
}
