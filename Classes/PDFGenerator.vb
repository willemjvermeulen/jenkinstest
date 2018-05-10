Imports System.Configuration.ConfigurationSettings
Imports Intuitive
Imports Intuitive.Functions
Imports IntuitivePDF.PDFGenerator


Public Class PDFGenerator

    'Public Shared Sub QuickPDFPrint(ByVal sStoredProcedure As String, _
    '        ByVal sParameters As String, ByVal sTemplate As String, ByVal sDocumentName As String)

    '    'have a clear out of any PDFs older than today
    '    Intuitive.Functions.TidyFolder(HttpContext.Current.Request.MapPath("~\PDFs\"), _
    '                        "pdf", Date.Now.AddDays(-1))

    '    If Not sDocumentName.ToLower.EndsWith(".pdf") Then sDocumentName += ".pdf"

    '    'generate the document
    '    PDFGenerator.QuickPDFGenerator(sStoredProcedure, sParameters, sTemplate, sDocumentName, False)

    '    'print off the document
    '    Dim oPrint As New PDFPrinter.Printer
    '    Dim sPDF As String = String.Format(HttpContext.Current.Request.MapPath("~\PDFs\{0}"), sDocumentName)

    '    If Not sPDF.ToLower.EndsWith(".pdf") Then sPDF += ".pdf"

    '    With oPrint
    '        .PrinterName = AppSettings("WarehousePrinter")
    '        .Print(sPDF)
    '    End With

    '    'move PDF file to Printer Location
    '    'If Not File.Exists(AppSettings("WarehousePrinterFolder") & Path.GetFileName(sPDF)) AndAlso File.Exists(sPDF) Then
    '    'System.IO.File.Move(sPDF, AppSettings("WarehousePrinterFolder") & Path.GetFileName(sPDF))
    '    'End If
    'End Sub

    Public Shared Sub QuickPDFGeneratorWithXML(ByVal sXML As String, _
            ByVal sXSLTemplate As String, ByVal sDocument As String, _
            Optional ByVal bAutoView As Boolean = True, Optional ByVal oPage As PageBase = Nothing, _
            Optional ByRef sLog As String = "")

        QuickPDFGenerator("", "", sXSLTemplate, sDocument, bAutoView, oPage, True, sLog, sXML)
    End Sub

    Public Shared Sub QuickPDFGenerator(ByVal sStoredProcedure As String, _
            ByVal sParameters As String, ByVal sTemplate As String, ByVal sDocumentName As String, _
            Optional ByVal bAutoView As Boolean = True, Optional ByVal oPage As PageBase = Nothing, _
            Optional ByVal bRawXML As Boolean = False, Optional ByRef sLog As String = "", _
            Optional ByVal sExistingXML As String = "", Optional ByVal oXSLTParameters As XSLTParams = Nothing)

        'make sure we have a page if autoview set
        If bAutoView And oPage Is Nothing Then
            Throw New Exception("The Page must be specified if AutoView has been set")
        End If

        Dim sSourceSQL As String = String.Format("exec {0} {1}", sStoredProcedure, sParameters)
        Dim sXSLTemplate As String = String.Format(HttpContext.Current.Request.MapPath("~\Secure\XSL\{0}"), _
                sTemplate)
        Dim sPDF As String = String.Format(HttpContext.Current.Request.MapPath("~\PDFs\{0}"), _
                sDocumentName)

        Dim sImagePath As String = HttpContext.Current.Request.MapPath("~/images")

        'call generate
        GeneratePDF(sSourceSQL, sXSLTemplate, sPDF, sImagePath, bRawXML, sLog, , oXSLTParameters, sExistingXML)

        'if autoview then show DocumentViewer
        If bAutoView And Not oPage Is Nothing Then

            'bit (!) of a fiddle.  can't get hold of resolve url as this is not a control
            'therefore will pick up the images and remove the image bit
            Dim sBase As String = GetBaseURL()
            Dim sPDFURL As String = String.Format(sBase & "PDFs/{0}", sDocumentName)

            'register script to do popup
            Dim sDocumentViewerPage As String = sBase & "DocumentViewer.aspx"

            oPage.RegisterStartupScript("popup", _
                String.Format("<script>javascript:ShowPDF('{0}','{1}')</script>", _
                    sDocumentViewerPage, HttpContext.Current.Server.UrlEncode(sPDFURL)))
        End If
    End Sub

    Public Shared Sub GenerateReportPDF(ByVal sStoredProcedure As String, _
            ByVal sParameters As String, ByVal sTemplate As String, ByVal sDocumentName As String, _
            Optional ByVal bAutoView As Boolean = True, Optional ByVal oPage As PageBase = Nothing)

        'make sure we have a page if autoview set
        If bAutoView And oPage Is Nothing Then
            Throw New Exception("The Page must be specified if AutoView has been set")
        End If

        Dim sSourceSQL As String = String.Format("exec {0} {1}", sStoredProcedure, sParameters)
        Dim sXSLTemplate As String = String.Format(HttpContext.Current.Request.MapPath("~\Secure\Reports\XSL\{0}"), _
                sTemplate)
        Dim sPDF As String = String.Format(HttpContext.Current.Request.MapPath("~\PDFs\Reports\{0}"), _
                sDocumentName)

        Dim sImagePath As String = HttpContext.Current.Request.MapPath("~/images")

        'call generate
        sDocumentName = GeneratePDF(sSourceSQL, sXSLTemplate, sPDF, sImagePath)

        'if autoview then show DocumentViewer
        If bAutoView And Not oPage Is Nothing Then

            'bit (!) of a fiddle.  can't get hold of resolve url as this is not a control
            'therefore will pick up the images and remove the image bit
            Dim sBase As String = GetBaseURL()
            Dim sPDFURL As String = String.Format(sBase & "PDFs/Reports/{0}", sDocumentName)

            'register script to do popup
            Dim sDocumentViewerPage As String = sBase & "DocumentViewer.aspx"

            oPage.RegisterStartupScript("popup", _
                String.Format("<script>javascript:ShowPDF('{0}','{1}')</script>", _
                    sDocumentViewerPage, HttpContext.Current.Server.UrlEncode(sPDFURL)))
        End If
    End Sub

    Public Shared Sub GeneratePDFWithoutXML(ByVal sXSLFO As String, ByVal sDocumentName As String, _
            ByVal sFolder As String, Optional ByVal bAutoView As Boolean = False, _
            Optional ByVal oPage As PageBase = Nothing)

        'make sure we have a page if autoview set
        If bAutoView And oPage Is Nothing Then
            Throw New Exception("The Page must be specified if AutoView has been set")
        End If

        Dim sImagePath As String = HttpContext.Current.Request.MapPath("~/images")
        Dim sPDF As String = String.Format(HttpContext.Current.Request.MapPath("~\PDFs\{1}\{0}"), _
                sDocumentName, sFolder)

        'call generate
        BuildDocument(sXSLFO, sPDF, sImagePath)

        'if autoview then show DocumentViewer
        If bAutoView And Not oPage Is Nothing Then

            'bit (!) of a fiddle.  can't get hold of resolve url as this is not a control
            'therefore will pick up the images and remove the image bit
            Dim sBase As String = GetBaseURL()
            Dim sPDFURL As String = String.Format(sBase & "PDFs/Reports/{0}", sDocumentName)

            'register script to do popup
            Dim sDocumentViewerPage As String = sBase & "DocumentViewer.aspx"

            oPage.RegisterStartupScript("popup", _
                String.Format("<script>javascript:ShowPDF('{0}','{1}')</script>", _
                    sDocumentViewerPage, HttpContext.Current.Server.UrlEncode(sPDFURL)))
        End If

    End Sub

    Public Shared Sub ViewExistingPDF(ByVal sDocumentName As String, ByVal oPage As PageBase)

        'Dim sPDF As String = String.Format(HttpContext.Current.Request.MapPath("~\PDFs\{0}"), _
        '       sDocumentName)
        'Dim sImagePath As String = HttpContext.Current.Request.MapPath("~/images")

        Dim sBase As String = GetBaseURL()
        Dim sPDFURL As String = String.Format(sBase & "PDFs/{0}", sDocumentName)

        'register script to do popup
        Dim sDocumentViewerPage As String = sBase & "DocumentViewer.aspx"

        oPage.RegisterStartupScript("popup", _
            String.Format("<script>javascript:ShowPDF('{0}','{1}')</script>", _
                sDocumentViewerPage, HttpContext.Current.Server.UrlEncode(sPDFURL)))
    End Sub
End Class

