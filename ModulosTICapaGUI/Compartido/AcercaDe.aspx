<%@ Page Title="" Language="C#" MasterPageFile="~/Compartido/Master.master" AutoEventWireup="true" CodeBehind="AcercaDe.aspx.cs" Inherits="ModulosTICapaGUI.Compartido.AcercaDe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">
	<div class="central">
		<div class="contorno">
			<fieldset style="background:#EBF4FB; border:solid 2px #336699">
				<legend class="titulos">Acerca de</legend>
				<div class="alinearArribaIzquierda">
					<div class="acomodarNombres">
						<asp:Label ID="_etiquetaDesarrolladores" Text="Desarrolladores" CssClass="letraNegrita" runat="server"></asp:Label>
						<br />
						<br />
						<asp:Label ID="_etiquetaDaniel" CssClass="colorLetrasGeneral" runat="server" Text="Daniel Madriz Granados"></asp:Label>
						<br />
						&#09;&#09;<asp:Label ID="_etiquetaJesus" CssClass="colorLetrasGeneral" runat="server" Text="Jesús Varela Otárola"></asp:Label>
						<br />
						<br />
						<asp:Label ID="_etiquetaColaboracion" CssClass="letraNegrita" Text="Aportes" runat="server"></asp:Label>
						<br />
						<br />
						<asp:Label ID="_etiquetaTaigin" CssClass="colorLetrasGeneral" runat="server" Text="Taigin Garro Acón"></asp:Label>
						<br />
						&#09;&#09;<asp:Label ID="Label1" CssClass="colorLetrasGeneral" runat="server" Text="Roberto Guzmán Torres"></asp:Label>
						<br />
						<br />
						<asp:Label ID="_etiquetaAportes" CssClass="letraNegrita" Text="Colaboraciones" runat="server"></asp:Label>
						<br />
						<br />
						<asp:Label ID="_etiquetaPablo" CssClass="colorLetrasGeneral" runat="server" Text="Jose Pablo Mora Zúñiga"></asp:Label>
						<br />
                        <asp:Label ID="_etiquetaMax" CssClass="colorLetrasGeneral" runat="server" Text="Max Román Bejarano"></asp:Label>
					</div>
				</div>
			</fieldset>
		</div>
	</div>
</asp:Content>
