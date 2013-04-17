<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloSolicitudes/MasterSolicitudes.master" AutoEventWireup="true" CodeBehind="AdministrarSolicitudUsuario.aspx.cs" Inherits="ModulosTICapaGUI.Compartido.WebForm2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadSolicitudes" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainSolicitudes" runat="server">
    <legend class="titulos" style="text-align: left" >Mis Solicitudes</legend>
    <fieldset class="fieldsetContornoBlanco">
                <asp:Table ID="_tContenedor" runat="server" Width="720px" Height="417px">

                    <asp:TableRow>
                        <asp:TableCell>
                            &nbsp;
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableCell>
                            &nbsp;
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow HorizontalAlign="Left">
                        <asp:TableCell>
                            <asp:Label ID="_lblSolicitud" runat="server" Text="Solicitues:"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="_lblAvance" runat="server" Text="Avance:"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow HorizontalAlign="Left">
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
                                    <textarea id="_taDescripcion" runat="server" rows="7" cols="40"></textarea>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:UpdatePanel ID="_upAvance" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <textarea id="_taAvance" rows="7" runat="server" cols="40"></textarea>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableCell>
                            &nbsp;
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Left">
                            <asp:UpdatePanel ID="_upResponsable" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="_lblMensajeResponsable" runat="server" Text="Responsable: "></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="_lblResponsable" runat="server"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Left">
                            <asp:UpdatePanel ID="_upEstado" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="_lblMensajeEstado" runat="server" Text="Estado: "></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="_lblEstado" runat="server"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Left">
                            <asp:UpdatePanel ID="_upFechaSolicitud" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="_lblMensajeFechaSolicitud" runat="server" Text="Fecha solicitud: "></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="_lblFechaSolicitud" runat="server"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Left">
                            <asp:UpdatePanel ID="_upFechaFin" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="_lblMensajeFechaFin" runat="server" Text="Fecha finalización: "></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="_lblFechaFin" runat="server"></asp:Label>
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
                        <asp:TableCell HorizontalAlign="Left">
                            <asp:Button ID="_btnCancelarSolicitud" runat="server" Text="Cancelar" />
                            <asp:ConfirmButtonExtender ID="_cbeCancelarSolicitud" runat="server" TargetControlID="_btnCancelarSolicitud" ConfirmText="¿Confirma que desea eliminar la solicitud actual?">
                            </asp:ConfirmButtonExtender>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableCell>
                            &nbsp;
                        </asp:TableCell>
                    </asp:TableRow>

                </asp:Table>

			</fieldset>

</asp:Content>
