Imports Intuitive
Imports Intuitive.Functions
Imports Intuitive.WebControls
Imports System.Text

Public Class BookingSource
    Inherits TableBase

    Public Enum FieldSet
        Main
        TermsAndConditions
    End Enum

    Private oFieldSet As FieldSet

#Region "new"

    Public Sub New(ByVal FieldSet As FieldSet)

        Me.Table = "BookingSource"

        'main
        If FieldSet = FieldSet.Main Then
            With Me.Fields
                .Add(New Field("BookingSourceID", "Integer"))
                .Add(New Field("BookingSource", "String", 30, ValidationType.NotEmptyNotDupe))
                .Add(New Field("BookingSourceCode", "String", 4, ValidationType.NotEmptyNotDupe))
                .Add(New Field("Custom", "Boolean"))
                .Add(New Field("OperateAs", "String", 20, ValidationType.NotEmpty))
                .Add(New Field("ConfirmTerms", "Boolean"))
                .Add(New Field("MaximumNumberOfRooms", "Integer", ValidationType.NotEmpty))
                .Add(New Field("TermsAndConditionsOnline", "String"))
                .Add(New Field("TermsAndConditionsCallCentre", "String"))
                .Add(New Field("CurrentBookingSource", "Boolean"))
                .Add(New Field("Commission", "Double"))
                .Field("TermsAndConditionsOnline").NonBinding = True
                .Field("TermsAndConditionsCallCentre").NonBinding = True
            End With
        End If

        'sperms and conditions
        If FieldSet = FieldSet.TermsAndConditions Then
            With Me.Fields
                .Add(New Field("BookingSourceID", "Integer"))
                .Add(New Field("TermsAndConditionsOnline", "Text"))
                .Add(New Field("TermsAndConditionsCallCentre", "Text"))
            End With
        End If

        'store the fieldset 
        oFieldSet = FieldSet

        Me.Clear()
    End Sub


#End Region

#Region "check delete"
    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean


        'check for records in Contract
        If Not SQL.FKCheck("Contract", "BookingSourceID", iTableID) Then
            Me.Warnings.Add("This Booking Source cannot be deleted as it is associated with one or more contracts")
        End If

        'check for records in TradeBookingSource
        If Not SQL.FKCheck("TradeBookingSource", "BookingSourceID", iTableID) Then
            Me.Warnings.Add("The Booking Source cannot be deleted as it has Trade Members associated with it")
        End If

        If Me.Warnings.Count = 0 Then
            'delete child records from BookingSourceCancellation
            SQL.Execute("Delete from BookingSourceCancellation Where BookingSourceID=" & iTableID.ToString)

            'delete child records from BookingSourceCancellationOverride
            SQL.Execute("Delete from BookingSourceCancellationOverride Where BookingSourceID=" & iTableID.ToString)

            'delete child records from BookingSourcePrePay
            'SQL.Execute("Delete from BookingSourcePrePay Where BookingSourceID=" & iTableID.ToString)
        End If


        Return Me.Warnings.Count = 0
    End Function

#End Region

#Region "setup"

    Public Sub Setup()
        Me.Clear()
        Me.SetField("Custom", True)
        Me.SetField("ConfirmTerms", True)
        Me.SetField("CurrentBookingSource", True)
    End Sub

#End Region

#Region "IsPublic"
    Public Shared Function IsPublic(ByVal iBookingSourceID As Integer) As Boolean

        Dim iPublicBookingSourceID As Integer = SafeInt(HttpContext.Current.Cache("PublicBookingSourceID"))
        If iPublicBookingSourceID = 0 Then

            iPublicBookingSourceID = SQL.ExecuteSingleValue("select BookingSourceID from BookingSource where BookingSource='Public'")
            AddToCache("PublicBookingSourceID", iPublicBookingSourceID, 60)
        End If

        Return iPublicBookingSourceID = iBookingSourceID

    End Function
#End Region

#Region "Select Default"
    Public Shared Function DefaultBookingSource() As Integer
        Return SQL.ExecuteSingleValue("select BookingSourceID from BookingSource where DefaultBookingSource=1")
    End Function
#End Region

End Class
