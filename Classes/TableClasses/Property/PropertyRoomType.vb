Imports Intuitive
Imports Intuitive.Functions

<Serializable()> _
Public Class PropertyRoomType
    Inherits TableBase

    Private iSavePropertyID As Integer
    Public BedTypeIDs As New ArrayList
    Public CanEdit As Boolean = True

#Region "New"

    Public Sub New()

        Me.Table = "PropertyRoomType"

        With Me.Fields
            .Add(New Field("PropertyRoomTypeID", "Integer"))
            .Add(New Field("PropertyID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("RoomTypeID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("RoomViewID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("Reference", "String", 20))
            .Add(New Field("MealBasisID", "Integer"))
            .Add(New Field("StandardAllocation", "Integer"))
            .Add(New Field("MinOcc", "Integer", ValidationType.NotEmpty))
            .Add(New Field("StdOcc", "Integer", ValidationType.NotEmpty))
            .Add(New Field("MaxOcc", "Integer", ValidationType.NotEmpty))
            .Add(New Field("MinAdults", "Integer"))
            .Add(New Field("MaxAdults", "Integer"))
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
            .Add(New Field("Sequence", "Integer"))
        End With

        Me.Clear()
    End Sub

#End Region

#Region "Check Delete"

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean
        'check for records in ContractRoomType
        If Not SQL.FKCheck("ContractRoomType", "PropertyRoomTypeID", iTableID) Then
            Me.Warnings.Add("The Room Type cannot be deleted as it is used in one or more contracts")
        End If

        Return Me.Warnings.Count = 0
    End Function

#End Region

#Region "Check Update"

    Public Overrides Function CheckUpdate() As Boolean
        iSavePropertyID = CType(Me.Fields("PropertyID").Value, Integer)


        MyBase.CheckUpdate()

        'occupancy min std max checks
        If CInt(GetField("MinOcc")) > CInt(GetField("MaxOcc")) Then
            Warnings.Add("The Min Occupancy cannot be more than the Max Occupancy")
        End If

        If CInt(GetField("StdOcc")) > CInt(GetField("MaxOcc")) Then
            Warnings.Add("The Std Occupancy cannot be more than the Max Occupancy")
        End If

        If CInt(GetField("StdOcc")) < CInt(GetField("MinOcc")) Then
            Warnings.Add("The Std Occupancy cannot be less than the Min Occupancy")
        End If

        'adult min max checks
        If CInt(GetField("MinAdults")) > CInt(GetField("MinOcc")) Then
            Warnings.Add("The Min Adults cannot be more than the Min Occupancy")
        End If

        If CInt(GetField("MaxAdults")) > CInt(GetField("MaxOcc")) Then
            Warnings.Add("The Max Adults cannot be more than the Max Occupancy")
        End If

        If CInt(GetField("MinAdults")) > CInt(GetField("MaxAdults")) Then
            Warnings.Add("The Min Adults cannot be more than the Max Adults")
        End If

        'children max check
        If CInt(GetField("MaxChildren")) > (CInt(GetField("MaxOcc")) - CInt(GetField("MinAdults"))) Then
            Warnings.Add("The Max Children cannot be greater than the Max Occupancy less the Min Adults")
        End If

        'child ages
        If CInt(GetField("ChildAgeFrom")) > CInt(GetField("ChildAgeTo")) Then
            Warnings.Add("The Min Child Age cannot be greater than the Max Child Age")
        End If

        If CInt(GetField("YouthAgeFrom")) > CInt(GetField("YouthAgeTo")) Then
            Warnings.Add("The Min Youth Age cannot be greater than the Max Youth Age")
        End If

        If CInt(GetField("YouthAgeFrom")) > 0 AndAlso _
            Not CInt(GetField("YouthAgeFrom")) > CInt(GetField("ChildAgeTo")) Then
            Warnings.Add("The Min Youth Age must be greater than the Max Child Age")
        ElseIf SafeInt(Me("ChildAgeTo")) > 0 AndAlso SafeInt(Me("YouthAgeFrom")) > 0 _
            AndAlso SafeInt(Me("ChildAgeTo")) <> SafeInt(Me("YouthAgeFrom")) - 1 Then

            Me.Warnings.Add("The Youth Age must follow directly from the Child Age")
        End If

        Return Me.Warnings.Count < 1
    End Function

#End Region

#Region "After Add"

    Private Sub PropertyRoomType_AfterAdd(ByVal iTableID As Integer) Handles MyBase.AfterAdd

        Dim oSQL As New SQLTransaction
        oSQL.Add("exec GeneratePropertyRoomTypeReference {0}", iTableID)

        'sync settings
        If Config.Installation = "Server" Then
            oSQL.Add("update PropertyRoomType set SyncGUID=newid() where PropertyRoomTypeID={0}", iTableID)
        ElseIf Config.Installation = "Client" Then
            oSQL.Add("update PropertyRoomType set SyncGUID='New' where PropertyRoomTypeID={0}", iTableID)
        End If

        oSQL.Execute()
    End Sub

#End Region

#Region "After Update"

    Private Sub PropertyRoomType_AfterUpdate(ByVal iTableID As Integer) Handles MyBase.AfterUpdate

        'update the bed types
        SQL.Execute("exec PropertyRoomTypeBedTypeAdd {0}, '{1}'", _
            iTableID, ArrayListToDelimitedString(Me.BedTypeIDs))

        'Re-Jig the Sequence
        SQL.Execute("exec UpdatePropertyRoomTypeSequence {0},{1}", _
            iSavePropertyID, iTableID)

        'sync settings
        PropertyTable.SetSyncRequired(SafeInt(Me("PropertyID")))

    End Sub

#End Region

#Region "After Delete - update Sequence"

    Private Sub UpdateSequence(ByVal iTableID As Integer) Handles MyBase.AfterDelete

        'Re-Jig the Sequence
        SQL.Execute("exec UpdatePropertyRoomTypeSequence {0},{1}", _
            iSavePropertyID, iTableID)

    End Sub

#End Region

#Region "After Go"
    Private Sub PropertyRoomType_AfterGo(ByVal iTableID As Integer) Handles MyBase.AfterGo

        'check if we can edit
        Me.CanEdit = SafeBoolean(SQL.GetValue("exec PropertyRoomTypeCheckEdit {0}", iTableID))
    End Sub
#End Region

End Class
