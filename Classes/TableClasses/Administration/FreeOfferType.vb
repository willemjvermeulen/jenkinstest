Imports Intuitive
Imports Intuitive.Functions
Public Class FreeOfferType
    Inherits TableBase
    Private oFreeOfferTypeGroup As New FreeOfferTypeGroup
    Public AddNewGroup As String = ""

    Public Sub New()

        Me.Table = "FreeOfferType"

        With Me.Fields
            .Add(New Field("FreeOfferTypeID", "Integer"))
            .Add(New Field("FreeOfferTypeGroupID", "Integer"))
            .Add(New Field("FreeOfferType", "String", 50, ValidationType.NotEmptyNotDupe, "FreeOfferTypeGroupID"))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in ContractFreeOffer
        If Not SQL.FKCheck("ContractFreeOffer", "FreeOfferTypeID", iTableID) Then
            Me.Warnings.Add("The Free Offer Type cannot be deleted as it is associated with one or more contracts")
        End If

        Return Me.Warnings.Count = 0
    End Function

    Public Overrides Function CheckUpdate() As Boolean

        'check we have an Id or a new group
        If safeint(Me("FreeOfferTypeGroupID")) = 0 AndAlso Me.AddNewGroup = "" Then
            Me.Warnings.Add("A Free Offer Type Group must be specified")
            Me.Fields("FreeOfferTypeGroupID").IsValid = False
        End If

        'do base check
        MyBase.CheckUpdate()

        'if all's ok and we're adding a new one then run update code for group
        If Me.Warnings.Count = 0 AndAlso Me.AddNewGroup <> "" Then
            oFreeOfferTypeGroup.SetField("FreeOfferTypeGroup", Me.AddNewGroup)

            If oFreeOfferTypeGroup.Update Then
                Me.SetField("FreeOfferTypeGroupID", oFreeOfferTypeGroup.TableID)
            Else
                Me.Warnings = oFreeOfferTypeGroup.Warnings
                Me.Fields("FreeOfferTypeGroupID").IsValid = False
            End If
        End If

        Return Me.Warnings.Count = 0
    End Function
End Class
