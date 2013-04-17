<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloReservacion/MasterReservacion.master" AutoEventWireup="true" CodeBehind="ModificaReservacion.aspx.cs" Inherits="ModulosTICapaGUI.ModuloReservacion.ModificaReservacion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadReservacion" runat="server">
    <title>Modificar Reservación</title>
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
    <fieldset  class="fieldsetContornoBlanco">
		<legend class="titulos" style="text-align: left">
			Gestión de Reservación
		</legend>
        <div class="alinearMedioIzquierda">
			<table class="alinearIzquierda">
				<tr>
					<td class="alinearMedioDerecha">
							<asp:Image ID="_imgMensajeMod" Visible="false" runat="server" />&nbsp
					</td>
					<td class="alinearMedioIzquierda" colspan="4">
						<asp:Label ID="_lblMensajeMod" Visible="false" runat="server"></asp:Label>
					</td>
				</tr>
			</table>
		</div>
        <div class="centrarGrid" style="width:auto;height:auto">
			<div class="scroll" style="width:auto;height:auto">
         <asp:GridView ID="_gridExcepciones" runat="server" CellPadding="4" ForeColor="#333333" 
					GridLines="None" AutoGenerateColumns="False" Width="250px">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775"/>
                    <Columns>
						<asp:TemplateField HeaderText="Fecha" >
							<ItemTemplate>
								<asp:Label ID="_lblFechaExcepcion" CssClass="alineacionJustificado" Text='<%# Bind("Fecha")%>' runat="server" ></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Motivo">
							<ItemTemplate> 
								<asp:Label ID="_lblMotivo" CssClass="alineacionJustificado" Text='<%# Bind("Motivo")%>' runat="server" ></asp:Label>
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
	    </div>
			</div>
         <asp:Table ID="_treservacion" runat="server" style="width: 960px" visible="false" class="alinearIzquierda">
		    <asp:TableRow runat="server">
			    <asp:TableCell runat="server" class="alinearCentro" colspan="6">
				    <asp:Label ID="_lblEncabezado" runat="server" CssClass="colorLetrasGeneral" Text="Todos los campos del formulario son obligatorios exceptuando la descripción de la reservación"></asp:Label>
                    <asp:Label ID="_lblPKReserv" runat="server" Visible="false"></asp:Label>
			    </asp:TableCell>
		    </asp:TableRow>

		    <asp:TableRow>
			    <asp:TableCell runat="server" colspan="6"></asp:TableCell>
		    </asp:TableRow>
		    
            <asp:TableRow>
                <asp:TableCell runat="server" class="style2"></asp:TableCell>
			    <asp:TableCell runat="server" class="alinearMedioDerecha">
				    <asp:Label id="_lblReservar" runat="server" Text="Laboratorio/Aula: " CssClass="colorLetrasGeneral"></asp:Label>
			    </asp:TableCell>
			    <asp:TableCell runat="server">
				    <asp:DropDownList id="_ddlLugar" runat="server" Width="140px" 
					    onselectedindexchanged="_ddlLugar_SelectedIndexChanged">
					    <asp:ListItem Selected="True">Seleccionar</asp:ListItem>
				    </asp:DropDownList>
			    </asp:TableCell>
			    <asp:TableCell runat="server" class="alinearMedioDerecha"  Width="5%">
				    <asp:Label ID="_lblCapacidad" Text="Capacidad: " runat="server" CssClass="colorLetrasGeneral"></asp:Label>
			    </asp:TableCell>
			    <asp:TableCell runat="server" ColumnSpan="2" class="AlinearMedioIzquierda">
				    <asp:UpdatePanel ID="_upPanelCantidad" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
					    <ContentTemplate>
						    <asp:Label ID="_lblCantidad" runat="server" CssClass="colorLetrasGeneral" Width="20px"></asp:Label>
						    <asp:Label ID="_lblPersonas" Text="personas" runat="server" CssClass="colorLetrasGeneral"></asp:Label>
					    </ContentTemplate>
				    </asp:UpdatePanel>
                
			    </asp:TableCell>
			    <asp:TableCell runat="server" colspan="2">
				    <asp:Label ID="_lblChoques" Visible="false" CssClass="colorLetrasGeneral" runat="server" Text="A continuación se presentan los choques encontrados"></asp:Label>
			    </asp:TableCell>
		    </asp:TableRow>

		    <asp:TableRow>
                <asp:TableCell  runat="server" class="style2"></asp:TableCell>
                <asp:TableCell  runat="server" class="alinearMedioDerecha">
				    <asp:Label ID="_lblEncargado" Text="Encargado:" runat="server" CssClass="colorLetrasGeneral"></asp:Label>
			    </asp:TableCell>
                <asp:TableCell runat="server" class="alinearMedioIzquierda">
				    <asp:UpdatePanel ID="_upEncargado" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
					    <ContentTemplate>
						    <asp:Label ID="_lblNombreEncargado" runat="server" CssClass="colorLetrasGeneral"></asp:Label>
					    </ContentTemplate>
				    </asp:UpdatePanel>
			    </asp:TableCell>
			    <asp:TableCell runat="server" class="alinearMedioDerecha"  Width="3%">
				    <asp:Label ID="_lblDescripción" Text="Descripción: " runat="server" CssClass="colorLetrasGeneral"></asp:Label>
			    </asp:TableCell>
			    <asp:TableCell runat="server" rowspan="2" class="style6">
				    <asp:UpdatePanel ID="_upDescripcion" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
					    <ContentTemplate>
						    <asp:TextBox ID="_txtDescripcion" Enabled="false" runat="server" Height="100px" 
							    TextMode="MultiLine" Width="140px"></asp:TextBox>
					    </ContentTemplate>
				    </asp:UpdatePanel>
			    </asp:TableCell>
			    <asp:TableCell runat="server" class="alineacionCentral" colspan="2" rowspan="5">
				    <asp:TextBox ID="_txtChoques" runat="server" Enabled="False" Height="153px" 
					    TextMode="MultiLine" Visible="false" Width="241px"></asp:TextBox>
			    </asp:TableCell>
		    </asp:TableRow>

		    <asp:TableRow>
                <asp:TableCell runat="server" class="style2"></asp:TableCell>
			    <asp:TableCell runat="server" class="alinearMedioDerecha" RowSpan="1">
				    <asp:Label ID="_lblTipo" runat="server" Text="Tipo: " CssClass="colorLetrasGeneral"></asp:Label>
			    </asp:TableCell>
			    <asp:TableCell runat="server" class="alinearMedioIzquierda">
				    <asp:UpdatePanel ID="_upTipo" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
					    <ContentTemplate>
						    <asp:Label ID="_lblNombreTipo" runat="server" CssClass="colorLetrasGeneral"></asp:Label>
					    </ContentTemplate>
				    </asp:UpdatePanel>
			    </asp:TableCell>
		    </asp:TableRow>

		    <asp:TableRow>
			    <asp:TableCell runat="server" class="style3"></asp:TableCell>
			    <asp:TableCell runat="server" class="style5"></asp:TableCell>
			    <asp:TableCell runat="server" class="style1"></asp:TableCell>
			    <asp:TableCell runat="server" class="style6"></asp:TableCell>
		    </asp:TableRow>
            <asp:TableRow>
			    <asp:TableCell ID="TableCell1" runat="server" class="style3"></asp:TableCell>
			    <asp:TableCell ID="TableCell2" runat="server" class="style5"></asp:TableCell>
			    <asp:TableCell ID="TableCell3" runat="server" class="style1"></asp:TableCell>
			    <asp:TableCell ID="TableCell4" runat="server" class="style6"></asp:TableCell>
 
		    </asp:TableRow>
            
		    <asp:TableRow>
                <asp:TableCell runat="server" class="style2"></asp:TableCell>
			    <asp:TableCell runat="server" class="alinearMedioDerecha" Width="3%">
				    <asp:Label id="_lblCarrera" runat="server" Text="Carrera: " CssClass="colorLetrasGeneral"></asp:Label>
			    </asp:TableCell>
			    <asp:TableCell runat="server" class="style5">
				    <asp:DropDownList id="_ddlCarrera" runat="server" Width="140px" 
					    onselectedindexchanged="_ddlCarrera_SelectedIndexChanged">
					    <asp:ListItem Selected="True">Seleccionar</asp:ListItem>
				    </asp:DropDownList>
			    </asp:TableCell>
			    <asp:TableCell runat="server" class="alinearMedioDerecha" >
				    <asp:Label id="_lblCurso" runat="server" Text="Curso: " CssClass="colorLetrasGeneral"></asp:Label>
			    </asp:TableCell>
			    <asp:TableCell runat="server" class="style6">
				    <asp:UpdatePanel ID="_upCurso" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
					    <ContentTemplate>
						    <asp:DropDownList id="_ddlCurso" runat="server" Width="140px" Enabled="False">
						    </asp:DropDownList>
					    </ContentTemplate>
				    </asp:UpdatePanel>
			    </asp:TableCell>
		    </asp:TableRow>

		    <asp:TableRow>
                <asp:TableCell ID="TableCell5" runat="server" class="style2"></asp:TableCell>
			    <asp:TableCell runat="server" class="alinearMedioDerecha">
				    <asp:Label runat="server" Text="Login Solicitante: " id="_lblLogin" CssClass="colorLetrasGeneral"></asp:Label>
			    </asp:TableCell>
			    <asp:TableCell runat="server" class="style5">
				    <asp:TextBox runat="server" id="_txtLogin" Width="140px" MaxLength="20"></asp:TextBox>
			    </asp:TableCell> 
			    <asp:TableCell runat="server" class="alinearMedioDerecha"  Width="3%">
				    <asp:Label runat="server" Text="Descripción de la Reservación: " id="_lblDescripcion" CssClass="colorLetrasGeneral"></asp:Label>
			    </asp:TableCell>
			    <asp:TableCell runat="server" class="style6">
				    <asp:TextBox runat="server" id="_txtDescripcionReservacion" TextMode="MultiLine" Height="100px" Width="140px"></asp:TextBox>
			    </asp:TableCell>
		    </asp:TableRow>

            <asp:TableRow>
			    <asp:TableCell ID="TableCell6" runat="server" class="style3"></asp:TableCell>
			    <asp:TableCell ID="TableCell7" runat="server" class="style5"></asp:TableCell>
			    <asp:TableCell ID="TableCell8" runat="server" class="style1"></asp:TableCell>
			    <asp:TableCell ID="TableCell9" runat="server" class="style6"></asp:TableCell>
		    </asp:TableRow>
            <asp:TableRow>
			    <asp:TableCell ID="TableCell10" runat="server" class="style3"></asp:TableCell>
			    <asp:TableCell ID="TableCell11" runat="server" class="style5"></asp:TableCell>
			    <asp:TableCell ID="TableCell12" runat="server" class="style1"></asp:TableCell>
			    <asp:TableCell ID="TableCell13" runat="server" class="style6"></asp:TableCell>
		    </asp:TableRow>
            
            <asp:TableRow>
                 <asp:TableCell runat="server" class="style2"></asp:TableCell>
			    <asp:TableCell runat="server" class="alinearMedioDerecha">
                    <asp:Label runat="server" Text="Fecha Inicio:" id="_lblFechaInicio" CssClass="colorLetrasGeneral"></asp:Label>
                 </asp:TableCell>
                <asp:TableCell runat="server" class="style5">
                    <asp:TextBox runat="server" id="_txtFechaInicio" Width="120px" 
					    ontextchanged="_txtFechaInicio_TextChanged" Enabled="False"></asp:TextBox>
                    <asp:Image ID="_imgFechaInicio" ImageUrl="~/Imagenes/Calendario.png" 
					    runat="server" ToolTip="Presione aquí para abrir el calendario" />
                    <asp:CalendarExtender ID="_axCalendarioInicio" runat="server"
                            TargetControlID="_txtFechaInicio" PopupButtonID="_imgFechaInicio" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                </asp:TableCell>
                <asp:TableCell runat="server" class="alinearMedioDerecha">
                    <asp:Label runat="server" Text="Fecha Final:" id="_lblFechaFinal" CssClass="colorLetrasGeneral"></asp:Label>
                </asp:TableCell>
                <asp:TableCell runat="server" class="style6"><asp:TextBox runat="server" id="_txtFechaFinal" Width="120px" Enabled="False" 
					    ontextchanged="_txtFechaFinal_TextChanged"></asp:TextBox>
                    <asp:Image ID="_imgFechaFinal" ImageUrl="~/Imagenes/Calendario.png" 
					    runat="server" ToolTip="Presione aquí para abrir el calendario" />
                    <asp:CalendarExtender ID="_axCalendarioFinal" runat="server"
                            TargetControlID="_txtFechaFinal" PopupButtonID="_imgFechaFinal" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
			    </asp:TableCell>
		    </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell  runat="server" class="style2"></asp:TableCell>
			    <asp:TableCell runat="server" class="alinearMedioDerecha">
                    <asp:Label runat="server" Text="Hora Inicio:" id="_lblHoraInicio" CssClass="colorLetrasGeneral"></asp:Label>
                 </asp:TableCell> 
                <asp:TableCell runat="server" class="style5"><asp:DropDownList ID="_ddlHoraInicio" runat="server" Width="53px">
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
                    <asp:DropDownList ID="_ddlMinutoInicio" Width="72px" runat="server">
                        <asp:ListItem>Minuto</asp:ListItem>
                        <asp:ListItem>00</asp:ListItem>
                        <asp:ListItem>30</asp:ListItem>
                    </asp:DropDownList>
                </asp:TableCell>
                <asp:TableCell runat="server" class="alinearMedioDerecha">
                    <asp:Label runat="server" Text="Hora Final:" id="_lblHoraFinal" CssClass="colorLetrasGeneral"></asp:Label>
                 </asp:TableCell> 
                <asp:TableCell runat="server" class="style6">
                 <asp:DropDownList ID="_ddlHoraFinal" runat="server" Width="53px">
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
                    <asp:DropDownList ID="_ddlMinutoFinal" Width="72px" runat="server">
                        <asp:ListItem>Minuto</asp:ListItem>
                        <asp:ListItem>00</asp:ListItem>
                        <asp:ListItem>30</asp:ListItem>
                    </asp:DropDownList>
                </asp:TableCell>
 
		    </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell  runat="server" class="alinearCentro"></asp:TableCell> 
                <asp:TableCell runat="server" class="alinearMedioDerecha">
                    <asp:Label runat="server" Text="Días:" id="_lblDias" CssClass="colorLetrasGeneral"></asp:Label>
                </asp:TableCell> 
                <asp:TableCell ColumnSpan="4">
				    <asp:UpdatePanel ID="_upDias" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
					    <ContentTemplate>
						    <asp:CheckBox ID="_cbLunes" Text="L" runat="server" />&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
						    <asp:CheckBox ID="_cbMartes" Text="K" runat="server" />&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
						    <asp:CheckBox ID="_cbMiercoles" Text="M" runat="server" />&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
						    <asp:CheckBox ID="_cbJueves" Text="J" runat="server" />&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
						    <asp:CheckBox ID="_cbViernes" Text="V" runat="server" />&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
						    <asp:CheckBox ID="_cbSabado" Text="S" runat="server" />&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
						    <asp:CheckBox ID="_cbDomingo" Text="D" runat="server" />
					    </ContentTemplate>
				    </asp:UpdatePanel>
                </asp:TableCell>
            </asp:TableRow>


		    <asp:TableRow>
			    <asp:TableCell runat="server" class="alinearCentro"></asp:TableCell> 
                <asp:TableCell runat="server" class="alinearCentro"></asp:TableCell> 
                <asp:TableCell  runat="server" colspan="2" class="alinearMedioIzquierda">
				    <asp:CheckBox ID="_cbHorario" Text="Marcar como horario permanente" CssClass="colorLetrasGeneral" runat="server" />
			    </asp:TableCell>
		    </asp:TableRow>

            <asp:TableRow>
			    <asp:TableCell ID="TableCell14" runat="server" class="style3"></asp:TableCell>
			    <asp:TableCell ID="TableCell15" runat="server" class="style5"></asp:TableCell>
			    <asp:TableCell ID="TableCell16" runat="server" class="style1"></asp:TableCell>
			    <asp:TableCell ID="TableCell17" runat="server" class="style6"></asp:TableCell>
		    </asp:TableRow>
            <asp:TableRow>
			    <asp:TableCell ID="TableCell18" runat="server" class="style3"></asp:TableCell>
			    <asp:TableCell ID="TableCell19" runat="server" class="style5"></asp:TableCell>
			    <asp:TableCell ID="TableCell20" runat="server" class="style1"></asp:TableCell>
			    <asp:TableCell ID="TableCell21" runat="server" class="style6"></asp:TableCell>
		    </asp:TableRow>
             
            <asp:TableRow>
			    <asp:TableCell  runat="server" class="alinearCentro"></asp:TableCell> 
                <asp:TableCell  runat="server" class="alinearMedioDerecha">
                         <asp:Label ID="_lblExcepcion" runat="server" Text="Excepcion" class="colorLetrasGeneral"></asp:Label>
			    </asp:TableCell> 
                <asp:TableCell  runat="server" class="style6">
                    <asp:TextBox runat="server" id="_txtFechaExcepcion" Width="140px" Enabled="False" 
					    ></asp:TextBox>
                    <asp:Image ID="_imgFechaExcepcion" ImageUrl="~/Imagenes/Calendario.png" 
					    runat="server" ToolTip="Presione aquí para abrir el calendario" />
                    <asp:CalendarExtender ID="_ceFechaExcepcion" runat="server"
                            TargetControlID="_txtFechaExcepcion" PopupButtonID="_imgFechaExcepcion" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
			    </asp:TableCell> 
                <asp:TableCell runat="server" rowspan="2" colspan="2" class="alinearCentro">
                         <asp:Button ID="_btnExcepcion" runat="server" Text="Agregar" OnClick="_btnAgregarExcepcion_Click"
                          Width="140px">
                         </asp:Button>
			    </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell  runat="server" class="alinearCentro"></asp:TableCell> 
                <asp:TableCell  runat="server" class="alinearMedioDerecha">
                         <asp:Label ID="_lblMotivo" runat="server" Text="Motivo" class="colorLetrasGeneral"></asp:Label>
			    </asp:TableCell>
                <asp:TableCell runat="server" >
                         <asp:DropDownList ID="_ddlExcepcion" runat="server"  Width="140px">
                            <asp:ListItem>Cancelar Dia</asp:ListItem>
                            <asp:ListItem>Agregar Dia</asp:ListItem>
                         </asp:DropDownList>
			    </asp:TableCell> 
                </asp:TableRow>

		    <asp:TableRow>
			    <asp:TableCell runat="server" colspan="6" class="alinearCentro">
			    </asp:TableCell> 
		    </asp:TableRow>

		    <asp:TableRow>
                <asp:TableCell runat="server" class="alinearCentro">
			    </asp:TableCell>
			    <asp:TableCell runat="server" class="alinearMedioDerecha">
                    <asp:Button ID="_btnCancelarReservacion" Text="Cancelar Reservacion" CausesValidation="true"  OnClick="_btnCancelaReservacion_Click"
					    ToolTip="Presione este botón para cancelar la reservación" runat="server" Width ="140px" />				   
			    </asp:TableCell> 
                <asp:TableCell runat="server" colspan="2"  class="alinearCentro">
				    <asp:Button ID="_btnModificar" Text="Modificar" CausesValidation="true" OnClick="_btnModificar_Click" 
					    ToolTip="Presione este botón para Modificar la reservación" runat="server" Width ="140px" />
			    </asp:TableCell>
                <asp:TableCell  runat="server"  class="alinearMedioIzquierda">
				    <asp:Button ID="_btnCancelar" Text="Cancelar Accion" CausesValidation="true"  OnClick="_btnCancelar_Click"
					    ToolTip="Presione este botón para no modificar la reservación" runat="server" Width ="140px" />
			    </asp:TableCell>
		    </asp:TableRow>
	    </asp:Table>
	    <div class="centrarGrid" style="width:auto;height:auto">
			<div class="scroll" style="width:auto;height:auto">
				<asp:GridView ID="_gridReservaciones" runat="server" CellPadding="4" ForeColor="#333333" 
					GridLines="None" AutoGenerateColumns="False" onrowediting="_gridReservaciones_RowEditing" 
					Width="925px">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775"/>
                    <Columns>
						<asp:TemplateField HeaderText="PKReservacion" Visible="false">
							<ItemTemplate>
								<asp:Label ID="_lblPKReservacion" runat="server" Text='<%# Bind("PKReservacion")%>'></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Dias">
							<ItemTemplate> 
								<asp:Label ID="_lblDia" CssClass="alineacionJustificado" runat="server" Text='<%# Bind("Dias")%>'></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Hora Inicio">
							<ItemTemplate> 
								<asp:Label ID="_lblHoraInicio" CssClass="alineacionJustificado" runat="server" Text='<%# Bind("Hora_Inicio")%>'></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Hora Final">
							<ItemTemplate>
								<asp:Label ID="_lblHoraFinal" CssClass="alineacionJustificado" runat="server" Text='<%# Bind("Hora_Final")%>'></asp:Label>								
							</ItemTemplate>
						</asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha Inicio">
							<ItemTemplate>
								<asp:Label ID="_lblfechaInicio" CssClass="alineacionJustificado" runat="server" Text='<%# Bind("Fecha_Inicio")%>'></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Fecha Final">
							<ItemTemplate>
								<asp:Label ID="_lblfechaFinal" CssClass="alineacionJustificado" runat="server" Text='<%# Bind("Fecha_Final")%>'></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
                        <asp:TemplateField HeaderText="Solicitante">
							<ItemTemplate>
								<asp:Label ID="_lblSolicitante" CssClass="alineacionJustificado" runat="server" Text='<%# Bind("Solicitante")%>'></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
                        <asp:TemplateField HeaderText="Curso">
							<ItemTemplate>
								<asp:Label ID="_lblCurso" CssClass="alineacionJustificado" runat="server" Text='<%# Bind("Curso")%>'></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
                        <asp:TemplateField HeaderText="Descripcion">
							<ItemTemplate>
								<asp:Label ID="_lblDescripcion" CssClass="alineacionJustificado" runat="server" Text='<%# Bind("Descripcion")%>'></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
                        <asp:TemplateField HeaderText="Lugar">
							<ItemTemplate> 
								<asp:Label ID="_lblLugar" CssClass="alineacionJustificado" runat="server" Text='<%# Bind("Lugar") %>'></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
                        <asp:TemplateField HeaderText="Carrera" Visible="False">
							<ItemTemplate>
								<asp:Label ID="_lblCarreraReserv" runat="server" Text='<%# Bind("Carrera")%>'></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:CommandField CancelText="Cancelar" EditText="Editar" ShowEditButton="True" 
							UpdateText="Actualizar" ControlStyle-Width="150px" >
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
		</div>
    </fieldset>
</asp:Content>
