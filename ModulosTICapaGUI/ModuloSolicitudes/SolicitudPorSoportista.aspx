<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloSolicitudes/MasterSolicitudes.master" AutoEventWireup="true" CodeBehind="SolicitudPorSoportista.aspx.cs" Inherits="ModulosTICapaGUI.ModuloSolicitudes.SolicitudPorSoportista" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadSolicitudes" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainSolicitudes" runat="server">

<legend class="titulos" style="text-align: left">Consultar Solicitudes</legend>
    <fieldset class="fieldsetContornoBlanco">

		<div style="float: right">
			<asp:HyperLink ID="_hlMode" runat="server" NavigateUrl="~/ModuloSolicitudes/ConsultarSolicitudEncargado.aspx" ForeColor="#808080" Font-Underline="true">Por usuarios</asp:HyperLink>
		</div>

        <asp:Table ID="_tContenedor" runat="server" Width="820px">
                    
            <asp:TableRow>
                <asp:TableCell>
                    &nbsp;
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow> 
                <asp:TableCell HorizontalAlign="left">
                    <asp:Label ID="_lblSoportista" runat="server" Text="Seleccione un soportista:"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:DropDownList ID="_ddlSoportista" CssClass="ddlSolicitudes" OnSelectedIndexChanged="ddlSoportistaSelection_Change" runat="server">
                    </asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow HorizontalAlign="left"> 
                <asp:TableCell> 
                    <asp:Label ID="_lblSolicitudes" runat="server" Text="Solicitudes"></asp:Label>
                </asp:TableCell>

                <asp:TableCell> 
                    <asp:Label ID="_lblAvance" runat="server" Text="Avance"></asp:Label>
                </asp:TableCell>

                <asp:TableCell>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow HorizontalAlign="left"> 
                <asp:TableCell> 
                    <asp:UpdatePanel ID="_upSolicitud" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="_ddlSolicitud" CssClass="ddlSolicitudes" runat="server" Visible="true" OnSelectedIndexChanged="ddlSolicitudSelection_Change">
                                <asp:ListItem Selected="True">Seleccionar</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:TableCell>

                <asp:TableCell> 
                    <asp:UpdatePanel ID="_upFechaAvance" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="_ddlFechaAvance" OnSelectedIndexChanged="ddlFechaAvanceSelection_Change" runat="server" Visible="true">
                        <asp:ListItem Selected="True">Seleccionar</asp:ListItem>
                    </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>                            
                </asp:TableCell>

                <asp:TableCell HorizontalAlign="Center">
                </asp:TableCell>
            </asp:TableRow>

                    
                   
            <asp:TableRow HorizontalAlign="Left">
                <asp:TableCell>
                    <asp:UpdatePanel ID="_upDescripcion" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                        <ContentTemplate>
                            <textarea id="_taDescripcion" runat="server" rows="7" cols="40"></textarea>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:UpdatePanel ID="_upAvance" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                        <ContentTemplate>
                            <textarea id="_taAvance" runat="server" rows="7" cols="40"></textarea>
                        </ContentTemplate>
                    </asp:UpdatePanel>                            
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                    &nbsp;
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow HorizontalAlign="Left">
                <asp:TableCell>
                            
                    <asp:UpdatePanel ID="_upEstado" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="_lblEstado" runat="server" Text="Estado:"></asp:Label>
                            &nbsp;
                            &nbsp;
                            &nbsp;
                            &nbsp;
                            <asp:DropDownList ID="_ddlEstado" runat="server">
                            </asp:DropDownList>
                            &nbsp;
                            <asp:Button ID="_btnCambiar" runat="server" Text="Cambiar" OnClick="btnCambiar_Click"/>
                                    
                        </ContentTemplate>
                    </asp:UpdatePanel>   
                </asp:TableCell>
                <asp:TableCell>                  
                    <asp:UpdatePanel ID="_upPostBy" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="_lblMensajePostBy" runat="server" Text="Enviado por:"></asp:Label>
                            &nbsp;
                            &nbsp;
                            &nbsp;
                            &nbsp;                                    
                            <asp:Label ID="_lblPostBy" runat="server" Text=""></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow HorizontalAlign="Left">
                <asp:TableCell>
                    
                </asp:TableCell>
                <asp:TableCell>                    
                    <asp:UpdatePanel ID="_upFechaSolicitud" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="_lblMensajeFechaSolicitud" runat="server" Text="Fecha de registro:"></asp:Label>
                            <asp:Label ID="_lblFechaSolicitud" runat="server"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                    &nbsp;
                </asp:TableCell>
            </asp:TableRow>

        </asp:Table>
	</fieldset>

</asp:Content>
