<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloSistema/MasterSistema.master" AutoEventWireup="true" CodeBehind="ModificarPeriodoLectivo.aspx.cs" Inherits="ModulosTICapaGUI.ModuloSistema.ModificarPeriodoLectivo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadSistema" runat="server">
    <title>Modificar Período</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainSistema" runat="server">
    <legend class="titulos" style="text-align: left">Modificar Período</legend>
	<fieldset class="fieldsetContornoBlanco">
		<legend class="titulos" style="text-align: left">
			Búsqueda
		</legend>
        <table border="0" style="width: 400px; height: 50px;">
	        <tr>
                <td class="alinearMedioIzquierda">
				    <asp:Label ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
				</td>
                <td class="alinearMedioIzquierda">
				    <asp:DropDownList ID="_ddlNombre" Width="165px" runat="server">
                    </asp:DropDownList>
                    <asp:DropDownList ID="_ddlId" Width="165px" runat="server" Visible="false">
                    </asp:DropDownList>
                    <asp:DropDownList ID="_ddlFechaIni" Width="165px" runat="server" Visible="false">
                    </asp:DropDownList>
                    <asp:DropDownList ID="_ddlFechaFin" Width="165px" runat="server" Visible="false">
                    </asp:DropDownList>
                    <asp:DropDownList ID="_ddlActivo" Width="165px" runat="server" Visible="false">
                    </asp:DropDownList>
			    </td>
            </tr>
            <tr>
			    <td class="alinearMedioCentro" colspan="2">
				    <asp:Button ID="_btnBuscar" runat="server" Text="Buscar" 
				        onclick="_btnBuscar_Click" style="margin-bottom: 0px" 
                        ToolTip="Presione éste botón para buscar el semestre" />
				</td>
		    </tr>
            <tr>
                <td class="alinearMedioCentro" colspan="2">
                    <asp:Image ID="_imgMensaje1" Visible="false" runat="server" />
                    <asp:Label ID="_lblMensaje1" Visible="false" runat="server"></asp:Label>
				</td>
            </tr>
        </table>
        </fieldset>
        <div class="alinearCentro">
        <fieldset class="fieldsetContornoBlanco">
        <legend class="titulos" style="text-align: left">
			Formulario
		</legend>
        <table border="0" style="width: 400px; height: 200px;">
            <tr>
                <td class="alinearMedioIzquierda">
				    <asp:Label ID="lblFechaInicio" runat="server" Text="Fecha Inicio"></asp:Label>
				</td>
                <td class="alinearMedioIzquierda">
				    <asp:TextBox id="_txtFechaInicio" Width="140px" Enabled="False" runat="server"></asp:TextBox>
                    <asp:Image ID="_imgFechaInicio" ImageUrl="~/Imagenes/Calendario.png" runat="server"
                        ToolTip="Presione aquí para abrir el calendario" EnableTheming="True" Visible="false"/>
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
                        ToolTip="Presione aquí para abrir el calendario" Visible="false"/>
                    <asp:CalendarExtender ID="_axCalendarioFinal" runat="server" TargetControlID="_txtFechaFinal" PopupButtonID="_imgFechaFinal"
                        Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
			    </td>
            </tr>
            <tr>
                <td class="alinearMedioIzquierda">
				    <asp:Label ID="_lblActivo" runat="server" Text="Activo"></asp:Label>
				</td>
                <td class="alinearMedioIzquierda">
                    <asp:CheckBox ID="_cbActivo" runat="server" Enabled="false"/>
                </td>
            </tr>
            <tr>
			    <td class="alinearMedioCentro" colspan="2">
				    <asp:Button ID="_btnModificar" runat="server" Text="Modificar" 
				        onclick="_btnModificar_Click" style="margin-bottom: 0px" 
                        ToolTip="Presione éste botón para modificar el semestre" Enabled="false"/>
                    <asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" 
				        onclick="_btnCancelar_Click" style="margin-bottom: 0px" 
                        ToolTip="Presione éste botón para cancelar" Enabled="false"/>
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
    </div>
 </asp:Content>
