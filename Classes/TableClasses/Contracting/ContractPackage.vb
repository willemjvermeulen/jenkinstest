Imports Intuitive
Imports Intuitive.DateFunctions
Imports Intuitive.Functions
Imports System.Text

Public Class ContractPackage
    Inherits TableBase

#Region "Properties"

    Private iMealPlansSelected As Integer = 0
    Private dBookingStartDate As Date
    Private dBookingEndDate As Date
    Public ExclusionCount As Integer = 0

    Public Property MealPlansSelected() As Integer
        Get
            Return iMealPlansSelected
        End Get
        Set(ByVal Value As Integer)
            iMealPlansSelected = Value
        End Set
    End Property

    Public Property BookingStartDate() As Date
        Get
            Return dBookingStartDate
        End Get
        Set(ByVal Value As Date)
            dBookingStartDate = Value
        End Set
    End Property

    Public Property BookingEndDate() As Date
        Get
            Return dBookingEndDate
        End Get
        Set(ByVal Value As Date)
            dBookingEndDate = Value
        End Set
    End Property

#End Region

    Public Sub New()

        Me.Table = "ContractPackage"

        With Me.Fields
            .Add(New Field("ContractPackageID", "Integer"))
            .Add(New Field("ContractID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("ContractRoomTypeID", "Integer"))
            .Add(New Field("Name", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("BookingStartDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("BookingEndDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("MealBasisType", "String", 20, ValidationType.NotEmpty))
            .Add(New Field("Description", "Text"))
            .Add(New Field("RateCalculation", "String", 20, ValidationType.NotEmpty))
            .Add(New Field("DurationCalculation", "String", 20, ValidationType.NotEmpty))
            .Add(New Field("MinStay", "Integer"))
            .Add(New Field("MaxStay", "Integer"))
            .Add(New Field("PassengerTypes", "String", 50))
            .Add(New Field("PackageDated", "Boolean"))
            .Add(New Field("ResidentsOnly", "Boolean"))
            .Add(New Field("ContractPackageXenonTypeID", "Integer"))
            .Add(New Field("XenonFreePrice", "Integer"))
        End With

        Me.Clear()
    End Sub

#Region "Check Delete"

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'delete child records from ContractPackageCost
        If Me.Warnings.Count = 0 Then
            SQL.Execute("Delete from ContractPackageCost Where ContractPackageID=" & iTableID.ToString)
            'delete child records from ContractPackageMealBasis
            SQL.Execute("Delete from ContractPackageMealBasis Where ContractPackageID=" & iTableID.ToString)
            SQL.Execute("Delete from ContractPackageExclusion Where ContractPackageID=" & iTableID.ToString)
        End If

        Return Me.Warnings.Count = 0
    End Function

#End Region

#Region "Check Update"

    Public Overrides Function CheckUpdate() As Boolean
        MyBase.CheckUpdate()
        Dim sWarn As String

        Fields("ContractPackageXenonTypeID").Value = HttpContext.Current.Request.Form.Get("ddContractPackageXenonTypes")

        'if this is for a 'All' roomtype, then we may get an warning
        'if its duped in any other contract for 'All' Roomtype - because they all share the same id (0)
        If GetField("ContractRoomTypeID") = "0" AndAlso Me.Warnings.Count > 0 Then
            For Each sWarn In Me.Warnings
                If sWarn = "This Name already exists" Then
                    Me.Warnings.RemoveAt(Me.Warnings.LastIndexOf(sWarn))
                    Exit For
                End If
            Next
        End If

        If Me.Warnings.Count < 1 Then
            Dim dStart As Date
            Dim dEnd As Date

            'Check Booking Dates in order
            dStart = CType(Me.GetField("BookingStartDate"), Date)
            dEnd = CType(Me.GetField("BookingEndDate"), Date)

            If dStart < Me.BookingStartDate Then
                Me.Warnings.Add("The earliest Booking Start Date allowed is " & _
                    DisplayDate(Me.BookingStartDate))
            ElseIf dEnd > Me.BookingEndDate Then
                Me.Warnings.Add("The latest Booking End Date allowed is " & _
                    DisplayDate(Me.BookingEndDate))
            ElseIf dStart > dEnd Then
                Me.Warnings.Add("The Booking Start Date must be before the Booking End Date")
            End If


            'if meal plan type is Selected, make sure we have at least one meal plan selected
            If Me("MealBasisType") = "Selected" AndAlso Me.MealPlansSelected < 1 Then
                Me.Warnings.Add("Sorry, at least one Meal Plan must be selected")
            End If

        End If

        'check exclusions
        If SafeInt(Me("ContractRoomTypeID")) = 0 AndAlso Me.ExclusionCount > 0 Then

            If Me.ExclusionCount >= SQL.ExecuteSingleValue("select count(*) from ContractRoomType where ContractID={0}", Me("ContractID")) Then
                Me.Warnings.Add("You cannot exclude all room types from a package")
            End If
        End If

            Return Me.Warnings.Count < 1
    End Function


#End Region

#Region "Set Costs"

    Public Shared Sub SetCosts(ByVal iContractPackageID As Integer, ByVal ePassengerType As enmPasengerType, ByVal aGridData As ArrayList)

        'clear existing records
        SQL.Execute("Delete from ContractPackageCost where ContractPackageId=" & iContractPackageID)

        'set up stored procedure declare
        Dim sb As New StringBuilder
        Dim sSql As String = "exec AddContractPackageCost {0},{1},{2},{3},{4},{5},{6} \n"

        'Work out the passenger types

        Dim iYouth As Integer = 0
        Dim iAdult As Integer = 0
        Dim iChild As Integer = 0
        Dim iAll As Integer = 0

        'build up the sp calls
        Dim oGridRow As Intuitive.WebControls.Grid.GridRow
        For Each oGridRow In aGridData

            If ePassengerType = enmPasengerType.ChildYouthAdult Then
                iYouth = Intuitive.Functions.SafeInt(oGridRow("Youth"))
            End If
            If ePassengerType = enmPasengerType.ChildYouthAdult Or ePassengerType = enmPasengerType.AdultChild Then
                iChild = Intuitive.Functions.SafeInt(oGridRow("Child"))
            End If
            If ePassengerType <> enmPasengerType.All Then
                iAdult = Intuitive.Functions.SafeInt(oGridRow("Adult"))
            End If

            If ePassengerType = enmPasengerType.All Then
                iAll = Intuitive.Functions.SafeInt(oGridRow("Value"))
            End If

            sb.AppendFormat(sSql, iContractPackageID, _
                oGridRow("Start"), oGridRow("End"), iChild, iYouth, iAdult, iAll)

        Next

        'execute if we have any values
        If sb.ToString <> "" Then
            SQL.Execute(sb.ToString.Replace("\n", ControlChars.NewLine))
        End If


    End Sub


#End Region

#Region "Pasenger type Enums"
    Public Enum enmPasengerType As Byte
        AdultOnly = 1
        AdultChild = 2
        ChildYouthAdult = 3
        All = 0
    End Enum
#End Region


#Region "Save Exclusions"

    Public Shared Sub SaveExclusions(ByVal ContractPackageID As Integer, ByVal Exclusions As ArrayList)

        Dim oSQL As New SQLTransaction
        With oSQL
            .Add("delete from ContractPackageExclusion where ContractPackageID={0}", ContractPackageID)

            For Each iContractRoomTypeID As Integer In Exclusions
                .Add("insert into ContractPackageExclusion select {0},{1}", ContractPackageID, iContractRoomTypeID)
            Next

            .Execute()
        End With
    End Sub

#End Region

End Class
