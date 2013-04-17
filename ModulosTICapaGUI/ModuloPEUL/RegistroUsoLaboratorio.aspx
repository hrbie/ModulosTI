<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloPEUL/MasterPEUL.master" AutoEventWireup="true" CodeBehind="RegistroUsoLaboratorio.aspx.cs" Inherits="ModulosTICapaGUI.ModuloPEUL.RegistroUsoLaboratorio" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPEUL" runat="server">
    <title>Registro PEUL</title>
    <style type="text/css">
        .style1
        {
            width: 39px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPEUL" runat="server">  
    <fieldset style="background:#EBF4FB">
		<legend class="titulos">Estado Actual de Laboratorios</legend>
		<table style="width:100%;">
            <tr>
                <td class="alinearCentro">
                    <asp:Timer ID="_tmReloj" OnTick="_tmReloj_Tick" runat="server" Interval="600000"></asp:Timer>
                    <asp:UpdatePanel ID="_upGrafico" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                        <ContentTemplate>
                            <asp:Chart ID="_chtUsoLaboratorio" Height="260px" Width="550px" BackColor="LightBlue" BackSecondaryColor="AliceBlue" BackGradientStyle="TopBottom" BorderlineColor="MidnightBlue" BorderlineWidth="2" BorderSkin-SkinStyle="Sunken" BorderSkin-BackHatchStyle="DarkVertical" runat="server">
                                <legends>
							        <asp:Legend TitleFont="Microsoft Sans Serif, 8pt, style=Bold" BackColor="Transparent" Font="Trebuchet MS, 8.25pt, style=Bold" IsTextAutoFit="False" BorderColor="5, 100, 146" Name="Default" LegendStyle="Row">
								        <position Y="5" Height="10" Width="40" X="35"></position>
							        </asp:Legend>
						        </legends>
                                <Series>
                                    <asp:Series ChartType="StackedColumn" Name="Computadoras" IsValueShownAsLabel="true" BorderWidth="2" BorderDashStyle="Solid" ShadowOffset="2" BackSecondaryColor="White" Color="220, 65, 140, 240">
                                        <SmartLabelStyle Enabled="True" />
                                    </asp:Series>
                                    <asp:Series ChartType="StackedColumn" Name="Laptops" IsValueShownAsLabel="true" BorderWidth="2" BorderDashStyle="Solid" ShadowOffset="2" BackSecondaryColor="White" Color="220, 252, 180, 65">
                                        <SmartLabelStyle Enabled="True" />
                                    </asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea Name="_chartAreaGrafico" BackColor="LightGray" BackGradientStyle="TopBottom" BackSecondaryColor="White" Area3DStyle-Enable3D="true" Area3DStyle-PointDepth="25">
                                        <position Y="18.3307" Height="85.58395" Width="88.77716" X="5.089137"></position>
                                        <AxisX IsMarginVisible="true">
                                            <MajorGrid LineDashStyle="NotSet"/>
                                        </AxisX>
                                        <AxisY>
                                        </AxisY>
                                    </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                        </ContentTemplate>
                        <Triggers>
							<asp:AsyncPostBackTrigger ControlID="_tmReloj" EventName="Tick" />
						</Triggers>
                    </asp:UpdatePanel>                    
                </td>
                <td align="left">
                    <table style="width:100%; border:solid 2px #336699">
                        <tr>
                            <td align="right">
                                &nbsp;</td>
                            <td align="left">
                                &nbsp;</td>
                                    </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="_lblLaboratorio" runat="server" Text="Laboratorio:" CssClass="colorLetrasGeneral"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="_ddlLaboratorio" runat="server" Width="153px">
                                    <asp:ListItem>Seleccionar</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                                    </tr>
                                    <tr>
                            <td align="right">
                                <asp:Label ID="_lblEstado" runat="server" Text="Estado:" CssClass="colorLetrasGeneral"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="_ddlEstado" runat="server" Width="153px">
                                    
                                </asp:DropDownList>
                            </td>
                                    </tr>
                                    <tr>
                            <td align="right">
                                <asp:Label ID="_lblCantidadUsuarios" runat="server" 
                                    Text="Cantidad de usuarios:" CssClass="colorLetrasGeneral"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="_txtCantidadUsuarios" runat="server" Width="150px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="_axFiltroCantidadUsuarios" runat="server" TargetControlID="_txtCantidadUsuarios" FilterType="Numbers">
                                </asp:FilteredTextBoxExtender>
                            </td>
                                    </tr>
                                    <tr>
                            <td align="right">
                                <asp:Label ID="_lblCantidadLaptops" runat="server" Text="Cantidad de laptops:" CssClass="colorLetrasGeneral"></asp:Label>                                        
                            </td>
                            <td align="left">
                                <asp:TextBox ID="_txtCantidadLaptops" runat="server" Width="150px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="_axFiltroCantidadLaptops" runat="server" TargetControlID="_txtCantidadLaptops" FilterType="Numbers">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="_lblComentario" runat="server" Text="Comentario:" CssClass="colorLetrasGeneral"></asp:Label>
                                </td>
                                <td align="left" rowspan="2">
                                    <asp:TextBox ID="_txtComentario" runat="server" Height="70px" 
                                        TextMode="MultiLine" Width="240px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    &nbsp;</td>
                            </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="_btnRegistrar" runat="server" Text="Registrar" 
                                    onclick="_btnRegistrar_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
							    <asp:Image ID="_imgMensaje" Visible="false" runat="server" />							
							    <asp:Label ID="_lblMensaje" Visible="false" runat="server" CssClass="colorLetrasGeneral"></asp:Label>
						    </td>
                        </tr>

                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <div class="scroll2">
                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="_gridUsoLaboratorio" runat="server" 
                                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                    GridLines="None" Width="940px">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Laboratorio">
                                            <ItemTemplate>
                                                <asp:Label ID="lLaboratorio" runat="server" Text='<%# Bind("Laboratorio")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Operador" ControlStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:Label ID="lOperador" runat="server" Text='<%# Bind("Operador")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ControlStyle Width="30px"></ControlStyle>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ultimo Registro">
                                            <ItemTemplate>
                                                <asp:Label ID="lUltimoRegistro" runat="server" Text='<%# Bind("UltimoRegistro")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>                            
                                        <asp:TemplateField HeaderText="Comentario">
                                            <ItemTemplate>
                                                <asp:Label ID="lComentario" runat="server" Text='<%# Bind("Comentario")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="350px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Porcentaje de Uso" ControlStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:Label ID="lPorcentaje" runat="server" Text='<%# Bind("Porcentaje")%>'></asp:Label>
                                            </ItemTemplate>
                                                <ControlStyle Width="30px"></ControlStyle>
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
                            </ContentTemplate>
                            <Triggers>
					            <asp:AsyncPostBackTrigger ControlID="_tmReloj" EventName="Tick" />
				            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            </table>				
	</fieldset>
</asp:Content>