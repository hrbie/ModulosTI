<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccesoDenegado.aspx.cs" Inherits="ModulosTICapaGUI.Compartido.AccesoDenegado" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Permiso Denegado</title>
	<link rel="stylesheet" type="text/css" href="Estilo.css" media="screen" />
	<link rel="shortcut icon" href="../Imagenes/favicon.ico" />
</head>
<body>
	<form id="form1" runat="server">
			<asp:ScriptManager ID="ScriptManager1" runat="server">
			</asp:ScriptManager>
			<div class="panelLogin">
				<asp:Panel ID="_pnlContenedor" runat="server" Height="250px" BackColor="#EBF4FB" HorizontalAlign="Center">
					<asp:Table ID="_tContenedor" runat="server" HorizontalAlign="Center" >
                       
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

						<asp:TableRow>
							<asp:TableCell>
								<asp:Image ID="_imgOficina" runat="server" ImageUrl="~/Imagenes/LogoTI.png" />
							</asp:TableCell>
						</asp:TableRow>

						<asp:TableRow>
							<asp:TableCell>
								&nbsp;
							</asp:TableCell>
						</asp:TableRow>


						<asp:TableRow>
							<asp:TableCell HorizontalAlign="Center">
								<asp:Label ID="_lblExplicacion" CssClass="colorLetraError" runat="server" Text="Usted no tiene permisos para ingresar a esta página"></asp:Label>
							</asp:TableCell>
						</asp:TableRow>

						<asp:TableRow>
							<asp:TableCell HorizontalAlign="Center">
								<asp:Label ID="Label1" CssClass="colorLetrasGeneral" runat="server" Text="Presione el link siguiente para volver al inicio"></asp:Label>
							</asp:TableCell>
						</asp:TableRow>
						<asp:TableRow>
							<asp:TableCell HorizontalAlign="Right">
								<asp:HyperLink ID="_hlkInicio" NavigateUrl="~/Compartido/Inicio.aspx" runat="server">Inicio</asp:HyperLink>
							</asp:TableCell>
						</asp:TableRow>
					</asp:Table>
				</asp:Panel>
                
			</div>
		</form>
</body>
</html>
