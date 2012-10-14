Imports System
Imports System.Text
Imports System.Windows.Forms
Imports System.Runtime.InteropServices

Public Class DirectoryDialog
    Public Structure BROWSEINFO
        Public hWndOwner As IntPtr
        Public pIDLRoot As Integer
        Public pszDisplayName As String
        Public lpszTitle As String
        Public ulFlags As Integer
        Public lpfnCallback As Integer
        Public lParam As Integer
        Public iImage As Integer
    End Structure

    Const MAX_PATH As Integer = 260

    Public Enum BrowseForTypes As Integer
        Computers = 4096
        Directories = 1
        FilesAndDirectories = 16384
        FileSystemAncestors = 8
    End Enum

    Declare Function CoTaskMemFree Lib "ole32" Alias "CoTaskMemFree" (ByVal hMem As IntPtr) As Integer
    Declare Function lstrcat Lib "kernel32" Alias "lstrcat" (ByVal lpString1 As String, ByVal lpString2 As String) As IntPtr
    Declare Function SHBrowseForFolder Lib "shell32" Alias "SHBrowseForFolder" (ByRef lpbi As BROWSEINFO) As IntPtr
    Declare Function SHGetPathFromIDList Lib "shell32" Alias "SHGetPathFromIDList" (ByVal pidList As IntPtr, ByVal lpBuffer As StringBuilder) As Integer
    Protected Function RunDialog(ByVal hWndOwner As IntPtr) As Boolean

        Dim udtBI As BROWSEINFO = New BROWSEINFO()
        Dim lpIDList As IntPtr
        Dim hTitle As GCHandle = GCHandle.Alloc(Title, GCHandleType.Pinned)
        udtBI.hWndOwner = hWndOwner
        udtBI.lpszTitle = Title
        udtBI.ulFlags = BrowseFor
        Dim buffer As StringBuilder = New StringBuilder(MAX_PATH)
        buffer.Length = MAX_PATH
        udtBI.pszDisplayName = buffer.ToString()
        lpIDList = SHBrowseForFolder(udtBI)
        hTitle.Free()
        If lpIDList.ToInt64() <> 0 Then
            If BrowseFor = BrowseForTypes.Computers Then
                m_Selected = udtBI.pszDisplayName.Trim()
            Else
                Dim path As StringBuilder = New StringBuilder(m_Selected, MAX_PATH)
                SHGetPathFromIDList(lpIDList, path)
                m_Selected = path.ToString()
            End If
            CoTaskMemFree(lpIDList)
        Else
            Return False
        End If
        Return True
    End Function

    Public Function ShowDialog() As DialogResult
        Return ShowDialog(Nothing)
    End Function

    Public Function ShowDialog(ByVal owner As IWin32Window) As DialogResult
        Dim handle As IntPtr
        If Not owner Is Nothing Then
            handle = owner.Handle
        Else
            handle = IntPtr.Zero
        End If
        If RunDialog(handle) Then
            Return DialogResult.OK
        Else
            Return DialogResult.Cancel
        End If
    End Function

    Public Property Title() As String
        Get
            Return m_Title
        End Get
        Set(ByVal Value As String)
            If Value Is DBNull.Value Then
                Throw New ArgumentNullException()
            End If
            m_Title = Value
        End Set
    End Property

    Public Property Selected() As String
        Get
            Return m_Selected
        End Get
        Set(value As String)
            m_Selected = value
        End Set
    End Property

    Public Property BrowseFor() As BrowseForTypes
        Get
            Return m_BrowseFor
        End Get
        Set(ByVal Value As BrowseForTypes)
            m_BrowseFor = Value
        End Set
    End Property

    Private m_BrowseFor As BrowseForTypes = BrowseForTypes.Directories
    Private m_Title As String = ""
    Private m_Selected As String = ""

    Public Sub New()
    End Sub
End Class