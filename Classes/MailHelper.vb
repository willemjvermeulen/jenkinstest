Imports System.Net.Mail
Imports System.Net
Imports System.IO
Imports System.Configuration
'Imports EASendMail

Public Class MailHelper

    Property Subject() As String

    Property Body() As String

    Property Recipient() As String

    Property SenderToDisplay() As String

    Property AttachmentPath() As String

    Property CC() As String

    Property IsBodyHtml() As Boolean = True

    Property LastError() As String

    Public Function TrySend() As Boolean
        Try
            Me.Send()
            Return True
        Catch ex As Exception
            Me.LastError = "Email Send Failed " & ex.ToString
            Return False
        End Try
    End Function

    Public Sub Send()

        Dim message As MailMessage = New MailMessage()
        message.From = New MailAddress(AppSettings("SMTP365_USERNAME"))
        message.Subject = Subject
        message.Body = Body
        message.IsBodyHtml = IsBodyHtml

        If Not String.IsNullOrWhiteSpace(Recipient) Then
            message.To.Add(Recipient)
        End If

        If Not String.IsNullOrWhiteSpace(CC) Then
            message.CC.Add(CC)
        End If

        If File.Exists(AttachmentPath) Then
            message.Attachments.Add(New Attachment(AttachmentPath))
        End If
        ServicePointManager.ServerCertificateValidationCallback = AddressOf AcceptAllCertifications
        Dim client As New SmtpClient()
        client.UseDefaultCredentials = False
        client.Credentials = New NetworkCredential(AppSettings("SMTP365_USERNAME"), AppSettings("SMTP365_PASSWORD"))
        client.Port = 587
        client.Host = AppSettings("SMTP365_HOST")
        client.DeliveryMethod = SmtpDeliveryMethod.Network
        client.EnableSsl = True
        client.Timeout = 3000000
        client.Send(message)
        message.Dispose()

    End Sub

    Public Function AcceptAllCertifications(ByVal sender As Object, ByVal certification As System.Security.Cryptography.X509Certificates.X509Certificate, ByVal chain As System.Security.Cryptography.X509Certificates.X509Chain, ByVal sslPolicyErrors As System.Net.Security.SslPolicyErrors) As Boolean
        Return True
    End Function

    Private Function AppSettings(ByVal key As String) As String
        Return ConfigurationManager.AppSettings(key)
    End Function

    Private Function AppSettingsAsBoolean(ByVal key As String) As Boolean

        Dim valueString As String = AppSettings(key)
        Dim value As Boolean
        If Boolean.TryParse(valueString, value) Then
            Return value
        End If

        Return False

    End Function

End Class
