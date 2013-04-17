<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloActivos/MasterActivos.master" AutoEventWireup="true" CodeBehind="RegistrarPrestamo.aspx.cs" Inherits="ModulosTICapaGUI.ModuloActivos.RegistrarPrestamo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadActivos" runat="server">
    <title>Consultar Activos</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainActivos" runat="server">
    <legend class="titulos">Registrar Movimiento de activo</legend>
    <fieldset class="fieldsetContornoBlanco">
		<legend class="titulos" style="text-align: left">
			Criterio de consulta
		</legend>
       <div class="alinearMedioIzquierda">
		<table class="alinearIzquierda">
            <tr>
                <td class="style1"></td>
                <td class="style1">
				    <asp:Label id="_lblCodigo" runat="server" Text="Codigo:" CssClass="colorLetrasGeneral" Height="21px"></asp:Label>
			    </td>
			    <td class="style4">
				    <asp:TextBox id="_txtCodigo" runat="server" Width="143px" >
				    </asp:TextBox>
			    </td>
                <td colspan="3" class="alinearMedioDerecha">
				    <asp:Image ID="_imgMensaje" Visible="false" runat="server" />&nbsp
			    </td>
			    <td colspan="3" class="alinearMedioIzquierda">
				    <asp:Label ID="_lblMensaje" Visible="false" runat="server"></asp:Label>
			    </td>
            </tr>
            <tr>
                 <td class="alinearMedioCentro" rowspan="2" colspan="3">
                    <asp:Button ID="_btnConsultar" Text="Consultar" CausesValidation="true" OnClick="_btnConsultar_Click" 
					    ToolTip="Presione este botón para consultar el activo" runat="server" />
                </td>

            </tr>
        </table>
       </div>
    </fieldset>
    <fieldset class="fieldsetContornoBlanco">
		<legend class="titulos" style="text-align: left">Activo</legend>
		<div class="alinearMedioIzquierda">
		<table class="alinearIzquierda">
            <tr>
                <td class="style1"></td>
                <td class="style1">
				    <asp:Label id="_lblCcodigo" runat="server" Text="Codigo:" CssClass="colorLetrasGeneral" Height="21px"></asp:Label>
			    </td>
			    <td class="style4">
				    <asp:TextBox id="_txtCcodigo" runat="server" Width="143px" Enabled="false">
				    </asp:TextBox>
                    <asp:TextBox id="_txtId" runat="server" Width="143px" Enabled="false" 
                        Visible="False"></asp:TextBox>
			    </td>
            </tr>
            <tr>
                <td class="style1"></td>
                <td class="style1">
				    <asp:Label id="_lblCdescripcion" runat="server" Text="Descripcion:" CssClass="colorLetrasGeneral" Height="21px"></asp:Label>
			    </td>
			    <td class="style4">
				    <asp:TextBox id="_txtCdescripcion" runat="server" Width="143px" Enabled="false" >
				    </asp:TextBox>
			    </td>
                <td colspan="3" class="alinearMedioDerecha">
				    <asp:Image ID="_imgCMensaje" Visible="false" runat="server" />&nbsp
			    </td>
			    <td colspan="3" class="alinearMedioIzquierda">
				    <asp:Label ID="_lblCMensaje" Visible="false" runat="server"></asp:Label>
			    </td>
            </tr>
            <tr>
                <td class="style1"></td>
                <td class="style1">
				    <asp:Label id="_lblCestado" runat="server" Text="Movimiento:" CssClass="colorLetrasGeneral" Height="21px"></asp:Label>
			    </td>
			    <td class="style4">
				    <asp:DropDownList id="_ddlEstado" runat="server" Width="143px" Enabled="False" >
				    </asp:DropDownList>
			    </td>
            </tr>
            <tr>
                <td class="style1"></td>
                <td class="style1">
				    <asp:Label id="_lblClogin" runat="server" Text="Login del solicitante:" CssClass="colorLetrasGeneral" Height="21px"></asp:Label>
			    </td>
			    <td class="style4">
				    <asp:TextBox id="_txtClogin" runat="server" Width="143px" Enabled="False" ></asp:TextBox>
			    </td>
            </tr>
            <tr>
                <td class="style1"></td>
                <td class="style1">
				    <asp:Label id="_txtCcomentario" runat="server" Text="Comentario:" CssClass="colorLetrasGeneral" Height="21px"></asp:Label>
			    </td>
			    <td class="style4">
				    <asp:TextBox id="_txtComentario" runat="server" Width="143px" 
                        TextMode="MultiLine" Enabled="False" ></asp:TextBox>
			    </td>
            </tr>
            <tr>
                 <td class="alinearMedioCentro" rowspan="2" colspan="3">
                    <asp:Button ID="_btnMovimiento" Text="Guardar Movimiento" 
                         CausesValidation="true" OnClick="_btnMovimiento_Click" 
					    ToolTip="Presione este botón para guardar el movimiento del activo" runat="server" 
                         Enabled="False" />
                    <asp:Button ID="_btnCancelar" Text="Cancelar" 
                         CausesValidation="true" OnClick="_btnCancelar_Click" 
					    ToolTip="Presione este botón para cancelar el movimiento del activo" runat="server" 
                         Enabled="False" />
                </td>

            </tr>
        </table>
       </div>
	</fieldset>
</asp:Content>

