<%@ Page Title="" Language="C#" MasterPageFile="~/Compartido/Master.master" AutoEventWireup="true" CodeBehind="UsoLaboratorio.aspx.cs" Inherits="ModulosTICapaGUI.ModuloPEUL.UsoLaboratorio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
<title>Uso de Laboratorios</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">
<fieldset style="background:#EBF4FB">
		<legend class="titulos">Estado Actual de Laboratorios</legend>
		<table style="width:100%;">
            <tr>
                <td>
                    &nbsp;</td>
                <td class="style1">
                    &nbsp;</td>
                <td align="left">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="alinearCentro" colspan="2">                    
                    <asp:Chart ID="_chtUsoLaboratorio" runat="server" Height="260px" Width="550px" BackColor="LightBlue" BackSecondaryColor="AliceBlue" BackGradientStyle="TopBottom" BorderlineColor="MidnightBlue" BorderlineWidth="2" BorderSkin-SkinStyle="Sunken" BorderSkin-BackHatchStyle="DarkVertical">
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
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>                 
                </td>
                </tr>
            <tr>
                <td class="alinearCentro" colspan="2">                    
							    <asp:Image ID="_imgMensaje" Visible="false" runat="server" />							
							    <asp:Label ID="_lblMensaje" Visible="false" runat="server" CssClass="colorLetrasGeneral"></asp:Label>
                </td>
                </tr>
            <tr>
                <td align="center" colspan="3">
                    <asp:GridView ID="_gridUsoLaboratorio" runat="server" 
                        AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                        GridLines="None" Width="945px" Height="202px" >
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
                </td>
            </tr>
            </table>				
	</fieldset>
</asp:Content>
