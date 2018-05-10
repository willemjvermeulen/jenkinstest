Imports Intuitive
Imports Intuitive.Functions
Imports System.IO
Imports System.Net
Imports System.Drawing
Imports System.Web

Public Class PropertyTable
    Inherits TableBase

    Public Enum eBindMode
        Main
        Address
    End Enum

    Public SavedFilePath As String = ""

#Region "Properties"

    Public ReadOnly Property ThumbNail() As String
        Get
            Return Me("Logo").ToLower.Replace("propertylogo_", "PropertyLogoThumb_")
        End Get
    End Property

#End Region

#Region "New"

    Public Sub New(Optional ByVal BindMode As eBindMode = eBindMode.Main)

        Me.Table = "Property"

        'main
        If BindMode = eBindMode.Main Then
            With Me.Fields
                .Add(New Field("PropertyID", "Integer"))
                .Add(New Field("Reference", "String", 20))
                .Add(New Field("Name", "String", 50, ValidationType.NotEmpty))
                .Add(New Field("ShortName", "String", 15, ValidationType.NotEmpty))
                .Add(New Field("PropertyGroupID", "Integer", ValidationType.NotEmpty))
                .Add(New Field("PayeeID", "Integer", ValidationType.NotEmpty))
                .Add(New Field("PropertyTypeID", "Integer", ValidationType.NotEmpty))
                .Add(New Field("GeographyLevel3ID", "Integer", ValidationType.NotEmpty))
                .Add(New Field("Rating", "String", 10))
                .Add(New Field("Logo", "String", 50))
                .Add(New Field("MaximumRooms", "Integer"))
                .Add(New Field("OptionExpiryAmount", "Integer"))
                .Add(New Field("OptionExpiryUnit", "String", 10))
                .Add(New Field("PrioritySelling", "Boolean"))
                .Add(New Field("Notes", "Text"))
                .Add(New Field("ExcludeFromInvoices", "Boolean"))
                .Add(New Field("InvoicePeriod", "Integer"))
                .Add(New Field("CurrentProperty", "Boolean"))
                .Add(New Field("EditStatus", "String"))
                Me.Fields("EditStatus").NonBinding = True

                .Field("GeographyLevel3ID").DisplayFieldName = "Resort"
            End With
        End If

        'address
        If BindMode = eBindMode.Address Then
            With Me.Fields
                .Add(New Field("PropertyID", "Integer"))
                .Add(New Field("Address1", "String", 40))
                .Add(New Field("Address2", "String", 40))
                .Add(New Field("TownCity", "String", 30))
                .Add(New Field("County", "String", 30))
                .Add(New Field("PostcodeZip", "String", 15))
                .Add(New Field("Country", "String", 50))
                .Add(New Field("Telephone", "String", 20))
                .Add(New Field("Fax", "String", 20))
                .Add(New Field("Email", "String", 255, ValidationType.IsEmail))
                .Add(New Field("Website", "String", 255))
                .Add(New Field("ContactName", "String", 40))
                .Add(New Field("ContactPosition", "String", 50))
                .Add(New Field("ContactTelephone", "String", 20))
                .Add(New Field("ContactFax", "String", 20))
                .Add(New Field("ContactEmail", "String", 255, ValidationType.IsEmail))
                .Add(New Field("ContactMobile", "String", 20))
                .Add(New Field("EditStatus", "String"))
                Me.Fields("EditStatus").NonBinding = True
            End With
        End If

        Me.Clear()
    End Sub

#End Region

#Region "Check Update"

    Public Overrides Function CheckUpdate() As Boolean

        MyBase.CheckUpdate()

        'option expiry; check that both fields completed, or nay
        If SafeInt(Me("OptionExpiryAmount")) > 0 AndAlso Me("OptionExpiryUnit") = "" Then
            Me.Warnings.Add("The Option Expiry Unit must be specified if an amount is entered")
            Me.Fields("OptionExpiryUnit").IsValid = False
        ElseIf Me("OptionExpiryUnit") <> "" AndAlso SafeInt(Me("OptionExpiryAmount")) = 0 Then
            Me.Warnings.Add("The Option Expiry Amount must be specified if a unit has been selected")
            Me.Fields("OptionExpiryAmount").IsValid = False
        End If

        If Me.SavedFilePath <> "" Then Me.SetField("Logo", New FileInfo(Me.SavedFilePath).Name)

        Return Me.Warnings.Count = 0

    End Function

#End Region

#Region "Check Delete"

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        Dim iSystemUserType As String
        iSystemUserType = SQL.GetValue("exec GetSystemUserGroupType {0}", UserSession.SystemUserID)

        'Check that the user is an Administrator; else, no delete
        If iSystemUserType = "Administrator" Then
            'Check if property has any contracts; if so - nay delete for ye laddy
            If Not SQL.FKCheck("Contract", "PropertyID", iTableID) Then
                Me.Warnings.Add("Sorry, this property cannot be deleted as it is currently associated with one or more contracts.")
            Else
                SQL.Execute("Delete From Property Where PropertyID={0}", iTableID)
            End If
        Else
            Me.Warnings.Add("Sorry, only System Administrators are able to delete properties.")
        End If

        If Me.Warnings.Count = 0 Then

            'delete child records from PropertyRoomType
            SQL.Execute("Delete from PropertyRoomType Where PropertyID=" & iTableID.ToString)
        End If

        Return Me.Warnings.Count = 0
    End Function

#End Region

#Region "After Add"

    Private Sub PropertyTable_AfterAdd(ByVal iTableID As Integer) Handles MyBase.AfterAdd

        'set property reference
        Dim oSQL As New SQLTransaction
        oSQL.Add("exec GeneratePropertyReference {0}", iTableID)

        'sync settings
        Select Case Config.Installation
            Case "Server"
                oSQL.Add("update Property set SyncGUID=newid(), SyncStatus='In', SyncSystemUserID=0, " & _
                    "SyncRequired=0, EditStatus='Full' where PropertyID={0}", iTableID)

                'add to audit trail 
                ContractSyncAuditTrail.AddAuditTrailEntry("Property", iTableID, UserSession.SystemUserID, _
                    ContractSyncAuditTrail.EventType.AddedOnServer)
            Case "Client"
                oSQL.Add("update Property set SyncGUID=newid(), SyncStatus='Out', SyncSystemUserID={1}, " & _
                    "SyncRequired=1, EditStatus='Full'  where PropertyID={0}", iTableID, UserSession.SystemUserID)
            Case Else
                Throw New Exception("Unknown Installtion Mode " & Config.Installation)
        End Select

        oSQL.Execute()

        Me.Refresh()

    End Sub

#End Region

#Region "After Update"

    Private Sub PropertyTable_AfterUpdate(ByVal iTableID As Integer) Handles MyBase.AfterUpdate
        PropertyTable.SetSyncRequired(iTableID)
    End Sub

#End Region

#Region "Set Sync Required"

    Public Shared Sub SetSyncRequired(ByVal iPropertyID As Integer)

        'if in client mode, set the sync required flag
        If Config.Installation = "Client" Then
            SQL.Execute("update Property set SyncRequired=1 where PropertyID={0}", iPropertyID)
        ElseIf Config.Installation = "Server" Then
            SQL.Execute("update Property set SyncRequired=1 where PropertyID={0} and SyncStatus='In' " & _
                "and SyncSystemUserID>0", iPropertyID)
        Else
            Throw New Exception("Unknown Installation Mode " & Config.Installation)
        End If

    End Sub

#End Region

#Region "Facility Stuff"
    Public Shared Sub SaveFacilities(ByVal iPropertyID As Integer, ByVal aFacilityIDs As ArrayList)

        Dim oSQL As New SQLTransaction
        With oSQL

            'delete the old
            .Add("delete from PropertyFacility where PropertyID={0}", iPropertyID)

            'add the new
            For Each iFacilityID As Integer In aFacilityIDs

                .Add("insert into PropertyFacility (PropertyID, FacilityID) values ({0},{1})", _
                    iPropertyID, iFacilityID)
            Next

            'and execute
            .Execute()
        End With
    End Sub
#End Region

#Region "Copy Address"

    Public Shared Sub CopyAddress(ByVal PropertyID As Integer, ByVal CopyPropertyID As Integer, _
            ByVal aAddressTypes As ArrayList)

        SQL.Execute("exec Property_CopyAddress", PropertyID, CopyPropertyID, _
                SQL.GetSqlValue(ArrayListToDelimitedString(aAddressTypes)))

    End Sub

#End Region






#Region "Image Handling"


    Public Shared Function SaveImage(ByVal oPostedFile As HttpPostedFile, ByVal sURL As String, ByVal PropertyID As Integer) As String

        Dim sWorkFile As String = ""

        Dim sImageFormat As String = ""


        'URL specified, download image and save in work folder
        If sURL <> "" Then
            sImageFormat = sURL.Substring(sURL.Length - 3, 3)
            'try and download the file
            sWorkFile = PropertyTable.DownloadImage(sURL)
        End If

        'no URL and posted file
        If sURL = "" AndAlso Not oPostedFile Is Nothing AndAlso oPostedFile.ContentLength > 0 Then
            sImageFormat = oPostedFile.FileName.Substring(oPostedFile.FileName.Length - 3, 3)
            sWorkFile = PropertyTable.GetRandomWorkFilename
            oPostedFile.SaveAs(sWorkFile)
        End If


        Dim sFullPath As String = MapPath(("/images/HotelLogos") & "/propertylogo_" & PropertyID & "." & sImageFormat.ToLower)
        Dim sThumbNailFullPath As String = MapPath(("/images/HotelLogos") & "/propertylogothumb_" & PropertyID & "." & sImageFormat.ToLower)


        'save the blighter
        Try

            If File.Exists(sFullPath) Then File.Delete(sFullPath)
            If File.Exists(sThumbNailFullPath) Then File.Delete(sThumbNailFullPath)

			Images.ImageFunctions.ImageResize(sWorkFile, sFullPath, 200, 80)
			Images.ImageFunctions.ImageResize(sWorkFile, sThumbNailFullPath, 40, 40)
        Catch ex As Exception

        End Try
       

        Return sFullPath
    End Function

#Region "Download Image"

    Public Shared Function DownloadImage(ByVal URL As String) As String


        Dim oClient As New WebClient
        Dim oImage As Image

        Dim sFilename As String = PropertyTable.GetRandomWorkFilename

        oImage = Image.FromStream(oClient.OpenRead(URL))
        oImage.Save(sFilename)

        Return sFilename

    End Function


#End Region


#Region "RandomWorkFilename"

    Public Shared Function GetRandomWorkFilename() As String
        Dim sFilename As String = Now.ToString("ddMMhhmmss") & Now.Millisecond.ToString & ".tmp"

        While File.Exists(PropertyTable.GetWorkFolder & sFilename)

            System.Threading.Thread.Sleep(2)
            sFilename = Now.ToString("ddMMhhmmss") & Now.Millisecond.ToString & ".tmp"
        End While


        Return PropertyTable.GetWorkFolder() & sFilename
    End Function


#End Region

#Region "Create Work Folder"

    Private Shared Function GetWorkFolder() As String

        Dim sFolder As String = MapPath("/work/")
        Intuitive.FileFunctions.CreateFolder(sFolder)
        Return sFolder
    End Function

#End Region

#End Region

End Class
