Imports Intuitive
Imports System.Text


Public Class autosort
    Inherits System.Web.UI.Control


    Public Table As String = ""
    Public SequenceField As String = "Sequence"
    Public DisplayExpression As String = ""
    Public Filter As String = ""
    Public WebService As String = ""



    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

        'quick check all the stuff is set
        If Me.Table = "" OrElse Me.SequenceField = "" OrElse Me.DisplayExpression = "" _
            OrElse Me.WebService = "" Then
            Throw New Exception("The Table, SequenceField and DisplayExpression and WebService must be specified")
        End If

        Dim sb As New StringBuilder
        Dim sSql As String = "select {1} as Display, {0}ID as ID from {0}"
        If Me.Filter <> "" Then
            sSql += " where " & Me.Filter
        End If
        sSql += " order by " & Me.SequenceField

        Dim dt As DataTable = SQL.GetDatatable(sSql, Me.Table, Me.DisplayExpression)

        sb.AppendFormat("<ul id=""{0}"">", Me.ID)

        For Each dr As DataRow In dt.Rows
            sb.AppendFormat("<li id=""{1}_{2}"">{0}</li>", dr("Display"), Me.ID, dr("ID"))
        Next

        sb.AppendFormat("</ul>\n\n")

        sb.Append("<script language=""javascript"">\n")
        sb.AppendFormat("Sortable.create('{0}', <<dropOnEmpty:true,containment:['{0}']," _
            & "constraint:false, onUpdate:function() <<AutoSortChangeSequence('{0}','{1}','{2}','{3}')>>>>);\n", _
                Me.ID, Me.Table, Me.SequenceField, Me.WebService)
        sb.Append("</script>\n")

        writer.Write(sb.ToString.Replace("\n", ControlChars.NewLine).Replace("<<", "{").Replace(">>", "}"))


    End Sub


End Class
