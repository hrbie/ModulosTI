<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloSolicitudes/MasterSolicitudes.master" AutoEventWireup="true" CodeBehind="CrearSolicitud.aspx.cs" Inherits="ModulosTICapaGUI.ModuloSolicitudes.CrearSolicitud" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadSolicitudes" runat="server">
    <title>Crear Solicitud</title>
    <style type="text/css">
        .style1
        {
            width: 195px;
        }
        .style2
        {
            width: 205px;
        }
    </style>

    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainSolicitudes" runat="server">
<legend class="titulos" style="text-align: left" >Crear Solicitud</legend>
<fieldset class="fieldsetContornoBlanco">
 <form action="CrearSolicitud.aspx" method="post" enctype="multipart/form-data">  				   
                    <asp:Table ID="_tContenedor1" runat="server" Width="720px" Height="300px">
                     
                        <asp:TableRow>
                            <asp:TableCell>
                                &nbsp;
                            </asp:TableCell>
                        </asp:TableRow>

                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right">
                                <asp:Label ID="_lblAsunto" runat="server" Text="Asunto"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="_txtAsunto" Columns="60" runat="server"></asp:TextBox>
                                &nbsp;
                                <asp:RequiredFieldValidator runat="server" id="_rfvAsunto" CssClass="textoError" ControlToValidate="_txtAsunto" ErrorMessage = "Capo requerido" display="Dynamic" ValidationGroup="crear_solicitud"/>

                            </asp:TableCell>
                        </asp:TableRow>
                        
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right">
                                <asp:Label ID="_lblSolicitante" runat="server" Text="Solicitante"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center">
                                <asp:TextBox ID="_txtSolicitante" Columns="52" runat="server"></asp:TextBox>
                                &nbsp;
                                <asp:Label ID="_lblLogin" runat="server" Text="(login)"></asp:Label>
                                &nbsp;
                                <asp:RequiredFieldValidator runat="server" id="_rfvSolicitante" CssClass="textoError" ControlToValidate="_txtSolicitante" ErrorMessage = "Campo requerido" display="Dynamic" ValidationGroup="crear_solicitud"/>

                                <asp:RegularExpressionValidator runat="server" id="_revLogin" CssClass="textoError" ControlToValidate="_txtSolicitante" ValidationExpression="^[a-zA-Z]+$" ErrorMessage = "No se permiten números" Display="Dynamic" ValidationGroup="crear_solicitud"/>

                            </asp:TableCell>
                        </asp:TableRow>

                        <asp:TableRow>
                            <asp:TableCell>
                                &nbsp;
                            </asp:TableCell>
                        </asp:TableRow>
                        
                        <asp:TableRow  VerticalAlign="Top">
                            <asp:TableCell HorizontalAlign="Right">
                                <asp:Label ID="_lblDescripcion" runat="server" Text="Descripcion"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <textarea id="_taDescripcion" runat="server" rows="10" cols="45"></textarea>
                            </asp:TableCell>
                        </asp:TableRow>
                            
                        <asp:TableRow>
                            <asp:TableCell>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:RequiredFieldValidator runat="server" id="_rfvDescripcion" CssClass="textoError" ControlToValidate="_taDescripcion" ErrorMessage = "Campo requerido" display="Dynamic" ValidationGroup="crear_solicitud"/>
                            </asp:TableCell>
                        </asp:TableRow>

                    </asp:Table>

                    <asp:Table ID="_tContenedor2" runat="server" Width="720px" Height="100px">
                        <asp:TableRow HorizontalAlign="Right">
                            <asp:TableCell>
                                <asp:Label ID="_lblInstruccion" runat="server" Text="Una vez presionado el botón la solicitud será enviada al equipo de soporte técnico"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                                                      
                    </asp:Table>
					<div>
						 <asp:Button ID="_btnEnviar" runat="server" Text="Enviar" OnClick="btnEnviar_Click" ValidationGroup="crear_solicitud"/>
					</div>
					<div>
						<asp:Image ID="_imgMensaje" Visible="false" runat="server" />
						<asp:Label ID="_lblEnvio" runat="server" Text="" Visible="false"></asp:Label>
					</div>
                </form>
			 </fieldset>
		   
</asp:Content>
