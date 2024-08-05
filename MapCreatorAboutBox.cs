/**************************************************************************************************************************
* Copyright 2024, Peter R. Nelson
*
* This file is part of the MapCreator application. The MapCreator application is intended
* for creating fantasy maps for gaming and world building.
*
* MapCreator is free software: you can redistribute it and/or modify it under the terms
* of the GNU General Public License as published by the Free Software Foundation,
* either version 3 of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
* without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
* See the GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License along with this program.
* The text of the GNU General Public License (GPL) is found in the LICENSE file.
* If the LICENSE file is not present or the text of the GNU GPL is not present in the LICENSE file,
* see https://www.gnu.org/licenses/.
*
* For questions about the MapCreator application or about licensing, please email
* contact@brookmonte.com
*
***************************************************************************************************************************/
using System.Reflection;

namespace MapCreator
{
    partial class MapCreatorAboutBox : Form
    {
        public MapCreatorAboutBox()
        {
            InitializeComponent();

            string? version = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

            VersionLabel.BringToFront();
            VersionLabel.ForeColor = Color.Black;

            if (!string.IsNullOrEmpty(version))
            {
                VersionLabel.Text = string.Concat("Version ", version.AsSpan(0, version.IndexOf('+')));
            }
            else
            {
                VersionLabel.Text = "";
            }
        }

        #region Assembly Attribute Accessors

        public static string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                if (Assembly.GetExecutingAssembly() != null
                    && Assembly.GetExecutingAssembly().GetName() != null
                    && Assembly.GetExecutingAssembly().GetName().Version != null)
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    return Assembly.GetExecutingAssembly().GetName().Version.ToString();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }
                else
                {
                    return "Unknown";
                }

            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion
    }
}
