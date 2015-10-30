<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CloudApp.Default" %>

<%@ Register Assembly="Infragistics4.Web.v15.1, Version=15.1.20151.1018, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v15.1, Version=15.1.20151.1018, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.DisplayControls" TagPrefix="ig" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <link href="Style/ControlPanelSingle.css" rel="stylesheet" type="text/css" />
            <style>
                .CustomCss {
                    background-color: black !important;
                    color: white !important;
                    /*   font-style: italic;*/
                }
            </style>
            <table align="left" width="100%">
                <tr>
                    <td style="font-size: small;"><strong>Gateway Options: &nbsp;&nbsp;</strong>
                        <asp:RadioButton ID="RadioButton1" runat="server" Text="Auto" TextAlign="left" />
                        <asp:RadioButton ID="RadioButton2" runat="server" Text="E3/NGA" TextAlign="Left" />
                        <asp:RadioButton ID="RadioButton7" runat="server" Text="S3" TextAlign="Left" />
                        <asp:RadioButton ID="RadioButton6" runat="server" Text="ANX" TextAlign="Left" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <strong>IP Address: 159.099.185.100</strong>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <div id="wrapper" class="wgDataBind">
                            <div id="container">
                                <div class="OptionPanel">
                                    <div id="Icon2" style="position: relative; float: left; padding-right: 10px; margin-bottom: 10px;">
                                        <img src="Images/StyleSet.png" alt="Style Set" title="Style Set" width="60" height="61" />
                                    </div>
                                    <div id="DropDown" style="position: relative; float: left; padding-top: 20px; margin-bottom: 10px;">
                                        <%--<asp:Literal runat="server" Text="<%$ Resources:WebDataGrid1, Styling_Theme_SelectATheme %>" />--%>
                                        <asp:DropDownList ID="DDList1" runat="server" AutoPostBack="True" CssClass="CSSform">
                                            <asp:ListItem>Appletini</asp:ListItem>
                                            <asp:ListItem>Caribbean</asp:ListItem>
                                            <asp:ListItem>Claymation</asp:ListItem>
                                            <asp:ListItem Selected="True">Default</asp:ListItem>
                                            <asp:ListItem>ElectricBlue</asp:ListItem>
                                            <asp:ListItem>Harvest</asp:ListItem>
                                            <asp:ListItem>LucidDream</asp:ListItem>
                                            <asp:ListItem>Nautilus</asp:ListItem>
                                            <asp:ListItem>Office2007Black</asp:ListItem>
                                            <asp:ListItem>Office2007Blue</asp:ListItem>
                                            <asp:ListItem>Office2007Silver</asp:ListItem>
                                            <asp:ListItem>Pear</asp:ListItem>
                                            <asp:ListItem>RedPlanet</asp:ListItem>
                                            <asp:ListItem>RubberBlack</asp:ListItem>
                                            <asp:ListItem>Trendy</asp:ListItem>
                                            <asp:ListItem>Windows7</asp:ListItem>
                                            <asp:ListItem>Office2010Blue</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div style="clear: both;"></div>
                                </div>
                                <div id="DataGridWrapper">
                                    <ig:WebDataGrid ID="WebDataGrid1" StyleSetName="Default" runat="server" Height="350px" Width="100%" Font-Names="verdana" Font-Size="X-Small"  OnInitializeRow="WebDataGrid1_InitializeRow">
                                        <%--EnableAjax="true"--%>

                                        <Behaviors>
                                            <ig:Activation />
                                            <ig:Selection CellClickAction="Row" RowSelectType="Multiple" ColumnSelectType="Multiple" />
                                            <ig:RowSelectors RowNumbering="false" />
                                            <%--<ig:Selection CellClickAction="Row" RowSelectType="Multiple"ColumnSelectType="Multiple"/>
                                <ig:EditingCore>
                                    <Behaviors>
                                        <ig:CellEditing>
                                            <EditModeActions MouseClick="Single" />
                                        </ig:CellEditing>
                                    </Behaviors>
                                </ig:EditingCore>--%>
                                        </Behaviors>
                                    </ig:WebDataGrid>
                                </div>
                            </div>
                    </td>
                </tr>
                <tr>
                    <td style="background: gray; font-size: small; font-weight: bold">Select Application Type to be downloaded</td>
                </tr>
                <tr>
                    <td style="padding: 5px; background: #17243a; color: lightblue">
                        <asp:RadioButton ID="RadioButton3" runat="server" Text="Configuration" />
                        <asp:RadioButton ID="RadioButton4" runat="server" Text="Firmware" />
                        <asp:RadioButton ID="RadioButton5" runat="server" Text="Enable LCD" />
                        <br />
                        <asp:CheckBox ID="CheckBox1" runat="server" Text="Send CAMs" />
                        <asp:CheckBox ID="CheckBox2" runat="server" Text="Send Configuration" />
                        <asp:CheckBox ID="CheckBox3" runat="server" Text="Send Device Labels" />
                    </td>
                </tr>
                <tr>
                    <td style="background: gray; font-size: small; font-weight: bold">
                        <table width="100%">
                            <tr>
                                <td style="width: 20%">Download Status</td>
                                <td style="padding-left: 20px; width: 80%">

                                    <ig:WebProgressBar ID="WebProgressBar1" Height="15" runat="server" Value="50" AnimationType="Linear" Width="100%">
                                    </ig:WebProgressBar>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="background: #17243a; color: aqua">

                        <asp:Timer ID="Timer1" Interval="100" OnTick="Timer1_Tick" runat="server" Enabled="false">
                        </asp:Timer>
                        <asp:Label runat="server" ID="statusLabel"></asp:Label>

                    </td>
                </tr>
                <tr>
                    <td align="right" style="padding: 5px">
                        <asp:Button ID="Button2" runat="server" Text="Upload" />
                        <asp:Button ID="btnDownload" runat="server" Text="Download" OnClick="btnDownload_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
