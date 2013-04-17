<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloPEUL/MasterPEUL.master" AutoEventWireup="true" CodeBehind="GenerarEstadisticaUso.aspx.cs" Inherits="ModulosTICapaGUI.ModuloPEUL.GenerarEstadisticaUso" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="HeadPEUL" runat="server">
        <title>Estadística de Uso</title>
</asp:Content>
          
<asp:Content ID="Content2" ContentPlaceHolderID="MainPEUL" runat="server">
        <legend class="titulos" style="text-align: left" >Estadísticas de uso</legend>
            <fieldset class="fieldsetContornoBlanco">
            <legend class="titulos" style="text-align: left">
                Filtros
            </legend>
                <div class="alinearMedioIzquierda">
                    <div class="contenedorTexto">
                        Laboratorio: 
                        &nbsp;
                    </div>
                    <asp:DropDownList id="_ddlLaboratorios" runat="server" Width="150px">
					    <asp:ListItem Selected="True">Seleccionar</asp:ListItem>
					</asp:DropDownList>
                </div>
                <div class="alinearMedioIzquierda">
                     <div class="contenedorTexto">
                        Mostrar: 
                        &nbsp; 
                     </div>
                     <div>
                         <asp:DropDownList ID="_ddlMostrar" runat="server" Width="150px" 
                            onselectedindexchanged="_ddlMostrar_SelectedIndexChanged">
                            <asp:ListItem Value="0">Seleccionar</asp:ListItem>
                            <asp:ListItem Value="1">Diario</asp:ListItem>
                            <asp:ListItem Value="2">Mensual</asp:ListItem>
                            <asp:ListItem Value="3">Semestral</asp:ListItem>
                            <asp:ListItem Value="4">Anual</asp:ListItem>
                         </asp:DropDownList>
                     </div>
                </div>
                <div class="alinearMedioIzquierda">
                    <div class="contenedorTexto">
                        Desde el: 
                        &nbsp;
                     </div>
                     <div style="vertical-align: middle; float:left">
                         <asp:UpdatePanel ID="_upSemestreInicial" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
					        <ContentTemplate>
                                <asp:DropDownList id="_ddlSemestreInicio" runat="server" Width="150px" 
                                    Enabled="false">
					                <asp:ListItem Selected="True">Semestre</asp:ListItem>
				                </asp:DropDownList>
                            </ContentTemplate>
                         </asp:UpdatePanel>
                     </div>
                     <div style="vertical-align: middle; float:left ">
                        <div style="text-align: right">
                            &nbsp;
                            &nbsp;
                            Hasta el:   
                            &nbsp;
                        </div>
                     </div>
                     <div>
                         <asp:UpdatePanel ID="_upSemestreFinal" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
					        <ContentTemplate>
                                &nbsp;
                                 <asp:DropDownList id="_ddlSemestreFinal" runat="server" Width="144px" 
                                        Enabled="false">
					                    <asp:ListItem Selected="True">Semestre</asp:ListItem>
				                    </asp:DropDownList>
                             </ContentTemplate>
                         </asp:UpdatePanel>
                     </div>
                </div>
                <div class="alinearMedioIzquierda">
                    <div class="contenedorTexto">
                        Desde: 
                        &nbsp;
                     </div>
                     <div style="vertical-align: middle; float:left">
                         <asp:UpdatePanel ID="_upFechaIncio" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
					        <ContentTemplate>
                                <asp:TextBox runat="server" id="_txtFechaInicio" Width="140px" 
								    Enabled="False"></asp:TextBox>
                                <asp:Image ID="_imgFechaInicio" ImageUrl="~/Imagenes/Calendario.png" 
								    runat="server" ToolTip="Presione aquí para abrir el calendario" />
                                <asp:CalendarExtender ID="_axCalendarioInicio" runat="server"
                                     TargetControlID="_txtFechaInicio" PopupButtonID="_imgFechaInicio" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                                &nbsp;
                                &nbsp;
                            </ContentTemplate>
                        </asp:UpdatePanel>
                     </div>
                     <div style="vertical-align: middle; float:left ">
                        <div style="text-align: right">
                            Hasta: 
                            &nbsp;
                            &nbsp;
                        </div>
                     </div>
                     <div>
                        <asp:UpdatePanel ID="_upFechaFinal" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
					       <ContentTemplate>
                                <asp:TextBox runat="server" id="_txtFechaFinal" Width="140px" 
						           Enabled="False"></asp:TextBox>
                                <asp:Image ID="_imgFechaFinal" ImageUrl="~/Imagenes/Calendario.png" 
							       runat="server" ToolTip="Presione aquí para abrir el calendario" />
                                <asp:CalendarExtender ID="_axCalendarioFinal" runat="server"
                                   TargetControlID="_txtFechaFinal" PopupButtonID="_imgFechaFinal" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                                &nbsp;
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="alinearMedioDerecha">
                            <div class="alinearMedioDerecha" style="padding-left: 415px; float:left ">
                                <asp:Button ID="_btnGraficar" runat="server" Text="Graficar" 
                                ToolTip="Generar gráfica" onclick="_btnGraficar_Click" 
                                    style="margin-left: 24px" />
                                &nbsp;
                                &nbsp;
                                &nbsp;
                                
                            </div>
                              <div class="alinearMedioIzquierda">
                                <asp:UpdatePanel ID="_upMensaje" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
					                <ContentTemplate>
                                    <asp:Image ID="_imgMensaje" runat="server" Visible="false" />
                                    <asp:Label ID="_lblMensaje" runat="server" CssClass="colorLetrasGeneral" 
                                            Visible="false"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                         </div>
                     </div>
                </div>
            </fieldset>
        <div style="height:373px">
        <fieldset class="fieldsetContornoBlanco" >
            <legend class="titulos" style="text-align: left">Gráficos</legend>
            <table style="width: 100%;">
                <tr>
                    <td>
                        <fieldset>
                            <legend class="titulos" style="text-align: left">Equipo del Instituto Tecnológico de 
                                CR</legend>
                            <asp:UpdatePanel runat="server" id="UpdatePanel" updatemode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger controlid="_btnGraficar" eventname="Click" />
                </Triggers>
                    <ContentTemplate>
                        <asp:Chart ID="_chtEstadisticas" runat="server" Height="294px" Width="430px" 
                            BackColor="LightBlue" BackSecondaryColor="AliceBlue" BackGradientStyle="TopBottom" 
                                   BorderlineColor="MidnightBlue" BorderlineWidth="2" 
                            BorderSkin-SkinStyle="Sunken" BorderSkin-BackHatchStyle="DarkVertical"  >
                            <Series>
                                 <asp:Series Name="_serieUso" IsValueShownAsLabel="true" 
                                     BorderWidth="2" BorderDashStyle="Solid" ShadowOffset="2" Legend="true"
                                     BackSecondaryColor="White" MarkerStyle="Circle" MarkerSize="7" 
                                     ChartType="Line" >

                                    <SmartLabelStyle Enabled="True" />
                                 </asp:Series>
                        
                             </Series>
                             <ChartAreas>
                                <asp:ChartArea Name="_chartAreaEstadisticas" BackColor="LightGray" BackGradientStyle="TopBottom" BackSecondaryColor="White" >
                                    <AxisY Title="Porcentaje de uso" TitleFont="Microsoft Sans Serif, 12pt"></AxisY>
                                    <AxisX IsMarginVisible="true" Enabled="True" Title="Fechas" 
                                        TitleFont="Microsoft Sans Serif, 12pt">
                                        <ScaleBreakStyle BreakLineStyle="None" />
                                    </AxisX>
                                    <Area3DStyle IsClustered="True" LightStyle="Realistic" PointDepth="30" 
                                        Rotation="15" />
                                </asp:ChartArea>
                             </ChartAreas>

                            <BorderSkin BackHatchStyle="DarkVertical" SkinStyle="Sunken" />

                        </asp:Chart>
                     </ContentTemplate>
                </asp:UpdatePanel>
                        </fieldset>
                    </td>
                    <td>
                        <fieldset>
                            <legend class="titulos" style="text-align: left">Equipo de estudiantes</legend>
                            <asp:UpdatePanel runat="server" id="_upGrafico" updatemode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger controlid="_btnGraficar" eventname="Click" />
                </Triggers>
                    <ContentTemplate>
                        <asp:Chart ID="_chtEstadisticasLaptops" runat="server" Height="294px" Width="430px" 
                            BackColor="LightBlue" BackSecondaryColor="AliceBlue" BackGradientStyle="TopBottom" 
                                   BorderlineColor="MidnightBlue" BorderlineWidth="2" 
                            BorderSkin-SkinStyle="Sunken" BorderSkin-BackHatchStyle="DarkVertical">
                            <Series>
                                 <asp:Series Name="_serieUsoLaptop" IsValueShownAsLabel="true" 
                                     BorderWidth="2" BorderDashStyle="Solid" ShadowOffset="2" Legend="true"
                                     BackSecondaryColor="White" MarkerStyle="Circle" MarkerSize="7" 
                                     ChartType="Line" Color="OrangeRed" >

                                    <SmartLabelStyle Enabled="True" />
                                 </asp:Series>
                        
                             </Series>
                             <ChartAreas>
                                <asp:ChartArea Name="_chartAreaEstadisticasLaptops" BackColor="LightGray" BackGradientStyle="TopBottom" BackSecondaryColor="White" >
                                    <AxisY Title="Cantidad de portátiles" TitleFont="Microsoft Sans Serif, 12pt"></AxisY>
                                    <AxisX IsMarginVisible="true" Enabled="True" Title="Fechas" 
                                        TitleFont="Microsoft Sans Serif, 12pt">
                                        <ScaleBreakStyle BreakLineStyle="None" />
                                    </AxisX>
                                    <Area3DStyle IsClustered="True" LightStyle="Realistic" PointDepth="30" 
                                        Rotation="15" />
                                </asp:ChartArea>
                             </ChartAreas>

                            <BorderSkin BackHatchStyle="DarkVertical" SkinStyle="Sunken" />

                        </asp:Chart>
                     </ContentTemplate>
                </asp:UpdatePanel>
                        </fieldset>
                    </td>
                </tr>
            </table>
    </fieldset>
        </div>        
   </asp:Content>
