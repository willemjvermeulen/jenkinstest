Imports Intuitive
Imports Intuitive.Functions

Public Class BookingCountry
    Inherits TableBase

    Public Sub New()

        Me.Table = "BookingCountry"

        With Me.Fields
            .Add(New Field("BookingCountryID", "Integer"))
            .Add(New Field("BookingCountry", "String", 30, ValidationType.NotEmptyNotDupe))
            .Add(New Field("DefaultCountry", "Boolean"))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in Customer
        If Not SQL.FKCheck("Customer", "BookingCountryID", iTableID) Then
            Me.Warnings.Add("BookingCountry cannot be deleted as it is in use")
        End If

        'check for records in CustomerDetail
        If Not SQL.FKCheck("CustomerDetail", "BookingCountryID", iTableID) Then
            Me.Warnings.Add("BookingCountry cannot be deleted as it is in use")
        End If

        'check for records in PropertyRoomGuest
        If Not SQL.FKCheck("PropertyRoomGuest", "BookingCountryID", iTableID) Then
            Me.Warnings.Add("BookingCountry cannot be deleted as it is in use")
        End If

        'check for records in Trade
        If Not SQL.FKCheck("Trade", "BookingCountryID", iTableID) Then
            Me.Warnings.Add("BookingCountry cannot be deleted as it is in use")
        End If

        If Me.Warnings.Count > 0 Then
            Me.Warnings.Clear()
            Me.Warnings.Add("BookingCountry cannot be deleted as it is in use")
        End If

        Return Me.Warnings.Count = 0
    End Function

    Public Overrides Function CheckUpdate() As Boolean

        MyBase.CheckUpdate()

        'if all's ok and the country is set to default then make sure the rest aren't
        If Me.Warnings.Count = 0 AndAlso SafeBoolean(Me("DefaultCountry")) Then
            SQL.Execute("update BookingCountry set DefaultCountry=0")
        End If

        Return Me.Warnings.Count = 0
    End Function
End Class

