<%@ Page Title="" Language="C#" MasterPageFile="~/ModuloSGC/MasterSGC.master" AutoEventWireup="true" CodeBehind="ConsultarCuenta.aspx.cs" Inherits="ModulosTICapaGUI.ModuloSGC.ConsultarCuenta" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadSGC" runat="server">
    <title>Consultar Cuenta</title>
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainSGC" runat="server" Visible = "false" >
    <legend class="titulos">Consultar Cuenta </legend>
    <fieldset class="fieldsetContornoBlanco"  >
		<legend class="titulos" style="text-align: left"> Búsqueda </legend>
			<asp:UpdatePanel ID="UpdatePanel2" UpdateMode= "Conditional" ChildrenAsTriggers="true" runat="server" >
			    <ContentTemplate>
                <table align="left"  border="0" style="width: 683px; height: auto;">
                   <tr> 
                       <td class="style4"  >
						    <asp:Label ID="_lblCriterio"  CssClass="colorLetrasGeneral" runat="server" Text="Criterio de Búsqueda"/>
					   </td>
                       <td class="alinearMedioIzquierda">
                            <asp:DropDownList ID="_ddlCriterio" Width="165px" runat="server" onselectedindexchanged="_ddlCriterioChanged">
                                <asp:ListItem>Seleccionar</asp:ListItem>
                                <asp:ListItem>Login</asp:ListItem>
                                <asp:ListItem>Carnet/Cédula</asp:ListItem>
                                <asp:ListItem>Rango de Fechas</asp:ListItem>
                            </asp:DropDownList>
					    </td>
                   </tr>  
                   <tr>
                       <td class="style4">
						    <asp:Label ID="_lblvalor" runat="server" CssClass="colorLetrasGeneral" Visible="false" Text="Valor"/>
					   </td>
                       <td class="alinearMedioIzquierda">
					        <asp:TextBox ID="_txtValor" Width="165px" runat="server"/>
                       </td>
                        <td class="style3">
                            <asp:Image ID="_imgMensajeBusqueda" Visible="false"  runat="server" />
                            <asp:Label ID="_lblMensajeBusqueda"  Visible="false" runat="server"/>
					    </td>
                    </tr>
                    <tr>
                        <td  class="style4">
                        </td>
                        <td class="alinearMedioIzquierda">
					        <asp:TextBox ID="_txtValor2" Width="165px" runat="server" Visible="false"/>
                            <asp:Image ID="_imgFechaInicio" ImageUrl="~/Imagenes/Calendario.png" runat="server" ToolTip="Presione aquí para abrir el calendario" 
                                style="width: 16px" Visible="false"/>
                            <asp:CalendarExtender ID="_axCalendarioInicio" runat="server" TargetControlID="_txtValor2" PopupButtonID="_imgFechaInicio" 
                                Format="dd/MM/yyyy"/>
				        </td>
                    </tr>
                   <tr>
                        <td>
                        </td>     
                        <td class="alinearMedioIzquierda">
                            <asp:TextBox ID="_txtValor3" Width="165px" runat="server" Visible="false"/>
                            <asp:Image ID="_imgFechaFinal" ImageUrl="~/Imagenes/Calendario.png" runat="server" ToolTip="Presione aquí para abrir el calendario" 
                                style="width: 16px" Visible="false"/>
                            <asp:CalendarExtender ID="_axCalendarioFinal" runat="server" TargetControlID="_txtValor3" PopupButtonID="_imgFechaFinal" 
                                Format="dd/MM/yyyy"/>
					    </td>                        
                     </tr>
                     </table> 
                     </ContentTemplate>
            </asp:UpdatePanel>
                     <table align="left"  border="0" style="width: 683px; height: auto;">
                                        <tr>
                        <td class="alinearMedioCentro" colspan="2">
						    <asp:Button ID="_bntConsultar" runat="server" Text="Buscar" onclick="_btnConsultar_Click" style="margin-bottom: 0px" 
                                ToolTip="Presione éste botón para buscar un usuario" />

                             <asp:Button ID="_bntModificar" runat="server" Text="Modificar" onclick="_btnModificar_Click" style="margin-bottom: 0px" 
                                ToolTip="Presione éste botón para modifica el usuario" Enabled ="false" />

					    </td>
                   </tr>
               </table>
               
		</fieldset>
        <fieldset class="fieldsetContornoBlanco"  >
            <legend class="titulos" style="text-align: left">
			    Formulario
		    </legend>
			<asp:UpdatePanel ID="_upConsultaUsuario" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server">
                <ContentTemplate>
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
									  &nbsp;<asp:DropDownList ID="_ddlCarrera" Width="165px" runat="server" Enabled ="false">
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
									<asp:Label ID="_lblUsuario" CssClass="colorLetrasGeneral" runat="server" Visible="false" Text="Nombre de Usuario"></asp:Label>
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
									<asp:TextBox ID="_txtUsuario" Width="165px" runat="server" Visible="false"  Enabled="False" ></asp:TextBox>
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
									<asp:Label ID="_lblPassword" CssClass="colorLetrasGeneral" runat="server" Text="Contraseña" Enabled="False" ></asp:Label>
								</td>
								<td class="style1">
									<asp:TextBox ID="_txtPassword" Width="165px" runat="server" TextMode="Password" Enabled="False" ></asp:TextBox>
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
									<asp:TextBox ID="_txtCPassword" Width="165px" runat="server" TextMode="Password" Enabled="False" ></asp:TextBox>
								</td>
								<td>
									
								</td>
							</tr>
							<tr>
								<td class="alinearMedioIzquierda">
									<asp:Label ID="_lblFoto"  CssClass="colorLetrasGeneral" runat="server" Text="Fotografía del Usuario" Visible ="true" 
									></asp:Label>
								</td>
								<td class="alinearMedioIzquierda">
									<input type="file" id="_txtCargarFoto" runat="server" Visible ="false" />
									<asp:Label runat="server" id="_lbImagen" style="display:none;" >  
											<img alt="" src="/Imagenes/uploading.gif"  />  
									</asp:Label>
								</td>
								<td>
									
								</td>
							</tr>
							<tr>
								<td class="alinearMedioCentro" colspan="3">
									<asp:Button ID="_btnGuardar" runat="server" Text="Guardar" 
										onclick="_btnGuardar_Click" style="margin-bottom: 0px" 
										ToolTip="Presione éste botón para guardar los datos modificados del ususario" 
										Visible = "false"/>
					
									<asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" 
										onclick="_btnCancelar_Click" style="margin-bottom: 0px" 
										ToolTip="Presione éste botón para cancelar las modificaciones realizadas sobre el usuario" 
										Visible = "false"/>
								</td>
							</tr>
						</table>
					</ContentTemplate>
                </asp:UpdatePanel>
		</fieldset>
</asp:Content>
