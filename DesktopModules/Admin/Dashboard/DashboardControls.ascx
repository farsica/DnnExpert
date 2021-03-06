<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Control Inherits="DotNetNuke.Modules.Admin.Dashboard.DashboardControls" Language="C#" AutoEventWireup="false" CodeFile="DashboardControls.ascx.cs" %>
<div class="dnnDashboardControls">
    <asp:datagrid id="grdDashboardControls" AutoGenerateColumns="false" width="100%" CellPadding="0" GridLines="None" cssclass="dnnGrid" Runat="server">
        <headerstyle cssclass="dnnGridHeader" verticalalign="Top" />
	    <itemstyle cssclass="dnnGridItem" />
	    <alternatingitemstyle cssclass="dnnGridAltItem" />
	    <edititemstyle cssclass="dnnFormInput" />
	    <selecteditemstyle cssclass="dnnFormError" />
	    <footerstyle cssclass="dnnGridFooter" />
	    <pagerstyle cssclass="dnnGridPager" />
	    <columns>
		    <dnn:imagecommandcolumn CommandName="Delete" Text="Delete" IconKey="Delete" HeaderText="Del" KeyField="DashboardControlID" />
		    <dnn:imagecommandcolumn commandname="MoveDown" IconKey="Dn" headertext="Dn" keyfield="DashboardControlID" />
		    <dnn:imagecommandcolumn commandname="MoveUp" IconKey="Up" headertext="Up" keyfield="DashboardControlID" />
		    <dnn:textcolumn DataField="DashboardControlKey" HeaderText="DashboardControlKey" Width="100px" />
		    <dnn:textcolumn DataField="DashboardControlSrc" HeaderText="DashboardControlSrc" Width="500px" />
		    <dnn:checkboxcolumn DataField="IsEnabled" HeaderText="IsEnabled" AutoPostBack="True" />
	    </columns>
    </asp:datagrid>
    <ul class="dnnActions dnnClear">
        <li><asp:LinkButton id="cmdUpdate" CssClass="dnnPrimaryAction" resourcekey="cmdApply" runat="server" /></li>
    	<li><asp:HyperLink id="cmdInstall" CssClass="dnnSecondaryAction" resourcekey="cmdInstall" runat="server" /></li>
        <li><asp:LinkButton id="cmdRefresh" CssClass="dnnSecondaryAction" resourcekey="cmdRefresh" runat="server" /></li>
        <li><asp:HyperLink id="cmdCancel" CssClass="dnnSecondaryAction" resourcekey="cmdCancel" runat="server" /></li>
    </ul>
</div>