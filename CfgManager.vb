Imports System.Configuration
Imports System.Windows.Forms

''' <summary>
''' Management of Cfg Application File
''' </summary>
Public Class CfgManager
    Public Function Read(ByVal key As String) As String
        Try
            Dim appSettings = ConfigurationManager.AppSettings
            Dim result As String = If(appSettings(key), "Nothing")
            Return (result)
        Catch __unusedConfigurationErrorsException1__ As ConfigurationErrorsException
            MessageBox.Show(__unusedConfigurationErrorsException1__.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            Return ("Erro")
        End Try
    End Function

    Public Sub Write(ByVal key As String, ByVal value As String)
        Try
            Dim configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
            Dim settings = configFile.AppSettings.Settings

            If settings(key) Is Nothing Then
                settings.Add(key, value)
            Else
                settings(key).Value = value
            End If

            configFile.Save(ConfigurationSaveMode.Modified)
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name)
        Catch __unusedConfigurationErrorsException1__ As ConfigurationErrorsException
            MessageBox.Show(__unusedConfigurationErrorsException1__.Message & vbCrLf)
        End Try
    End Sub

End Class
