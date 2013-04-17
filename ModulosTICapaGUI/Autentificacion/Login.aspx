<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ModulosTICapaGUI.Autentificacion.Login" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
	<head runat="server">
		<title></title>
		<link rel="stylesheet" type="text/css" href="../Compartido/Estilo.css" media="screen" />
		<link rel="shortcut icon" href="../Imagenes/favicon.ico" />
	</head>
	<body>
		<form id="form1" runat="server">
			<asp:ScriptManager ID="ScriptManager1" runat="server">
			</asp:ScriptManager>
			<div class="panelLogin">
				<asp:Panel ID="_pnlContenedor" runat="server" Height="250px" BackColor="Gray" HorizontalAlign="Center">
					<asp:Table ID="_tContenedor" runat="server" HorizontalAlign="Center">
        
						<asp:TableRow>
							<asp:TableCell>
								&nbsp;
							</asp:TableCell>
						</asp:TableRow>

						<asp:TableRow HorizontalAlign="Center">
							<asp:TableCell>
								&nbsp;
							</asp:TableCell>
							<asp:TableCell>
								<asp:Image ID="_imgOficina" runat="server" ImageUrl="../Imagenes/LogoTI.png" />
							</asp:TableCell>
						</asp:TableRow>

						<asp:TableRow>
							<asp:TableCell>
								&nbsp;
							</asp:TableCell>
						</asp:TableRow>


						<asp:TableRow>
							<asp:TableCell HorizontalAlign="Right">
								<asp:Label CssClass="textoLogin" ID="_lUsuario" runat="server" Text="Usuario"></asp:Label>
							</asp:TableCell>
							<asp:TableCell>
								<asp:TextBox ID="_txtUsuario" CssClass="textBoxLogin" runat="server"></asp:TextBox>
							</asp:TableCell>
						</asp:TableRow>

						<asp:TableRow>
							<asp:TableCell HorizontalAlign="Right">
								<asp:Label CssClass="textoLogin" ID="_lPassword" runat="server" Text="Contraseña"></asp:Label>
							</asp:TableCell>
							<asp:TableCell>
								<asp:TextBox ID="_txtPassword" TextMode="Password" CssClass="textBoxLogin" runat="server"></asp:TextBox>
							</asp:TableCell>
							<asp:TableCell>                        
								<asp:ImageButton ID="_ibtnLogin" runat="server" ImageUrl="../Imagenes/botonLogin.png" OnClick="ibtnLogin_Click"/>
							</asp:TableCell>
						</asp:TableRow>
                
						<asp:TableFooterRow>
							<asp:TableCell HorizontalAlign="Right">
								<asp:Image ID="_imgError" Visible="false" runat="server" />
							</asp:TableCell>
							<asp:TableCell VerticalAlign="Middle">
								<asp:Label CssClass="textoLogin" Visible="false" ID="_lblError" runat="server"></asp:Label>
							</asp:TableCell>
						</asp:TableFooterRow>
					</asp:Table>
				</asp:Panel>
				<asp:RoundedCornersExtender ID="_axRoundedCornersExtender" runat="server" TargetControlID="_pnlContenedor" Radius="8" Corners="All">
				</asp:RoundedCornersExtender>
			</div>
		</form>
	</body>
</html>
