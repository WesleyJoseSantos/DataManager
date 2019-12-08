Imports System.IO
Imports System.Windows.Forms

''' <summary>
''' Allows to save a variable values to a text file
''' </summary>
Public Class FileVarSystem
    Dim filePath As String = ""
    Dim separator = "="
    Public Sub SaveData(ByVal nameList() As String, ByVal dataList() As String)
        Dim data As New List(Of String)
        For i = 0 To nameList.Count - 1
            data.Add(nameList(i) & " " & separator & " " & dataList(i))
        Next
        File.WriteAllLines(filePath, data)
    End Sub

    Public Sub DeletFile()
        If File.Exists(filePath) Then
            File.Delete(filePath)
        End If
    End Sub

    Public Sub AppendData(ByVal nameList() As String, ByVal dataList() As String)
        Dim data As New List(Of String)
        For i = 0 To nameList.Count - 1
            data.Add(nameList(i) & " " & separator & " " & dataList(i))
        Next
        File.AppendAllLines(filePath, data)
    End Sub

    Public Sub LoadData(ByVal nameList, ByRef dataList)
        Dim data() As String
        Dim names As New List(Of String)
        Dim values As New List(Of String)
        If (File.Exists(filePath)) Then
            data = File.ReadAllLines(filePath)
            For Each item In data
                Dim newData() As String = item.Split(separator)
                names.Add(newData(0).TrimEnd)
                values.Add(newData(1).TrimStart)
            Next
        End If
        Dim i As Byte = 0
        For Each item In names
            For Each name In nameList
                If (item = name) Then
                    dataList(i) = values(i)
                End If
            Next
            i += 1
        Next
    End Sub

    Public Sub LoadDataInInterval(ByVal nameList, ByRef dataList, ByVal inicio, ByVal fim)
        Dim data() As String
        Dim names As New List(Of String)
        Dim values As New List(Of String)
        If (File.Exists(filePath)) Then
            data = File.ReadAllLines(filePath)
            If inicio > data.Length - 1 Or fim > data.Length - 1 Then
                MessageBox.Show("O invervalo de leitura não existe no arquivo selecionado.", "Erro")
                Exit Sub
            End If
            For i1 = inicio To fim
                Dim item = data(i1)
                Dim newData() As String = item.Split(separator)
                names.Add(newData(0).TrimEnd)
                values.Add(newData(1).TrimStart)
            Next
        End If
        Dim i As Byte = 0
        For Each item In names
            For Each name In nameList
                If (item = name) Then
                    dataList(i) = values(i)
                End If
            Next
            i += 1
        Next
    End Sub

    Public Sub SetSeparator(ByVal newSeparator)
        separator = newSeparator
    End Sub

    Public Function ReadVar(ByVal name)
        Dim data() As String
        Dim names As New List(Of String)
        Dim values As New List(Of String)
        Dim line As UInt16 = 0
        Try
            If (File.Exists(filePath)) Then
                data = File.ReadAllLines(filePath)
                For Each item In data
                    Dim newData() As String = item.Split(separator)
                    names.Add(newData(0).TrimEnd)
                    Dim value As String = ""
                    For k = 0 To newData.Count - 1
                        If k > 0 Then
                            value &= newData(k)
                            If k < newData.Count - 1 Then
                                value &= "="
                            End If
                        End If
                    Next
                    values.Add(value.TrimStart)
                Next
            End If
            Dim i As Byte = 0
            For Each item In names
                If (item = name) Then
                    Return (values(i))
                End If
                i += 1
            Next
        Catch ex As Exception
            MessageBox.Show("Erro de Leirura." & vbLf &
                            "Erro         : " & ex.ToString & vbLf &
                            "Variavel Name: " & name & vbLf &
                            "Line     : " & line)
        End Try
        Return ("Erro")
    End Function

    Public Sub WriteVar(ByVal name As String, ByVal valor As String)
        Dim data() As String
        Dim names As New List(Of String)
        Dim values As New List(Of String)
        If (File.Exists(filePath)) Then
            data = File.ReadAllLines(filePath)
            For Each item In data
                Dim newData() As String = item.Split(separator)
                names.Add(newData(0).TrimEnd)
                values.Add(newData(1).TrimStart)
            Next
            Dim i As Byte = 0
            Dim varFounded As Boolean = False
            For Each item In names
                If (item = name) Then
                    data(i) = name & " " & separator & " " & valor
                    varFounded = True
                End If
                i += 1
            Next
            If varFounded Then
                File.WriteAllLines(filePath, data)
            Else
                Dim newData As String = name & " " & separator & " " & valor & vbCrLf
                File.AppendAllText(filePath, newData)
            End If
        Else
            Dim newData As String = name & " " & separator & " " & valor & vbCrLf
            File.WriteAllText(filePath, newData)
        End If
    End Sub

    Public Function VerifyVar(name As String, value As String) As Boolean
        Return ReadVar(name) = value
    End Function

    Public Function VerifyFile(names() As String, values() As String)
        If names.Count = values.Count Then
            For i = 0 To names.Length - 1
                If ReadVar(names(i)) <> values(i) Then
                    Return False
                End If
            Next
        End If
        Return True
    End Function

    Public Sub ClearFile()
        File.WriteAllText(filePath, "")
    End Sub

    Public Function VarExists(ByVal value)
        Dim data() As String
        Dim founded As Boolean = False
        If (File.Exists(filePath)) Then
            data = File.ReadAllLines(filePath)
            For Each item In data
                If (item.Split(separator)(0).Trim = value) Then
                    founded = True
                End If
            Next
        End If
        Return founded
    End Function

    Public Function DataExists(ByVal value)
        Dim data() As String
        Dim founded As Boolean = False
        If (File.Exists(filePath)) Then
            data = File.ReadAllLines(filePath)
            For Each item In data
                If (item = value) Then
                    founded = True
                End If
            Next
        End If
        Return founded
    End Function

    Public Sub AddNewData(ByVal value)
        If Not (DataExists(value)) Then
            Dim bkp As String = separator
            File.AppendAllText(filePath, value & vbCrLf)
        End If
    End Sub

    Public Function LoadLines(ByVal path)
        If (File.Exists(path)) Then
            Dim Data() As String = File.ReadAllLines(path)
            Return (Data)
        End If
        Return (0)
    End Function

    Public Sub SetFilePath(ByVal path As String)
        If (File.Exists(path)) Then
            filePath = path
        Else
            Try
                Dim fs As FileStream = File.Create(path)
                fs.Close()
                filePath = path
            Catch ex As Exception
                Directory.CreateDirectory(ParsePath(path))
                Dim fs As FileStream = File.Create(path)
                fs.Close()
                filePath = path
                MessageBox.Show("Erro em fileVar/System, setFilePath: " & ex.Message)
            End Try

        End If
    End Sub

    Public Function GetFilePath()
        Return filePath
    End Function

    Function ParsePath(sPathIn As String) As String
        Dim I As Integer
        For I = Len(sPathIn) To 1 Step -1
            If InStr(":\", Mid$(sPathIn, I, 1)) Then Exit For
        Next
        ParsePath = Left$(sPathIn, I)
    End Function

    Public Function Exists()
        Return (File.Exists(filePath))
    End Function

    Public Function Empty()
        Return (File.ReadAllText(filePath) = "")
    End Function
End Class
