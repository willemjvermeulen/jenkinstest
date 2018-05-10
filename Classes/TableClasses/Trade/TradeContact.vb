Imports Intuitive
Imports Intuitive.Functions
Public Class TradeContact
    Inherits TableBase
    Private BindMode As eBindMode
    Public TradeID As Integer
    Public Enum eBindMode
        Main
        Details
        Invoicing
    End Enum

    Public Sub New(Optional ByVal BindMode As eBindMode = eBindMode.Main)

        Me.Table = "TradeContact"
        Me.BindMode = BindMode

        'main
        If BindMode = TradeContact.eBindMode.Main Then
            With Me.Fields
                .Add(New Field("TradeContactID", "Integer"))
                .Add(New Field("TradeID", "Integer", ValidationType.NotEmpty))
                .Add(New Field("ContactName", "String", 40, ValidationType.NotEmpty))
                .Add(New Field("Title", "String", 10, ValidationType.NotEmpty))
                .Add(New Field("Forename", "String", 30, ValidationType.NotEmpty))
                .Add(New Field("Surname", "String", 30, ValidationType.NotEmpty))
                .Add(New Field("Email", "String", 50, ValidationType.IsEmail))
                .Field("Email").ControlIDOverride = "txtContactEmail"
                .Add(New Field("PasswordHash", "String", 20))
                .Add(New Field("TradeContactGroupID", "Integer", ValidationType.NotEmpty))
                .Add(New Field("JobTitle", "String", 60))
                .Add(New Field("Telephone", "String", 30))
                .Field("Telephone").ControlIDOverride = "txtContactTelephone"
            End With

        End If

        If BindMode = TradeContact.eBindMode.Details Then
            With Me.Fields
                .Add(New Field("TradeContactID", "Integer"))
                .Add(New Field("TradeID", "Integer", ValidationType.NotEmpty))
                .Add(New Field("ContactName", "String", 40, ValidationType.NotEmpty))
                .Add(New Field("Address1", "String", 40))
                .Add(New Field("Address2", "String", 40))
                .Add(New Field("TownCity", "String", 30))
                .Add(New Field("County", "String", 30))
                .Add(New Field("Postcode", "String", 20))
                .Add(New Field("Country", "String", 30))
                .Add(New Field("Telephone", "String", 20))
                .Add(New Field("Mobile", "String", 20))
                .Add(New Field("Fax", "String", 20))
            End With
        End If

        If BindMode = eBindMode.Invoicing Then
            With Me.Fields
                .Add(New Field("TradeContactID", "Integer"))
                .Add(New Field("DefaultInvoiceContact", "Boolean"))
                .Add(New Field("NotificationMethod", "String", ValidationType.NotEmpty))
                .Add(New Field("Email", "String", 50))
                .Add(New Field("Fax", "String", 20))
            End With
        End If

        Me.Clear()
    End Sub

    Public Overrides Function CheckUpdate() As Boolean

        If Me.BindMode <> eBindMode.Invoicing Then

            'if we are adding then we'll need a password
            'otherwise get the password from the database and set it
            If Me.TableID = 0 Then
                Dim sValidatePassword As String = SystemUser.ValidatePassword(Me("PasswordHash"))
                If sValidatePassword <> "" Then
                    Me.Warnings.Add(sValidatePassword)
                    Me.Fields("PasswordHash").IsValid = False
                End If
            End If

            MyBase.CheckUpdate()

            'if all's well then hash the password or get the old one
            If Me.Warnings.Count = 0 Then

                If Me.TableID <> 0 And Me("PasswordHash") = "" Then
                    Dim oTempContact As New TradeContact
                    oTempContact.Go(Me.TableID)
                    Me.SetField("PasswordHash", oTempContact("PasswordHash"))
                Else
                    Me.SetField("PasswordHash", UserSession.PasswordHash(Me("PasswordHash")))
                End If
            End If

        Else
            MyBase.CheckUpdate()

            'need to check that if they choose fax or address then we actually have the details
            Dim sNotificationMethod As String = Me("NotificationMethod")
            Select Case sNotificationMethod
                Case "Email"
                    If Me("Email") = "" Then
                        Me.Warnings.Add("No Email Address has been set up for this Contact")
                    End If
                Case "Fax"
                    If Me("Fax") = "" Then
                        Me.Warnings.Add("No Fax Number has been set up for this Contact")
                    End If
            End Select
        End If

        Return Me.Warnings.Count = 0
    End Function


    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in TradeNote
        If Not SQL.FKCheck("TradeNote", "TradeContactID", iTableID) Then
            Me.Warnings.Add("TradeContact cannot be deleted as it is in use")
        End If

        'delete child records from TradeNote
        SQL.Execute("Delete from TradeNote Where TradeContactID=" & iTableID.ToString)

        Return Me.Warnings.Count = 0
    End Function

    Public Shared Function GetName(ByVal iTradeContactID As Integer) As String

        Return SQL.GetValue("select ContactName from TradeContact where TradeContactID={0}", _
            iTradeContactID)

    End Function

    Public Shared Function GetEmail(ByVal iTradeContactID As Integer) As String
        Return SQL.GetValue("select Email from TradeContact where TradeContactID={0}", _
            iTradeContactID)
    End Function

    Private Sub TradeContact_BeforeUpdate(ByVal iTableID As Integer) Handles MyBase.BeforeUpdate

        'if they've been set to default then set all others not to default
        If BindMode = eBindMode.Invoicing AndAlso SafeBoolean(Me("DefaultInvoiceContact")) Then

            SQL.Execute("update TradeContact Set DefaultInvoiceContact=0 where TradeID={0}", _
                Me.TradeID)
        End If
    End Sub
End Class

