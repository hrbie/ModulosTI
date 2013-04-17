<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloBitacora/MasterBitacora.master" AutoEventWireup="true" CodeBehind="ConsultarBitacora.aspx.cs" Inherits="ModulosTICapaGUI.ModuloBitacora.ConsultarBitacora" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadBitacora" runat="server">
    <title>Consultar Bitácora</title>
    <style type="text/css">
        .style1
        {
            width: 114px;
        }
        .style2
        {
            width: 185px;
        }
        .style6
        {
            width: 876px;
        }
        .style7
        {
            width: 95px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBitacora" runat="server">
    <legend class="titulos" style="text-align: left" >Consultar bitácora</legend>
<fieldset class="fieldsetContornoBlanco">
    <legend class="titulos" style="text-align: left" >Laboratorio y fecha</legend>
    <table style="width: 100%;">
        <tr>
            <td class="style1">
               <div class="contenedorTexto"> Laboratorio:</div>
            </td>
            <td class="style2" style="text-align:left">
                 <asp:DropDownList id="_ddlLaboratorios" runat="server" Width="150px" 
                     style="margin-left: 0px">
			        <asp:ListItem Selected="True" Value="0">Seleccionar</asp:ListItem>
			    </asp:DropDownList>
		
            </td>
            <td class="style6" style="text-align:left">
            
            <asp:UpdatePanel ID="_upMensaje" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Image ID="_imgMensaje" Visible="false" runat="server" Height="26" 
                    style="margin-left: 0px; margin-right: 15px" />
				    <asp:Label ID="_lblMensaje" Visible="false" runat="server"></asp:Label> 
                </ContentTemplate>
            </asp:UpdatePanel>
            </td>
            
        </tr>
        <tr>
            <td class="style1">
               <div class="contenedorTexto"> En la fecha: </div> 
            </td>
            <td class="style2" style="text-align:left">
                 <asp:TextBox runat="server" id="_txtFechaConsulta" Width="146px" Enabled="false"></asp:TextBox>
                    <asp:Image ID="_imgFechaFinal" ImageUrl="~/Imagenes/Calendario.png" 
			            runat="server" ToolTip="Presione aquí para abrir el calendario"/>
                    <asp:CalendarExtender ID="_ajCalendarExtenderConsulta" runat="server"
                        TargetControlID="_txtFechaConsulta" PopupButtonID="_imgFechaFinal" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
              </td>
            <td class="style6" style="text-align:left">

                <table style="width: 100%;">
                <tr>
                <td style="text-align:left" class="style7"><asp:Button ID="_btnConsultar" runat="server" Text="Consultar" onclick="_btnConsultar_Click"/>  </td>
                <td style="text-align:left">
                    <asp:Button ID="_btnExportarExcel" runat="server" 
                        Text="Exportar a Excel" onclick="_btnExportarExcel_Click" /></td>
                </tr>
                </table>
                              
            </td>
            
        </tr>
        </table>
        </fieldset>  
    <div/>
       

<div class="alinearCentro">
    <fieldset class="fieldsetContornoBlanco" style="height:415px">
        <legend class="titulos" style="text-align: left" >Eventos del día</legend>
          <div class="scroll" style="height:400px ">
           <asp:UpdatePanel ID="_upEvento" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
			 <ContentTemplate>
                <asp:GridView ID="_gvwEventos" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" ForeColor="#333333" GridLines="None" 
                        Width="100%">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="fecha" HeaderText="Fecha" >
                        <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Operador" HeaderText="Operador" >
                        <HeaderStyle Width="200px" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Evento" HeaderText="Evento" >
                        <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
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
        </div>
   
</asp:Content>
