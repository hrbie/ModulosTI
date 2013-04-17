<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloSolicitudes/MasterSolicitudes.master" AutoEventWireup="true" CodeBehind="ConsultarSolicitudEncargado.aspx.cs" Inherits="ModulosTICapaGUI.Compartido.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadSolicitudes" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainSolicitudes" runat="server">
    <legend class="titulos" style="text-align: left" >Consultar Solicitudes</legend>
    <fieldset class="fieldsetContornoBlanco">

		<div style="float: right">
			<asp:HyperLink ID="_hlMode" runat="server" NavigateUrl="~/ModuloSolicitudes/SolicitudPorSoportista.aspx" ForeColor="#808080" Font-Underline="true">Por soportista</asp:HyperLink>
		</div>

                <asp:Table ID="_tContenedor" runat="server" Width="820px">
                    
                    <asp:TableRow>
                        <asp:TableCell>
                            &nbsp;
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow> 
                        <asp:TableCell HorizontalAlign="left">
                            <asp:Label ID="_lblUsuario" runat="server" Text="Seleccione un usuario:"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Left">
                            <asp:DropDownList ID="_ddlUsuario" CssClass="ddlSolicitudes" OnSelectedIndexChanged="ddlUsuarioSelection_Change" runat="server">
                            </asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow HorizontalAlign="left"> 
                        <asp:TableCell> 
                            <asp:Label ID="_lblSolicitudes" runat="server" Text="Solicitudes"></asp:Label>
                        </asp:TableCell>

                        <asp:TableCell> 
                            <asp:Label ID="_lblAvance" runat="server" Text="Avance"></asp:Label>
                        </asp:TableCell>

                        <asp:TableCell>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow HorizontalAlign="left"> 
                        <asp:TableCell> 
                            <asp:UpdatePanel ID="_upSolicitud" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="_ddlSolicitud" CssClass="ddlSolicitudes" runat="server" Visible="true" OnSelectedIndexChanged="ddlSolicitudSelection_Change">
                                        <asp:ListItem Selected="True">Seleccionar</asp:ListItem>
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:TableCell>

                        <asp:TableCell> 
                            <asp:UpdatePanel ID="_upFechaAvance" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="_ddlFechaAvance" OnSelectedIndexChanged="ddlFechaAvanceSelection_Change" runat="server" Visible="true">
                                <asp:ListItem Selected="True">Seleccionar</asp:ListItem>
                            </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>                            
                        </asp:TableCell>

                        <asp:TableCell HorizontalAlign="Center">
                        </asp:TableCell>
                    </asp:TableRow>
                   
                    <asp:TableRow HorizontalAlign="Left">
                        <asp:TableCell>
                            <asp:UpdatePanel ID="_upDescripcion" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <textarea id="_taDescripcion" runat="server" rows="7" cols="40"></textarea>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:UpdatePanel ID="_upAvance" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <textarea id="_taAvance" runat="server" rows="7" cols="40"></textarea>
                                </ContentTemplate>
                            </asp:UpdatePanel>                            
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableCell>
                            &nbsp;
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow HorizontalAlign="Left">
                        <asp:TableCell>
                            
                            <asp:UpdatePanel ID="_upEstado" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="_lblEstado" runat="server" Text="Estado:"></asp:Label>
                                    &nbsp;
                                    &nbsp;
                                    &nbsp;
                                    &nbsp;
                                    <asp:DropDownList ID="_ddlEstado" runat="server">
                                    </asp:DropDownList>
                                    &nbsp;
                                    <asp:Button ID="_btnCambiar" runat="server" Text="Cambiar" OnClick="btnCambiar_Click"/>
                                    
                                </ContentTemplate>
                            </asp:UpdatePanel>   
                        </asp:TableCell>
                        <asp:TableCell>                  
                            <asp:UpdatePanel ID="_upPostBy" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="_lblMensajePostBy" runat="server" Text="Enviado por:"></asp:Label>
                                    &nbsp;
                                    &nbsp;
                                    &nbsp;
                                    &nbsp;                                    
                                    <asp:Label ID="_lblPostBy" runat="server" Text=""></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow HorizontalAlign="Left">
                        <asp:TableCell>
                            <asp:UpdatePanel ID="_upSoporte" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="_lblAsignado" runat="server" Text="Asignado a: "></asp:Label>
                                    &nbsp;
                                    <asp:DropDownList ID="_ddlSoporte" runat="server">
                                    </asp:DropDownList>
                                    &nbsp;
                                    <asp:Button ID="_btnAsignado" runat="server" OnClick="btnAsignado_Click" Text="Cambiar"/>
                                </ContentTemplate>                        
                            </asp:UpdatePanel>
                        </asp:TableCell>
                        <asp:TableCell>                    
                            <asp:UpdatePanel ID="_upFechaSolicitud" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="_lblMensajeFechaSolicitud" runat="server" Text="Fecha de registro:"></asp:Label>
                                    <asp:Label ID="_lblFechaSolicitud" runat="server"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableCell>
                            &nbsp;
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow HorizontalAlign="Left">
                        <asp:TableCell>
                            <asp:Label ID="_lblAgregarDescripcion" runat="server" Text="Agregar a la descripción:"></asp:Label>
                            <asp:UpdatePanel ID="_upAgregarDescripcion" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <textarea id="_taAgregarDescripcion" cols="40" rows="7" runat="server"></textarea>
                                     <asp:RequiredFieldValidator runat="server" id="_rfvAgregarDescripcion" CssClass="textoError" ControlToValidate="_taAgregarDescripcion" ErrorMessage = "Campo vacio" display="Dynamic" />
                                </ContentTemplate>                                
                            </asp:UpdatePanel>
                            <asp:Button ID="_btnAgregarDescripcion" runat="server" OnClick="btnAgregarDescripcion_Click" Text="Agregar" />
                        </asp:TableCell>
                        
                    </asp:TableRow>
                    <asp:TableRow HorizontalAlign="Left">
                        <asp:TableCell>
                         <asp:UpdatePanel ID="_upErrorAgregarDescripcion" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="_lblErrorAgregarDescripcion" CssClass="textoError" runat="server" Text="No se ha seleccionado una solicitud" Visible="false"></asp:Label>
                            </ContentTemplate>
                          </asp:UpdatePanel>
                        </asp:TableCell>
                    </asp:TableRow>

                </asp:Table>
			</fieldset>

</asp:Content>