<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloHorario/MasterHorario.master" AutoEventWireup="true" CodeBehind="HorarioDisponibilidad.aspx.cs" Inherits="ModulosTICapaGUI.ModuloHorario.HorarioDisponibilidad" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadHorario" runat="server">
    <title>Horario de Disponibilidad</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHorario" runat="server">
    <fieldset style="background:#EBF4FB">
		<legend class="titulos">Horario de Disponibilidad</legend>
		
	    <table style="width:100%;">
            <tr>
                <td class="alinearIzquierda">
                    <asp:Button ID="_btnHorario" runat="server" Text="Ingresar Horario" 
                        onclick="_btnHorario_Click" />
                &nbsp;<asp:Button ID="_btnGuardar" runat="server" Text="Guardar" Enabled="false" 
                        onclick="_btnGuardar_Click"/>
                &nbsp;<asp:Image ID="_imgMensaje" Visible="false" runat="server" />							
                </td>
                <td class="alineacionIzquierda">
							    <asp:Label ID="_lblMensaje" Visible="false" runat="server" CssClass="colorLetrasGeneral"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
					<div class="scroll">
						<asp:GridView ID="_gridHorario" runat="server" AutoGenerateColumns="False" 
							CellPadding="4" ForeColor="#333333" GridLines="None" Width="925px" 
							Height="255px" Enabled="false" onrowcommand="_gridHorario_RowCommand">
							<AlternatingRowStyle BackColor="White" ForeColor="#284775"/>
							<Columns>
								<asp:ButtonField Text="SingleClick" CommandName="SingleClick"
									Visible="False"/>
								<asp:TemplateField HeaderText="Turno">
									<ItemTemplate>
										<asp:Label ID="_lblTurno" runat="server" Text='<%# Bind("Turno")%>'></asp:Label>
									</ItemTemplate>
									<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Lunes">
									<ItemTemplate>
										<asp:Label ID="_lblLunes" runat="server" Text='<%# Bind("Lunes")%>'></asp:Label>
										<asp:TextBox ID="_txtLunes" runat="server" Text='<%# Eval("Lunes") %>' Width="175px" visible="false"></asp:TextBox>
										<asp:FilteredTextBoxExtender ID="_axFiltroLunes" 
											runat="server" TargetControlID="_txtLunes" FilterMode="InvalidChars"
											InvalidChars="0123456789">
										</asp:FilteredTextBoxExtender>
									</ItemTemplate>                                            
									<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Martes">
									<ItemTemplate>
										<asp:Label ID="_lblMartes" runat="server" Text='<%# Bind("Martes")%>'></asp:Label>
										<asp:TextBox ID="_txtMartes" runat="server" Text='<%# Eval("Martes") %>' Width="175px" visible="false"></asp:TextBox>
										<asp:FilteredTextBoxExtender ID="_axFiltroMartes" 
											runat="server" TargetControlID="_txtMartes" FilterMode="InvalidChars"
											InvalidChars="0123456789">
										</asp:FilteredTextBoxExtender>
									</ItemTemplate>
									<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Miércoles">
									<ItemTemplate>
										<asp:Label ID="_lblMiercoles" runat="server" Text='<%# Bind("Miércoles")%>'></asp:Label>
										<asp:TextBox ID="_txtMiercoles" runat="server" Text='<%# Eval("Miércoles") %>' Width="175px" visible="false"></asp:TextBox>
										<asp:FilteredTextBoxExtender ID="_axFiltroMiercoles" 
											runat="server" TargetControlID="_txtMiercoles" FilterMode="InvalidChars"
											InvalidChars="0123456789">
										</asp:FilteredTextBoxExtender>
									</ItemTemplate>
									<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Jueves">
									<ItemTemplate>
										<asp:Label ID="_lblJueves" runat="server" Text='<%# Bind("Jueves")%>'></asp:Label>
										<asp:TextBox ID="_txtJueves" runat="server" Text='<%# Eval("Jueves") %>' Width="175px" visible="false"></asp:TextBox>
										<asp:FilteredTextBoxExtender ID="_axFiltroJueves" 
											runat="server" TargetControlID="_txtJueves" FilterMode="InvalidChars"
											InvalidChars="0123456789">
										</asp:FilteredTextBoxExtender>
									</ItemTemplate>
									<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Viernes">
									<ItemTemplate>
										<asp:Label ID="_lblViernes" runat="server" Text='<%# Bind("Viernes")%>'></asp:Label>
										<asp:TextBox ID="_txtViernes" runat="server" Text='<%# Eval("Viernes") %>' Width="175px" visible="false"></asp:TextBox>
										<asp:FilteredTextBoxExtender ID="_axFiltroViernes" 
											runat="server" TargetControlID="_txtViernes" FilterMode="InvalidChars"
											InvalidChars="0123456789">
										</asp:FilteredTextBoxExtender>
									</ItemTemplate>
									<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Sábado">
									<ItemTemplate>
										<asp:Label ID="_lblSabado" runat="server" Text='<%# Bind("Sábado")%>'></asp:Label>
										<asp:TextBox ID="_txtSabado" runat="server" Text='<%# Eval("Sábado") %>' Width="175px" visible="false"></asp:TextBox>
										<asp:FilteredTextBoxExtender ID="_axFiltroSabado" 
											runat="server" TargetControlID="_txtSabado" FilterMode="InvalidChars"
											InvalidChars="0123456789">
										</asp:FilteredTextBoxExtender>
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
                </td>
            </tr>
        </table>
		
	</fieldset>
</asp:Content>
