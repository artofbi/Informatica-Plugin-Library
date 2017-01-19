using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;


namespace InfaBGInfo
{
    public partial class Form1 : Form
    {
        // Constant Variables
        public const string CONFIG_XML_FILE_NAME = "InfaBGInfoConfig.xml";
        public const string WALLPAPER_IMAGE_FILE_NAME = "FyghtSoft_InfaBGInfo.bmp";

        private string currentDirectory;
        private string outputPathForFinalImage;
        private string outputPathForConfigXML;
        private string outputFullFilePathForConfigXML;
        private string outputFullFilePathForFinalImage;

        private InfaWSHMetaDataInfo oInfaWSHMetaData = new InfaWSHMetaDataInfo();
        InfaBGInfo.Resources.AboutBox1 fAbout = new InfaBGInfo.Resources.AboutBox1();

        private bool isOkayToSetWallpaper = false;
        private bool isInfaWSHServerHavingIssue = false;
        private bool isAutoRefreshEnabled = true;
        private bool wasAutoRefreshTimerStartedAtApplicationStart = false;

        //private int alarmCounter = 1;
        //private bool exitFlag = false;
        //private int timeLeft = 0;
        private int timerInterval = 5000;

        public Form1()
        {
            InitializeComponent();

            // set output path for image
            currentDirectory = Directory.GetCurrentDirectory();
            outputPathForFinalImage = Directory.GetCurrentDirectory();
            outputPathForConfigXML = Directory.GetCurrentDirectory();
            outputFullFilePathForConfigXML = outputPathForConfigXML + "\\" + CONFIG_XML_FILE_NAME;
            outputFullFilePathForFinalImage = outputPathForFinalImage + "\\" + WALLPAPER_IMAGE_FILE_NAME;
            // get temp dir
            //System.IO.Path.GetTempPath();


            // Determin Timer and Begin with it.
            if (File.Exists(outputFullFilePathForConfigXML) && isAutoRefreshEnabled)
            {
                XmlDocument ConfigXMLDoc = new XmlDocument();
                ConfigXMLDoc.Load(outputFullFilePathForConfigXML);

                InfaBGInfo.Config.ConfigXML oConfig = new InfaBGInfo.Config.ConfigXML(ConfigXMLDoc);
                
                timerInterval = oConfig.TimerIntervalSeconds * 1000;

                // Timer
                if (oConfig.TimerEnabled)
                {
                    StartAutoRefreshTimerDuringAppUsage();

                    wasAutoRefreshTimerStartedAtApplicationStart = true;
                }
            }
        }

        /// <summary>
        /// Starts the timer properly and manages the Event Handler without creating a 
        /// duplicate EventHandler. Basically if a user click enable timer then apply then this 
        /// method gets hit and starts the timer so the user doesn't have to quit the app.  Also if,
        /// the configuration file reads that the timer was previously set in the app at the start of the 
        /// app, the timer will start by calling this method also. Either was the EventHandler get set just once.
        /// </summary>
        private void StartAutoRefreshTimerDuringAppUsage()
        {
            if (wasAutoRefreshTimerStartedAtApplicationStart)
            {
                // Sets the timer and starts timer ticking
                timer1.Interval = timerInterval;
                timer1.Start();
            }
            else
            {
                timer1.Tick += new EventHandler(TimerEventProcessor);

                // Sets the timer and starts timer ticking
                timer1.Interval = timerInterval;
                timer1.Start();
            }

            // Regardless this now needs to be set to true as even if the user didn't have the app 
            // pre-set to run the refresh, starting it in the application means we don't want to hit
            // the new Event Hanndler again.
            wasAutoRefreshTimerStartedAtApplicationStart = true;
        }

        private void llblFyghtSoft1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set default colors
            btnForegroundColor.BackColor = Color.Black;
            btnBackgroundColor.BackColor = Color.White;
            colorDialogBackground.Color = Color.White;
            colorDialogForeground.Color = Color.Black;
            
            // Set URL links
            LinkLabel.Link link1 = new LinkLabel.Link();
            link1.LinkData = "www.fyghtsoft.com";
            llblFyghtSoft1.Text = "FyghtSoft Website";
            llblFyghtSoft1.Links.Add(link1);

            LinkLabel.Link link2 = new LinkLabel.Link();
            link2.LinkData = "www.fyghtsoft.com/index.php/resources";
            llblSupportA.Text = "Resources and Training";
            llblSupportA.Links.Add(link2);

            LinkLabel.Link link3 = new LinkLabel.Link();
            link3.LinkData = "www.fyghtsoft.com/forums/";
            llblSupportB.Text = "Community Support Forums";
            llblSupportB.Links.Add(link3);


            // Set default image paths
            tbxImageOutputPath.Text = outputPathForFinalImage;
            lblOutputImagePath.Text = outputPathForFinalImage;

            // Set Dimension Layout Info
            CurrentDesktopDimensions oCDD = 
                WallpaperManager.GetDesktopDimensions();
           
            lblDesktopDimensions.Text = oCDD.DesktopWidth.ToString() + " x " 
                + oCDD.DesktopHeight.ToString() + " (pixels)";

            // Default URLs
            lblDefaultWSHDataIntegrationURL.Text = oInfaWSHMetaData.strDefaultURL_DataIntegration;
            lblDefaultWSHMetaDataURL.Text = oInfaWSHMetaData.strDefaultURL_MetaData;

            // set dropdown defaults
            ddWallpaperPosition.SelectedIndex = 0;
            ddFontSize.SelectedIndex = 6;



            if (File.Exists(outputFullFilePathForConfigXML))
            {
                //MessageBox.Show("Configuration File Exists!");

                XmlDocument ConfigXMLDoc = new XmlDocument();
                ConfigXMLDoc.Load(outputFullFilePathForConfigXML);

                InfaBGInfo.Config.ConfigXML oConfig = new InfaBGInfo.Config.ConfigXML(ConfigXMLDoc);
                PopulateFormWithConfigXMLValues(oConfig);
            }
            //else
                //MessageBox.Show("Configuration File Does Not Exist");
        }



        private bool RefreshWallpaperOnTimer()
        {
            if (File.Exists(outputFullFilePathForConfigXML) && isAutoRefreshEnabled)
            {
                XmlDocument ConfigXMLDoc = new XmlDocument();
                ConfigXMLDoc.Load(outputFullFilePathForConfigXML);

                InfaBGInfo.Config.ConfigXML oConfig = new InfaBGInfo.Config.ConfigXML(ConfigXMLDoc);


                // 
                StringBuilder sb = new StringBuilder();

                // 
                textToImage oTextToImage = new textToImage();

                try
                {
                    InfaWSHMetaDataInfo oGo = null;

                    try
                    {
                        // Attempt to connect to WSH
                        if (!isInfaWSHServerHavingIssue)
                        {
                            oGo = new InfaWSHMetaDataInfo(oConfig.WSHServerName, (int)oConfig.WSHServerPort
                                , "", oConfig.DomainName, oConfig.RepositoryName, oConfig.RepositoryUsername, oConfig.RepositoryPassword
                                , oConfig.DomainName, "");
                        }
                    }
                    catch (Exception ex)
                    {
                        isInfaWSHServerHavingIssue = true;

                        MessageBox.Show("InfaBGInfo Process Failure or Server Connectivity Issue:\n" 
                            + ex.Message.ToString()
                            + "\n\n"
                            + "Please correct the issue with the Informatica Server."
                            + "\n\n"
                            + "You must close and re-open the application in order to attempt to re-connect."
                            + "\n\n"
                            + "Timer Enabled: True"
                            );

                        return false;
                    }


                    // provide options as a struct and passed to wallpaper settings
                    ShowOutputInfoOptions oBkgOptions = new ShowOutputInfoOptions();
                    oBkgOptions.showWSHRepositoriesAvailable = oConfig.ShowAvailableWSHRepositories;
                    oBkgOptions.showAllRepositoryFolders = oConfig.ShowAllRepositoryFolders;
                    oBkgOptions.showGlobalRepositoryWorkflowInfo = oConfig.ShowGlobalRepositoryWorkflows;
                    oBkgOptions.showRepositoryAssociatedIntSvc = oConfig.ShowAssociatedIntegrationServices;
                    oBkgOptions.showOSInformation = oConfig.ShowSystemInformationBlock;

                    //...testing..
                    sb.AppendLine("Last Refresh: " + DateTime.Now.ToLongTimeString());

                    // Add compiles values to the output
                    sb.Append(oGo.GatherStringDataValues(oBkgOptions).ToString());

                    if (oBkgOptions.showOSInformation)
                    {
                        WindowsSystemInfo oWSI = new WindowsSystemInfo();
                        sb.Append(oWSI.GetSystemInfoString());
                    }

                    string tmpOutput = sb.ToString();

                    // Build Image with Critical Text Values
                    oTextToImage.SaveImageToFile(tmpOutput
                        , oConfig.BkgColor, oConfig.ForegroundColor
                        , oConfig.FontSize
                        , FontStyle.Regular
                        , outputFullFilePathForFinalImage
                        , WallpaperManager.GetDesktopDimensions()
                        , oConfig.LayerOnImage);
                    oTextToImage = null;

                    tmpOutput = "";
                    sb.Length = 0;

                    //Main Task to Swap wallpaper
                    WallpaperManager.SetWallpaper(outputFullFilePathForFinalImage
                        , (Wallpaper.Style)oConfig.BkgPosition);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Process Failure (Refresh Enabled):\n" + ex.Message.ToString());
                }

            }
            else
                return false;


            return true;
        }



        // This is the method to run when the timer is raised.
        private void TimerEventProcessor(object sender, EventArgs e)
        {
            timer1.Stop();

            if (!isInfaWSHServerHavingIssue)
            {
                RefreshWallpaperOnTimer();

                timer1.Enabled = true;
                timer1.Start();
            }
        }






        private void PopulateFormWithConfigXMLValues(Config.ConfigXML oConfig)
        {
            colorDialogBackground.Color = oConfig.BkgColor;
            colorDialogForeground.Color = oConfig.ForegroundColor;


            // Settings
            tbxWSHServerName.Text = oConfig.WSHServerName;
            tbxWSHServerPort.Text = oConfig.WSHServerPort.ToString();
            tbxDomainName.Text = oConfig.DomainName;
            tbxRepName.Text = oConfig.RepositoryName;
            tbxRepUserName.Text = oConfig.RepositoryUsername;
            tbxRepPassword.Text = oConfig.RepositoryPassword;
            cbxUsesSSL.Checked = oConfig.WSHUsesSSL;

            //Output
            btnBackgroundColor.BackColor = oConfig.BkgColor;
            btnForegroundColor.BackColor = oConfig.ForegroundColor;
            ddFontSize.SelectedItem = oConfig.FontSize;
            ddWallpaperPosition.SelectedItem = oConfig.BkgPosition;
            tbxOutputExistingImage.Text = oConfig.LayerOnImage;

            if (oConfig.BkgCustomOption == 0)
                rbtnBackgrounCustomOptionA.Checked = true;
            else
                rbtnBackgrounCustomOptionB.Checked = true;

            //Advanced
            cbxShowAvailableWSHReps.Checked = oConfig.ShowAvailableWSHRepositories;
            cbxShowAllRepFolders.Checked = oConfig.ShowAllRepositoryFolders;
            cbxShowGlobalRepWorkflows.Checked = oConfig.ShowGlobalRepositoryWorkflows;
            cbxShowAssociatedIntSvc.Checked = oConfig.ShowAssociatedIntegrationServices;
            cbxShowSystemInfoBlock.Checked = oConfig.ShowSystemInformationBlock;

            cbxTimerEnabled.Checked = oConfig.TimerEnabled;
            stopRefreshToolStripMenuItem.Checked = oConfig.TimerEnabled;

            if (oConfig.TimerIntervalSeconds < tbxRefreshRate.Minimum
                || oConfig.TimerIntervalSeconds > tbxRefreshRate.Maximum)
                tbxRefreshRate.Value = tbxRefreshRate.Minimum;
            else
                tbxRefreshRate.Value = oConfig.TimerIntervalSeconds;

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnBrowseExistingImage_Click(object sender, EventArgs e)
        {
            openFileDialogExistingImage.Filter =
                "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF"; //|All files (*.*)|*.*";
               //"All Files (*.*)|*.*|JPEG Files (*.jpg)|*.jpg|GIF files (*.gif)|*.gif|PNG Files (*.png)|*.png|Bitmap Files (*.bmp)|*.bmp";
            openFileDialogExistingImage.InitialDirectory = "C:\\";
            openFileDialogExistingImage.FileName = "";
            openFileDialogExistingImage.Title = "Select an Exiting Image/Wallpaper file!";
            openFileDialogExistingImage.Multiselect = false;
            // Keeps the user from selecting a custom color.
            //openFileDialogExistingImage.AllowFullOpen = false;
            openFileDialogExistingImage.SupportMultiDottedExtensions = false;
            // Allows the user to get help. (The default is false.)
            openFileDialogExistingImage.ShowHelp = false;
            
            // Sets the initial color select to the current text color.
            //MyDialog.Color = textBox1.ForeColor;

            

            if (openFileDialogExistingImage.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialogExistingImage.CheckFileExists)
                    tbxOutputExistingImage.Text = openFileDialogExistingImage.FileName;
                else
                    MessageBox.Show("The file you selected does not exist,\nor you do not have permissions to access it.");
            }

        }


        private void btnBackgroundColor_Click(object sender, EventArgs e)
        {
            // Keeps the user from selecting a custom color.
            colorDialogBackground.AllowFullOpen = false;
            colorDialogBackground.SolidColorOnly = false;

            // Allows the user to get help. (The default is false.)
            colorDialogBackground.ShowHelp = false;

            // Sets the initial color select to the current text color.
            //colorDialogBackground.Color = btnBackgroundColor.BackColor;

            // Update the text box color if the user clicks OK 
            if (colorDialogBackground.ShowDialog() == DialogResult.OK)
                btnBackgroundColor.BackColor = colorDialogBackground.Color;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            saveForm();
            //Form1.ActiveForm.Close();
            Form1.ActiveForm.Hide();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            saveForm();

            //Conduct Swap of Wallpaper
            if(isOkayToSetWallpaper)
                swapDesktopWallpaper();
        }



        private void saveForm()
        {
            // Determine Current Directory
            //string tmpCurrentDir = Directory.GetCurrentDirectory();

            // Determine Radio Buttons Checked
            // Custom Bkg option
            int tmpCustomBkgOption = rbtnBackgrounCustomOptionA.Checked ? 0 : 1;

            // Desktop Section placement
            
            
            // Verify that settings are established
            if (!verifyInput_Settings_Tab())
            {
                MessageBox.Show("Input Error!\nAll fields on the settings tab form must have valid values!");
            }
            else
            {
                try
                {
                    InfaBGInfo.Config.ConfigXML oConfig = new InfaBGInfo.Config.ConfigXML(tbxWSHServerName.Text
                        , int.Parse(tbxWSHServerPort.Value.ToString()), cbxUsesSSL.Checked
                        , tbxDomainName.Text, tbxRepName.Text
                        , tbxRepUserName.Text, tbxRepPassword.Text
                        , tmpCustomBkgOption, colorDialogBackground.Color
                        , ddWallpaperPosition.SelectedIndex
                        , @tbxOutputExistingImage.Text, colorDialogForeground.Color
                        , int.Parse(ddFontSize.SelectedItem.ToString()), 0
                        , cbxShowAvailableWSHReps.Checked, cbxShowAllRepFolders.Checked, cbxShowGlobalRepWorkflows.Checked
                        , cbxShowAssociatedIntSvc.Checked, cbxShowSystemInfoBlock.Checked
                        , cbxTimerEnabled.Checked, int.Parse(tbxRefreshRate.Value.ToString()))
                        ;

                    oConfig.outputXMLToFile(@currentDirectory + "\\" + CONFIG_XML_FILE_NAME);

                    isOkayToSetWallpaper = true;

                    // This must be reset in case the timer was enable, the form was opened and the 
                    // checkbox to disable the refresh was selected and OK/Apply are clicked.
                    isAutoRefreshEnabled = cbxTimerEnabled.Checked;

                    if (isAutoRefreshEnabled)
                    {
                        timerInterval = int.Parse(tbxRefreshRate.Value.ToString()) * 1000;
                        StartAutoRefreshTimerDuringAppUsage();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private bool swapDesktopWallpaper()
        {
            // 
            StringBuilder sb = new StringBuilder();

            // 
            textToImage oTextToImage = new textToImage();

            try
            {
                // Attempt to connect to WSH
                InfaWSHMetaDataInfo oGo = null;
                try
                {
                    oGo = new InfaWSHMetaDataInfo(tbxWSHServerName.Text, (int)tbxWSHServerPort.Value
                        , "", tbxDomainName.Text, tbxRepName.Text, tbxRepUserName.Text, tbxRepPassword.Text
                        , tbxDomainName.Text, "");
                }
                catch (Exception ex)
                {
                    isInfaWSHServerHavingIssue = true;

                    MessageBox.Show("InfaBGInfo Process Failure or Server Connectivity Issue:\n"
                        + ex.Message.ToString()
                        + "\n\n"
                        + "Please correct the issue with the Informatica Server."
                        + "\n\n"
                        + "You may need to close and re-open the application in order to attempt to re-connect."
                        );

                    return false;
                }

                // provide options as a struct and passed to wallpaper settings
                ShowOutputInfoOptions oBkgOptions = new ShowOutputInfoOptions();
                oBkgOptions.showWSHRepositoriesAvailable = cbxShowAvailableWSHReps.Checked;
                oBkgOptions.showAllRepositoryFolders = cbxShowAllRepFolders.Checked;              
                oBkgOptions.showGlobalRepositoryWorkflowInfo = cbxShowGlobalRepWorkflows.Checked;
                oBkgOptions.showRepositoryAssociatedIntSvc = cbxShowAssociatedIntSvc.Checked;
                oBkgOptions.showOSInformation = cbxShowSystemInfoBlock.Checked;

                //...testing...
                //sb.AppendLine("Last Refresh: " + DateTime.Now.ToLongTimeString());

                // Add compiles values to the output
                sb.Append(oGo.GatherStringDataValues(oBkgOptions).ToString());

                if (oBkgOptions.showOSInformation)
                {
                    WindowsSystemInfo oWSI = new WindowsSystemInfo();
                    sb.Append(oWSI.GetSystemInfoString());
                }

                string tmpOutput = sb.ToString();

                // Build Image with Critical Text Values
                oTextToImage.SaveImageToFile(tmpOutput
                    , colorDialogBackground.Color, colorDialogForeground.Color
                    , int.Parse(ddFontSize.SelectedItem.ToString())
                    , FontStyle.Regular
                    , outputFullFilePathForFinalImage
                    , WallpaperManager.GetDesktopDimensions()
                    , tbxOutputExistingImage.Text);
                oTextToImage = null;

                tmpOutput = "";
                sb.Length = 0;

                //Main Task to Swap wallpaper
                WallpaperManager.SetWallpaper(outputFullFilePathForFinalImage
                    , (Wallpaper.Style)ddWallpaperPosition.SelectedIndex);


                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Process Failure:\n" + ex.Message.ToString());
                return false;
            }
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            Form1.ActiveForm.Close();
        }

        private void btnForegroundColor_Click(object sender, EventArgs e)
        {
            // Keeps the user from selecting a custom color.
            colorDialogForeground.AllowFullOpen = false;
            colorDialogForeground.SolidColorOnly = false;

            // Allows the user to get help. (The default is false.)
            colorDialogForeground.ShowHelp = false;

            // Sets the initial color select to the current text color.
            //colorDialogForeground.Color = btnForegroundColor.BackColor;

            // Update the text box color if the user clicks OK 
            if (colorDialogForeground.ShowDialog() == DialogResult.OK)
                btnForegroundColor.BackColor = colorDialogForeground.Color;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void btnVerifyWSH_Click(object sender, EventArgs e)
        {
            if (!verifyInput_Settings_Tab())
            {
                MessageBox.Show("Problem:\nAll fields on the settings for must have valid values");
            }
            else
            {
                tabControl1.SelectTab(tabWSHStatus);
            }
        }

        private bool verifyInput_Settings_Tab()
        {
            if (String.IsNullOrEmpty(tbxWSHServerName.Text)
                || String.IsNullOrEmpty(tbxWSHServerPort.Value.ToString())
                || String.IsNullOrEmpty(tbxDomainName.Text.ToString())
                || String.IsNullOrEmpty(tbxRepName.Text)
                || String.IsNullOrEmpty(tbxRepUserName.Text)
                || String.IsNullOrEmpty(tbxRepPassword.Text)
               )
            {
                return false;
            }
            else
                return true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1.ActiveForm.Close();
        }

        private void supportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(tabSupport);
        }

        private void informaticaBGInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewAboutBox();
        }

        private void ViewAboutBox()
        {
            if(!fAbout.Modal)
                fAbout.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void stopRefreshToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (stopRefreshToolStripMenuItem.Checked)
            {
                isAutoRefreshEnabled = false;
                cbxTimerEnabled.Checked = false;
            }
            else
            {
                isAutoRefreshEnabled = true;
                cbxTimerEnabled.Checked = true;
            }
        }

        private void aboutInformaticaBGInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewAboutBox();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
                Hide();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void llblSupportA_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void llblSupportB_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void stopRefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void cbxTimerEnabled_CheckedChanged(object sender, EventArgs e)
        {
            stopRefreshToolStripMenuItem.Checked = !cbxTimerEnabled.Checked;
            isAutoRefreshEnabled = cbxTimerEnabled.Checked;
        }

    }
}
