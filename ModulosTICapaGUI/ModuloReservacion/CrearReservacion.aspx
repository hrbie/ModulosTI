<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloReservacion/MasterReservacion.master" AutoEventWireup="true" CodeBehind="CrearReservacion.aspx.cs" Inherits="ModulosTICapaGUI.ModuloReservacion.CrearReservacion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadReservacion" runat="server">
	<title>Crear Reservación</title>
    <style type="text/css">
        .style1
        {
            width: 187px;
        }
        .style2
        {
            height: 26px;
            width: 110px;
        }
        .style3
        {
            width: 110px;
        }
        .style4
        {
            height: 26px;
            width: 178px;
        }
        .style5
        {
            width: 178px;
        }
    	.style6
		{
			width: 167px;
		}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainReservacion" runat="server">
	<legend class="titulos">Reservación de Lugares</legend>
    <fieldset class="fieldsetContornoBlanco">
		<legend class="titulos" style="text-align: left">
			Formulario
		</legend>
	    <table style="width: 960px" class="alinearIzquierda">
		    <tr>
			    <td class="alinearIzquierda" colspan="6">
				    <asp:Label ID="_lblEncabezado" runat="server" CssClass="colorLetrasGeneral" Text="Todos los campos del formulario son obligatorios exceptuando la descripción de la reservación"></asp:Label>
			    </td>
		    </tr>
		    <tr>
			    <td colspan="6"></td>
		    </tr>
		    <tr>
			    <td class="style2">
				    <asp:Label id="_lblReservar" runat="server" Text="Laboratorio/Aula:" CssClass="colorLetrasGeneral"></asp:Label>
			    </td>
			    <td class="style4">
				    <asp:DropDownList id="_ddlLugar" runat="server" Width="143px" 
					    onselectedindexchanged="_ddlLugar_SelectedIndexChanged">
					    <asp:ListItem Selected="True">Seleccionar</asp:ListItem>
				    </asp:DropDownList>
			    </td>
			    <td class="style1">
				    <asp:Label ID="_lblCapacidad" Text="Capacidad" runat="server" CssClass="colorLetrasGeneral"></asp:Label>
			    </td>
			    <td class="style6">
				    <asp:UpdatePanel ID="_upPanelCantidad" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
					    <ContentTemplate>
						    <asp:Label ID="_lblCantidad" runat="server" CssClass="colorLetrasGeneral"></asp:Label>&nbsp
						    <asp:Label ID="_lblPersonas" Text="personas" runat="server" CssClass="colorLetrasGeneral"></asp:Label>
					    </ContentTemplate>
				    </asp:UpdatePanel>
			    </td>
			    <td colspan="2">
				    <asp:Label ID="_lblChoques" Visible="false" CssClass="colorLetrasGeneral" runat="server" Text="A continuación se presentan los choques encontrados"></asp:Label>
			    </td>
		    </tr>
		    <tr>
			    <td class="alinearArribaIzquierda">
				    <asp:Label ID="_lblEncargado" Text="Encargado" runat="server" CssClass="colorLetrasGeneral"></asp:Label>
			    </td>
			    <td class="alinearArribaIzquierda">
				    <asp:UpdatePanel ID="_upEncargado" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
					    <ContentTemplate>
						    <asp:Label ID="_lblNombreEncargado" runat="server" CssClass="colorLetrasGeneral"></asp:Label>
					    </ContentTemplate>
				    </asp:UpdatePanel>
			    </td>
			    <td rowspan="2" class="alinearArribaIzquierda">
				    <asp:Label ID="_lblDescripción" Text="Descripción" runat="server" CssClass="colorLetrasGeneral"></asp:Label>
			    </td>
			    <td rowspan="2" class="style6">
				    <asp:UpdatePanel ID="_upDescripcion" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
					    <ContentTemplate>
						    <asp:TextBox ID="_txtDescripcion" Enabled="false" runat="server" Height="100px" 
							    TextMode="MultiLine" Width="166px"></asp:TextBox>
					    </ContentTemplate>
				    </asp:UpdatePanel>
			    </td>
			    <td class="alineacionCentral" colspan="2" rowspan="5">
				    <asp:TextBox ID="_txtChoques" runat="server" Enabled="False" Height="153px" 
					    TextMode="MultiLine" Visible="false" Width="241px"></asp:TextBox>
			    </td>
		    </tr>
		    <tr>
			    <td class="alinearArribaIzquierda">
				    <asp:Label ID="_lblTipo" runat="server" Text="Tipo" CssClass="colorLetrasGeneral"></asp:Label>
			    </td>
			    <td class="alinearArribaIzquierda">
				    <asp:UpdatePanel ID="_upTipo" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
					    <ContentTemplate>
						    <asp:Label ID="_lblNombreTipo" runat="server" CssClass="colorLetrasGeneral"></asp:Label>
					    </ContentTemplate>
				    </asp:UpdatePanel>
			    </td>
		    </tr>
		    <tr>
			    <td class="style3"></td>
			    <td class="style5"></td>
			    <td class="style1"></td>
			    <td class="style6"></td>
		    </tr>
		    <tr>
			    <td class="style3">
				    <asp:Label id="_lblCarrera" runat="server" Text="Carrera:" CssClass="colorLetrasGeneral"></asp:Label>
			    </td>
			    <td class="style5">
				    <asp:DropDownList id="_ddlCarrera" runat="server" Width="143px" 
					    onselectedindexchanged="_ddlCarrera_SelectedIndexChanged">
					    <asp:ListItem Selected="True">Seleccionar</asp:ListItem>
				    </asp:DropDownList>
			    </td>
			    <td class="style1">
				    <asp:Label id="_lblCurso" runat="server" Text="Curso" CssClass="colorLetrasGeneral"></asp:Label>
			    </td>
			    <td class="style6">
				    <asp:UpdatePanel ID="_upCurso" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
					    <ContentTemplate>
						    <asp:DropDownList id="_ddlCurso" runat="server" Width="143px" Enabled="False">
						    </asp:DropDownList>
					    </ContentTemplate>
				    </asp:UpdatePanel>
			    </td>
		    </tr>
		    <tr>
			    <td class="style3">
				    <asp:Label runat="server" Text="Login Solicitante" id="_lblLogin" CssClass="colorLetrasGeneral"></asp:Label>
			    </td>
			    <td class="style5">
				    <asp:TextBox runat="server" id="_txtLogin" Width="140px" MaxLength="20"></asp:TextBox>
			    </td>
			    <td class="alinearArribaIzquierda">
				    <asp:Label runat="server" Text="Descripción de la Reservación" id="_lblDescripcion" CssClass="colorLetrasGeneral"></asp:Label>
			    </td>
			    <td class="style6">
				    <asp:TextBox runat="server" id="_txtDescripcionReservacion" TextMode="MultiLine" Height="100px" Width="166px"></asp:TextBox>
			    </td>
		    </tr>
            <tr>
			    <td class="style3"><asp:Label runat="server" Text="Fecha Inicio" id="_lblFechaInicio" CssClass="colorLetrasGeneral"></asp:Label></td>
                <td class="style5">
                    <asp:TextBox runat="server" id="_txtFechaInicio" Width="140px" 
					    ontextchanged="_txtFechaInicio_TextChanged" Enabled="False"></asp:TextBox>
                    <asp:Image ID="_imgFechaInicio" ImageUrl="~/Imagenes/Calendario.png" 
					    runat="server" ToolTip="Presione aquí para abrir el calendario" />
                    <asp:CalendarExtender ID="_axCalendarioInicio" runat="server"
                            TargetControlID="_txtFechaInicio" PopupButtonID="_imgFechaInicio" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                </td>
                <td class="style1"><asp:Label runat="server" Text="Fecha Final" id="_lblFechaFinal" CssClass="colorLetrasGeneral"></asp:Label></td>
                <td class="style6"><asp:TextBox runat="server" id="_txtFechaFinal" Width="140px" Enabled="False" 
					    ontextchanged="_txtFechaFinal_TextChanged"></asp:TextBox>
                    <asp:Image ID="_imgFechaFinal" ImageUrl="~/Imagenes/Calendario.png" 
					    runat="server" ToolTip="Presione aquí para abrir el calendario" />
                    <asp:CalendarExtender ID="_axCalendarioFinal" runat="server"
                            TargetControlID="_txtFechaFinal" PopupButtonID="_imgFechaFinal" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
			    </td>
			    <td colspan="2">
				    <asp:CheckBox ID="_cbHorario" Text="Marcar como horario permanente" CssClass="colorLetrasGeneral" runat="server" />
			    </td>
		    </tr>
            <tr>
			    <td class="style3"><asp:Label runat="server" Text="Hora Inicio" id="_lblHoraInicio" CssClass="colorLetrasGeneral"></asp:Label></td>
                <td class="style5"><asp:DropDownList ID="_ddlHoraInicio" runat="server" Width="66px">
                        <asp:ListItem Selected="True">Hora</asp:ListItem>
                        <asp:ListItem>07</asp:ListItem>
                        <asp:ListItem>08</asp:ListItem>
                        <asp:ListItem>09</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                        <asp:ListItem>13</asp:ListItem>
                        <asp:ListItem>14</asp:ListItem>
                        <asp:ListItem>15</asp:ListItem>
                        <asp:ListItem>16</asp:ListItem>
                        <asp:ListItem>17</asp:ListItem>
                        <asp:ListItem>18</asp:ListItem>
                        <asp:ListItem>19</asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                        <asp:ListItem>21</asp:ListItem>
                        <asp:ListItem>22</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp
                    <asp:DropDownList ID="_ddlMinutoInicio" Width="66px" runat="server">
                        <asp:ListItem>Minuto</asp:ListItem>
                        <asp:ListItem>00</asp:ListItem>
                        <asp:ListItem>30</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="style1"><asp:Label runat="server" Text="Hora Final" id="_lblHoraFinal" CssClass="colorLetrasGeneral"></asp:Label></td>
                <td class="style6"><asp:DropDownList ID="_ddlHoraFinal" runat="server" Width="66px">
                        <asp:ListItem Selected="True">Hora</asp:ListItem>
                        <asp:ListItem>07</asp:ListItem>
                        <asp:ListItem>08</asp:ListItem>
                        <asp:ListItem>09</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                        <asp:ListItem>13</asp:ListItem>
                        <asp:ListItem>14</asp:ListItem>
                        <asp:ListItem>15</asp:ListItem>
                        <asp:ListItem>16</asp:ListItem>
                        <asp:ListItem>17</asp:ListItem>
                        <asp:ListItem>18</asp:ListItem>
                        <asp:ListItem>19</asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                        <asp:ListItem>21</asp:ListItem>
                        <asp:ListItem>22</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp
                    <asp:DropDownList ID="_ddlMinutoFinal" Width="66px" runat="server">
                        <asp:ListItem>Minuto</asp:ListItem>
                        <asp:ListItem>00</asp:ListItem>
                        <asp:ListItem>30</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td><asp:Label runat="server" Text="Días" id="_lblDias" CssClass="colorLetrasGeneral"></asp:Label></td>
                <td>
				    <asp:UpdatePanel ID="_upDias" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
					    <ContentTemplate>
						    <asp:CheckBox ID="_cbLunes" Text="L" runat="server" />
						    <asp:CheckBox ID="_cbMartes" Text="K" runat="server" />
						    <asp:CheckBox ID="_cbMiercoles" Text="M" runat="server" />
						    <asp:CheckBox ID="_cbJueves" Text="J" runat="server" />
						    <asp:CheckBox ID="_cbViernes" Text="V" runat="server" />
						    <asp:CheckBox ID="_cbSabado" Text="S" runat="server" />
						    <asp:CheckBox ID="_cbDomingo" Text="D" runat="server" />
					    </ContentTemplate>
				    </asp:UpdatePanel>
                </td>
		    </tr>
		    <tr>
			    <td colspan="6" class="alinearCentro">
			    </td>
		    </tr>
		    <tr>
			    <td colspan="6" class="alinearCentro">
			    </td>
		    </tr>
		    <tr>
			    <td colspan="6" class="alinearCentro">
				    <asp:Button ID="_btnReservar" Text="Reservar" CausesValidation="true" OnClick="_btnReservar_Click" 
					    ToolTip="Presione este botón para confirmar la reservación" runat="server" />
			    </td>
		    </tr>
		    <tr>
			    <td colspan="3" class="alinearMedioDerecha">
				    <asp:Image ID="_imgMensaje" Visible="false" runat="server" />&nbsp
			    </td>
			    <td colspan="3" class="alinearMedioIzquierda">
				    <asp:Label ID="_lblMensaje" Visible="false" runat="server"></asp:Label>
			    </td>
		    </tr>
	    </table>
    </fieldset>
</asp:Content>
