<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloActivos/MasterActivos.master" AutoEventWireup="true" CodeBehind="ConsultarActivo.aspx.cs" Inherits="ModulosTICapaGUI.ModuloActivos.ConsultarActivo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadActivos" runat="server">
    <title>Consultar Activos</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainActivos" runat="server">
    <legend class="titulos">Consultar Activos</legend>
    <fieldset class="fieldsetContornoBlanco">
		<legend class="titulos" style="text-align: left">
			Criterios de consulta
		</legend>
       <div class="alinearMedioIzquierda">
       <asp:UpdatePanel ID="_upPanelBusqueda" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
	   <ContentTemplate>
		<table class="alinearIzquierda">
            <tr>
                <td class="style1"></td>
                <td class="style1"> 
                    <asp:RadioButton ID="f1" runat="server" GroupName="_rbfiltro" OnCheckedChanged="_rbfiltro1_OnChecked" />
                </td>
                <td class="style1">
				    <asp:Label id="_lblDescrip" runat="server" Text="Descripción:" CssClass="colorLetrasGeneral" Height="21px"></asp:Label>
			    </td>
			    <td class="style4">
				    <asp:TextBox runat="server" id="_txtDescrip" Width="140px" ></asp:TextBox>
                
			    </td>
            </tr>
            <tr>
                <td class="style1"></td>
                <td class="style1">
                    <asp:RadioButton ID="f2" runat="server" GroupName="_rbfiltro" OnCheckedChanged="_rbfiltro2_OnChecked" />
                </td>
                <td class="style3"><asp:Label runat="server" Text="Código" id="_lblCodigo" CssClass="colorLetrasGeneral"></asp:Label></td>
                <td class="style5">
                    <asp:TextBox runat="server" id="_txtCodigo" Width="140px" ></asp:TextBox>
                </td>
                <td class="style1"></td>
            </tr>
            <tr>
                <td class="style1"></td>
                <td class="style1">
                    <asp:RadioButton ID="f3" runat="server"  GroupName="_rbfiltro" OnCheckedChanged="_rbfiltro3_OnChecked" />
                </td>
                <td class="style1"><asp:Label runat="server" Text="Estado" id="_lblEstado" CssClass="colorLetrasGeneral"></asp:Label></td>
                
                <td>
                    <asp:DropDownList id="_ddlEstado" runat="server" Width="143px" >
				    </asp:DropDownList>
			    </td>
            </tr>
            <tr>
              <td class="style1"></td>
                <td class="style1">
                    <asp:RadioButton ID="f4" runat="server"  GroupName="_rbfiltro" OnCheckedChanged="_rbfiltro4_OnChecked" Visible="false"/>
                </td>
              <td class="style1"><asp:Label runat="server" Text="Descripcion" id="_lblDescripcion" CssClass="colorLetrasGeneral" visible="false"></asp:Label></td>
                 <td>
                    <asp:TextBox runat="server" id="_txtDescripcion" Width="140px" visible="false" ></asp:TextBox>
			    </td>
            </tr>
        </table>
       </ContentTemplate>
	   </asp:UpdatePanel>
       <table class="alinearIzquierda">
       <tr>
                <td class="style1"></td>
                <td class="style1"></td>
                <td class="style1"></td>
                <td class="style1"></td>
                <td class="style1"></td>
                <td class="style1"></td>
                <td class="style1"></td>
                 <td class="alinearMedioCentro" rowspan="2" colspan="3">
                    <asp:Button ID="_btnConsultar" Text="Consultar" CausesValidation="true" OnClick="_btnConsultar_Click" 
					    ToolTip="Presione este botón para consultar el activo" runat="server" />
                </td>
                <td class="style1"></td>
                <td class="alinearMedioDerecha">
				    <asp:Image ID="_imgMensaje" Visible="false" runat="server" />&nbsp
			    </td>
			    <td class="alinearMedioIzquierda">
				    <asp:Label ID="_lblMensaje" Visible="false" runat="server"></asp:Label>
			    </td>
            </tr>
            </table>
       </div>
    </fieldset>
    <fieldset class="fieldsetContornoBlanco">
		<legend class="titulos" style="text-align: left">Activos</legend>
		<div class="centrarGrid">
			<div class="scroll">
				<asp:GridView ID="_gridActivos" runat="server" AutoGenerateColumns="True" 
                            CellPadding="4" ForeColor="#333333" GridLines="None" Width="925px" 
                            Height="255px" Visible="false">
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

