Imports Intuitive
Imports Intuitive.Functions
Imports Intuitive.Images
Imports System.io

Public Class PropertyCMS
    Inherits TableBase
    Public MainImageID As Integer
    Public ImageIDs As New ArrayList

    Public Sub New()

        Me.Table = "PropertyCMS"

        With Me.Fields
            .Add(New Field("PropertyCMSID", "Integer"))
            .Add(New Field("PropertyID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("Strapline", "String", 200, ValidationType.NotEmpty))
            .Add(New Field("Description", "Text"))
        End With

        Me.Clear()
    End Sub

#Region "Image Handling"
    Public Function UploadImage(ByVal oImage As HttpPostedFile, ByVal bSetAsMain As Boolean) As Boolean

        Dim iPropertyCMSImageID As Integer

        Try

            'first check we have a file
            If oImage.ContentLength > 0 Then

                'insert a new record into the property cms image bit
                iPropertyCMSImageID = SQL.ExecuteSingleValue("exec AddNewPropertyImage {0},{1}", _
                    Me.TableID, SQL.GetSqlValue(bSetAsMain, SQL.SqlValueType.Boolean))

                'set all the file bases and that shite
                Dim sExtension As String = Right(oImage.FileName, 3).ToLower
                Dim sFileBase As String = "{3}{0}_{1}.{2}"
                Dim sOriginalFile As String = String.Format(sFileBase, "CMSDelete", _
                    Me.TableID & "_" & Now.ToString("ddMMmmhhss"), sExtension, Config.ImageContentFolder)
                Dim sDestinationFile As String

                oImage.SaveAs(sOriginalFile)

                'do big image
                sDestinationFile = String.Format(sFileBase, "CMSImage", iPropertyCMSImageID, "jpg", _
                    Config.ImageContentFolder)
                If File.Exists(sDestinationFile) Then File.Delete(sDestinationFile)
                ImageFunctions.ImageResize(sOriginalFile, sDestinationFile, 210, 140)

                'now do the thumb
                sDestinationFile = String.Format(sFileBase, "CMSImageThumb", iPropertyCMSImageID, "jpg", _
                    Config.ImageContentFolder)
                If File.Exists(sDestinationFile) Then File.Delete(sDestinationFile)
                ImageFunctions.ImageResize(sOriginalFile, sDestinationFile, 70, 70)

            End If

            Return True
        Catch ex As Exception

            'delete the record we just added
            SQL.Execute("delete from PropertyCMSImage where PropertyCMSImageID={0}", iPropertyCMSImageID)

            Return False
        End Try


    End Function


    Public Sub DeleteImage(ByVal iPropertyCMSImageID As Integer)

        Try
            'get the filepath
            Dim sFileBase As String = "{2}{0}_{1}.jpg"

            'do main image
            Dim sFile As String = String.Format(sFileBase, "CMSImage", iPropertyCMSImageID, _
                    Config.ImageContentFolder)
            If File.Exists(sFile) Then File.Delete(sFile)

            'do the thumb
            sFile = String.Format(sFileBase, "CMSImageThumb", iPropertyCMSImageID, _
                    Config.ImageContentFolder)
            If File.Exists(sFile) Then File.Delete(sFile)

        Finally

            SQL.Execute("delete from PropertyCMSImage where PropertyCMSImageID={0}", iPropertyCMSImageID)
        End Try
    End Sub

    Public Sub SetAsMain(ByVal iPropertyCMSImageID As Integer)

        Dim oSql As New SQLTransaction
        With oSql
            .Add("update PropertyCMSImage set Main=0 where PropertyCMSID={0}", Me.TableID)
            .Add("update PropertyCMSImage set Main=1 where PropertyCMSImageID={0}", iPropertyCMSImageID)
            .Execute()
        End With
     End Sub
#End Region

#Region "Get Images"
    Public Sub GetImages()

        Dim dt As DataTable = SQL.GetDatatable("select PropertyCMSImageID, Main from PropertyCMSImage " & _
            "where PropertyCMSID={0} order by main desc, PropertyCMSImageID", Me.TableID)
        Me.ClearImages()
        For Each dr As DataRow In dt.Rows

            If SafeBoolean(dr("Main")) Then
                Me.MainImageID = SafeInt(dr("PropertyCMSImageID"))
            Else
                Me.ImageIDs.Add(SafeInt(dr("PropertyCMSImageID")))
            End If

        Next
    End Sub
#End Region

#Region "Clear Images"
    Public Sub ClearImages()

        Me.MainImageID = 0
        Me.ImageIDs.Clear()
    End Sub
#End Region

#Region "After Go"
    Private Sub PropertyCMS_AfterGo(ByVal iTableID As Integer) Handles MyBase.AfterGo

        Me.GetImages()
    End Sub
#End Region

#Region "After Update"
    Private Sub PropertyCMS_AfterUpdate(ByVal iTableID As Integer) Handles MyBase.AfterUpdate

        PropertyCMS.CleanupImageFolder()
    End Sub
#End Region

#Region "Cleanup Image Folder"
    Public Shared Sub CleanupImageFolder()

        Dim oDir As New DirectoryInfo(MapPath("~\secure\cms\images\"))
        For Each oFileInfo As FileInfo In oDir.GetFiles

            Try

                If oFileInfo.Name.ToLower.StartsWith("cmsdelete") Then oFileInfo.Delete()
            Catch ex As Exception

            End Try
        Next
    End Sub
#End Region

End Class
