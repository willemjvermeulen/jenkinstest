Imports Intuitive

Public Class ContractSection

    Public Shared Function GetContractSectionIDFromName(ByVal sContractSection As String) As Integer

        Dim iContractSectionID As Integer = _
            SQL.ExecuteSingleValue("Select ContractSectionID from ContractSection " & _
                "Where ContractSection={0}", SQL.GetSqlValue(sContractSection, SQL.SqlValueType.String))
        If iContractSectionID < 1 Then
            Throw New Exception("Could not establish SectionID for " & sContractSection)
        End If

        Return iContractSectionID


    End Function

#Region "section lookups"

    'get section name from section
    Public Shared Function GetSectionNameFromSection(ByVal sSection As String) As String

        Dim sSections As String = "Contract,ContractRoomType,Rates,AllocationRelease," & _
            "CloseOut,Tax,Supplement,Facility,MinMaxStay,SpecialOffers,Packages,PaymentSchedule,FreeOffer,GroupDiscount"
        Dim aSections() As String = sSections.Split(","c)
        Dim sSectionNames As String = "Contract,Room Types,Rates,Allocation & Release," & _
            "Close Outs,Taxes,Supplements,Facilities,Min & Max Stay,Special Offers,Packages,Payment Schedule,Free Offers,Group Discounts"
        Dim aSectionNames() As String = sSectionNames.Split(","c)

        Dim i As Integer
        For i = 0 To aSections.Length - 1
            If aSections(i).ToLower = sSection.ToLower Then Return aSectionNames(i)
        Next

        Throw New Exception("Section Lookup failed for " & sSection)

    End Function

    'get contract sectionid from section
    Public Shared Function GetContractSectionIDFromSection(ByVal sSection As String) As Integer

        Dim sSections As String = "Rates,AllocationRelease," & _
            "CloseOut,Tax,Supplement,Facility,MinMaxStay,SpecialOffers,Packages,PaymentSchedule," & _
            "FreeOffer,GroupDiscount,OtherInformation"
        Dim aSections() As String = sSections.Split(","c)

        Dim i As Integer
        For i = 0 To aSections.Length - 1
            If aSections(i).ToLower = sSection.ToLower Then Return i + 1
        Next

        If sSection.ToLower = "contract" Then Return 0

        Throw New Exception("Section Lookup failed for " & sSection)



    End Function

#End Region
End Class
