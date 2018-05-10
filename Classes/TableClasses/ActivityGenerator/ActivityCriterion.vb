Imports Intuitive
Imports Intuitive.Functions

Public Class ActivityCriterion
    Inherits TableBase

    Public ActivityCriterionIn As ArrayList

    Public Sub New()

        Me.Table = "ActivityCriterion"
        Me.IgnoreInvisibleControlsOnUnbind = True

        With Me.Fields
            .Add(New Field("ActivityCriterionID", "Integer"))
            .Add(New Field("ActivityID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("ActivityCriterionDefID", "Integer"))
            .Add(New Field("CriterionType", "String", 15))
            .Add(New Field("AndOr", "String", 15))
            .Add(New Field("Invert", "String", 15))
            .Add(New Field("Criterion", "String", 30))
            .Add(New Field("Operator", "String", 30))
            .Add(New Field("Value", "String", 30))
            .Add(New Field("RangeFrom", "String", 10))
            .Add(New Field("RangeTo", "String", 10))
            .Add(New Field("Sequence", "Integer"))
        End With

        Me.Clear()
    End Sub


#Region "before add - set the sequence"

    Private Sub ActivityCriterion_BeforeAdd() Handles MyBase.BeforeAdd
        Dim sSQL As String

        sSQL = String.Format("Select Count(*)+1 " & _
        "from ActivityCriterion " & _
        "Where ActivityID={0}", Me("ActivityID"))

        Me.SetField("Sequence", SQL.ExecuteSingleValue(sSQL))

    End Sub


#End Region

#Region "after update - sort out the activitycriterionin table"

    Private Sub ActivityCriterion_AfterUpdate(ByVal iTableID As Integer) Handles MyBase.AfterUpdate

        Dim oSqlTrans As New SQLTransaction
        oSqlTrans.Add("Delete from ActivityCriterionIn where ActivityCriterionID={0}", _
            Me.TableID)

        For Each iInID As Integer In Me.ActivityCriterionIn
            oSqlTrans.Add("Insert into ActivityCriterionIn (ActivityCriterionID, InID) " & _
                "values ({0},{1})", Me.TableID, iInID)
        Next

        oSqlTrans.Execute()

    End Sub

#End Region

    Public Overrides Function CheckUpdate() As Boolean
        MyBase.CheckUpdate()

        'criterion selected
        If SafeInt(Me("ActivityCriterionDefID")) = 0 Then
            Me.Warnings.Add("A Criterion must be selected")
            Me.Fields("ActivityCriterionDefID").IsValid = False

        ElseIf Me("CriterionType") = "" OrElse Me("Criterion") = "" Then
            Throw New Exception("The CriterionType or Criterion should be specified")
        Else

            'and or
            If SafeInt(Me("Sequence")) > 1 And Me("AndOr") = "" Then
                Me.Warnings.Add("The And/Or field must be specified")
                Me.Fields("AndOr").IsValid = False
            End If

            'should have an operator if CriterionType is Text, Numeric, Date
            If InList(Me("CriterionType"), "Text", "Numeric", "Date") AndAlso Me("Operator") = "" Then
                Me.Warnings.Add("An Operator must be specified")
                Me.Fields("Operator").IsValid = False
            End If

            'if operator is equals, less than, more than then shuold have a value
            If InList(Me("Operator"), "Equals", "Less Than", "More Than") AndAlso Me("Value") = "" Then
                Me.Warnings.Add("A Value must be specified")
                Me.Fields("Value").IsValid = False
            End If

            'value and CriterionType is date, make sure a date has been input
            If InList(Me("Operator"), "Equals", "Less Than", "More Than") AndAlso _
                Me("CriterionType") = "Date" And Not DateFunctions.IsDisplayDate(Me("Value")) Then
                Me.Warnings.Add("A valid Date must be input for the Value")
                Me.Fields("Value").IsValid = False
            End If

            'if range then a range from and to must be set
            If Me("Operator") = "Range" AndAlso _
                (Me("RangeFrom") = "" OrElse Me("RangeTo") = "") Then
                Me.Warnings.Add("A Range From and Range To must have values")
                Me.Fields("RangeFrom").IsValid = False
                Me.Fields("RangeTo").IsValid = False
            End If

            'if a range and dates, make sure they are then
            If Me("Operator") = "Range" AndAlso Me("CriterionType") = "Date" AndAlso _
                Not (DateFunctions.IsDisplayDate(Me("RangeFrom")) AndAlso _
                    DateFunctions.IsDisplayDate(Me("RangeTo"))) Then
                Me.Warnings.Add("The Range values should be valid dates")
                Me.Fields("RangeFrom").IsValid = False
                Me.Fields("RangeTo").IsValid = False
            End If

            'if list, make sure we have some of the good stuff
            If Me("CriterionType") = "List" And Me.ActivityCriterionIn.Count = 0 Then
                Me.Warnings.Add("At least one item must be selected")
            End If
        End If


        Return Me.Warnings.Count = 0
    End Function


End Class
