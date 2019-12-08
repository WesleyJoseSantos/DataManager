Imports System.Windows.Forms

Public Class CfgsEnumDataType
    Dim myCFGs As New CfgManager
    Dim dataList As New List(Of String)
    Dim dataNames As New List(Of String)
    Dim myEnumType As Object

    Public Sub New(enumType As Object)
        myEnumType = enumType
        getEnumNames()
        getEnumValues()
    End Sub

    Public Sub New(enumType As Object, defaultValues() As String)
        myEnumType = enumType
        getEnumNames()
        getEnumValues()
        VerifyDefault(defaultValues)
    End Sub

    Private Sub VerifyDefault(defValues() As String)
        Dim defList As List(Of String) = defValues.ToList
        Dim cont As UInt16 = 0
        For i = 0 To defList.Count - 1
            Dim item = defList(i)
            Dim dtItem = dataList(cont)
            If dtItem = "Erro" Or dtItem = "" Or dtItem = "Nothing" Then
                dataList(i) = item
            End If
            cont += 1
        Next
    End Sub

    Private Sub getEnumNames()
        Dim names() As String
        names = System.Enum.GetNames(myEnumType)
        dataNames.AddRange(names)
    End Sub

    Private Sub getEnumValues()
        For Each name In dataNames
            dataList.Add(myCFGs.Read(name))
        Next
    End Sub

    Private Sub ShowError(errorText As String)
        MessageBox.Show(errorText, "EnumDataType Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Public Sub SetValue(name As String, value As String)
        Try
            dataList(dataNames.IndexOf(name)) = value
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
        For Each name In dataNames
            myCFGs.Write(name, dataList.IndexOf(name))
        Next
    End Sub
End Class
