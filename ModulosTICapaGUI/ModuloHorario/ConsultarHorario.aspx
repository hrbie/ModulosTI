<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloHorario/MasterHorario.master" AutoEventWireup="true" CodeBehind="ConsultarHorario.aspx.cs" Inherits="ModulosTICapaGUI.ModuloHorario.ConsultarHorario" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadHorario" runat="server">
    <title>Consultar Horario</title>
    <style type="text/css">
        .style1
        {
            vertical-align: middle;
            text-align: left;
            height: 26px;
        }
        .style5
        {
            width: 10px;
        }
        .style6
        {
            width: 27px;
        }
        .style7
        {
            width: 113px;
        }
        .ColorPickerText
        {
            width: 123px;
            text-align:center;
        }
        .ColorPicker
        {
            width: 100px;
            text-align:right;
        }
        .style8
        {
            width: 85px;
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHorario" runat="server">
    <legend class="titulos" style="text-align: left" >Consultar Horario</legend>
	<fieldset class="fieldsetContornoBlanco">
		<legend class="titulos" style="text-align: left">
			Información
		</legend>
		<div class="alinearMedioIzquierda">
			<table class="alinearIzquierda">
				<tr>
					<td class="style1">
						<asp:Label ID="_lblLugar" runat="server" Text="Lugar" CssClass="colorLetrasGeneral"></asp:Label>
					</td>
					<td class="style1">
						<asp:DropDownList ID="_ddlLugar" Width="172px" runat="server">
                        </asp:DropDownList>
					</td>
					<td class="style5">
                    </td>
					<td>
						<asp:Button ID="_btnConsultarHorario" runat="server" Text="Consultar" 
                            Width="90px" onclick="_btnConsultarHorario_Click" />
					&nbsp;<asp:Button ID="_btnExportar" runat="server" Text="Exportar" 
                            Width="90px" onclick="_btnExportar_Click" Enabled="False" />
					&nbsp;
					</td>
				    <td class="style6" class="alinearMedioDerecha">
							<asp:Image ID="_imgMensaje" Visible="false" runat="server" />
                    </td>
                    <td class="style7" class="alinearMedioDerecha">
						<asp:Label ID="_lblMensaje" Visible="false" runat="server"></asp:Label>
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
                            Height="255px" Enabled="false">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775"/>
                    <Columns>
                        <asp:ButtonField Text="SingleClick" CommandName="SingleClick"
                            Visible="False"/>
                        <asp:TemplateField HeaderText="Turno">
                            <ItemTemplate>
                                <asp:Label ID="_lblTurno" runat="server" Text='<%# Bind("Turno")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="139px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Lunes">
                            <ItemTemplate>
                                <asp:Label ID="_lblLunes" runat="server" Text='<%# Bind("Lunes")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Martes">
                            <ItemTemplate>
                                <asp:Label ID="_lblMartes" runat="server" Text='<%# Bind("Martes")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Miércoles">
                            <ItemTemplate>
                                <asp:Label ID="_lblMiercoles" runat="server" Text='<%# Bind("Miércoles")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Jueves">
                            <ItemTemplate>
                                <asp:Label ID="_lblJueves" runat="server" Text='<%# Bind("Jueves")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Viernes">
                            <ItemTemplate>
                                <asp:Label ID="_lblViernes" runat="server" Text='<%# Bind("Viernes")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sábado">
                            <ItemTemplate>
                                <asp:Label ID="_lblSabado" runat="server" Text='<%# Bind("Sábado")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Domingo">
                            <ItemTemplate>
                                <asp:Label ID="_lblSabado" runat="server" Text='<%# Bind("Domingo")%>'></asp:Label>
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
