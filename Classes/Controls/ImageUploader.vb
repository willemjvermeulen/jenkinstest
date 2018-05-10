Imports Intuitive
Imports Intuitive.WebControls
Imports Intuitive.Functions
Imports Intuitive.Images.ImageFunctions
Imports System.text
Imports System.IO


Public Class ImageUploader
    Inherits Control

    'properties
    Public HideByDefault As Boolean = False
    Public WorkingImage As String
    Public ShowUploadButton As Boolean = True

    'events
    Public Event Upload()
    Public Event UploadWarning(ByVal Warning As String)


#Region "Render"
    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)


        Dim sb As New StringBuilder
        With sb

            'open the div
            sb.AppendFormat("<div id=""{0}"" class=""imageuploader""{1}>\n", Me.ID, _
                IIf(Me.HideByDefault, " style=""display:none;""", ""))

            sb.Append("<label>Add File</label>\n")

            'add file upload control
            sb.Append("<div>")
            sb.AppendFormat("<input type=""file"" id=""fil{0}"" name=""fil{0}"" class=""file""  enctype=""multipart/form-data""/>\n", Me.ID)
            sb.Append("</div>")

            sb.Append("<label>or URL</label>\n")

            'add textbox
            Dim oTextBox As New TextBox
            oTextBox.ID = "txt" & Me.ID
            sb.Append("<div>").Append(RenderControlToString(oTextBox)).Append("</div>\n")


            'add in buttons if we're showing upload or cancel button
            If Me.ShowUploadButton OrElse Me.HideByDefault Then

                sb.Append("<div class=""buttons"">\n")

                'add upload button
                If Me.ShowUploadButton Then
                    Dim oButton As New Button
                    oButton.ID = "btnImageUploader" & Me.ID
                    oButton.Caption = "Upload"
                    sb.Append(RenderControlToString(oButton)).Append("\n")
                End If

                'add cancel button
                If Me.HideByDefault Then
                    Dim oButton As New Button
                    oButton.ID = "btnCancelImageUploader" & Me.ID
                    oButton.Caption = "Cancel"
                    oButton.Script = String.Format("f.Hide('{0}');f.ClearFileUpload('fil{0}');" & _
                        "f.SetValue('txt{0}','');", Me.ID)
                    sb.Append(RenderControlToString(oButton)).Append("\n")
                End If

                sb.Append("</div>\n")
            End If



            'close the div
            sb.Append("</div>")

        End With

        writer.Write(sb.ToString.Replace("\n", ControlChars.NewLine))
        MyBase.Render(writer)
    End Sub
#End Region

#Region "OnLoad - Postback checking"

    Private Sub ImageUploader_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        'check to see if a url has been specified, if so try and download and store the file
        'else check for a posted file, if so store the file
        If Me.Page.IsPostBack AndAlso Me.Page.Request("Command") = "btn" & Me.ID Then

            Dim sWorkFolder As String = ImageUploader.GetWorkFolder()

            'specify image name
            Dim sWorkingImageName As String = UserSession.Username.Replace(" ", "") & "_" & Now.ToString("HHmmss")
            Dim sExtension As String = ""

            'try the url
            Dim sURL As String = SafeString(Me.Page.Request("txt" & Me.ID))
            If sURL <> "" Then

                'try and download image to working folder
                sExtension = Right(sURL, 4)
                DownloadImage(sURL, sWorkFolder, sWorkingImageName & sExtension)

            End If

            'if no url look for a posted file
            If sURL = "" Then


                Dim oPostedImage As HttpPostedFile = Me.Page.Request.Files("fil" & Me.ID)
                If oPostedImage.ContentLength > 0 Then
                    sExtension = Right(oPostedImage.FileName, 4)
                    oPostedImage.SaveAs(sWorkFolder & sWorkingImageName & sExtension)

                End If

            End If

            'check it's all ok
            If File.Exists(sWorkFolder & sWorkingImageName & sExtension) Then
                Me.WorkingImage = sWorkFolder & sWorkingImageName & sExtension
            End If

        End If

    End Sub

#End Region



#Region "File Handling"

#Region "Get Work folder"

    Public Shared Function GetWorkFolder() As String

        Dim sFolder As String = MapPath("/work")
        Intuitive.FileFunctions.CreateFolder(sFolder)

        Return sFolder
    End Function

#End Region

#End Region

End Class
