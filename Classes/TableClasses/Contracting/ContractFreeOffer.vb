Imports Intuitive

Public Class ContractFreeOffer
    Inherits TableBase

#Region "Properties"

    Private dStayStartDate As Date
    Private dStayEndDate As Date

    Public Property StayStartDate() As Date
        Get
            Return dStayStartDate
        End Get
        Set(ByVal Value As Date)
            dStayStartDate = Value
        End Set
    End Property
    Public Property StayEndDate() As Date
        Get
            Return dStayEndDate
        End Get
        Set(ByVal Value As Date)
            dStayEndDate = Value
        End Set
    End Property
#End Region

#Region "new"
    Public Sub New()

        Me.Table = "ContractFreeOffer"

        With Me.Fields
            .Add(New Field("ContractFreeOfferID", "Integer"))
            .Add(New Field("ContractID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("ContractRoomTypeID", "Integer"))
            .Add(New Field("FreeOfferTypeID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("Notes", "Text"))
            .Add(New Field("StartDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("EndDate", "Date", ValidationType.NotEmptyIsDate))
        End With

        Me.Clear()
    End Sub

#End Region

#Region "UpdateFreeOffers"
    Public Shared Sub UpdateFreeOffers(ByVal iContractID As Integer, ByVal iContractRoomTypeID As Integer, _
        ByVal aFreeOfferTypeIDs As ArrayList, ByVal dDefaultStartDate As Date, ByVal dDefaultEndDate As Date)

        'conver the freeoffer types to a csv
        Dim sFreeOfferTypeCSV As String = Intuitive.Functions.ArrayListToDelimitedString(aFreeOfferTypeIDs)

        SQL.Execute("exec UpdateContractFreeOffers {0},{1},{2},{3},{4}", _
            iContractID, iContractRoomTypeID, SQL.GetSqlValue(sFreeOfferTypeCSV, "String"), _
                SQL.GetSqlValue(dDefaultStartDate, "Date"), SQL.GetSqlValue(dDefaultEndDate, "Date"))

    End Sub

#End Region

#Region "checkupdate"

    Public Overrides Function CheckUpdate() As Boolean

        MyBase.CheckUpdate()

        'bit of extra date validate - in stay date bounds and end date > start date
        If CType(Me("StartDate"), Date) < Me.StayStartDate Then
            Me.Warnings.Add("The Start Date cannot be before " & DateFunctions.DisplayDate(Me.StayStartDate))
            Me.Fields("StartDate").IsValid = False
        End If
        If CType(Me("EndDate"), Date) > Me.StayEndDate Then
            Me.Warnings.Add("The End Date cannot be after " & DateFunctions.DisplayDate(Me.StayEndDate))
            Me.Fields("EndDate").IsValid = False
        End If

        If CType(Me("EndDate"), Date) < CType(Me("StartDate"), Date) Then
            Me.Warnings.Add("The End Date must be after the Start Date")
            Me.Fields("StartDate").IsValid = False
            Me.Fields("EndDate").IsValid = False
        End If

        Return Me.Warnings.Count = 0

    End Function

#End Region

End Class
