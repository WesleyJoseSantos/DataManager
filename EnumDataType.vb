Imports System.Windows.Forms

Public Class EnumDataType
    Dim myFileSystem As New FileVarSystem
    Dim dataList As New List(Of String)
    Dim dataNames As New List(Of String)
    Dim myEnumType As Object

    Public Sub New(enumType As Object, filePath As String)
        myEnumType = enumType
        getEnumNames()
        getEnumValues()
        myFileSystem.SetFilePath(filePath)
    End Sub

    Public Sub New(enumType As Object, filePath As String, defaultValues() As String)
        myEnumType = enumType
        getEnumNames()
        getEnumValues()
        VerifyDefault(defaultValues)
        myFileSystem.SetFilePath(filePath)
    End Sub

    Private Sub VerifyDefault(defValues() As String)
        Dim defList As List(Of String) = defValues.ToList
        For i = 0 To dataList.Count - 1
            Dim item = dataList(i)
            If item = "Erro" Or item = "" Then
                dataList(i) = defList(i)
            End If
        Next
    End Sub

    Private Sub getEnumNames()
        Dim names() As String
        names = System.Enum.GetNames(myEnumType)
        dataNames.AddRange(names)
    End Sub

    Private Sub getEnumValues()
        For Each name In dataNames
            dataList.Add(myFileSystem.ReadVar(name))
        Next
    End Sub

    Private Sub ShowError(errorText As String)
        MessageBox.Show(errorText, "EnumDataType Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Public Sub SetValue(index As UInt16, value As String)
        Try
            dataList(index) = value
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Sub

    Public Function GetValue(index As UInt16)
        Try
            Return dataList(index)
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
        Return Nothing
    End Function

    Public Sub Save()
        myFileSystem.SaveData(dataNames.ToArray, dataList.ToArray)
    End Sub

End Class
