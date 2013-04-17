<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloHorario/MasterHorario.master" AutoEventWireup="true" CodeBehind="CrearHorario.aspx.cs" Inherits="ModulosTICapaGUI.ModuloHorario.CrearHorario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadHorario" runat="server">
	<title>Gestionar Horario</title>
    <style type="text/css">
        .style1
        {
            vertical-align: middle;
            text-align: left;
            height: 26px;
        }
        .style3
        {
            vertical-align: middle;
            text-align: left;
            height: 43px;
        }
        .style4
        {
            height: 43px;
        }
    	.style5
		{
			vertical-align: middle;
			text-align: left;
			height: 43px;
			width: 176px;
		}
		.style6
		{
			vertical-align: middle;
			text-align: left;
			height: 26px;
			width: 176px;
		}
		</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHorario" runat="server">
	<legend class="titulos" style="text-align: left" >Gestión de Lugares</legend>
	<fieldset class="fieldsetContornoBlanco">
		<legend class="titulos" style="text-align: left">
			Información
		</legend>
		<div class="alinearMedioIzquierda">
			<table class="alinearIzquierda">
				<tr>
					<td class="style3">
						<asp:Label ID="_lblSemestre" runat="server" Text="Semestre" CssClass="colorLetrasGeneral"></asp:Label>
					</td>
					<td class="style5">
						<asp:DropDownList ID="_ddlSemestre" Width="172px" runat="server" 
							AutoPostBack="True" onselectedindexchanged="_ddlSemestre_SelectedIndexChanged">
						</asp:DropDownList>
					</td>
					<td class="style4">
						&nbsp;</td>
					<td class="alinearMedioCentro">
						<asp:Image ID="_imgMensaje" Visible="false" runat="server" />
					</td>
                    <td>
						<asp:Label ID="_lblMensaje" Visible="false" runat="server"></asp:Label>
					</td>
					<td class="alinearMedioCentro" colspan="4">
						&nbsp;</td>
                    <td></td>
				</tr>
				<tr>
					<td class="style1">
						<asp:Label ID="_lblLugar" runat="server" Text="Lugar" CssClass="colorLetrasGeneral"></asp:Label>
					</td>
					<td class="style6">
						<asp:DropDownList ID="_ddlLugar" Width="172px" runat="server" 
							onselectedindexchanged="_ddlLugar_SelectedIndexChanged" AutoPostBack="True" Enabled="False">
						</asp:DropDownList>
					</td>
					<td>
						&nbsp;</td>
					<td>
						&nbsp;</td>
                    <td></td>
					<td>
						&nbsp;</td>
                    <td>
						&nbsp;</td>
					<td></td>
					<td></td>
				</tr>
				<tr>
					<td></td>
					<td class="alineacionDerecha">
						<asp:Button ID="_btnCrearHorario" runat="server" Text="Crear Horario" 
                            Width="90px" onclick="_btnCrearHorario_Click" Enabled="False"/>
					&nbsp;<asp:Button ID="_btnGuardarHorario" Text="Guardar"
                            Width="60px" runat="server" onclick="_btnGuardarHorario_Click" 
                            Enabled="False"/>
					</td>
					<td>
						&nbsp;</td>
					<td>
						&nbsp;</td>
                    <td>
						&nbsp;</td>
				</tr>
			</table>
		</div>
	</fieldset>
    <fieldset class="fieldsetContornoBlanco">
        <legend class="titulos" style="text-align: left">Horario Disponibilidad</legend>
        <div>
        <table class="alineacionIzquierda">
				<tr>
                    <td>
						<asp:Label ID="_lblDisponibilidad" runat="server" 
                            Text="Horario de disponibilidad:" CssClass="colorLetrasGeneral"></asp:Label>
                    </td>
                    <td>
					<asp:Button ID="_btnHorarioDisponibilidad" runat="server" Text="Generar" Width="80px" style="margin-bottom: 0px"
                        onclick="_btnHorarioDisponibilidad_Click" Enabled="False"/>
                    &nbsp;
                    <asp:Button ID="_btnDeshabilitarHorarioDisponibilidad" runat="server" Text="Deshabilitar" Width="80px" 
                        onclick="_btnDeshabilitarHorarioDisponibilidad_Click" Enabled="False"/>
                    &nbsp;
                    <asp:Button ID="_btnExportar" runat="server" Text="Exportar" Width="80px" onclick="_btnExportar_Click"/>
                    </td>
                </tr>
        </table>            
        </div>
    </fieldset>
	<fieldset class="fieldsetContornoBlanco">
		<legend class="titulos" style="text-align: left">Horario</legend>
		<div class="centrarGrid">
			<div class="scroll">
				<asp:GridView ID="_gridHorario" runat="server" AutoGenerateColumns="False" 
							CellPadding="4" ForeColor="#333333" GridLines="None" Width="925px" 
							Height="240px" 
					onrowcommand="_gridHorario_RowCommand" onrowdatabound="GridView1_RowDataBound">
					<AlternatingRowStyle BackColor="White" ForeColor="#284775"/>
					<Columns>
						<asp:ButtonField Text="SingleClick" CommandName="SingleClick"
							Visible="False"/>
						<asp:TemplateField HeaderText="Turno">
							<ItemTemplate>
								<asp:Label ID="_lblTurno" runat="server" Text='<%# Bind("Turno")%>'></asp:Label>
							</ItemTemplate>
							<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="85px" />
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Lunes">
							<ItemTemplate>
								<asp:Label ID="_lblLunes" runat="server" Text='<%# Bind("Lunes")%>'></asp:Label>
								<asp:TextBox ID="_txtLunes" runat="server" Text='<%# Eval("Lunes") %>' Width="175px" visible="false"></asp:TextBox>
								<asp:Label ID="_lblIdTurnoLunes" Visible="false" Text='<%# Bind("Lunes")%>' runat="server"></asp:Label>
							</ItemTemplate>
							<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Martes">
							<ItemTemplate>
								<asp:Label ID="_lblMartes" runat="server" Text='<%# Bind("Martes")%>'></asp:Label>
								<asp:TextBox ID="_txtMartes" runat="server" Text='<%# Eval("Martes") %>' Width="175px" visible="false"></asp:TextBox>
								<asp:Label ID="_lblIdTurnoMartes" Visible="false" Text='<%# Bind("Martes")%>' runat="server"></asp:Label>
							</ItemTemplate>
							<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Miercoles">
							<ItemTemplate>
								<asp:Label ID="_lblMiercoles" runat="server" Text='<%# Bind("Miércoles")%>'></asp:Label>
								<asp:TextBox ID="_txtMiercoles" runat="server" Text='<%# Eval("Miércoles") %>' Width="175px" visible="false"></asp:TextBox>
								<asp:Label ID="_lblIdTurnoMiercoles" Visible="false" Text='<%# Bind("Miércoles")%>' runat="server"></asp:Label>
							</ItemTemplate>
							<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Jueves">
							<ItemTemplate>
								<asp:Label ID="_lblJueves" runat="server" Text='<%# Bind("Jueves")%>'></asp:Label>
								<asp:TextBox ID="_txtJueves" runat="server" Text='<%# Eval("Jueves") %>' Width="175px" visible="false"></asp:TextBox>
								<asp:Label ID="_lblIdTurnoJueves" Visible="false" Text='<%# Bind("Jueves")%>' runat="server"></asp:Label>
							</ItemTemplate>
							<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Viernes">
							<ItemTemplate>
								<asp:Label ID="_lblViernes" runat="server" Text='<%# Bind("Viernes")%>'></asp:Label>
								<asp:TextBox ID="_txtViernes" runat="server" Text='<%# Eval("Viernes") %>' Width="175px" visible="false"></asp:TextBox>
								<asp:Label ID="_lblIdTurnoViernes" Visible="false" Text='<%# Bind("Viernes")%>' runat="server"></asp:Label>
							</ItemTemplate>
							<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Sabado">
							<ItemTemplate>
								<asp:Label ID="_lblSabado" runat="server" Text='<%# Bind("Sábado")%>'></asp:Label>
								<asp:TextBox ID="_txtSabado" runat="server" Text='<%# Eval("Sábado") %>' Width="175px" visible="false"></asp:TextBox>
								<asp:Label ID="_lblIdTurnoSabado" Visible="false" Text='<%# Bind("Sábado")%>' runat="server"></asp:Label>
							</ItemTemplate>
							<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Domingo">
							<ItemTemplate>
								<asp:Label ID="_lblDomingo" runat="server" Text='<%# Bind("Domingo")%>'></asp:Label>
								<asp:TextBox ID="_txtDomingo" runat="server" Text='<%# Eval("Domingo") %>' Width="175px" visible="false"></asp:TextBox>
								<asp:Label ID="_lblIdTurnoDomingo" Visible="false" Text='<%# Bind("Domingo")%>' runat="server"></asp:Label>
							</ItemTemplate>
							<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
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
			</div>
		</div>
	</fieldset>
</asp:Content>
