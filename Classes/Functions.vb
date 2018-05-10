Imports Intuitive
Public Class Functions


    'datesoverlap
    Public Shared Function DatesOverlap(ByVal dStartDate1 As Date, _
        ByVal dEndDate1 As Date, ByVal dStartDate2 As Date, ByVal dEndDate2 As Date) As Boolean

        Return (dStartDate2 >= dStartDate1 AndAlso dStartDate2 <= dEndDate1) _
            OrElse (dEndDate2 >= dStartDate1 AndAlso dEndDate2 <= dEndDate1) _
            OrElse (dStartDate2 <= dStartDate1 AndAlso dEndDate2 >= dEndDate1)
    End Function


    Public Shared Function GetReferenceFromBase(ByVal sBase As String, ByVal sInput As String, _
    ByVal iSuffixLength As Integer) As String

        If sInput.Length > 0 AndAlso iSuffixLength - sInput.Length > 0 Then
            Return sBase & sInput.Trim.PadLeft(iSuffixLength, "0"c)
        ElseIf sInput.Length > 0 AndAlso iSuffixLength - sInput.Length = 0 Then
            Return sBase & sInput
        Else
            Return sInput
        End If

    End Function

    Public Shared Sub LogEntry(ByVal sLogEntry As String)

        SQL.Execute("insert into LogEntry (LogEntry) values ({0})", SQL.GetSqlValue(sLogEntry))
    End Sub

    Public Shared Function GetBaseURL(Optional ByVal Secure As Boolean = False) As String

        Dim oRequest As HttpRequest = HttpContext.Current.Request

        Dim sFullURL As String = oRequest.Url.AbsoluteUri.ToLower
        Dim sApplicationName As String = oRequest.ApplicationPath.ToLower
        Dim sBase As String

        If sApplicationName <> "/" Then
            sBase = sFullURL.Substring(0, sFullURL.IndexOf(sApplicationName) + sApplicationName.Length + 1)
        Else
            sBase = sFullURL.Substring(0, sFullURL.IndexOf("/", 9)) & "/"
        End If

        If Not Secure Then
            Return sBase.Replace("https:", "http:")
        Else
            Return sBase.Replace("http:", "https:")
        End If

    End Function


End Class
