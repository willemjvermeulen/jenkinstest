Imports Intuitive
Imports Intuitive.Functions


Public Class ContractRoomType
    Inherits TableBase

    Private iSaveContractID As Integer
    Public BedTypeIDs As New ArrayList

#Region "New"

    Public Sub New()

        Me.Table = "ContractRoomType"

        With Me.Fields
            .Add(New Field("ContractRoomTypeID", "Integer"))
            .Add(New Field("ContractID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("PropertyRoomTypeID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("MealBasisID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("ContractBasis", "String", 20))
            .Add(New Field("MinOcc", "Integer", ValidationType.NotEmpty))
            .Add(New Field("StdOcc", "Integer", ValidationType.NotEmpty))
            .Add(New Field("MaxOcc", "Integer", ValidationType.NotEmpty))
            .Add(New Field("MinAdults", "Integer"))
            .Add(New Field("MaxAdults", "Integer", ValidationType.NotEmpty))
            .Add(New Field("MaxChildren", "Integer"))
            .Add(New Field("Infants", "Boolean"))
            .Add(New Field("InfantsOccupancy", "Boolean"))
            .Add(New Field("ChildAgeFrom", "Integer"))
            .Add(New Field("ChildAgeTo", "Integer"))
            .Add(New Field("YouthAgeFrom", "Integer"))
            .Add(New Field("YouthAgeTo", "Integer"))
            .Add(New Field("AdjoiningRooms", "Boolean"))
            .Add(New Field("DisabledFacilities", "Boolean"))
            .Add(New Field("SmokingRooms", "Boolean"))
            .Add(New Field("ExtraBedTypeID", "Integer"))
            .Add(New Field("WeekendDayMon", "Boolean"))
            .Add(New Field("WeekendDayTue", "Boolean"))
            .Add(New Field("WeekendDayWed", "Boolean"))
            .Add(New Field("WeekendDayThu", "Boolean"))
            .Add(New Field("WeekendDayFri", "Boolean"))
            .Add(New Field("WeekendDaySat", "Boolean"))
            .Add(New Field("WeekendDaySun", "Boolean"))
            .Add(New Field("Sequence", "Integer"))
        End With

        Me.Clear()
    End Sub

#End Region

#Region "Check Delete"

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean
        'save the ContractID
        iSaveContractID = SQL.ExecuteSingleValue( _
            "Select ContractID from ContractRoomType " & _
            "Where ContractRoomTypeID={0}", iTableID)

        If Me.Warnings.Count = 0 Then

            'delete child records from ContractAllocationRelease
            SQL.Execute("Delete from ContractAllocationRelease Where ContractRoomTypeID=" & iTableID.ToString)

            'delete child records from ContractCloseOut
            SQL.Execute("Delete from ContractCloseOut Where ContractRoomTypeID=" & iTableID.ToString)

            'delete child records from ContractFacility
            SQL.Execute("Delete from ContractFacility Where ContractRoomTypeID=" & iTableID.ToString)

            'delete child records from ContractFreeOffer
            SQL.Execute("Delete from ContractFreeOffer Where ContractRoomTypeID=" & iTableID.ToString)

            'delete child records from ContractGroupDiscount
            SQL.Execute("Delete from ContractGroupDiscount Where ContractRoomTypeID=" & iTableID.ToString)

            'delete child records from ContractMinMaxStay
            SQL.Execute("Delete from ContractMinMaxStay Where ContractRoomTypeID=" & iTableID.ToString)

            'delete child records from ContractOther
            'SQL.Execute("Delete from ContractOther Where ContractRoomTypeID=" & iTableID.ToString)

            'delete child records from ContractPackage
            SQL.Execute("Delete from ContractPackage Where ContractRoomTypeID=" & iTableID.ToString)

            'delete child records from ContractRateDate
            SQL.Execute("Delete from ContractRateDate Where ContractRoomTypeID=" & iTableID.ToString)

            'delete child records from ContractRateHeader
            SQL.Execute("Delete from ContractRateHeader Where ContractRoomTypeID=" & iTableID.ToString)

            'delete child records from ContractRegionTax
            SQL.Execute("Delete from ContractRegionTax Where ContractRoomTypeID=" & iTableID.ToString)

            'delete child records from ContractSpecialOffer
            SQL.Execute("Delete from ContractSpecialOffer Where ContractRoomTypeID=" & iTableID.ToString)

            'delete child records from ContractSupplement
            SQL.Execute("Delete from ContractSupplement Where ContractRoomTypeID=" & iTableID.ToString)

            'delete child records from ContractTax
            SQL.Execute("Delete from ContractTax Where ContractRoomTypeID=" & iTableID.ToString)

            'delete child records from ContractSpecialOfferExclusion
            SQL.Execute("Delete from ContractSpecialOfferExclusion Where ContractRoomTypeID=" & iTableID.ToString)

            'delete child records from ContractPackageExclusion
            SQL.Execute("Delete from ContractPackageExclusion Where ContractRoomTypeID=" & iTableID.ToString)


        End If

        Return Me.Warnings.Count = 0

    End Function

#End Region

#Region "Check Update"

    Public Overrides Function CheckUpdate() As Boolean

        iSaveContractID = CType(Me.Fields("ContractID").Value, Integer)

        MyBase.CheckUpdate()

        'occupancy min std max checks
        If CInt(GetField("MinOcc")) > CInt(GetField("MaxOcc")) Then
            Me.Warnings.Add("The Min Occupancy cannot be more than the Max Occupancy")
        End If

        If CInt(GetField("StdOcc")) > CInt(GetField("MaxOcc")) Then
            Me.Warnings.Add("The Std Occupancy cannot be more than the Max Occupancy")
        End If

        If CInt(GetField("StdOcc")) < CInt(GetField("MinOcc")) Then
            Me.Warnings.Add("The Std Occupancy cannot be less than the Min Occupancy")
        End If

        'adult min max checks
        If CInt(GetField("MinAdults")) > CInt(GetField("MinOcc")) Then
            Me.Warnings.Add("The Min Adults cannot be more than the Min Occupancy")
        End If

        If CInt(GetField("MaxAdults")) > CInt(GetField("MaxOcc")) Then
            Me.Warnings.Add("The Max Adults cannot be more than the Max Occupancy")
        End If

        If CInt(GetField("MinAdults")) > CInt(GetField("MaxAdults")) Then
            Me.Warnings.Add("The Min Adults cannot be more than the Max Adults")
        End If

        'children max check
        If CInt(GetField("MaxChildren")) > (CInt(GetField("MaxOcc")) - CInt(GetField("MinAdults"))) Then
            Me.Warnings.Add("The Max Children cannot be greater than the Max Occupancy less the Min Adults")
        End If

        'check that, if room allows children, the child ages are specfied
        If SafeInt(Me("MaxChildren")) > 0 AndAlso SafeInt(Me("ChildAgeTo")) = 0 Then
            Me.Warnings.Add("If a maximum number of Children is specified, the Child Age Range must be specified")
            Me.Fields("ChildAgeFrom").IsValid = False
            Me.Fields("ChildAgeTo").IsValid = False
        End If

        'child ages
        If CInt(GetField("ChildAgeFrom")) > CInt(GetField("ChildAgeTo")) Then
            Me.Warnings.Add("The Min Child Age cannot be greater than the Max Child Age")
        End If

        If CInt(GetField("YouthAgeFrom")) > CInt(GetField("YouthAgeTo")) Then
            Me.Warnings.Add("The Min Youth Age cannot be greater than the Max Youth Age")
        End If

        If CInt(GetField("YouthAgeFrom")) > 0 AndAlso _
            Not CInt(GetField("YouthAgeFrom")) > CInt(GetField("ChildAgeTo")) Then
            Me.Warnings.Add("The Min Youth Age must be greater than the Max Child Age")
        ElseIf SafeInt(Me("ChildAgeTo")) > 0 AndAlso SafeInt(Me("YouthAgeFrom")) > 0 _
            AndAlso SafeInt(Me("ChildAgeTo")) <> SafeInt(Me("YouthAgeFrom")) - 1 Then

            Me.Warnings.Add("The Youth Age must follow directly from the Child Age")
        End If

        Dim oContract As New Contract
        oContract.Go(CInt(GetField("ContractId")))

        If oContract("Type").ToString.ToLower = "allotted" Then

            If GetField("ContractBasis") = "" Then
                Me.Warnings.Add("The Contract Basis must be specified")
                Me.Fields("ContractBasis").IsValid = False
            End If
        End If

        Return Me.Warnings.Count < 1

    End Function

#End Region


    Private Sub UpdateSequence(ByVal iTableID As Integer) Handles MyBase.AfterUpdate, MyBase.AfterDelete

        'update the bed types
        SQL.Execute("exec ContractRoomTypeBedTypeAdd {0}, '{1}'", _
            iTableID, ArrayListToDelimitedString(Me.BedTypeIDs))

        'Re-Jig the Sequence
        SQL.Execute("exec UpdateContractRoomTypeSequence {0},{1}", _
            iSaveContractID, iTableID)

    End Sub

End Class
