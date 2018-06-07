Imports System.Web.Hosting

Public Class Master
    Inherits System.Web.UI.MasterPage

    Private _credenciales As Collections.Hashtable = CType(Me.Context.Items("Credentials"), Collections.Hashtable)
    'Private _log As ILog = LogManager.GetLogger("xx")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            If Page.User.Identity.IsAuthenticated Then
                CargarControles()
            Else
                Salir()
            End If
            'MyBase.OnLoad(e)
            Page.Header.DataBind()
        End If
    End Sub

    Protected Sub CargarControles()
        Try
            Me.lbtnUsuario.Text = String.Format(" {0} | {1}", _credenciales("Usuario"), _credenciales("RolUsuario"))
            Me.lbtnUsuario.PostBackUrl = Tools.EncriptarQS("~/_Usuario/CuentaUsuario.aspx?id=", _credenciales("IDUsuario"))
            CargarMenu()
        Catch ex As Exception
            'Tools.MostrarMensaje(ex.Message & " (En master cargando controles)", Me.Page, TipoMensaje.Error)
            '_log.Error(ex.Message & " (En master cargando controles)", ex)
        End Try
    End Sub

    Protected Sub CargarMenu()
        Try


            Dim idRol As Integer = Convert.ToInt32(_credenciales("IDRol"))
            Dim urlDirVirtual As String = "~" & HostingEnvironment.ApplicationVirtualPath
            Dim dsRelacion As DataSet = BLL.Seguridad.MenuXRolManager.Obtener(_credenciales, idRol, True)

            Dim xmlDataSource As XmlDataSource = New XmlDataSource()
            xmlDataSource.ID = "XmlSource1"
            xmlDataSource.EnableCaching = False

            dsRelacion.DataSetName = "Menus"
            dsRelacion.Tables(0).TableName = "Menu"

            Dim datarelation As DataRelation = New DataRelation("ParentChild",
                                                        dsRelacion.Tables("Menu").Columns("ID"),
                                                        dsRelacion.Tables("Menu").Columns("IDMenuPadre"),
                                                        True)
            datarelation.Nested = True
            dsRelacion.Relations.Add(datarelation)

            xmlDataSource.Data = dsRelacion.GetXml()

            'Reformat the xmldatasource from the dataset to fit menu into xml format
            xmlDataSource.TransformFile = Server.MapPath("~/Menu/Menu.xslt")

            'assigning the path to start read all MenuItem under MenuItems
            xmlDataSource.XPath = "MenuItems/MenuItem"

            'Finally, bind the source to the Menu1 control
            mnuSires.DataSource = xmlDataSource
            mnuSires.DataBind()

        Catch ex As Exception
            Dim x As String = ""
            'Tools.MostrarMensaje(ex.Message & " (En master cargando menú)", Me.Page, TipoMensaje.Error)
            '_log.Error(ex.Message & " (En master cargando menú)", ex)
        End Try
    End Sub

    Protected Sub lbtnSalir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnSalir.Click
        Salir()
    End Sub
    Private Sub Salir()
        HttpContext.Current.Session.Clear()
        HttpContext.Current.Session.Abandon()
        HttpContext.Current.User = Nothing
        System.Web.Security.FormsAuthentication.SignOut()
        Response.Redirect(System.Web.Security.FormsAuthentication.LoginUrl)
    End Sub
End Class