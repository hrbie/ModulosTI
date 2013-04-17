<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloActivos/MasterActivos.master" AutoEventWireup="true" CodeBehind="AgregarActivo.aspx.cs" Inherits="ModulosTICapaGUI.ModuloActivos.AgregarActivo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadActivos" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainActivos" runat="server">
    <legend class="titulos">Agregar activo</legend>
    <fieldset class="fieldsetContornoBlanco">
		<legend class="titulos" style="text-align: left">
			Formulario
		</legend>
	    <table style="width: 960px" class="alinearIzquierda">
		    <tr>
			    <td colspan="6"></td>
		    </tr>
            <tr>
			    <td class="style1">
				    <asp:Label ID="_lblCodigo" Text="Codigo" runat="server" CssClass="colorLetrasGeneral"></asp:Label>
			    </td>
                <td class="style4">
                    <asp:TextBox runat="server" id="_txtCodigo" Width="250px"></asp:TextBox>
			    </td>
		    </tr>
		    <tr>
			    <td class="style2">
				    <asp:Label id="_lblDescripcion" runat="server" Text="Descripción :" CssClass="colorLetrasGeneral"></asp:Label>
			    </td>
			    <td class="style4">
                    <asp:TextBox runat="server" id="_txtDescripcion" TextMode="MultiLine" Height="60px" Width="250px"></asp:TextBox>
			    </td>
                <td colspan="3" class="alinearMedioDerecha">
				    <asp:Image ID="_imgMensaje" Visible="false" runat="server" />&nbsp
			    </td>
			    <td colspan="3" class="alinearMedioIzquierda">
				    <asp:Label ID="_lblMensaje" Visible="false" runat="server"></asp:Label>
			    </td>
            </tr>
            <tr>
			    <td class="style1">
				    <asp:Label ID="_lblTipo" Text="Tipo:" runat="server" CssClass="colorLetrasGeneral"></asp:Label>
			    </td>
                <td class="style4">
				    <asp:DropDownList id="_ddlTipo" runat="server" Width="250px" >
				    </asp:DropDownList>
			    </td>
		    </tr>
            <tr>
			    <td class="alinearMedioCentro" rowspan="2" colspan="3">
                    <asp:Button ID="_btnGuardar" Text="Agregar" CausesValidation="true" OnClick="_btnGuardar_Click" 
					    ToolTip="Presione este botón para ingresar el activo" runat="server" />
                </td>
		    </tr>
            </table>
	    </fieldset>
   </asp:Content>