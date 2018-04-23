Imports System
Imports System.Data
Imports System.Configuration
Imports System.Collections
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Reflection
Imports DevExpress.Web
Imports DevExpress.Web.Rendering

Namespace GetCellDiplayText
    Partial Public Class _Default
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
            SetupGrid()
            ProcessCellValuesAndText()
        End Sub

        Private Sub ProcessCellValuesAndText()
            For i As Integer = 0 To ASPxGridView1.VisibleRowCount - 1
                Dim fields() As String = {"ID", "Date", "Price"}
                Dim values As Object = ASPxGridView1.GetRowValues(i, fields)
                ' TODO: process cell values

                Dim text() As String = GetDisplayText(ASPxGridView1, i, fields)
                ' TODO process cell text
                WriteLine(text)
            Next i
        End Sub

        Private Sub WriteLine(ByVal text() As String)
            For Each word As String In text
                Response.Write(word)
                Response.Write(" ")
            Next word
            Response.Write("<br/>")
        End Sub

        Private Function GetDisplayText(ByVal grid As ASPxGridView, ByVal row As Integer, ByVal fields() As String) As String()
            Dim count As Integer = fields.GetLength(0)
            Dim result(count - 1) As String
            For i As Integer = 0 To count - 1
                result(i) = GetDisplayText(grid, row, fields(i))
            Next i
            Return result
        End Function

        Public Function GetDisplayText(ByVal grid As ASPxGridView, ByVal row As Integer, ByVal field As String) As String
            Dim col As GridViewDataColumn = CType(grid.Columns(field), GridViewDataColumn)
            Dim pi As PropertyInfo = GetType(ASPxGridView).GetProperty("RenderHelper", BindingFlags.Instance Or BindingFlags.NonPublic)
            Dim renderHelper As GridViewRenderHelper = DirectCast(pi.GetValue(grid, Nothing), GridViewRenderHelper)
            Return renderHelper.TextBuilder.GetRowDisplayText(col, row)
        End Function

        Private Sub SetupGrid()
            ASPxGridView1.DataSource = GetData()
            If ASPxGridView1.Columns.Count = 0 Then
                Dim col As New GridViewDataTextColumn()
                col.FieldName = "ID"
                col.VisibleIndex = 0
                ASPxGridView1.Columns.Add(col)
                col = New GridViewDataTextColumn()
                col.FieldName = "Date"
                col.PropertiesTextEdit.DisplayFormatString = "g"
                col.VisibleIndex = 1
                ASPxGridView1.Columns.Add(col)
                col = New GridViewDataTextColumn()
                col.FieldName = "Price"
                col.PropertiesTextEdit.DisplayFormatString = "c"
                col.VisibleIndex = 2
                ASPxGridView1.Columns.Add(col)
            End If
            ASPxGridView1.DataBind()
        End Sub
        Private Function GetData() As DataTable
            Dim table As New DataTable()
            table.Columns.Add("ID", GetType(Integer))
            table.Columns.Add("Date", GetType(Date))
            table.Columns.Add("Price", GetType(Decimal))
            table.Rows.Add(1, Date.Now, 1.25D)
            table.Rows.Add(2, New Date(2001, 1, 1), 0.99D)
            Return table
        End Function
    End Class
End Namespace
