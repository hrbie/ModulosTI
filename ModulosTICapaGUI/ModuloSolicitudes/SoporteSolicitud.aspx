<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloSolicitudes/MasterSolicitudes.master" AutoEventWireup="true" CodeBehind="SoporteSolicitud.aspx.cs" Inherits="ModulosTICapaGUI.Compartido.WebForm3" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadSolicitudes" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainSolicitudes" runat="server">
    <legend class="titulos" style="text-align: left" >Consultar Solicitudes</legend>
    <fieldset class="fieldsetContornoBlanco">

                <asp:Table ID="_tContenedor" runat="server" Width="820px">
                    
                    <asp:TableHeaderRow> 
                        <asp:TableHeaderCell HorizontalAlign="left">
                            <asp:Label ID="_lblPendientes" runat="server" Text="Solicitudes pendientes:"></asp:Label>
                        </asp:TableHeaderCell>
                    </asp:TableHeaderRow>

                    <asp:TableRow>
                        <asp:TableCell>
                            &nbsp;
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
                            <asp:Label ID="_lblNuevoAvance" runat="server" Text="Nuevo avance:"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow HorizontalAlign="left"> 
                        <asp:TableCell> 
                            <asp:UpdatePanel ID="_upSolicitud" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="_ddlSolicitud" CssClass="ddlSolicitudes" OnSelectedIndexChanged="ddlSolicitudSelection_Change" runat="server">
                            </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>                            
                        </asp:TableCell>

                        <asp:TableCell> 
                            <asp:UpdatePanel ID="_upFechaAvance" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="_ddlFechaAvance" OnSelectedIndexChanged="ddlFechaAvanceSelection_Change" runat="server">
                                        <asp:ListItem Selected="True">Seleccionar       </asp:ListItem>
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:TableCell>

                        <asp:TableCell HorizontalAlign="Center">
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableCell>
                            &nbsp;
                        </asp:TableCell>
                    </asp:TableRow>
                   
                    <asp:TableRow HorizontalAlign="Left">
                        <asp:TableCell>
                            <asp:UpdatePanel ID="_upDescripcion" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <textarea id="_taDescripcion" rows="10" cols="30" runat="server"></textarea>
                                </ContentTemplate>
                            </asp:UpdatePanel> 
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:UpdatePanel ID="_upAvance" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <textarea id="_taAvance" rows="10" cols="30" runat="server"></textarea>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:UpdatePanel ID="_upNuevoAvance" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <textarea id="_taNuevoAvance" rows="10" cols="30" runat="server"></textarea>
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
                        </asp:TableCell>
                        <asp:TableCell>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="_btnAvance" runat="server" Text="Agregar"  OnClick="btnAvance_Click"/>
                            <asp:ConfirmButtonExtender ID="_cbeNuevoAvance" runat="server" TargetControlID="_btnAvance" ConfirmText="¿Agregar el nuevo avance a la Base de Datos?">
                            </asp:ConfirmButtonExtender>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow HorizontalAlign="Left">
                        <asp:TableCell>
                            <asp:UpdatePanel ID="_upEstado" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="_lblMensajeEstado" runat="server" Text="Estado:"></asp:Label>
                                    &nbsp;
                                    <asp:DropDownList ID="_ddlEstado" runat="server">
                                    </asp:DropDownList>
                                    &nbsp;
                                    <asp:Button ID="_btnCambiar" runat="server" Text="Cambiar" OnClick="btnCambiar_Click"/>
                                    <asp:ConfirmButtonExtender ID="_cbeCambio" runat="server" TargetControlID="_btnCambiar" ConfirmText="¿Realizar el cambio de estado en la solicitud?">
                                    </asp:ConfirmButtonExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>  
                        </asp:TableCell>
                    </asp:TableRow>
                    
                    <asp:TableRow HorizontalAlign="Left">
                        <asp:TableCell>
                            <asp:UpdatePanel ID="_upFechaSolicitud" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="_lblMensajeFechaSolicitud" runat="server" Text="Fecha de solicitud:"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="_lblFechaSolicitud" runat="server" Text="" Visible="false"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow HorizontalAlign="Left">
                        <asp:TableCell>
                            <asp:UpdatePanel ID="_upFechaFin" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="_lblMensajeFechaFin" runat="server" Text="Fecha finalización:"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="_lblFechaFin" runat="server" Text="" Visible="false"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow HorizontalAlign="Left">
                        <asp:TableCell>
                            <asp:UpdatePanel ID="_upSolicitante" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="_lblMensajeSolicitante" runat="server" Text="Solicitante:"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="_lblSolicitante" runat="server" Text="" Visible="false"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow HorizontalAlign="Left">
                        <asp:TableCell>
                            <asp:UpdatePanel ID="_upPostBy" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="_lblMensajePostBy" runat="server" Text="Enviado por:"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="_lblPostBy" runat="server" Text="" Visible="false"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:TableCell>
                    </asp:TableRow>

                </asp:Table>
			</fieldset>


</asp:Content>
