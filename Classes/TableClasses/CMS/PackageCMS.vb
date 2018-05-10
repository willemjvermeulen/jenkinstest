Imports Intuitive
Imports Intuitive.Functions
Imports Intuitive.Images
Imports System.io
Public Class PackageCMS

    Public MainImageID As Integer
    Public ImageIDs As New ArrayList
    Public ParentType As String
    Public ParentID As Integer

#Region "Image Handling"
    Public Function UploadImage(ByVal oImage As HttpPostedFile, ByVal bSetAsMain As Boolean) As Boolean

        Dim iPackageCMSImageID As Integer

        Try

            'first check we have a file
            If oImage.ContentLength > 0 Then

                'insert a new record into the property cms image bit
                iPackageCMSImageID = SQL.Execute("insert into PackageCMSImage (ParentType, " & _
                    "ParentID, Main) values ({0},{1},{2})", _
                    SQL.GetSqlValue(Me.ParentType), Me.ParentID, _
                    SQL.GetSqlValue(bSetAsMain, SQL.SqlValueType.Boolean))

                'set all the file bases and that shite
                Dim sExtension As String = Right(oImage.FileName, 3).ToLower
                Dim sFileBase As String = "{3}{0}_{1}.{2}"
                Dim sOriginalFile As String = String.Format(sFileBase, "CMSDeletePackage", _
                    Me.ParentType & Me.ParentID & "_" & Now.ToString("ddMMmmhhss"), sExtension, _
                    Config.ImageContentFolder)
                Dim sDestinationFile As String

                oImage.SaveAs(sOriginalFile)

                'do big image
                sDestinationFile = String.Format(sFileBase, "CMSPackageImage", iPackageCMSImageID, "jpg", _
                    Config.ImageContentFolder)
                If File.Exists(sDestinationFile) Then File.Delete(sDestinationFile)
                ImageFunctions.ImageResize(sOriginalFile, sDestinationFile, 210, 140)

                'now do the thumb
                sDestinationFile = String.Format(sFileBase, "CMSPackageImageThumb", iPackageCMSImageID, "jpg", _
                    Config.ImageContentFolder)
                If File.Exists(sDestinationFile) Then File.Delete(sDestinationFile)
                ImageFunctions.ImageResize(sOriginalFile, sDestinationFile, 70, 70)

            End If

            Return True
        Catch ex As Exception

            'delete the record we just added
            SQL.Execute("delete from PackageCMSImage where PackageCMSImageID={0}", iPackageCMSImageID)

            Return False
        End Try

        'clear folder
        PropertyCMS.CleanupImageFolder()


    End Function


    Public Sub DeleteImage(ByVal iPackageCMSImageID As Integer)

        Try
            'get the filepath
            Dim sFileBase As String = "{2}{0}_{1}.jpg"

            'do main image
            Dim sFile As String = String.Format(sFileBase, "CMSPackageImage", iPackageCMSImageID, _
                    Config.ImageContentFolder)
            If File.Exists(sFile) Then File.Delete(sFile)

            'do the thumb
            sFile = String.Format(sFileBase, "CMSPackageImageThumb", iPackageCMSImageID, _
                    Config.ImageContentFolder)
            If File.Exists(sFile) Then File.Delete(sFile)

        Finally

            SQL.Execute("delete from PackageCMSImage where PackageCMSImageID={0}", iPackageCMSImageID)
        End Try
    End Sub

    Public Sub SetAsMain(ByVal iPackageCMSImageID As Integer)

        Dim oSql As New SQLTransaction
        With oSql
            .Add("update PackageCMSImage set Main=0 where ParentType={0} and ParentID={1}", _
                SQL.GetSqlValue(Me.ParentType), Me.ParentID)
            .Add("update PackageCMSImage set Main=1 where PackageCMSImageID={0}", iPackageCMSImageID)
            .Execute()
        End With
    End Sub
#End Region

#Region "Get Images"
    Public Sub GetImages()

        Dim dt As DataTable = SQL.GetDatatable("select PackageCMSImageID, Main from PackageCMSImage " & _
            "where ParentType={0} and ParentID={1} order by main desc, PackageCMSImageID", _
            SQL.GetSqlValue(Me.ParentType), Me.ParentID)
        Me.ClearImages()
        For Each dr As DataRow In dt.Rows

            If SafeBoolean(dr("Main")) Then
                Me.MainImageID = SafeInt(dr("PackageCMSImageID"))
            Else
                Me.ImageIDs.Add(SafeInt(dr("PackageCMSImageID")))
            End If

        Next
    End Sub
#End Region

#Region "Clear Images"
    Public Sub ClearImages()

        Me.MainImageID = 0
        Me.ImageIDs.Clear()
        Me.ParentType = ""
        Me.ParentID = 0
    End Sub
#End Region

End Class
