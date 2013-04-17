<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloBitacora/MasterBitacora.master" AutoEventWireup="true" CodeBehind="RegistrarBitacora.aspx.cs" Inherits="ModulosTICapaGUI.ModuloBitacora.RegistrarBitacora" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadBitacora" runat="server">
    <title>Registrar Bitácora</title>
    <style type="text/css">
        .style1
        {
            width: 100px;
        }
        .style3
        {
            width: 160px;
        }
    </style>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBitacora" runat="server">
    <legend class="titulos" style="text-align: left" >Registrar en la bitácora
</legend>
<fieldset class="fieldsetContornoBlanco" style="text-align: left" >
    <legend class="titulos" style="text-align: left" >Laboratorio y descripción del evento 
    </legend>    
    <table style="width: 100%;">
        <tr>
            <td class="style1">
               <div class="contenedorTexto"> Laboratorio:</div>
            </td>
            <td class="style3">
                 <asp:DropDownList id="_ddlLaboratorios" runat="server" Width="150px" 
                onselectedindexchanged="_ddlLaboratorios_SelectedIndexChanged">
			    <asp:ListItem Selected="True" Value="0">Seleccionar</asp:ListItem>
			</asp:DropDownList>
		
            </td>
            <td>
            <asp:UpdatePanel ID="_upMensaje" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Image ID="_imgMensaje" Visible="false" runat="server" Height="26" 
                    style="margin-left: 32px; margin-right: 15px" />
				    <asp:Label ID="_lblMensaje" Visible="false" runat="server"></asp:Label> 
                </ContentTemplate>
            </asp:UpdatePanel>
            </td>
        </tr>
        </table>
        
        <div class="contenedorTexto"> &nbsp; Descripción:</div>
        <asp:TextBox ID="_txtComentario" runat="server" Height="100px" 
            TextMode="MultiLine" Width="415px" style="margin-left: 6px"></asp:TextBox>
        <div class=alinearMedioCentro>
        <asp:Button ID="_btnRegistrar" runat="server" Text="Registrar" 
                onclick="_btnRegistrar_Click" style="margin-left: 49px" />
        </div>    
     
</fieldset>

<div class="alinearCentro">
    <fieldset class="fieldsetContornoBlanco" style="height:300px">
        <legend class="titulos" style="text-align: left" >Eventos durante la sesión</legend>
     <asp:UpdatePanel ID="_upEventos" runat="server" UpdateMode="Conditional">
     <ContentTemplate>
       <div class="scroll" style="height:285px">
              <asp:GridView ID="_gvwEventos" runat="server" CellPadding="4" ForeColor="#333333" 
					GridLines="None" AutoGenerateColumns="False" 
					onrowcancelingedit="_gvwEventos_RowCancelingEdit" 
					onrowediting="_gvwEventos_RowEditing" onrowupdating="_gvwEventos_RowUpdating" 
					Width="925px" >
					<AlternatingRowStyle BackColor="White" ForeColor="#284775" />
					<Columns>
						<asp:TemplateField HeaderText="PK_Entrada" Visible="false">
							<ItemTemplate>
								<asp:Label ID="_lblPKEntrada" runat="server" Text='<%# Bind("PK_Entrada")%>'></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Fecha">
							<ItemTemplate>
								<asp:Label ID="_lblFecha" CssClass="alineacionJustificado" runat="server" Text='<%# Bind("Fecha")%>'></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Operador">
							<ItemTemplate>
								<asp:Label ID="_lblOperador" CssClass="alineacionJustificado" runat="server" Text='<%# Bind("Operador")%>'></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Evento" ControlStyle-Width="300px">
							<EditItemTemplate>
								<asp:TextBox ID="_txtEvento" TextMode="MultiLine" Height="60px" Width="320px" runat="server" Text='<%# Bind("Evento")%>'></asp:TextBox>
							</EditItemTemplate>
							<ItemTemplate>
								<asp:Label ID="_lblEvento" CssClass="alineacionJustificado" runat="server" Text='<%# Bind("Evento")%>'></asp:Label>
							</ItemTemplate>
						    <ControlStyle Width="300px" />
						</asp:TemplateField>
                       	<asp:CommandField CancelText="Cancelar" EditText="Editar" ShowEditButton="True" 
							    UpdateText="Actualizar" ControlStyle-Width="150px" visible="True" HeaderText="Acciones">
                       
					    <ControlStyle Width="150px" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</div>
</asp:Content>
