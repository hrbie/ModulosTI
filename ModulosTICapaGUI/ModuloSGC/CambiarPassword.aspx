<%@ Page Title="" Language="C#" MasterPageFile="~/Compartido/Master.master" AutoEventWireup="true" CodeBehind="CambiarPassword.aspx.cs" Inherits="ModulosTICapaGUI.ModuloSGC.CambiarPassword" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

	<title>Cambiar Contraseña</title>
    <style type="text/css">
        .style1
        {
            vertical-align: middle;
            text-align: left;
            height: 26px;
        }
        .style2
        {  
            height: 26px;
        }
        .style3
        {
            vertical-align: middle;
            text-align: center;
            height: 26px;
        }
        .style4
        {
            vertical-align: middle;
            text-align: left;
            width: 190px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">
	
         <fieldset class="fieldsetContornoBlanco"  >
                 <legend class="titulos" style="text-align: left" >
			        Formulario
		        
		        </legend>

						<table border="0"  style="width: 940px; height: 109px" >
							<tr>
								<td class="alinearMedioIzquierda">
									<asp:Label ID="_lblCarnet" CssClass="colorLetrasGeneral" runat="server" Text="Carnet/Cédula" ></asp:Label>
								</td>
								<td class="alinearMedioIzquierda">
									<asp:TextBox ID="_txtCarnet" Width="165px" runat="server" MaxLength="9" Enabled="False"></asp:TextBox>
								</td>
								<td class="alinearMedioIzquierda">
						   
									<asp:Label ID="_lblCarrera"  CssClass="colorLetrasGeneral" runat="server" Text="Carrera"></asp:Label>
									  &nbsp;<asp:DropDownList ID="_ddlCarrera" Width="165px" runat="server" Enabled =false>
									</asp:DropDownList>
								</td>
                     
							</tr>
							<tr>
								<td class="alinearMedioIzquierda">
									<asp:Label ID="_lblNombre" CssClass="colorLetrasGeneral" runat="server" Text="Nombre"></asp:Label>
								</td>
								<td class="alinearMedioIzquierda">
									<asp:TextBox ID="_txtNombre" Width="165px" runat="server" Enabled="False"></asp:TextBox>
								</td>
                        
								<td class="alinearMedioIzquierda" rowspan="2">
									 <asp:RadioButtonList ID="_rblUsarios" CssClass="colorLetrasGeneral" runat="server" Enabled = "False">
										<asp:ListItem>Estudiante</asp:ListItem>
										<asp:ListItem>Profesor</asp:ListItem>
									</asp:RadioButtonList>
								</td>
							</tr>
							<tr>
								<td class="alinearMedioIzquierda">
									<asp:Label ID="_lblPApellido"  CssClass="colorLetrasGeneral" runat="server" Text="Primer Apellido" Enabled="False" ></asp:Label>
								</td>
								<td class="alinearMedioIzquierda">
									<asp:TextBox ID="_txtPApellido" Width="165px" runat="server" Enabled="False" ></asp:TextBox>
								</td>
							</tr>
							<tr>
								<td class="alinearMedioIzquierda">
									<asp:Label ID="_lblSApellido" CssClass="colorLetrasGeneral" runat="server" Text="Segundo Apellido"></asp:Label>
								</td>
								<td class="alinearMedioIzquierda">
									<asp:TextBox ID="_txtSApellido" Width="165px" runat="server" Enabled="False"></asp:TextBox>
								</td>
								<td class="alinearMedioIzquierda">
									<asp:Label ID="_lblUsuario" CssClass="colorLetrasGeneral" runat="server" Text="Nombre de Usuario"></asp:Label>
								</td>
							</tr>
							<tr>
								<td class="alinearMedioIzquierda">
									<asp:Label ID="_lblTelefono" CssClass="colorLetrasGeneral" runat="server" Text="Teléfono Fijo"></asp:Label>
								</td>
								<td class="alinearMedioIzquierda">
									<asp:TextBox ID="_txtTelefono" Width="165px" runat="server"  Enabled="False"></asp:TextBox>
								</td>
								<td class="alinearMedioIzquierda">
									<asp:TextBox ID="_txtUsuario" Width="165px" runat="server"  Enabled="False" ></asp:TextBox>
								</td>
							</tr>
							<tr>
								<td class="style1">
									<asp:Label ID="_lblCelular" CssClass="colorLetrasGeneral" runat="server" Text="Teléfono Celular" ></asp:Label>
								</td>
								<td class="style1">
									<asp:TextBox ID="_txtCelular" Width="165px" runat="server" Enabled="False" ></asp:TextBox>
								</td>
								<td class="style1">
									</td>
							</tr>
							<tr>
								<td class="style1">
									<asp:Label ID="_lblCorreo" CssClass="colorLetrasGeneral" runat="server" Text="Correo Electrónico" ></asp:Label>
								</td>
								<td class="style1">
									<asp:TextBox ID="_txtCorreo" Width="165px" runat="server" Enabled="False" ></asp:TextBox>
								</td>
								<td class="style3">
									<asp:Image ID="_imgMensaje" Visible="false" runat="server" />
									<asp:Label ID="_lblMensaje" Visible="false" runat="server"></asp:Label>
								</td>
							</tr>
							<tr>
								<td class="style1">
									<asp:Label ID="_lblContraActual" CssClass="colorLetrasGeneral" runat="server" Text="Contraseña" ></asp:Label>
								</td>
								<td class="style1">
									<asp:TextBox ID="_txtContraActual" Width="165px" runat="server" Enabled="True" TextMode="Password"></asp:TextBox>
								</td>
								<td>
								</td>
							</tr>
							<tr>
								<td class="style1">
									<asp:Label ID="_lblPassword" CssClass="colorLetrasGeneral" runat="server" Text="Nueva Contraseña"></asp:Label>
								</td>
								<td class="style1">
									<asp:TextBox ID="_txtPassword" Width="165px" runat="server" Enabled="false" TextMode="Password"></asp:TextBox>
								</td>
								<td class="style2">
									<asp:PasswordStrength ID="_axPasswordStrength" runat="server"
										TargetControlID="_txtPassword"
										DisplayPosition="RightSide"
										StrengthIndicatorType="BarIndicator"
										PreferredPasswordLength="12"
										MinimumNumericCharacters="4"
										MinimumSymbolCharacters="0"
										RequiresUpperAndLowerCaseCharacters="false"
										BarBorderCssClass="barra"
										StrengthStyles="barraTipo1;barraTipo2;barraTipo3"
										CalculationWeightings="50;35;15;0">
									</asp:PasswordStrength>
								</td>
							</tr>
							<tr>
								<td class="alinearMedioIzquierda">
									<asp:Label ID="_lblCPassword" CssClass="colorLetrasGeneral" runat="server" Text="Confirmar Contraseña"></asp:Label>
								</td>
								<td class="alinearMedioIzquierda">
									<asp:TextBox ID="_txtCPassword" Width="165px" runat="server" Enabled="false" TextMode="Password"></asp:TextBox>
								</td>
								<td>
									
								</td>
							</tr>
							<tr>
								<td class="alinearMedioIzquierda">
									
								</td>
								<td class="alineacionIzquierda" colspan="3">
									<asp:Button ID="_btnGuardar" runat="server" Text="Guardar" 
										OnClick="_btnGuardar_Click" style="margin-bottom: 0px" 
										ToolTip="Presione éste botón para guardar los datos modificados del ususario"/>

								</td>
								<td>
								</td>
							</tr>
						</table>
						<div class="style1">
							<asp:Label ID="_lblMensaje1" runat="server" Text="IMPORTANTE: "></asp:Label>
							
						</div>
						<div class="style1">
							<asp:Label ID="_lblMensaje2" runat="server" Text="La contraseña debe contener un mínimo de doce caracteres, números, letras en minúscula y mayuscula, además no son permitidos caracteres especiales."></asp:Label>
						</div>
		</fieldset>

</asp:Content>
