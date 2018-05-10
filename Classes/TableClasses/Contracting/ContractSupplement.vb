Imports Intuitive
Imports Intuitive.Functions

Public Class ContractSupplement
    Inherits TableBase

    Public Sub New()

        Me.Table = "ContractSupplement"

        With Me.Fields
            .Add(New Field("ContractSupplementID", "Integer"))
            .Add(New Field("ContractID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("ContractRoomTypeID", "Integer"))
            .Add(New Field("SupplementType", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("BaseRoomTypeID", "Integer"))
            .Add(New Field("MealBasisID", "Integer"))
            .Add(New Field("SupplementName", "String", 50))
            .Add(New Field("PaxType", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("ChildValueType", "String", 20))
            .Add(New Field("YouthValueType", "String", 20))
            .Add(New Field("ChildAgeFrom", "Integer"))
            .Add(New Field("ChildAgeTo", "Integer"))
            .Add(New Field("YouthAgeFrom", "Integer"))
            .Add(New Field("YouthAgeTo", "Integer"))
            .Add(New Field("RateCalculation", "String", 20, ValidationType.NotEmpty))
            .Add(New Field("DurationCalculation", "String", 20, ValidationType.NotEmpty))
            .Add(New Field("IsMarginSupplement", "Integer"))
            .Add(New Field("IsPercentageSupplement", "Integer"))
        End With

        Me.Clear()
    End Sub

#Region "Check Update"

    Public Overrides Function CheckUpdate() As Boolean

        'Call default CheckUpdate()
        MyBase.CheckUpdate()

        Dim sSupplementType As String = Me("SupplementType")

        'Check that the Base Room Type field is not blank
        If sSupplementType = "Room Type" Then
            If Not Me("BaseRoomTypeID") = "" Then
                'build the supplement name
                Me.BuildSupplementName(sSupplementType, Me("ContractRoomTypeID"))
            Else
                Me.Warnings.Add("The Base Room Type must be specified")
                Me.Fields("BaseRoomTypeID").IsValid = False
            End If
        End If

        'Check that the Meal Basis field is not blank
        If sSupplementType = "Meal Basis Upgrade" Then
            If Not Me("MealBasisID") = "" Then
                'build the supplement name
                Me.BuildSupplementName(sSupplementType, Me("MealBasisID"))
            Else
                Me.Warnings.Add("The Meal Basis must be specified")
                Me.Fields("MealBasisID").IsValid = False
            End If
        End If

        'Check that the Supplement Name field is not blank
        If sSupplementType = "Mandatory" OrElse sSupplementType = "Optional" Then
            Fields("IsPercentageSupplement").Value = If(Me.GetField("IsPercentageSupplement") = "true", 1, 0)
            If Me("SupplementName") = "" Then
                If SafeInt(Me("IsMarginSupplement")) > 0 Then
                    Me.Warnings.Add("The Margin Name must be specified")
                Else
                    Me.Warnings.Add("The Supplement Name must be specified")
                End If
                Me.Fields("SupplementName").IsValid = False
            End If
        End If

        'Check that the Child and Youth Value Type fields are not blank
        If InList(Me("PaxType"), "Adult/Child", "Adult/Child/Youth", _
                "Adult/1st Child/2nd Child", "Adult/1st Child/2nd Child/Youth") Then
            If Me("ChildValueType") = "" Then
                Me.Warnings.Add("The Child Value Type must be specified")
                Me.Fields("ChildValueType").IsValid = False
            End If
        End If

        If InList(Me("PaxType"), "Adult/Child/Youth", "Adult/1st Child/2nd Child/Youth") Then
            If Me("YouthValueType") = "" Then
                Me.Warnings.Add("The Youth Value Type must be specified")
                Me.Fields("YouthValueType").IsValid = False
            End If
        End If

        'Check that the Child and Youth ages are not blank
        If InList(Me("PaxType"), "Adult/Child", "Adult/Child/Youth", _
                "Adult/1st Child/2nd Child", "Adult/1st Child/2nd Child/Youth") Then
            If Me("ChildAgeFrom") = "" OrElse Me("ChildAgeTo") = "" Then
                Me.Warnings.Add("The Child Ages must be specified")
                Me.Fields("ChildAgeFrom").IsValid = False
                Me.Fields("ChildAgeTo").IsValid = False
            End If
        End If

        If InList(Me("PaxType"), "Adult/Child/Youth", "Adult/1st Child/2nd Child/Youth") Then
            If Me("YouthAgeFrom") = "" OrElse Me("YouthAgeTo") = "" Then
                Me.Warnings.Add("The Youth Ages must be specified")
                Me.Fields("YouthAgeFrom").IsValid = False
                Me.Fields("YouthAgeTo").IsValid = False
            End If
        End If

        Return Me.Warnings.Count = 0

    End Function

#End Region

#Region "BuildSupplementName"

    'build the supplement name
    Private Sub BuildSupplementName(ByVal sSupplementType As String, ByVal sID As String)

        Dim sSupplementName As String

        Select Case sSupplementType
            Case "Room Type"
                sSupplementName = SQL.GetValue("exec GetContractRoomTypeDescription {0}", sID)
            Case "Meal Basis Upgrade"
                sSupplementName = SQL.GetValue("select MealBasis from MealBasis where MealBasisID ={0}", sID)
            Case Else
                Throw New Exception("Could not find Supplement Type " & sSupplementType)
        End Select

        'set the field
        sSupplementName = sSupplementName + " Supplement"
        Me.SetField("SupplementName", sSupplementName)

    End Sub

#End Region

#Region "After Delete"
    Private Sub ContractSupplement_AfterDelete(ByVal iTableID As Integer) Handles MyBase.AfterDelete

        'delete the supplement child and cache entries
        Dim oSQL As New SQLTransaction
        With oSQL

            'delete child records from ContractSupplementRegionTax
            .Add("Delete from ContractSupplementRegionTax Where ContractSupplementID={0}", iTableID)

            'delete child records from ContractSupplementTax
            .Add("Delete from ContractSupplementTax Where ContractSupplementID={0}", iTableID)

            'delete child records from ContractSupplementValue
            .Add("Delete from ContractSupplementValue Where ContractSupplementID={0}", iTableID)

            'delete the cache entries
            .Add("delete from SupplementCache where ContractSupplementID={0}", iTableID)

            .Execute()
        End With
    End Sub
#End Region

End Class
