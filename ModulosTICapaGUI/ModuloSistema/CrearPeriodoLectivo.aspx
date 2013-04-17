<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloSistema/MasterSistema.master" AutoEventWireup="true" CodeBehind="CrearPeriodoLectivo.aspx.cs" Inherits="ModulosTICapaGUI.ModuloSistema.CrearPeriodoLectivo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadSistema" runat="server">
    <title>Crear Período</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainSistema" runat="server">
    <legend class="titulos" style="text-align: left">Crear Período</legend>
	<fieldset class="fieldsetContornoBlanco">
		<legend class="titulos" style="text-align: left">
			Formulario
		</legend>
        <table border="0" style="width: 400px; height: 200px;">
	        <tr>
                <td class="alinearMedioIzquierda">
				    <asp:Label ID="_lblModalidad" runat="server" Text="Modalidad"></asp:Label>
				</td>
                <td class="alinearMedioIzquierda">
				    <asp:DropDownList ID="_ddlModalidad" Width="165px" runat="server"
                    onselectedindexchanged="_ddlModalidad_SelectedIndexChanged">
                        <asp:ListItem>Seleccionar</asp:ListItem>
                        <asp:ListItem>Semestre</asp:ListItem>
                        <asp:ListItem>Verano</asp:ListItem>
                    </asp:DropDownList>
			    </td>
            </tr>
            <tr>
                <td class="alinearMedioIzquierda">
				    <asp:Label ID="lblPeriodo" runat="server" Text="Período"></asp:Label>
				</td>
                <td class="alinearMedioIzquierda">
                    <asp:UpdatePanel ID="_upPanelPeriodo" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
					    <ContentTemplate>
				            <asp:DropDownList ID="_ddlPeriodo" Width="165px" runat="server">
                                <asp:ListItem>Seleccionar</asp:ListItem>
                                <asp:ListItem>I</asp:ListItem>
                                <asp:ListItem>II</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
				    </asp:UpdatePanel>
			    </td>
            </tr>
            <tr>
                <td class="alinearMedioIzquierda">
				    <asp:Label ID="lblAnho" runat="server" Text="Año"></asp:Label>
				</td>
                <td class="alinearMedioIzquierda">
				    <asp:TextBox ID="_txtAnho" Width="165px" runat="server" MaxLength="9"></asp:TextBox>
			    </td>
            </tr>
            <tr>
                <td class="alinearMedioIzquierda">
				    <asp:Label ID="lblFechaInicio" runat="server" Text="Fecha Inicio"></asp:Label>
				</td>
                <td class="alinearMedioIzquierda">
				    <asp:TextBox id="_txtFechaInicio" Width="140px" Enabled="False" runat="server"></asp:TextBox>
                    <asp:Image ID="_imgFechaInicio" ImageUrl="~/Imagenes/Calendario.png" runat="server"
                        ToolTip="Presione aquí para abrir el calendario" />
                    <asp:CalendarExtender ID="_axCalendarioInicio" runat="server" TargetControlID="_txtFechaInicio" PopupButtonID="_imgFechaInicio"
                        Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
			    </td>
            </tr>
            <tr>
                <td class="alinearMedioIzquierda">
				    <asp:Label ID="lblFechaFinal" runat="server" Text="Fecha Final"></asp:Label>
				</td>
                <td class="alinearMedioIzquierda">
				    <asp:TextBox id="_txtFechaFinal" Width="140px" Enabled="False" runat="server"></asp:TextBox>
                    <asp:Image ID="_imgFechaFinal" ImageUrl="~/Imagenes/Calendario.png" runat="server"
                        ToolTip="Presione aquí para abrir el calendario" />
                    <asp:CalendarExtender ID="_axCalendarioFinal" runat="server" TargetControlID="_txtFechaFinal" PopupButtonID="_imgFechaFinal"
                        Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
			    </td>
            </tr>
            <tr>
			    <td class="alinearMedioCentro" colspan="2">
				    <asp:Button ID="_btnCrear" runat="server" Text="Crear" 
				        onclick="_btnCrear_Click" style="margin-bottom: 0px" 
                        ToolTip="Presione éste botón para crear el semestre" />
				</td>
		    </tr>
            <tr>
                <td class="alinearMedioCentro" colspan="2">
                    <asp:Image ID="_imgMensaje" Visible="false" runat="server" />
                    <asp:Label ID="_lblMensaje" Visible="false" runat="server"></asp:Label>
				</td>
            </tr>
        </table>
    </fieldset>
    
</asp:Content>
