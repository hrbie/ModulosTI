<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloPEUL/MasterPEUL.master" AutoEventWireup="true" CodeBehind="ConsultarReporteUso.aspx.cs" Inherits="ModulosTICapaGUI.ModuloPEUL.ConsultarReporteUso" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPeul" runat="server">
    <title>Registro de Reportes de Uso</title>
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainPeul" runat="server">
    <legend class="titulos" style="text-align: left" >Registro de Reportes de Uso de Laboratorio</legend>
	<fieldset class="fieldsetContornoBlanco">
		<legend class="titulos" style="text-align: left">
			Criterio de Búsqueda
		</legend>
		<div class="alinearMedioIzquierda">
			<table class="alinearIzquierda">
				<tr>
					<td>
						<asp:Label ID="_lblLaboratorios" runat="server" Text="Laboratorio" CssClass="colorLetrasGeneral"></asp:Label>
					</td>
					<td class="style1">
                        <asp:DropDownList ID="_ddlLaboratorios" Width="168px" runat="server" 
                            Height="21px">
                        </asp:DropDownList>
					</td>
					<td>
						<asp:Label ID="_lblFechaInicio" runat="server" Text="Fecha Inicio" CssClass="colorLetrasGeneral"></asp:Label>
					</td>
					<td class="style2">
						<asp:TextBox ID="_txtFechaInicio" runat="server" Width="166px"></asp:TextBox>
						<asp:Image ID="_imgFechaInicio" ImageUrl="~/Imagenes/Calendario.png" 
							runat="server" ToolTip="Presione aquí para abrir el calendario" style="width: 16px" />
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
                        <asp:Button ID="_btnConsultar" runat="server" Text="Consultar" 
                            onclick="_btnConsultar_Click" />
					</td>
					<td class="alinearMedioIzquierda">
						<asp:Label ID="_lblFechaFinal" runat="server" Text="Fecha Final" CssClass="colorLetrasGeneral"></asp:Label>
					</td>
					<td class="style2">
						<asp:TextBox ID="_txtFechaFinal" runat="server" Width="166px"></asp:TextBox>
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
		<legend class="titulos" style="text-align: left">Registro</legend>
		<div class="scroll">
			<asp:UpdatePanel ID="_uplLugares" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
				<ContentTemplate>
					<asp:GridView ID="_gvwRegistro" runat="server" CellPadding="4" ForeColor="#333333" 
						GridLines="None" AutoGenerateColumns="False" Width="925px">
						<AlternatingRowStyle BackColor="White" ForeColor="#284775" />
						<Columns>
							<asp:TemplateField HeaderText="Fecha">
								<ItemTemplate>
									<asp:Label ID="_lblFecha" runat="server" Text='<%# Bind("Fecha")%>'></asp:Label>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Estado">
								<ItemTemplate>
									<asp:Label ID="_lblEstado" CssClass="alineacionJustificado" runat="server" Text='<%# Bind("Estado")%>'></asp:Label>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Operador">
								<ItemTemplate>
									<asp:Label ID="_lblOperador" CssClass="alineacionJustificado" runat="server" Text='<%# Bind("Operador")%>'></asp:Label>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Porcentaje de Uso">
								<ItemTemplate>
									<asp:Label ID="_lblPorcentajeUso" CssClass="alineacionJustificado" runat="server" Text='<%# Bind("Porcentaje")%>'></asp:Label>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Cantidad Usuarios">
								<ItemTemplate>
									<asp:Label ID="_lblCantidadUsuarios" CssClass="alineacionJustificado" runat="server" Text='<%# Bind("Usuarios")%>'></asp:Label>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Cantidad Portátiles">
								<ItemTemplate>
									<asp:Label ID="_lblCantidadPortatiles" CssClass="alineacionJustificado" runat="server" Text='<%# Bind("Portatiles")%>'></asp:Label>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Comentario">
								<ItemTemplate>
									<asp:Label ID="_lblComentario" CssClass="alineacionJustificado" runat="server" Text='<%# Bind("Comentario")%>'></asp:Label>
								</ItemTemplate>
							</asp:TemplateField>
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
				</ContentTemplate>
			</asp:UpdatePanel>
		</div>
	</fieldset>
</asp:Content>
