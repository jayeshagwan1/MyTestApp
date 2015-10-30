using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml.XPath;
using System.Diagnostics;
using HTSL.FCI.CAMWorks.Globals;
using HTSL.FCI.CAMWorks.Controller;
using HTSL.FCI.CAMWorks.Context;
using HTSL.FCI.CAMWorks.ExceptionManager;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Net;
using Infragistics.Web.UI.GridControls;

namespace CloudApp
{
    public partial class Default : Page
    {


        CommonMethods objCommonMethods;
        DownloadSettings objDownloadSettings;
        CMWException objCMWException;
        string strProjectDBPath = @"C:\Users\h113429\Documents\CAMWorks2.0_Projects\NetworkSimulatorBase\NetworkSimulatorBase.FCI";//@"C:\Users\e379011\Documents\CAMWorks2.0_Projects\Test Lab Network New\Test Lab Network New.fci";
        //Remoting - Begin      

        public delegate void CrossThreadCall(object e);

        public CallbackClass objCallback = null;

        private delegate void DelegateCommandCompleted();

        private event DelegateCommandCompleted OnCommandCompleted;

        //Remoting - End

        public static IController objIController;
        public CController objCController;
        int intProgressMaxValue = 100;
        int intCallBackState = 0;
        int lstStatusCount;
        private static string currentStatus;
        ArrayList arListStatus = new ArrayList();
        PanelInfo[] objPanels = null;
        private static string strStatus = string.Empty;
        private static double intProgress = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)
            //{
            objCommonMethods = new CommonMethods();
            objCController = new CController();
            objIController = (CController)objCController;

            objCMWException = new CMWException();
            objCommonMethods = new CommonMethods();

            objIController.SetFilePath(strProjectDBPath);
            objPanels = objIController.ReadPanelIds();
            WebDataGrid1.DataSource = GetDataTable();

            if (!Page.IsPostBack)
            {
                WebDataGrid1.DataSource = GetDataTable();
                //this.WebDataGrid1.DataSourceID = "AccessDataSource1";
            }
            else
            {
                WebDataGrid1.DataSource = "";
                //this.WebDataGrid1.DataSourceID = "";
                WebDataGrid1.StyleSetName = this.DDList1.SelectedValue;
                WebDataGrid1.DataSource = GetDataTable();
                //this.WebDataGrid1.DataSourceID = "AccessDataSource1";
            }
            //}
            // WebProgressBar1.Value = intProgress;
            if (intProgress <= 100)
                WebProgressBar1.Value = intProgress;

        }

        private DataTable GetDataTable()
        {
            string[] columns = 
            {
                " Node"," Type"," System Label"," Description"," Validation"," Last Modified"," Last Downloaded"," Last CAM D/L"," Last Label/Text"
            };



            DataTable dataTable = new DataTable();
            foreach (string name in columns)
            {
                dataTable.Columns.Add(name);
            }

            for (int i = 0; i < objPanels.Length; i++)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow[0] = objPanels[i].NodeID;
                dataRow[1] = objPanels[i].NodeTypeID.ToString();
                dataRow[2] = objPanels[i].SystemLabel;
                dataRow[3] = objPanels[i].Description;
                dataRow[4] = objPanels[i].IsValidatedAfterModification;
                dataRow[5] = DateTime.Now.ToShortDateString();
                dataRow[6] = DateTime.Now.ToShortDateString();
                dataRow[7] = DateTime.Now.ToShortDateString();
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        #region Command Completed

        /// <summary>

        /// Invoked when the download completed either Successfully/Failure

        /// </summary>

        private void Query_OnCommandCompleted()
        {

            bool blnNoError = true;

            bool IsNGAImageDownloadRequired;

            DistributorInformation objDistributorInformation;

            IsNGAImageDownloadRequired = false;

            try
            {

                objIController.UnregisterRemotingChannel();

                Timer1.Enabled = false;


                //if (IsDownloadSuucessful && IsFCPCFGUpload == false && (rdoConfiguration.Checked || rdoFirmware.Checked || radLCDSLP.Checked))
                //{
                for (int lp = 1; lp <= Constants.DELAY_AFTER_DOWNLOAD; lp++)
                    Thread.Sleep(new TimeSpan(0, 0, 1));



                // }

                //Update the downloaded timestamp

                //if (rdoConfiguration.Checked && IsDownloadSuucessful == true &&

                //    chkCAMOnly.Checked == false && IsFCPCFGUpload == false)
                //if (rdoConfiguration.Checked && IsDownloadSuucessful == true &&

                //IsFCPCFGUpload == false)//Removed the condition that chkCAMOnly=false to show the Downloaded by Ranjith on 31/10/2008.
                //{

                //    PanelInfo[] objCurrentPanel;



                //    objCurrentPanel = new PanelInfo[1];



                //for (int nodeCnt = 0; nodeCnt < objPanelInfo.Length; nodeCnt++)
                //{

                //    if (objPanelInfo[nodeCnt].NodeID.ToString() == strNodeNo)
                //    {

                //        objCurrentPanel[0] = objPanelInfo[nodeCnt];
                //        //newly added by Ranjith to update the last downloaded date for the CAM,Label,Configuration,ScreenSaver and Logo on Dec12,2008.
                //        if (rdoNGALogo.Checked)
                //        {
                //            if (chkLogoReposition.Visible && chkLogoReposition.Checked)
                //            {
                //                objCurrentPanel[0].LastLogoDownloaded = DateTime.Now;
                //            }
                //        }
                //        else if (rdoNGAScreenSaver.Checked)
                //        {
                //            objCurrentPanel[0].LastScreenSaverDownLoaded = DateTime.Now;
                //        }
                //        if (!chkCAMOnly.Checked && !(chkSLCLabels.Checked || chkTextMessage.Checked) &&
                //            !chkConfigurationOnly.Checked)
                //        {
                //            objCurrentPanel[0].LastConfigDownLoaded = DateTime.Now;
                //            objCurrentPanel[0].LastCAMDownloaded = DateTime.Now;
                //            objCurrentPanel[0].LastLabelDownloaded = DateTime.Now;
                //        }
                //        else if (objCurrentPanel[0].NodeTypeID == NodeType.INI_CC || objCurrentPanel[0].NodeTypeID == NodeType.INI_VG || objCurrentPanel[0].NodeTypeID == NodeType.INI_VGE)
                //        {
                //            objCurrentPanel[0].LastConfigDownLoaded = DateTime.Now;
                //            objCurrentPanel[0].LastCAMDownloaded = DateTime.Now;
                //            objCurrentPanel[0].LastLabelDownloaded = DateTime.Now;
                //        }
                //        if (chkCAMOnly.Checked)
                //        {
                //            objCurrentPanel[0].LastCAMDownloaded = DateTime.Now;
                //        }
                //        if (chkConfigurationOnly.Checked)
                //        {
                //            objCurrentPanel[0].LastConfigDownLoaded = DateTime.Now;
                //        }
                //        if (chkSLCLabels.Checked || chkTextMessage.Checked)
                //        {
                //            if (chkSLCLabels.Checked && chkSLCLabels.Visible)
                //                objCurrentPanel[0].LastLabelDownloaded = DateTime.Now;
                //            else if (chkTextMessage.Checked && chkTextMessage.Visible)
                //                objCurrentPanel[0].LastLabelDownloaded = DateTime.Now;
                //        }
                //        //
                //        objCurrentPanel[0].LastDownloaded = DateTime.Now;

                //        objCurrentPanel[0].IsDownloadedAfterModification = true;

                //        blnNoError = objIController.SavePanelInfo(objCurrentPanel);



                //        break;

                //    }

                //}



                //objCurrentPanel = null;
                //if (!blnNoError)
                //{
                //    string strErrMsg = objCMWException.LastException;
                //    if (strErrMsg != null)
                //    {
                //        Shared.showErrorMessage(strErrMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    }
                //}
                //else
                //{
                //    LoadNodeList();
                //    if (intSelectedNodeIndex >= 0)
                //    {
                //        dgNode.Rows[intSelectedNodeIndex].Cells[(int)ColumnIndex.Type].Selected = true;
                //    }
                //}
                //}
                //else
                //{
                //    if (rdoNGAScreenSaver.Checked || rdoNGALogo.Checked)
                //    {
                //        //To update the download date on Dec11,2008
                //        PanelInfo[] objCurrentPanel;
                //        objCurrentPanel = new PanelInfo[1];
                //        for (int nodeCnt = 0; nodeCnt < objPanelInfo.Length; nodeCnt++)
                //        {
                //            if (objPanelInfo[nodeCnt].NodeID.ToString() == strNodeNo)
                //            {
                //                objCurrentPanel[0] = objPanelInfo[nodeCnt];
                //                //newly added by Ranjith to update the last downloaded date for the CAM,Label,Configuration,ScreenSaver and Logo on Dec12,2008.
                //                if (objCurrentPanel[0].NodeTypeID == NodeType.NGA)
                //                {
                //                    if (rdoNGALogo.Checked)
                //                    {
                //                        if (chkLogoReposition.Visible && chkLogoReposition.Checked)
                //                        {
                //                            objCurrentPanel[0].LastLogoDownloaded = DateTime.Now;
                //                        }
                //                    }
                //                    else if (rdoNGAScreenSaver.Checked)
                //                    {
                //                        objCurrentPanel[0].LastScreenSaverDownLoaded = DateTime.Now;
                //                    }
                //                    //
                //                    blnNoError = objIController.SavePanelInfo(objCurrentPanel);
                //                }
                //                break;
                //            }
                //        }
                //    }
                //    if (!blnNoError)
                //    {
                //        string strErrMsg = objCMWException.LastException;
                //        if (strErrMsg != null)
                //        {
                //            Shared.showErrorMessage(strErrMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        }
                //    }
                //    else
                //    {
                //        LoadNodeList();
                //        if (intSelectedNodeIndex >= 0)
                //        {
                //            dgNode.Rows[intSelectedNodeIndex].Cells[(int)ColumnIndex.Type].Selected = true;
                //        }
                //    }
                //    //
                //}









                //this.Cursor = Cursors.Default;

                //dgNode.Cursor = Cursors.Default;



                ////10-1202007 Krishna commented.

                ////if (Shared.udtNetworkType == NetworkType.STANDALONE_7100)

                ////{

                ////    btnFCPUpload.Enabled = true;

                ////}





                //panelCommConfig.Enabled = true;

                //panelDownloadSettings.Enabled = true;

                //PnlNodeList.Enabled = true;

                //ultraPnlBottomButton.Enabled = true;





                //prompt for NGA graphics download in case of NGA firmware downloads

                //if (IsDownloadSuucessful == true && rdoFirmware.Checked &&

                //    udtSelectedNodeType == NodeType.NGA)
                //{



                //    string strMsg = objCMWException.GetExceptions("UI259", "");

                //    DialogResult result = Shared.showErrorMessage(strMsg, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);



                //    if (result == DialogResult.OK)
                //    {

                //        IsNGAImageDownloadRequired = true;



                //        //rdoNGAGraphics.Visible = true;

                //        //rdoNGALogo.Visible = true;



                //        rdoNGAGraphics.Checked = true;
                //        if (Shared.flagCommandLine)
                //            delegateFreezeUIForDownload(true);
                //        else
                //            delegateFreezeUIForDownload(false);

                //        return;

                //    }

                //}

                //if (IsDownloadSuucessful == true && rdoFirmware.Checked && radLCDSLP.Checked && radLCDSLPImage.Checked && 

                //    (udtSelectedNodeType == NodeType.SLP_SS || udtSelectedNodeType == NodeType.SLP_Apollo))
                //{



                //    string strMsg = objCMWException.GetExceptions("UI259", "");

                //    DialogResult result = Shared.showErrorMessage(strMsg, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);



                //    if (result == DialogResult.OK)
                //    {

                //        IsNGAImageDownloadRequired = true;



                //        //rdoNGAGraphics.Visible = true;

                //        //rdoNGALogo.Visible = true;



                //        rdoNGAGraphics.Checked = true;
                //        if (Shared.flagCommandLine)
                //            delegateFreezeUIForDownload(true);
                //        else
                //            delegateFreezeUIForDownload(false);

                //        return;

                //    }

                //}
                //    if (!IsNGAImageDownloadRequired)
                //    {

                //        if (IsDownloadSuucessful == true)
                //        {
                //            //Update config settings in registry
                //            //Update Commport

                //            objIController.SetRegistrySettings(Shared.RegKeyCOMM, cboComm.SelectedIndex.ToString());



                //            if (IsFCPCFGUpload == false)
                //            {

                //                //Update Gateway

                //                if (rdo7100Gateway.Checked)

                //                    objIController.SetRegistrySettings(Shared.RegKeyGateWay, "1"); //7100

                //                else if (rdoE3Gateway.Checked)

                //                    objIController.SetRegistrySettings(Shared.RegKeyGateWay, "2"); //E3/NGA

                //                else

                //                    objIController.SetRegistrySettings(Shared.RegKeyGateWay, "0"); //AUTO



                //                //Save the Logo path

                //                if (rdoNGALogo.Checked && txtAppnFIlePath.Text.Length > 0)
                //                {

                //                    objDistributorInformation = new DistributorInformation();

                //                    objDistributorInformation.NGALogo = txtAppnFIlePath.Text;

                //                    objIController.SetDistributorInformation(objDistributorInformation);



                //                    objDistributorInformation = null;

                //                }
                //                if (Shared.flagCommandLine)
                //                    delegateFreezeUIForDownload(true);
                //                else
                //                    delegateFreezeUIForDownload(false);

                //                //"Download completed successfully."
                //                if (lstStatus.Items.Count > 1)//to avoid error before commmunicating to panel itself on displaying completed successfully message.
                //                    Shared.showErrorMessage(objCMWException.GetExceptions("DW125", ""), MessageBoxButtons.OK, MessageBoxIcon.Information);
                //                else
                //                    Shared.showErrorMessage(objCMWException.GetExceptions("UI523", ""), MessageBoxButtons.OK, MessageBoxIcon.Information);//Failed to connect CAMWorks Downloader service.  Please try once again (OR) Please exit CAMWorks and restart the program.

                //            }

                //            else
                //            {

                //                //ReleaseAllObjects();
                //                if (Shared.flagCommandLine)
                //                    delegateFreezeUIForDownload(true);
                //                else
                //                    delegateFreezeUIForDownload(false);

                //                Shared.delegateStatusBarUpdate(true, 0, false, false);

                //                Shared.delegateStatusMessageUpdate("Ready");

                //                Shared.IsDownloadInProgress = false;

                //                Application.DoEvents();



                //                UploadSummary objUploadSummary;

                //                objUploadSummary = new UploadSummary(objIController, delegateCancelClicked, IsGlobalCommunication,

                //                            delegateFreezeUIForDownload, delegateChangeToolStrip, delegateLoadScreenFromOtherUI);

                //                //this.Controls.Owner.Controls[0].Dispose();

                //                this.Controls.Owner.Controls.Clear();

                //                objUploadSummary.Dock = DockStyle.Fill;



                //                this.Controls.Owner.Controls.Add(objUploadSummary);

                //            }

                //        }

                //        else
                //        {
                //            if (Shared.flagCommandLine)
                //                delegateFreezeUIForDownload(true);
                //            else
                //                delegateFreezeUIForDownload(false);

                //            //"Download failed.\nPlease see the download status for more details."

                //            Shared.showErrorMessage(objCMWException.GetExceptions("DW126", ""), MessageBoxButtons.OK, MessageBoxIcon.Error);

                //        }

                //    }

            }

            catch (Exception errCmdComplete)
            {

                //Shared.showErrorMessage(errCmdComplete.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //if (Shared.flagCommandLine)
                //    Application.Exit();

            }

            //finally
            //{



            //    Shared.delegateStatusBarUpdate(true, 0, false, false);

            //    Shared.delegateStatusMessageUpdate("Ready");

            //    EnableDisableGateway();
            //    //intSelectedNodeIndex = -1;


            //    Shared.IsDownloadInProgress = false;



            //    if (IsNGAImageDownloadRequired)

            //        DoApplicationDownloads("&Download");

            //    if (dgNode != null)
            //    {
            //        if (dgNode.Rows.Count > 0)
            //        {
            //            dgNode.PerformLayout();
            //        }
            //    }
            //    if (Shared.flagCommandLine)
            //        Application.Exit();
        }

        #endregion

        #region Progress Change Events

        /// <summary>

        /// This method will be fired thru the delegate from the service.

        /// </summary>

        /// <param name="sender"></param>

        /// <param name="e"></param>

        /// <returns></returns>

        public bool Status_Changed(string sender, object e)
        {

            // fire&forget

            ThreadPool.QueueUserWorkItem(new WaitCallback(ShowProgress), e);

            return true;

        }



        /// <summary>

        /// check to see if any cross thread issue arised.

        /// cross-thread issue will come up since the windows controls are not thread safe.

        /// </summary>

        /// <param name="e"></param>

        private void ShowProgress(object e)
        {
            UpdateDownloadStatus(e);
        }



        /// <summary>

        /// This methods will get fired thru the call back.

        /// Check to see whether the call back is completed otherwise the status will

        /// be updated.

        /// </summary>

        /// <param name="e"></param>

        private void UpdateDownloadStatus(object e)
        {
            CallbackEventArgs objCallBckArgs = new CallbackEventArgs();
            objCallBckArgs = e as CallbackEventArgs;


            try
            {

                // a simple work with the parent's control

                lock (this)
                {

                    if (objCallBckArgs.CommandId != CommandName.NetSOLOQuery)
                    {

                        if (objCallBckArgs.ProgressMaxValue != intProgressMaxValue || objCallBckArgs.ProgressMaxChanged)
                        {

                            intProgressMaxValue = objCallBckArgs.ProgressMaxValue;

                            // Shared.delegateStatusBarUpdate(true, intProgressMaxValue, false, true);

                        }

                        if (objCallBckArgs.Finished == false)
                        {

                            if (objCallBckArgs.UpdateStatus)
                            {

                                if (objCallBckArgs.StatusChanged)
                                {

                                    arListStatus.Add(objCallBckArgs.Message);

                                    intCallBackState = (int)objCallBckArgs.State;

                                    //lstStatus.SelectedIndex = lstStatus.Items.Count - 1;

                                    // arListStatus.ClearClearSelection();



                                    if (objCallBckArgs.IsError)
                                    {

                                        // IsDownloadSuucessful = false;

                                    }

                                }

                                else
                                {



                                    lstStatusCount = arListStatus.Count - 1;

                                    currentStatus = arListStatus[lstStatusCount].ToString();

                                    if (!objCallBckArgs.IsError)
                                    {

                                        currentStatus = currentStatus + "Done.";
                                        strStatus = currentStatus;
                                        Application.Add("currentStatus", currentStatus);
                                        // lblStatus.Text = currentStatus;
                                        //lstStatus.Items.Add(currentStatus);

                                        // Page.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);

                                    }

                                    else
                                    {

                                        currentStatus = currentStatus + "Failed.";
                                        strStatus = currentStatus;
                                        Application.Add("currentStatus", currentStatus);
                                        //lstStatus.Items.Add(currentStatus);
                                        // lblStatus.Text = currentStatus;
                                        //lstStatus.Items[lstStatusCount].Text = currentStatus;
                                        //Page.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);



                                        if (objCallBckArgs.Message.IndexOf("\n") > -1)
                                        {

                                            string[] arrMsg = objCallBckArgs.Message.Split('\n');



                                            for (int errCnt = 0; errCnt < arrMsg.Length; errCnt++)
                                            {

                                                arListStatus.Add(arrMsg[errCnt]);
                                                arListStatus.Add(currentStatus);

                                            }

                                        }

                                        else

                                            arListStatus.Add(objCallBckArgs.Message);
                                        arListStatus.Add(currentStatus);

                                        //  IsDownloadSuucessful = false;

                                    }





                                }

                            }



                            //if ((int)objCallBckArgs.State == 909090 && chkShowCAMUsage.Checked)
                            //{

                            //    DisplayMemoryUsage(objCallBckArgs.Message);

                            //}

                            //progressBarStatus.PerformStep();

                            // Shared.delegateStatusBarUpdate(false, intProgressMaxValue, true, true);

                        }

                        else
                        {

                            if (objCallBckArgs.Message.Length != 0)
                            {

                                arListStatus.Add(objCallBckArgs.Message);
                                arListStatus.Add(currentStatus);

                                //lstStatus.SelectedIndex = lstStatus.Items.Count - 1;

                                // lstStatus.ClearSelection();

                            }

                            OnCommandCompleted();

                        }



                    }

                }



            }

            catch (Exception errCallBack)
            {

                //Shared.showErrorMessage(errCallBack.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);

                OnCommandCompleted();

            }

            finally
            {

                objCallBckArgs = null;

            }



        }

        #endregion

        protected void btnDownload_Click(object sender, System.EventArgs e)
        {
            Timer1.Enabled = true;

            //Label1.Text = "Refreshed at " +
            //DateTime.Now.ToString();
            bool blnServiceStarted;
            bool blnNoError;
            try
            {
                objCommonMethods = new CommonMethods();
                objCController = new CController();
                objIController = (CController)objCController;

                objCMWException = new CMWException();
                objCommonMethods = new CommonMethods();

                objIController.SetFilePath(strProjectDBPath);
                Shared.udtNetworkType = objIController.GetNetworkType(strProjectDBPath);
                ResgistrySettings RegSettings;
                RegSettings = objIController.GetRegistrySettings();

                Shared.ProjectDBPath = strProjectDBPath;
                Shared.ApplicationPath = RegSettings.InstallationPath;
                objDownloadSettings = new DownloadSettings();

                //start downloader Service
                blnServiceStarted = objCommonMethods.StartDownloaderService();
                //Communication Type
                objDownloadSettings.TCP = true;
                objDownloadSettings.ipAddress = "159.99.185.100";
                //Port number
                objDownloadSettings.portNumber = Constants.portNumber;
                //Gateway type
                objDownloadSettings.enumDownloadGateway = GateWay.E3NGA;
                //NetworkType
                objDownloadSettings.enumNetworkType = Shared.udtNetworkType;
                //Node type
                objDownloadSettings.enumBoardTypeID = BoardTypeID.ILI_S_E3;
                //Node Number
                objDownloadSettings.NodeNumber = 2;
                //Firmware path
                objDownloadSettings.FirmwarePath = "";
                //7100 network card
                objDownloadSettings.Do7100CardDownload = false;
                //RPT card
                objDownloadSettings.DoRPTDownload = false;
                //LCD-SLP load
                objDownloadSettings.DoLCDSLPDownload = false;
                //LCD-SLP Graphics
                objDownloadSettings.DoLCDSLPGraphicsDownload = false;
                //Project Path
                objDownloadSettings.ProjectPath = Shared.ProjectDBPath;
                //Level 4 Password
                objDownloadSettings.L4Password = "555555";
                //Download Type
                objDownloadSettings.DownloadTypes = (int)DownloadTypes.CAMOnly | (int)DownloadTypes.ConfigOnly | (int)DownloadTypes.LabelsOnly;
                //Installer ID
                objDownloadSettings.InstallerID = "1234";
                //Virtual Switch
                objDownloadSettings.LoadVirtualSwitch = false;
                //Site Specific key
                objDownloadSettings.SiteSpecificKey = 1234;
                //Application path
                objDownloadSettings.ApplicationPath = Shared.ApplicationPath;
                OnCommandCompleted = new DelegateCommandCompleted(Query_OnCommandCompleted);
                objCallback = new CallbackClass();
                objCallback.OnHostToClient += new RemoteCallback(Status_Changed);
                //call back delegate
                RemoteCallback wire = new RemoteCallback(objCallback.HandleToClient);
                //Parameters to be passed between the client and CAMWorksDownloader service.
                CallbackEventArgs objCallBckArgs = new CallbackEventArgs();
                objCallBckArgs.CallBack = wire;
                objCallBckArgs.IsError = false;

                objCallBckArgs.Finished = false;

                objCallBckArgs.CommandId = CommandName.ConfigDownload;

                objCallBckArgs.State = 0;

                objCallBckArgs.Sender = GetType().ToString();

                objCallBckArgs.Parameters = objDownloadSettings;

                blnNoError = objIController.DownloadConfigData(objCallBckArgs);

                if (!blnNoError)
                {

                    arListStatus.Add(objCMWException.GetExceptions(objCMWException.LastException, ""));

                    // IsDownloadSuucessful = false;

                    OnCommandCompleted();

                }

                //objCMWException = null;

                objCallBckArgs = null;

                objDownloadSettings = null;


            }
            catch (Exception Ex)
            {

            }
        }



        protected void Timer1_Tick(object sender, System.EventArgs e)
        {

            if (strStatus != string.Empty)
            {
                intProgress += 20;
                //string updates = Application["currentStatus"] as string;
                //if (updates != null)
                //{
                statusLabel.Text = (strStatus);
                //}               
                strStatus = string.Empty;

            }
        }

        protected void WebDataGrid1_InitializeRow(object sender, RowEventArgs e)
        {
            if (DDList1.SelectedValue == "Default")
            {
                for (int i = 0; i < e.Row.Items.Count; i++)
                {
                    e.Row.Items[i].CssClass = "CustomCss";
                }
            }
        }
    }
}