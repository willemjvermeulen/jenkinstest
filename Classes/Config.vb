Imports System.Configuration.ConfigurationManager
Imports Intuitive.Functions

Public Class Config

    Public Shared ReadOnly Property OpenExchangeRateKey() As String
        Get
            Return Config.GetSetting("OpenExchangeRateKey")
        End Get
    End Property

    Public Shared ReadOnly Property Installation() As String
        Get
            Return Config.GetSetting("Installation")
        End Get
    End Property

    Public Shared ReadOnly Property InterfaxUser() As String
        Get
            Return Config.GetSetting("InterfaxUser")
        End Get
    End Property

    Public Shared ReadOnly Property InterfaxPassword() As String
        Get
            Return Config.GetSetting("InterfaxPassword")
        End Get
    End Property

    Public Shared ReadOnly Property SMTPHost() As String
        Get
            Return Config.GetSetting("SMTPHost")
        End Get
    End Property

    Public Shared ReadOnly Property CompanyName() As String
        Get
            Return Config.GetSetting("CompanyName")
        End Get
    End Property

    Public Shared ReadOnly Property FullCompanyName() As String
        Get
            Return Config.GetSetting("FullCompanyName", False)
        End Get
    End Property

    Public Shared ReadOnly Property CompanyTelephone() As String
        Get
            Return Config.GetSetting("CompanyTelephone", False)
        End Get
    End Property

    Public Shared ReadOnly Property CompanyFax() As String
        Get
            Return Config.GetSetting("CompanyFax", False)
        End Get
    End Property

    Public Shared ReadOnly Property CompanyAddressLine1() As String
        Get
            Return Config.GetSetting("CompanyAddressLine1", False)
        End Get
    End Property

    Public Shared ReadOnly Property CompanyAddressLine2() As String
        Get
            Return Config.GetSetting("CompanyAddressLine2", False)
        End Get
    End Property

    Public Shared ReadOnly Property ViewCreditCardNumber() As Boolean
        Get
            Return SafeBoolean(Config.GetSetting("ViewCreditCardNumber", False))
        End Get
    End Property

    Public Shared ReadOnly Property ImageContentFolder() As String
        Get
            Return Config.GetSetting("ImageContentFolder")
        End Get
    End Property

    Public Shared ReadOnly Property ImageWebFolder() As String
        Get
            Return Config.GetSetting("ImageWebFolder")
        End Get
    End Property

    Public Shared ReadOnly Property SyncURLBase() As String
        Get
            Return Config.GetSetting("SyncURLBase")
        End Get
    End Property

    Public Shared ReadOnly Property AllowTransferBookings() As Boolean
        Get
            Return SafeBoolean(Config.GetSetting("AllowTransferBookings", False))
        End Get
    End Property

    Public Shared ReadOnly Property SecuritySet() As Boolean
        Get
            Return SafeBoolean(Config.GetSetting("SecuritySet", False))
        End Get
    End Property

#Region "get setting"

    Private Shared Function GetSetting(ByVal sSetting As String, Optional ByVal bMustHaveValue As Boolean = True) As String

        If Not AppSettings(sSetting) Is Nothing Then

            If bMustHaveValue = False OrElse Not AppSettings(sSetting).ToString = "" Then
                Return AppSettings(sSetting).ToString
            Else
                Throw New Exception(sSetting & " was found in the config, but does not have a value")
            End If
        Else
            Throw New Exception("Could not find " & sSetting & " setting")
        End If
    End Function

#End Region

End Class
