using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Reflection;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView.Rendering;

namespace GetCellDiplayText {
    public partial class _Default : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            SetupGrid();
            ProcessCellValuesAndText();
        }

        private void ProcessCellValuesAndText() {
            for(int i = 0; i < ASPxGridView1.VisibleRowCount; i++) {
                string[] fields = new string[] {"ID", "Date", "Price"};
                object values = ASPxGridView1.GetRowValues(i, fields);
                // TODO: process cell values
                
                string[] text = GetDisplayText(ASPxGridView1, i, fields);
                // TODO process cell text
                WriteLine(text);
            }
        }

        private void WriteLine(string[] text) {
            foreach(string word in text) {
                Response.Write(word);
                Response.Write(" ");
            }
            Response.Write("<br/>");
        }

        private string[] GetDisplayText(ASPxGridView grid, int row, string[] fields) {
            int count = fields.GetLength(0);
            string[] result = new string[count];
            for(int i = 0; i < count; i++) {
                result[i] = GetDisplayText(grid, row, fields[i]);
            }
            return result;
        }

        public string GetDisplayText(ASPxGridView grid, int row, string field) {
            GridViewDataColumn col = (GridViewDataColumn)grid.Columns[field];
            PropertyInfo pi = typeof(ASPxGridView).GetProperty("RenderHelper", BindingFlags.Instance | BindingFlags.NonPublic);
            ASPxGridViewRenderHelper renderHelper = (ASPxGridViewRenderHelper)pi.GetValue(grid, null);
            return renderHelper.TextBuilder.GetRowDisplayText(col, row);
        }

        private void SetupGrid() {
            ASPxGridView1.DataSource = GetData();
            if(ASPxGridView1.Columns.Count == 0) {
                GridViewDataTextColumn col = new GridViewDataTextColumn();
                col.FieldName = "ID";
                col.VisibleIndex = 0;
                ASPxGridView1.Columns.Add(col);
                col = new GridViewDataTextColumn();
                col.FieldName = "Date";
                col.PropertiesTextEdit.DisplayFormatString = "g";
                col.VisibleIndex = 1;
                ASPxGridView1.Columns.Add(col);
                col = new GridViewDataTextColumn();
                col.FieldName = "Price";
                col.PropertiesTextEdit.DisplayFormatString = "c";
                col.VisibleIndex = 2;
                ASPxGridView1.Columns.Add(col);
            }
            ASPxGridView1.DataBind();
        }
        private DataTable GetData() {
            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Price", typeof(decimal));
            table.Rows.Add(1, DateTime.Now, 1.25m);
            table.Rows.Add(2, new DateTime(2001, 1, 1), 0.99m);
            return table;
        }
    }
}
