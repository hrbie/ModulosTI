<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloPEUL/MasterPEUL.master" AutoEventWireup="true" CodeBehind="GestionLugar.aspx.cs" Inherits="ModulosTICapaGUI.ModuloPEUL.GestionLugar" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPEUL" runat="server">
	<title>Gestionar Lugares</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPEUL" runat="server">
	<legend class="titulos" style="text-align: left" >Gestión de Lugares</legend>
	<fieldset class="fieldsetContornoBlanco">
		<legend class="titulos" style="text-align: left">
			Crear Lugar
		</legend>
		<div class="alinearMedioIzquierda">
			<table class="alinearIzquierda">
				<tr>
					<td class="alinearMedioIzquierda">
						<asp:Label ID="_lblNombreLugar" runat="server" Text="Nombre" CssClass="colorLetrasGeneral"></asp:Label>
					</td>
					<td class="alinearMedioIzquierda">
						<asp:TextBox ID="_txtNombreLugar" runat="server" Width="166px"></asp:TextBox>
					</td>
					<td class="alinearMedioIzquierda">
						&nbsp;</td>
					<td class="alinearMedioIzquierda">
						<asp:Label ID="_lblNombreDescripcion" runat="server" Text="Descripción" CssClass="colorLetrasGeneral"></asp:Label>
					</td>
					<td class="alinearArribaIzquierda" rowspan="4">
						<asp:TextBox ID="_txtDescripcion" runat="server" TextMode="MultiLine" 
							Height="110px" Width="455px"></asp:TextBox>
					</td>
					<td rowspan="4">
						&nbsp;
					</td>
					<td>
					</td>
				</tr>
				<tr>
					<td class="alinearMedioIzquierda">
						<asp:Label ID="_lblLoginEncargado" runat="server" Text="Login Encargado" CssClass="colorLetrasGeneral"></asp:Label>
					</td>
					<td class="alinearMedioIzquierda">
						<asp:TextBox ID="_txtLoginEncargado" runat="server" Width="166px"></asp:TextBox>
						<asp:FilteredTextBoxExtender ID="_axNombreEncargado" 
							runat="server" TargetControlID="_txtLoginEncargado" FilterMode="InvalidChars"
							InvalidChars="0123456789">
						</asp:FilteredTextBoxExtender>
					</td>
					<td class="alinearMedioIzquierda">
                        &nbsp;</td>
					<td rowspan="3">
						&nbsp;</td>
					<td>
					</td>
				</tr>
                <tr>
                    <td class="alinearMedioIzquierda">
						<asp:Label ID="_lblCapacidad" runat="server" Text="Capacidad" 
							CssClass="colorLetrasGeneral"></asp:Label>
					</td>
                    <td>
						<asp:TextBox ID="_txtCapacidad" runat="server" Width="166px"></asp:TextBox>
						<asp:FilteredTextBoxExtender ID="_axfiltroCapacidad" 
							runat="server" TargetControlID="_txtCapacidad"
							FilterType="Numbers">
						</asp:FilteredTextBoxExtender>
                    </td>
					<td>
						&nbsp;
					</td>
					<td>
					</td>
                </tr>
				<tr>
					<td class="alinearArribaIzquierda">
                        <asp:Label ID="_lblTipoLugar" runat="server" Text="Tipo" CssClass="colorLetrasGeneral"></asp:Label>
					</td>
					<td class="alinearArribaIzquierda">
                        <asp:DropDownList ID="_ddlTipoLugar" Width="172px" runat="server">
                            <asp:ListItem>Seleccionar</asp:ListItem>
                        </asp:DropDownList>
					</td>
					<td>
					</td>
					<td>
					</td>
				</tr>
				<tr>
					<td class="alinearMedioDerecha">
							<asp:Image ID="_imgMensaje" Visible="false" runat="server" />&nbsp
					</td>
					<td class="alinearMedioIzquierda" colspan="4">
						<asp:Label ID="_lblMensaje" Visible="false" runat="server"></asp:Label>
					</td>
					<td>
						<asp:Button ID="_btnCrearLugar" runat="server" Text="Crear" 
							onclick="_btnCrearLugar_Click" />
					</td>
				</tr>
			</table>
		</div>
	</fieldset>
	<fieldset class="fieldsetContornoBlanco">
		<legend class="titulos" style="text-align: left">Lugares</legend>
		<div class="centrarGrid">
			<div class="scroll">
				<asp:GridView ID="_gvwLugares" runat="server" CellPadding="4" ForeColor="#333333" 
					GridLines="None" AutoGenerateColumns="False" 
					onrowediting="_gvwLugares_RowEditing" 
					Width="925px">
					<AlternatingRowStyle BackColor="White" ForeColor="#284775" />
					<Columns>
						<asp:TemplateField HeaderText="PKLugar" Visible="false">
							<ItemTemplate>
								<asp:Label ID="_lblPKLugar" runat="server" Text='<%# Bind("PKLugar")%>'></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Lugar">
							<ItemTemplate> 
								<asp:Label ID="_lblLugares" CssClass="alineacionJustificado" runat="server" Text='<%# Bind("Lugar")%>'></asp:Label>
								<asp:TextBox ID="_txtLugares" Width="85px" Visible="false" runat="server" Text='<%# Bind("Lugar") %>'></asp:TextBox> 
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Capacidad">
							<ItemTemplate> 
								<asp:Label ID="_lblCapacidad" CssClass="alineacionJustificado" runat="server" Text='<%# Bind("Capacidad")%>'></asp:Label>
								<asp:TextBox ID="_txtCapacidad" Visible="false" Width="45px" runat="server" Text='<%# Bind("Capacidad") %>'></asp:TextBox> 
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Descripción" ControlStyle-Width="300px">
							<ItemTemplate>
								<asp:Label ID="_lblDescripcion" CssClass="alineacionJustificado" runat="server" Text='<%# Bind("Descripción")%>'></asp:Label>
								<asp:TextBox ID="_txtDescripcion" Visible="false" TextMode="MultiLine" Height="60px" Width="320px" runat="server" Text='<%# Bind("Descripción")%>'></asp:TextBox>								
							</ItemTemplate>

<ControlStyle Width="300px"></ControlStyle>
						</asp:TemplateField>
                        <asp:TemplateField HeaderText="Login Encargado">
							<ItemTemplate>
								<asp:Label ID="_lblLogin" CssClass="alineacionJustificado" runat="server" Text='<%# Bind("Login")%>'></asp:Label>
								<asp:TextBox ID="_txtLogin" Visible="false" runat="server" Text='<%# Bind("Login")%>'></asp:TextBox>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Tipo">
							<ItemTemplate>
								<asp:Label ID="_lblTipo" CssClass="alineacionJustificado" runat="server" Text='<%# Bind("Tipo")%>'></asp:Label>
								<asp:DropDownList ID="_ddlTipo" Visible="false" runat="server">
								</asp:DropDownList>
							</ItemTemplate>
						</asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado">
							<ItemTemplate>
								<asp:Label ID="_lblEstado" CssClass="alineacionJustificado" runat="server" Text='<%# Bind("Estado")%>'></asp:Label>
								<asp:DropDownList ID="_ddlEstado" Visible="false" runat="server">
									<asp:ListItem Selected="True">Seleccionar</asp:ListItem>
									<asp:ListItem>Habilitado</asp:ListItem>
									<asp:ListItem>Inhabilitado</asp:ListItem>
								</asp:DropDownList>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:CommandField CancelText="Cancelar" EditText="Editar" ShowEditButton="True" 
							UpdateText="Actualizar" ControlStyle-Width="150px" >
<ControlStyle Width="150px"></ControlStyle>
						</asp:CommandField>
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
			<div class="alineacionDerecha">
				<asp:Button ID="_btnGuardar" runat="server" Text="Guardar" Enabled="false" OnClick="_btnGuardar_Click" />&nbsp;&nbsp;
				<asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" Enabled="false" OnClick="_btnCancelar_Click" />
			</div>
		</div>
	</fieldset>
</asp:Content>
