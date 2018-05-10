Imports Intuitive
Imports System.Text


Public Class ContractFacility
    Inherits TableBase

    Public Sub New()

        Me.Table = "ContractFacility"

        With Me.Fields
            .Add(New Field("ContractFacilityID", "Integer"))
            .Add(New Field("ContractID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("ContractRoomTypeID", "Integer"))
            .Add(New Field("FacilityID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("Notes", "Text"))
            .Add(New Field("CostInformation", "Boolean"))
        End With

        Me.Clear()
    End Sub


#Region "update facilities"

    Public Sub UpdateFacilities(ByVal iContractID As Integer, ByVal iContractRoomTypeID As Integer, _
        ByVal aFacilityID As ArrayList)
        Dim sbSql As New StringBuilder

        'if we have any facilities then 
        If aFacilityID.Count > 0 Then

            'csvify the ids
            Dim sFacilityID As String
            Dim sFacilityIDs As String = ""
            For Each sFacilityID In aFacilityID
                sFacilityIDs += sFacilityID & ","
            Next
            sFacilityIDs = Intuitive.Functions.Chop(sFacilityIDs)

            'execute the update stored procedure
            SQL.Execute("exec UpdateFacilities {0},{1},'{2}'", _
                iContractID, iContractRoomTypeID, sFacilityIDs)

        Else

            'delete any existing facilities
            SQL.Execute("Delete from ContractFacility where ContractID={0} and ContractRoomtypeID={1}", _
                iContractID, iContractRoomTypeID)
        End If

    End Sub

#End Region

End Class
