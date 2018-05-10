Imports Intuitive

Public Class OtherType
    Inherits TableBase

    Public Sub New()

        Me.Table = "OtherType"

        With Me.Fields
            .Add(New Field("OtherTypeID", "Integer"))
            .Add(New Field("OtherType", "String", 50, ValidationType.NotEmptyNotDupe))
            .Add(New Field("DataType", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("MandatoryFlag", "Boolean"))
            .Add(New Field("DefaultText", "Text"))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in ContractOther
        If Not SQL.FKCheck("ContractOther", "OtherTypeID", iTableID) Then
            Me.Warnings.Add("Sorry, the Information Type cannot be deleted as it is in use")
        End If

        Return Me.Warnings.Count = 0
    End Function

#Region "GetDataType"

    Public Shared Function GetDataType(ByVal iOtherTypeID As Integer) As String

        Dim oOtherType As New OtherType
        oOtherType.Go(iOtherTypeID)
        Return oOtherType("DataType")

    End Function

#End Region

#Region "GetOtherType"

    Public Shared Function GetOtherType(ByVal iOtherTypeID As Integer) As String
        Dim oOtherType As New OtherType
        oOtherType.Go(iOtherTypeID)
        Return oOtherType("OtherType").ToString
    End Function
#End Region

End Class

