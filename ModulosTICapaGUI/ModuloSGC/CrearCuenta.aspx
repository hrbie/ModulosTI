<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloSGC/MasterSGC.master" AutoEventWireup="true" CodeBehind="CrearCuenta.aspx.cs" Inherits="ModulosTICapaGUI.ModuloSGC.CrearCuenta" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadSGC" runat="server">
    <title>Crear Usuario</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainSGC" runat="server">
  			<legend class="titulos">Crear Cuenta</legend>
            <fieldset class="fieldsetContornoBlanco">
		        <legend class="titulos" style="text-align: left">
			        Formulario
		        </legend>
			    <table border="0" style="width: 940px; height: 351px;">
				    <tr>
                        
					    <td class="alinearMedioIzquierda">
						    <asp:Label ID="_lblCarnet" CssClass="colorLetrasGeneral" runat="server" Text="Carnet/Cédula"></asp:Label>
					    </td>
					    <td class="alinearMedioIzquierda">
						    <asp:TextBox ID="_txtCarnet" Width="165px" runat="server" MaxLength="10"></asp:TextBox>
					    </td>
					    <td class="alinearMedioIzquierda" colspan="2">
						    <asp:Label ID="_lblCarrera" CssClass="colorLetrasGeneral" runat="server" Text="Carrera"></asp:Label>
                            &nbsp;<asp:DropDownList ID="_ddlCarrera" Width="165px" runat="server">
                            </asp:DropDownList>
					    </td>
				    </tr>
				    <tr>
					    <td class="alinearMedioIzquierda">
						    <asp:Label ID="_lblNombre" CssClass="colorLetrasGeneral" runat="server" Text="Nombre"></asp:Label>
					    </td>
					    <td class="alinearMedioIzquierda">
						    <asp:TextBox ID="_txtNombre" Width="165px" runat="server"></asp:TextBox>
					    </td>
					    <td class="alinearMedioIzquierda">
						    <asp:RadioButton ID="_rbEstudiante" CssClass="colorLetrasGeneral" Text="Estudiante" runat="server" />
					    </td>
						<td>
						</td>
				    </tr>
				    <tr>
					    <td class="alinearMedioIzquierda">
						    <asp:Label ID="_lblPApellido" CssClass="colorLetrasGeneral" runat="server" Text="Primer Apellido"></asp:Label>
					    </td>
					    <td class="alinearMedioIzquierda">
						    <asp:TextBox ID="_txtPApellido" Width="165px" runat="server"></asp:TextBox>
					    </td>
					    <td class="alinearMedioIzquierda">
						    <asp:RadioButton ID="_rbProfesor" CssClass="colorLetrasGeneral" Text="Profesor" runat="server" />
					    </td>
						<td>
						</td>
				    </tr>
				    <tr>
					    <td class="alinearMedioIzquierda">
						    <asp:Label ID="_lblSApellido" CssClass="colorLetrasGeneral" runat="server" Text="Segundo Apellido"></asp:Label>
					    </td>
					    <td class="alinearMedioIzquierda">
						    <asp:TextBox ID="_txtSApellido" Width="165px" runat="server"></asp:TextBox>
					    </td>
					    <td class="alinearMedioIzquierda">
						    <asp:Label ID="_lblUsuario" CssClass="colorLetrasGeneral" runat="server" Text="Nombre de Usuario"></asp:Label>
					    </td>
						<td>
						</td>
				    </tr>
				    <tr>
					    <td class="alinearMedioIzquierda">
						    <asp:Label ID="_lblTelefono" CssClass="colorLetrasGeneral" runat="server" Text="Teléfono Fijo"></asp:Label>
					    </td>
					    <td class="alinearMedioIzquierda">
						    <asp:TextBox ID="_txtTelefono" Width="165px" runat="server"></asp:TextBox>
					    </td>
					    <td class="alinearMedioIzquierda">
						    <asp:TextBox ID="_txtUsuario" Width="165px" runat="server"></asp:TextBox>
					    </td>
						<td>
						</td>
				    </tr>
				    <tr>
					    <td class="alinearMedioIzquierda">
						    <asp:Label ID="_lblCelular" CssClass="colorLetrasGeneral" runat="server" Text="Teléfono Celular"></asp:Label>
					    </td>
					    <td class="alinearMedioIzquierda">
						    <asp:TextBox ID="_txtCelular" Width="165px" runat="server"></asp:TextBox>
					    </td>
					    <td class="alinearMedioIzquierda">
						    <asp:Button ID="_btnSugerir" runat="server" Text="Sugerir Usuario" 
							    onclick="_btnSugerir_Click" 
                                ToolTip="Presione este botón para buscar un nombre de usuario para la persona" />
					    </td>
						<td>
						</td>
				    </tr>
				    <tr>
					    <td class="alinearMedioIzquierda">
						    <asp:Label ID="_lblCorreo" CssClass="colorLetrasGeneral" runat="server" Text="Correo Electrónico"></asp:Label>
					    </td>
					    <td class="alinearMedioIzquierda">
						    <asp:TextBox ID="_txtCorreo" Width="165px" runat="server"></asp:TextBox>
					    </td>
					    <td class="alinearMedioCentro">
                            <asp:Image ID="_imgMensaje" Visible="false" runat="server" />
                            <asp:Label ID="_lblMensaje" CssClass="colorLetrasGeneral" Visible="false" runat="server"></asp:Label>
					    </td>
						<td>
						</td>
				    </tr>
				    <tr>
					    <td class="alinearMedioIzquierda">
						    <asp:Label ID="_lblPassword" CssClass="colorLetrasGeneral" runat="server" Text="Contraseña"></asp:Label>
					    </td>
					    <td class="alinearMedioIzquierda">
						    <asp:TextBox ID="_txtPassword" Width="165px" runat="server" TextMode="Password"></asp:TextBox>
					    </td>
					    <td>
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
						    <asp:TextBox ID="_txtCPassword" Width="165px" runat="server" TextMode="Password"></asp:TextBox>
					    </td>
					    <td>
					    </td>
				    </tr>
				    <tr>
					    <td class="alinearMedioIzquierda">
						    <asp:Label ID="_lblFoto" CssClass="colorLetrasGeneral" runat="server" Text="Fotografía del Usuario"></asp:Label>
					    </td>
					    <td class="alinearMedioIzquierda">
						    <input type="file" id="_txtCargarFoto" runat="server" />
						    <asp:Label runat="server" id="_lbImagen" style="display:none;" >  
								    <img alt="" src="/Imagenes/uploading.gif" />  
						    </asp:Label>
					    </td>
					    <td>
					    </td>
				    </tr>
				    <tr>
					    <td class="alinearMedioCentro" colspan="3">
						    <asp:Button ID="_btnCrear" runat="server" Text="Crear Usuario" 
							    onclick="_btnCrear_Click" style="margin-bottom: 0px" 
                                ToolTip="Presione éste botón para crear la cuenta" />
					    </td>
				    </tr>
			    </table>

				
				
            </fieldset>

			<fieldset class="fieldsetContornoBlanco">
		        <legend class="titulos" style="text-align: left">
			        Generar cuentas a partir de un archivo de Excel
				</legend>
				
				<div>
					&nbsp;
				</div>
				<div class="alineacionIzquierda">
					<asp:FileUpload ID="_fluExcel" runat="server" />
					<asp:Label ID="_lblMensajeUpload" CssClass="textoError" Visible="false" runat="server"></asp:Label>
				</div>
				<div class="alineacionIzquierda">
					<asp:Button ID="_btnGenerar" runat="server" Text="Generar" 
						onclick="_btnGenerar_Click" style="margin-bottom: 0px" 
                        ToolTip="Presione éste botón para crear cuentas a partir de un archivo de Excel" />
				</div>
				<div>
				</div>
				<div class="alineacionIzquierda">
					<textarea id="_taErrores" Visible="false" rows="5" cols="112" runat="server"></textarea>
				</div>

			</fieldset>

</asp:Content>
