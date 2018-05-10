Imports Intuitive
Imports Intuitive.Functions
Public Class TransferContractDepartureArrival

#Region "Validate/Update Locations"
    Public Shared Function ValidateLocations(ByVal aDepartureIDs As ArrayList, ByVal aArrivalIDs As ArrayList) _
            As FunctionReturn

        Dim oReturn As New FunctionReturn

        'check we have at least one of each
        If aDepartureIDs.Count = 0 Then
            oReturn.AddWarning("You must select at least one Departure Location")
        End If

        If aArrivalIDs.Count = 0 Then
            oReturn.AddWarning("You must select at least one Arrival Location")
        End If

        Return oReturn
    End Function

    Public Shared Sub UpdateLocations(ByVal iTransferContractID As Integer, ByVal aDepartureIDs As ArrayList, _
            ByVal aArrivalIDs As ArrayList)

        'get the parent types and ids into new arraylists
        Dim aDepartureAirportIDs As New ArrayList
        Dim aDepartureResortIDs As New ArrayList
        Dim aArrivalAirportIDs As New ArrayList
        Dim aArrivalResortIDs As New ArrayList
        Dim oLocation As Location

        For Each iLocationID As Integer In aDepartureIDs
            oLocation = TransferContractDepartureArrival.GetLocationLevelAndID(iLocationID)

            Select Case oLocation.LocationType
                Case "Airport"
                    aDepartureAirportIDs.Add(oLocation.LocationID)
                Case "Resort"
                    aDepartureResortIDs.Add(oLocation.LocationID)
            End Select
        Next

        For Each iLocationID As Integer In aArrivalIDs
            oLocation = TransferContractDepartureArrival.GetLocationLevelAndID(iLocationID)

            Select Case oLocation.LocationType
                Case "Airport"
                    aArrivalAirportIDs.Add(oLocation.LocationID)
                Case "Resort"
                    aArrivalResortIDs.Add(oLocation.LocationID)
            End Select
        Next

        Dim oSQLTransaction As New SQLTransaction
        With oSQLTransaction

            'do the updates
            .Add("exec UpdateTransferContractDepartureArrival {0},'{1}','{2}','{3}','{4}'", iTransferContractID, _
                        ArrayListToDelimitedString(aDepartureAirportIDs), _
                        ArrayListToDelimitedString(aDepartureResortIDs), _
                        ArrayListToDelimitedString(aArrivalAirportIDs), _
                        ArrayListToDelimitedString(aArrivalResortIDs))

            'do a cleanup and insert empty rates
            .Add("exec CleanupTransferContractRates")
            .Add("exec TransferContractRateInsertDefault '',0,{0}", iTransferContractID)

            .Execute()

        End With

        TransferContract.SetLastModifiedData(iTransferContractID)
    End Sub
#End Region

#Region "Helper Functions and Structure"
    Private Shared Function GetLocationLevelAndID(ByVal iLocationID As Integer) As Location

        Dim oLocation As New Location
        Select Case iLocationID
            Case Is > CType(10 ^ 6, Integer)
                oLocation.LocationType = "Resort"
            Case Else
                oLocation.LocationType = "Airport"
        End Select

        oLocation.LocationID = CType(iLocationID Mod 10 ^ 6, Integer)
        Return oLocation
    End Function

    Private Structure Location
        Public LocationType As String
        Public LocationID As Integer
    End Structure
#End Region

End Class
