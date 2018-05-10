Imports Intuitive
Imports Intuitive.Functions
Imports System.text
Public Class SystemUser
    Inherits TableBase

#Region "New"

    Public Sub New()

        Me.Table = "SystemUser"

        With Me.Fields
            .Add(New Field("SystemUserID", "Integer"))
            .Add(New Field("UserName", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("Email", "String", 50, ValidationType.NotEmptyIsEmail))
            .Add(New Field("Password", "String", 50))
            .Add(New Field("UserGroupID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("CurrentSystemUser", "Boolean"))
            .Add(New Field("PasswordExpires", "Date"))
            .Add(New Field("ContractAdministrator", "Boolean"))
        End With

        Me.Clear()
    End Sub

#End Region

#Region "Check Update"

    Public Overrides Function CheckUpdate() As Boolean

        'do some password stuff
        'if we are adding then we'll need a password
        'otherwise get the password from the database and set it
        If Me.TableID = 0 Then
            Dim sValidatePassword As String = SystemUser.ValidatePassword(Me("Password"))
            If sValidatePassword <> "" Then
                Me.Warnings.Add(sValidatePassword)
            End If
        End If

        If Me.TableID <> 0 And Me("Password") = "" Then
            Dim oTempUser As New SystemUser
            oTempUser.Go(Me.TableID)
            Me.SetField("Password", oTempUser("Password"))
        End If

        MyBase.CheckUpdate()

        Return Me.Warnings.Count = 0
    End Function

#End Region

#Region "Check Delete"

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in Contract
        If Not SQL.FKCheck("Contract", "SystemUserID", iTableID) Then
            Me.Warnings.Add("The User cannot be deleted as he/she is associated with one or more contracts")
        End If

        'check for records in AuditTrailEntry
        If Not SQL.FKCheck("AuditTrailEntry", "SystemUserID", iTableID) Then
            Me.Warnings.Add("The User cannot be deleted as he/she is associated with one or more audit trail entries")
        End If

        Return Me.Warnings.Count = 0
    End Function

#End Region

#Region "GetUserNameFromSystemUserID"

    Public Shared Function GetUserNameFromSystemUserID(ByVal iSystemUserID As Integer) As String
        Return SQL.GetValue("Select UserName from SystemUser where SystemUserID={0}", iSystemUserID)
    End Function

#End Region

#Region "ChangePassword"

    Public Shared Function ChangePassword(ByVal iSystemUserID As Integer, _
        ByVal sOldPassword As String, ByVal sNewPassword As String, _
        ByVal sNewPasswordConfirm As String) As FunctionReturn

        Dim oReturn As New FunctionReturn

        'get the user information
        Dim oSystemUser As New SystemUser
        oSystemUser.Go(iSystemUserID)

        'old password
        If Not UserSession.PasswordHash(sOldPassword) = oSystemUser("Password") Then
            oReturn.AddWarning("The Old Password you supplied is incorrect")
        End If

        'new password and confirmation
        Dim sValidatePassword As String = SystemUser.ValidatePassword(sNewPassword)
        If sValidatePassword <> "" Then
            oReturn.AddWarning(sValidatePassword)
        ElseIf sNewPassword.ToLower <> sNewPasswordConfirm.ToLower Then
            oReturn.AddWarning("The New Password has not been confirmed correctly")
        End If

        'if all is well in the world update
        If oReturn.Success Then
            oSystemUser.SetField("Password", UserSession.PasswordHash(sNewPassword))
            oSystemUser.Update()
            SetPasswordExpiry(iSystemUserID)
        End If

        Return oReturn

    End Function

#End Region

#Region "Validate Password"

    Public Shared Function ValidatePassword(ByVal sPassword As String) As String

        If sPassword.Trim.Length < 3 Then
            Return "The Password must be at least 3 characters long"
        Else
            Return ""
        End If
    End Function
#End Region

#Region "SetPasswordExpiry"

    Public Shared Sub SetPasswordExpiry(ByVal iSystemUserID As Integer)
        SQL.Execute("exec SystemUserSetPasswordExpiry {0}", iSystemUserID)
    End Sub

#End Region

#Region "Change User Group"
    Public Shared Sub ChangeUserGroup(ByVal iSystemUserID As Integer, ByVal iUserGroupID As Integer)

        SQL.Execute("update SystemUser set UserGroupID={0} where SystemUserID={1}", iUserGroupID, iSystemUserID)
    End Sub
#End Region

#Region "Get Contract Admin Email"
    Public Shared Function GetContractAdminEmail() As String

        Dim dt As DataTable = SQL.GetDatatable("select Email from SystemUser where ContractAdministrator=1")
        Dim sb As New StringBuilder
        For Each dr As DataRow In dt.Rows

            sb.Append(dr("Email")).Append(";")
        Next

        Return Chop(sb.ToString)
    End Function
#End Region

    Private Sub SystemUser_AfterAdd(ByVal iTableID As Integer) Handles MyBase.AfterAdd
        SystemUser.SetPasswordExpiry(iTableID)
    End Sub
End Class
