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
using System.Collections.Generic;

using System.Text;
using Microsoft.Win32;
using System.Runtime.InteropServices;


namespace InfaBGInfo
{
    class SafeNativeMethods
    {
        private SafeNativeMethods()
        {
        }

        /// <summary>
        /// Gets or sets hardware and configuration info from the system.
        /// </summary>
        /// <param name="uAction">The action that should be executed.</param>
        /// <param name="uParam">The data that should be set. Depends on the action.</param>
        /// <param name="lpvParam">The data that should be set. Depends on the action.</param>
        /// <param name="fuWinIni">Specifies how changes are saved/broadcasted.</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
    }
}
