<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloSolicitudes/MasterSolicitudes.master" AutoEventWireup="true" CodeBehind="AdministrarSolicitudes.aspx.cs" Inherits="ModulosTICapaGUI.ModuloSolicitudes.AdministrarSolicitudes" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadSolicitudes" runat="server">

    <title>Administrar Solicitudes</title>
    <style type="text/css">
        .style1
        {
            width: 195px;
        }
        .style2
        {
            width: 205px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainSolicitudes" runat="server">
    <legend class="titulos" style="text-align: left" >Administrar Solicitudes</legend>
    <fieldset class="fieldsetContornoBlanco">

                <asp:Table ID="_tContenedor" runat="server" Width="720px" Height="417px">

                    <asp:TableRow>
                        <asp:TableCell>
                            &nbsp;
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Right">
                            <asp:Label ID="_lblMensaje" runat="server" Text="Solicitudes pendientes:"></asp:Label>
                            &nbsp;
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left">
                            <asp:UpdatePanel ID="_upSolicitudes" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="_ddlSolicitudes" CssClass="ddlSolicitudes"  OnSelectedIndexChanged="ddlSolicitudSelection_Change" runat="server">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Right">
                            <asp:Label ID="_lblMensajeUsuario" runat="server" Text="Usuario:"></asp:Label>
                            &nbsp;                          
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left">
                            <asp:UpdatePanel ID="_upUsuario" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>                                    
                                    <asp:Label ID="_lblUsuario" runat="server" Text=""></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:TableCell>
                    </asp:TableRow>


                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Right">
                            <asp:Label ID="_lblMensajePostBy" runat="server" Text="Enviado por:"></asp:Label>  
                            &nbsp;                        
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left">
                            <asp:UpdatePanel ID="_upPosBy" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>                                    
                                    <asp:Label ID="_lblPostBy" runat="server" Text=""></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow VerticalAlign="Top">
                        <asp:TableCell HorizontalAlign="Right">
                            <asp:Label ID="_lblDescripcion" runat="server" Text="Descripcion:"></asp:Label>
                            &nbsp;
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left">
                            <asp:UpdatePanel ID="_upDescripcon" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <textarea id="_taDescripcion" runat="server" rows="5" cols="40"></textarea>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:TableCell>
                    </asp:TableRow>


                    <asp:TableRow VerticalAlign="Top">
                        <asp:TableCell HorizontalAlign="Right">
                            <asp:Label ID="_lblAgregarDescripcion" runat="server" Text="Agregar a la descripción:"></asp:Label>
                            &nbsp;
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left">
                            <asp:UpdatePanel ID="_upAgregarDescripcion" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <textarea id="_taAgregarDescripcion" cols="40" rows="5" runat="server"></textarea>
                                     <asp:RequiredFieldValidator runat="server" id="_rfvAgregarDescripcion" CssClass="textoError" ControlToValidate="_taAgregarDescripcion" ErrorMessage = "Campo vacio" display="Dynamic" ValidationGroup="agregar"/>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:Button ID="_btnAgregarDescripcion" runat="server" OnClick="btnAgregarDescripcion_Click" Text="Agregar" ValidationGroup="agregar"/>
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


                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Right">
                            <asp:Label ID="_lblMensajeFechaSolicitud" runat="server" Text="Fecha de registro:"></asp:Label>
                            &nbsp;
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left">
                            <asp:UpdatePanel ID="_upFechaSolicitud" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="_lblFechaSolicitud" runat="server"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Right">
                            <asp:Label ID="_fechaFin" runat="server" Text="Fecha fin:"></asp:Label>
                            &nbsp;
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left">
                            <asp:UpdatePanel ID="_upFechaFinal" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox runat="server" id="_txtFechaFinal" Width="140px" Enabled="False"></asp:TextBox>
                                        &nbsp;
                                        <asp:Image ID="_imgFechaFinal" ImageUrl="~/Imagenes/Calendario.png" 
								            runat="server" ToolTip="Presione aquí para abrir el calendario" />
                                        <asp:CalendarExtender ID="_axCalendarioFinal" runat="server"
                                             TargetControlID="_txtFechaFinal" PopupButtonID="_imgFechaFinal" Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Right" VerticalAlign="Top">
                            <asp:Label ID="_lblEncargado" runat="server" Text="Encargado:"></asp:Label>
                            &nbsp;
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left">
                            <asp:UpdatePanel ID="_upSoporte" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="_ddlSoporte" runat="server">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            &nbsp;
                            <asp:Button ID="_btnAsignar" runat="server" OnClick="btnAsignar_Click" Text="Asignar" />
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableCell>
                            &nbsp;
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Right">
                            <asp:Label ID="_lblCancelar" runat="server" Text="Rechazar solicitud"></asp:Label>
                            &nbsp;
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left">
                            <asp:Button ID="_btnCancelar" runat="server" OnClick="btnCancelar_Click" Text="Rechazar" CausesValidation="false"/>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableCell>
                            &nbsp;
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="_lblAccion" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>

                </asp:Table>
    
    </fieldset>

</asp:Content>
