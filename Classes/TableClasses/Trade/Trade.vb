Imports Intuitive
Imports Intuitive.Functions

Public Class Trade
    Inherits TableBase

    Public BookingSourceIDs As New ArrayList
    Public EditMode As String

#Region "new"

    Public Sub New()

        Me.Table = "Trade"

        With Me.Fields
            .Add(New Field("TradeID", "Integer"))
            .Add(New Field("ABTAATOLNumber", "String", 15))
            .Add(New Field("TradeName", "String", 50, ValidationType.NotEmptyNotDupe))
            .Add(New Field("TradeParentGroupID", "Integer"))
            .Add(New Field("TradeGroupID", "Integer"))
            .Add(New Field("ConfirmationType", "String", 15, ValidationType.NotEmpty))
            .Add(New Field("ConfirmationEmail", "String", 50))
            .Add(New Field("NotificationStyle", "String", 10, ValidationType.NotEmpty))
            .Add(New Field("Address1", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("Address2", "String", 40))
            .Add(New Field("TownCity", "String", 30, ValidationType.NotEmpty))
            .Add(New Field("County", "String", 30))
            .Add(New Field("Postcode", "String", 20, ValidationType.NotEmpty))
            .Add(New Field("BookingCountryID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("Telephone", "String", 30))
            .Add(New Field("Fax", "String", 30))
            .Add(New Field("Email", "String", 50, ValidationType.IsEmail))
            .Add(New Field("Website", "String", 50))
            .Add(New Field("PreferredContact", "String", 20, ValidationType.NotEmpty))
            .Add(New Field("CurrentTrade", "Boolean"))
        End With

        Me.Clear()
    End Sub


#End Region

#Region "checkdelete"

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'delete child records from TradeAttributeBridge
        SQL.Execute("Delete from TradeAttributeBridge Where TradeID=" & iTableID.ToString)

        'delete child records from TradeBookingSource
        SQL.Execute("Delete from TradeBookingSource Where TradeID=" & iTableID.ToString)

        'delete child records from TradeContact
        SQL.Execute("Delete from TradeContact Where TradeID=" & iTableID.ToString)

        'delete child records from TradeGeography
        SQL.Execute("Delete from TradeGeography Where TradeID=" & iTableID.ToString)

        'delete child records from TradeNote
        SQL.Execute("Delete from TradeNote Where TradeID=" & iTableID.ToString)

        'TODO: Add some Delete checks in!

        Return Me.Warnings.Count = 0
    End Function

#End Region

#Region "checkupdate"

    Public Overrides Function CheckUpdate() As Boolean

        'if the confirmation type is set to batch then confirmation email field is mandatory
        If Me("ConfirmationType") = "Batch" Then
            Me.Fields("ConfirmationEmail").ValidationType = ValidationType.NotEmpty
        End If

        'do the base checkupdate
        MyBase.CheckUpdate()

        'verify that at least one booking source has been selected
        If Me.BookingSourceIDs.Count = 0 Then
            Me.Warnings.Add("At least one Booking Source must be selected")
        End If

        Return Me.Warnings.Count = 0
    End Function



#End Region

#Region "after update - booking sources"

    Private Sub Trade_AfterUpdate(ByVal iTableID As Integer) Handles MyBase.AfterUpdate

        Dim iTradeID As Integer

        'if adding, iTableID is actually SearchID, so get the TradeID from the Search table 
        If Me.EditMode = "Add" Then
            iTradeID = SQL.ExecuteSingleValue("select ParentID from Search where SearchID={0}", _
                iTableID)

            Me.TableID = iTradeID
        Else
            iTradeID = iTableID
        End If

        SQL.Execute("exec TradeBookingSourceAdd {0},'{1}'", _
            iTradeID, ArrayListToDelimitedString(Me.BookingSourceIDs))

    End Sub

#End Region

#Region "Get Booking Country ID"
    Public Shared Function GetBookingCountryID(ByVal iTradeID As Integer) As Integer

        Return SQL.ExecuteSingleValue("select BookingCountryID from Trade where TradeID={0}", iTradeID)
    End Function
#End Region

#Region "Get Trade Name"
    Public Shared Function GetTradeName(ByVal iTradeID As Integer) As String
        Return SQL.GetValue("select TradeName from Trade where TradeID={0}", iTradeID)
    End Function
#End Region

End Class

