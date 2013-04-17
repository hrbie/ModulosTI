<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloSistema/MasterSistema.master" AutoEventWireup="true" CodeBehind="GestionBitError.aspx.cs" Inherits="ModulosTICapaGUI.ModuloSistema.GestionBitError" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadSistema" runat="server">
    <title>Gestión Bitácora Error</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainSistema" runat="server">
    <legend class="titulos" style="text-align: left">Reporte de Errores de la Plataforma</legend>
	<fieldset class="fieldsetContornoBlanco">
		<legend class="titulos" style="text-align: left">
			Manejo de Errores
		</legend>
        <div class="divReporte">
            <fieldset>
		        <legend class="titulos" style="text-align: left">
			        Listado de Errores
		        </legend>
                <table class="alinearDivReporte">
                    <tr>
                        <td>
                            <asp:Label ID="_lblDetalle" CssClass="colorLetrasGeneral" runat="server" Text="A continuación se listan los errores encontrados"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br/>
                            <asp:ListBox ID="_ltbErrores" Width="270px" Height="160px" runat="server" 
                                onselectedindexchanged="_ltbErrores_SelectedIndexChanged"></asp:ListBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br/>
                            <asp:Label ID="_lblFiltro" CssClass="letraNegrita" runat="server" Text="Aplicar filtros"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br/>
                            <asp:Label ID="_lblFechaInicio" CssClass="colorLetrasGeneral" runat="server" Text="Fecha Inicio"></asp:Label>
                            &nbsp
                            <asp:TextBox id="_txtFechaInicio" Width="140px" Enabled="False" runat="server"></asp:TextBox>
                            <asp:Image ID="_imgFechaInicio" ImageUrl="~/Imagenes/Calendario.png" runat="server"
                                ToolTip="Presione aquí para abrir el calendario" />
                            <asp:CalendarExtender ID="_axCalendarioInicio" runat="server" TargetControlID="_txtFechaInicio" 
                                PopupButtonID="_imgFechaInicio" Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="_lblFechaFinal" CssClass="colorLetrasGeneral" runat="server" Text="Fecha Final"></asp:Label>
                            &nbsp&nbsp
                            <asp:TextBox id="_txtFechaFinal" Width="140px" Enabled="False" runat="server"></asp:TextBox>
                            <asp:Image ID="_imgFechaFinal" ImageUrl="~/Imagenes/Calendario.png" runat="server"
                                ToolTip="Presione aquí para abrir el calendario" />
                            <asp:CalendarExtender ID="_axCalendarioFinal" runat="server" TargetControlID="_txtFechaFinal" PopupButtonID="_imgFechaFinal"
                                Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="_lblEstado" CssClass="colorLetrasGeneral" runat="server" Text="Estado"></asp:Label>
                            &nbsp
                            <asp:DropDownList ID="_ddlEstado" Width="170px" runat="server">
                                <asp:ListItem Selected="True">Seleccionar</asp:ListItem>
                                <asp:ListItem>Pendiente</asp:ListItem>
                                <asp:ListItem>En proceso</asp:ListItem>
                                <asp:ListItem>Resuelto</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="_btnFiltrar" runat="server" Text="Filtrar" 
                                onclick="_btnFiltrar_Click" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
        <div class="divDetalle">
            <table>
                <tr>
                    <td class="alinearMedioDerecha">
                        <asp:Label ID="_lblEstadoDetalle" CssClass="colorLetrasGeneral" runat="server" Text="Estado Actual"></asp:Label>
                    </td>
                    <td class="alinearCentro">
                        <asp:DropDownList ID="_ddlEstadoDetalle" Width="130px" runat="server">
                            <asp:ListItem Selected="True">Seleccionar</asp:ListItem>
                                <asp:ListItem>Pendiente</asp:ListItem>
                                <asp:ListItem>En proceso</asp:ListItem>
                                <asp:ListItem>Resuelto</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="alinearCentro">
                        <asp:Button ID="_btnActualizar" runat="server" Text="Actualizar Estado" 
                            Width="118px" onclick="_btnActualizar_Click" />
                    </td>
                    <td class="alinearCentro">
                        <asp:Button ID="_btnNotificar" runat="server" Text="Notificar" 
                            onclick="_btnNotificar_Click" />
                    </td>
                    <td class="alinearCentro">
                        <asp:Button ID="_btnEliminar" runat="server" Text="Eliminar" 
                            onclick="_btnEliminar_Click" />
                    </td>
                </tr>
                <tr>
                    <td/>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:UpdatePanel ID="_upDetalle" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
					        <ContentTemplate>
                                    <asp:TextBox ID="_txtDetalle" Width="380px" Height="320px" TextMode="MultiLine" Enabled="false" runat="server"></asp:TextBox>
                            </ContentTemplate>
			            </asp:UpdatePanel>
                    </td>
                    <td colspan="2">
                        <asp:UpdatePanel ID="_upDetalleUs" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
					        <ContentTemplate>
                                    <asp:TextBox ID="_txtDetalleUs" Width="200px" Height="320px" TextMode="MultiLine" Enabled="false" runat="server"></asp:TextBox>
                            </ContentTemplate>
			            </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td class="alinearMedioCentro">
						<asp:Image ID="_imgMensaje" Visible="false" runat="server" />
                    </td>
                    <td class="alinearMedioIzquierda">
                        <asp:Label ID="_lblMensaje" Visible="false" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </fieldset>
</asp:Content>
