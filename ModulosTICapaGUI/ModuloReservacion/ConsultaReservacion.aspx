<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloReservacion/MasterReservacion.master" AutoEventWireup="true" CodeBehind="ConsultaReservacion.aspx.cs" Inherits="ModulosTICapaGUI.ModuloReservacion.ConsultaReservacion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadReservacion" runat="server">
    <title>Consultar Reservación</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainReservacion" runat="server">
    <legend class="titulos">Consultar Reservaciones</legend>
    <fieldset class="fieldsetContornoBlanco">
		<legend class="titulos" style="text-align: left">
			Datos de consulta
		</legend>
       <div class="alinearMedioIzquierda">
		<table class="alinearIzquierda">
            <tr>
                <td class="style1">
				    <asp:Label id="_lblLugar" runat="server" Text="Laboratorio/Aula:" CssClass="colorLetrasGeneral" Height="21px"></asp:Label>
			    </td>
			    <td class="style4">
				    <asp:DropDownList id="_ddlLugar" runat="server" Width="143px">
				    </asp:DropDownList>
			    </td>
                <td class="style3"><asp:Label runat="server" Text="Fecha Inicio" id="_lblFechaInicio" CssClass="colorLetrasGeneral"></asp:Label></td>
                <td class="style5">
                    <asp:TextBox runat="server" id="_txtFechaInicio" Width="140px"  Enabled="False"></asp:TextBox>
                    <asp:Image ID="_imgFechaInicio" ImageUrl="~/Imagenes/Calendario.png" 
					    runat="server" ToolTip="Presione aquí para abrir el calendario" />
                    <asp:CalendarExtender ID="_axCalendarioInicio" runat="server"
                            TargetControlID="_txtFechaInicio" PopupButtonID="_imgFechaInicio" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                </td>
                <td colspan="3" class="alinearMedioDerecha">
				    <asp:Image ID="_imgMensaje" Visible="false" runat="server" />&nbsp
			    </td>
			    <td colspan="3" class="alinearMedioIzquierda">
				    <asp:Label ID="_lblMensaje" Visible="false" runat="server"></asp:Label>
			    </td>
            </tr>
            <tr>
                 <td class="alinearMedioCentro" rowspan="2" colspan="2">
                    <asp:Button ID="_btnConsultar" Text="Consultar" CausesValidation="true" OnClick="_btnConsultar_Click" 
					    ToolTip="Presione este botón para consultar la reservación" runat="server" />
                </td>

                <td class="style1"><asp:Label runat="server" Text="Fecha Final" id="_lblFechaFinal" CssClass="colorLetrasGeneral"></asp:Label></td>
                <td class="style6"><asp:TextBox runat="server" id="_txtFechaFinal" Width="140px" Enabled="False"></asp:TextBox>
                    <asp:Image ID="_imgFechaFinal" ImageUrl="~/Imagenes/Calendario.png" 
					    runat="server" ToolTip="Presione aquí para abrir el calendario" />
                    <asp:CalendarExtender ID="_axCalendarioFinal" runat="server"
                            TargetControlID="_txtFechaFinal" PopupButtonID="_imgFechaFinal" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
			    </td>
            </tr>
        </table>
       </div>
    </fieldset>
    <fieldset class="fieldsetContornoBlanco">
		<legend class="titulos" style="text-align: left">Horario</legend>
		<div class="centrarGrid">
			<div class="scroll">
				<asp:GridView ID="_gridHorario" runat="server" AutoGenerateColumns="True" 
                            CellPadding="4" ForeColor="#333333" GridLines="None" Width="925px" 
                            Height="255px" Enabled="false" 
                    onselectedindexchanged="_gridHorario_SelectedIndexChanged">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775"/>
                    <Columns>
                        
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>
			</div>
		</div>
	</fieldset>
</asp:Content>
