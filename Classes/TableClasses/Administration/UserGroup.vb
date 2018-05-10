Imports Intuitive
Imports Intuitive.Functions

Public Class UserGroup
    Inherits TableBase

    Public Sub New()

        Me.Table = "UserGroup"

        With Me.Fields
            .Add(New Field("UserGroupID", "Integer"))
            .Add(New Field("UserGroupName", "String", 50, ValidationType.NotEmptyNotDupe))
            .Add(New Field("UserGroupType", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("BookingAuthorityID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("LoginRedirect", "String", 60))
            .Add(New Field("PasswordExpiresDays", "Integer", ValidationType.NotEmpty))
            .Add(New Field("OptionExpiryUnit", "String", 10))
            .Add(New Field("OptionExpiryAmount", "Integer"))
            .Add(New Field("SuperuserAccess", "Boolean"))
            .Add(New Field("UsDateFormat", "Boolean"))
        End With

        Me.Clear()
    End Sub

#Region "Check Update"

    Public Overrides Function CheckUpdate() As Boolean

        MyBase.CheckUpdate()

        'option expiry; check that both fields completed, or nay
        If SafeInt(Me("OptionExpiryAmount")) > 0 AndAlso Me("OptionExpiryUnit") = "" Then
            Me.Warnings.Add("The Option Expiry Unit must be specified if an amount is entered")
            Me.Fields("OptionExpiryUnit").IsValid = False
        ElseIf Me("OptionExpiryUnit") <> "" AndAlso SafeInt(Me("OptionExpiryAmount")) = 0 Then
            Me.Warnings.Add("The Option Expiry Amount must be specified if a unit has been selected")
            Me.Fields("OptionExpiryAmount").IsValid = False
        End If

        Return Me.Warnings.Count = 0

    End Function

#End Region

#Region "Check Delete"

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in SystemUser
        If Not SQL.FKCheck("SystemUser", "UserGroupID", iTableID) Then
            Me.Warnings.Add("The User Group can not be deleted as it has users associated with it")
        End If

        'delete child records from UserGroupContractSection, UserGroupLocation, UserGroupPropertyGroup
        If Me.Warnings.Count = 0 Then
            SQL.Execute("Delete from UserGroupContractSection Where UserGroupID=" & iTableID.ToString)
            SQL.Execute("Delete from UserGroupLocation Where UserGroupID=" & iTableID.ToString)
            SQL.Execute("Delete from UserGroupPropertyGroup Where UserGroupID=" & iTableID.ToString)
        End If

        Return Me.Warnings.Count = 0
    End Function

#End Region

#Region "SetAccessRights"

    Public Shared Sub SetAccessRights(ByVal iUserGroupID As Integer, ByVal aAccessRightIDs As ArrayList)

        Dim sAccessRightIDs As String = ArrayListToDelimitedString(aAccessRightIDs)
        SQL.Execute("exec UserGroupSetAccessRights {0}, '{1}'", _
            iUserGroupID, sAccessRightIDs)

    End Sub

#End Region

#Region "GetOptionExpiryDate"
    Public Function GetOptionExpiryDate(ByVal dStartDateTime As DateTime) As DateTime

        Dim iOptionExpiryUnit As Integer = SafeInt(Me("OptionExpiryAmount"))
        Select Case Me("OptionExpiryUnit")
            Case "Years"
                dStartDateTime = dStartDateTime.AddYears(iOptionExpiryUnit)
            Case "Months"
                dStartDateTime = dStartDateTime.AddMonths(iOptionExpiryUnit)
            Case "Days"
                dStartDateTime = dStartDateTime.AddDays(iOptionExpiryUnit)
            Case "Minutes"
                dStartDateTime = dStartDateTime.AddMinutes(iOptionExpiryUnit)
            Case "Seconds"
                dStartDateTime = dStartDateTime.AddSeconds(iOptionExpiryUnit)
        End Select
        Return dStartDateTime

    End Function

#End Region
End Class
