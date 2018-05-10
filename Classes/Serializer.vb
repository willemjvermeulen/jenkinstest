Imports System.Xml
Imports System.Xml.Serialization
Imports System.IO
Public Class Serializer

    Private Const sXmlHeader As String = "<?xml version=""1.0""?>"

#Region "serialize"

    Public Shared Function Serialize(ByVal oObject As Object, Optional ByVal bAutoClean As Boolean = False) As XmlDocument

        Dim oXMLSerializer As New XmlSerializer(oObject.GetType)

        Dim oStream As New MemoryStream
        oXMLSerializer.Serialize(oStream, oObject)

        oStream.Position = 0

        Dim oXmlDocument As New XmlDocument
        oXmlDocument.Load(oStream)

        If Not bAutoClean Then
            Return oXmlDocument
        Else
            Return Serializer.CleanXml(oXmlDocument, True)
        End If


    End Function

#End Region

    'deserialize
    Public Shared Function DeSerialize(ByVal oType As Type, _
        ByVal sString As String, Optional ByVal bAppendheader As Boolean = True) As Object

        Dim oXMLSerializer As New XmlSerializer(oType)

        Dim oStream As New MemoryStream
        Dim oStreamwriter As New StreamWriter(oStream)
        oStreamwriter.Write(IIf(bAppendheader, sXmlHeader, "").ToString & sString.Trim)
        oStreamwriter.Flush()

        oStream.Position = 0

        Return oXMLSerializer.Deserialize(oStream)

    End Function


#Region "Clone"
    Public Shared Function Clone(ByVal oObjectToClone As Object) As Object

        If Not oObjectToClone.GetType.IsSerializable Then
            Throw New Exception("You can only clone serializable objects")
        End If

        Dim sSerializedObject As String = CleanXml(Serializer.Serialize(oObjectToClone)).InnerXml

        Dim oObjectCopied As Object = Serializer.DeSerialize(oObjectToClone.GetType, sSerializedObject)

        Return oObjectCopied

    End Function
#End Region

    Public Shared Function CleanXml(ByVal oDocumentToClean As XmlDocument, _
            Optional ByVal bStripArrayOf As Boolean = False) As XmlDocument

        'Clean XML. Else you end up with the doc definition in the XML
        Dim sCleanXml As String = oDocumentToClean.InnerXml
        sCleanXml = sCleanXml.Replace("<?xml version=""1.0""?>", "")
        sCleanXml = sCleanXml.Replace("xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""", "")
        sCleanXml = sCleanXml.Replace("<ArrayOf", "<")
        sCleanXml = sCleanXml.Replace("</ArrayOf", "</")

        'Replace any dodgy charecters.Must eb a better way
        ' idealy you'd tell SQl to expect UTF-16. Cant work out how though.
        'sCleanXml = sCleanXml.Replace("£", "")
        'sCleanXml = sCleanXml.Replace("€", "")
        'sCleanXml = sCleanXml.Replace("$", "")
        'sCleanXml = sCleanXml.Replace("@", "")
        'sCleanXml = sCleanXml.Replace("%", "")
        'sCleanXml = sCleanXml.Replace("½", "&#189;")

        Dim oCleaned As New XmlDocument
        oCleaned.LoadXml(sCleanXml)
        Return oCleaned
    End Function
End Class