Public Class SortedListView
    Public Class ListViewColumnSorter
        Implements IComparer

        ''' <summary>
        ''' Specifies the column to be sorted
        ''' </summary>
        Private ColumnToSort As Integer

        ''' <summary>
        ''' Specifies the order in which to sort (i.e. 'Ascending').
        ''' </summary>
        Private OrderOfSort As SortOrder

        ''' <summary>
        ''' Case insensitive comparer object
        ''' </summary>
        Private ObjectCompare As CaseInsensitiveComparer

        ''' <summary>
        ''' Class constructor.  Initializes various elements
        ''' </summary>
        Public Sub New()
            MyBase.New()
            ' Initialize the column to '0'
            ColumnToSort = 0
            ' Initialize the sort order to 'none'
            OrderOfSort = SortOrder.None
            ' Initialize the CaseInsensitiveComparer object
            ObjectCompare = New CaseInsensitiveComparer
        End Sub

        ''' <summary>
        ''' This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        ''' </summary>
        ''' <param name="x">First object to be compared</param>
        ''' <param name="y">Second object to be compared</param>
        ''' <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
            Dim compareResult As Integer
            Dim listviewY As ListViewItem
            Dim listviewX As ListViewItem
            ' Cast the objects to be compared to ListViewItem objects
            listviewX = CType(x, ListViewItem)
            listviewY = CType(y, ListViewItem)
            ' Compare the two items
            compareResult = ObjectCompare.Compare(listviewX.SubItems(ColumnToSort).Text, listviewY.SubItems(ColumnToSort).Text)
            ' Calculate correct return value based on object comparison
            If (OrderOfSort = SortOrder.Ascending) Then
                ' Ascending sort is selected, return normal result of compare operation
                Return compareResult
            ElseIf (OrderOfSort = SortOrder.Descending) Then
                ' Descending sort is selected, return negative result of compare operation
                Return (compareResult * -1)
            Else
                ' Return '0' to indicate they are equal
                Return 0
            End If
        End Function

        ''' <summary>
        ''' Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        ''' </summary>
        Public Property SortColumn As Integer
            Get
                Return ColumnToSort
            End Get
            Set(value As Integer)
                ColumnToSort = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        ''' </summary>
        Public Property Order As SortOrder
            Get
                Return OrderOfSort
            End Get
            Set(value As SortOrder)
                OrderOfSort = value
            End Set
        End Property
    End Class

    Private lvwColumnSorter As New ListViewColumnSorter

    Private Sub SortedListView_ColumnClick(sender As Object, e As ColumnClickEventArgs) Handles Me.ColumnClick
        Me.ListViewItemSorter = lvwColumnSorter
        ' Determine if clicked column is already the column that is being sorted.
        If (e.Column = lvwColumnSorter.SortColumn) Then
            ' Reverse the current sort direction for this column.
            If (lvwColumnSorter.Order = SortOrder.Ascending) Then
                lvwColumnSorter.Order = SortOrder.Descending
            Else
                lvwColumnSorter.Order = SortOrder.Ascending
            End If
        Else
            ' Set the column number that is to be sorted; default to ascending.
            lvwColumnSorter.SortColumn = e.Column
            lvwColumnSorter.Order = SortOrder.Ascending
        End If
        ' Perform the sort with these new sort options.
        Me.Sort()
        Me.ListViewItemSorter = Nothing
    End Sub
End Class
