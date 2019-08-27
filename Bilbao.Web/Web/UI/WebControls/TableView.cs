using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bilbao.Web.UI.WebControls
{

    public class TableView : CompositeDataBoundControl
    {
        private DataControlFieldCollection _fieldCollection;

        [
            DefaultValue(null),
            //Editor("System.Web.UI.Design.WebControls.DataControlFieldTypeEditor, " + AssemblyRef.SystemDesign, typeof(UITypeEditor)),
            MergableProperty(false),
            PersistenceMode(PersistenceMode.InnerProperty),
            //WebCategory("Default"),
            //WebSysDescription(SR.DataControls_Columns)
        ]
        public virtual DataControlFieldCollection Columns
        {
            get
            {
                if (_fieldCollection == null)
                {
                    _fieldCollection = new DataControlFieldCollection();
                    //_fieldCollection.FieldsChanged += new EventHandler(OnFieldsChanged);
                    if (IsTrackingViewState)
                        ((IStateManager)_fieldCollection).TrackViewState();
                }
                return _fieldCollection;
            }
        }

        protected override int CreateChildControls(IEnumerable dataSource, bool dataBinding)
        {
            int count = 0;

            if (dataSource != null)
            {

            }

            return count;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (DesignMode)
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("Column 1");
                dt.Columns.Add("Column 2");
                dt.Columns.Add("Column 3");

                DataRow newRow = dt.NewRow();

                newRow[0] = "1";
                newRow[1] = "Algo";
                newRow[2] = "Descripción algo";

                dt.Rows.Add(newRow);

                this.DataSource = dt;
            }

            this.EnsureDataBound();
                
            base.Render(writer);
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "table-responsive");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "table");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            RenderTableHeader(writer);

            writer.RenderBeginTag(HtmlTextWriterTag.Tbody);

            DataTable dataTableSource = this.DataSource as DataTable;
            if (dataTableSource != null)
            {
                foreach (DataRow row in dataTableSource.Rows)
                {
                    RenderTableRow(writer, row);
                }
            }
            else
            {
                base.RenderContents(writer);
            }

            writer.RenderEndTag(); // TBody
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            writer.RenderEndTag(); // Table
            writer.RenderEndTag(); // Div
        }

        protected virtual void RenderTableHeader(HtmlTextWriter writer)
        {
            if ((DataSource == null) || (Columns == null) || (Columns.Count < 1))
                return;

            // Render THEAD
            writer.RenderBeginTag(HtmlTextWriterTag.Thead);

            foreach (DataControlField column in Columns)
            {
                if (column.Visible)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Th);

                    if (column.ShowHeader)
                        writer.Write(column.HeaderText);

                    writer.RenderEndTag();
                }
            }
            
            writer.RenderEndTag(); // THead
        }

        protected virtual void RenderTableRow(HtmlTextWriter writer, object row)
        {
            DataRow rowData = row as DataRow;

            if (rowData != null)
            {
                RenderDataRow(writer, rowData);
            }
        }

        private void RenderDataRow(HtmlTextWriter writer, DataRow row)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            foreach (DataControlField column in Columns)
            {
                if (column.Visible)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);

                    if (column is BoundField)
                    {
                        writer.Write(row[((BoundField)column).DataField]);
                    }

                    if (column is CommandField)
                    {
                        CommandField cmdField = column as CommandField;

                        if (cmdField.ShowEditButton)
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, Page.ClientScript.GetPostBackEventReference(this, "Edit"));
                            writer.RenderBeginTag(HtmlTextWriterTag.A);

                            writer.AddAttribute(HtmlTextWriterAttribute.Src, cmdField.EditImageUrl);
                            writer.AddAttribute(HtmlTextWriterAttribute.Alt, cmdField.EditText);
                            writer.RenderBeginTag(HtmlTextWriterTag.Img);
                            writer.RenderEndTag();

                            writer.RenderEndTag(); // A
                        }

                        if (cmdField.ShowDeleteButton)
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, Page.ClientScript.GetPostBackEventReference(this, "Delete"));
                            writer.RenderBeginTag(HtmlTextWriterTag.A);

                            writer.AddAttribute(HtmlTextWriterAttribute.Src, cmdField.DeleteImageUrl);
                            writer.AddAttribute(HtmlTextWriterAttribute.Alt, cmdField.DeleteText);
                            writer.RenderBeginTag(HtmlTextWriterTag.Img);
                            writer.RenderEndTag();

                            writer.RenderEndTag(); // A
                        }

                        if (cmdField.ShowInsertButton)
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, Page.ClientScript.GetPostBackEventReference(this, "Insert"));
                            writer.RenderBeginTag(HtmlTextWriterTag.A);
                            
                            writer.AddAttribute(HtmlTextWriterAttribute.Src, cmdField.InsertImageUrl);
                            writer.AddAttribute(HtmlTextWriterAttribute.Alt, cmdField.InsertText);
                            writer.RenderBeginTag(HtmlTextWriterTag.Img);
                            writer.RenderEndTag();

                            writer.RenderEndTag(); // A
                        }

                        if (cmdField.ShowSelectButton)
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, Page.ClientScript.GetPostBackEventReference(this, "Select"));
                            writer.RenderBeginTag(HtmlTextWriterTag.A);
                            
                            writer.AddAttribute(HtmlTextWriterAttribute.Src, cmdField.SelectImageUrl);
                            writer.AddAttribute(HtmlTextWriterAttribute.Alt, cmdField.SelectText);
                            writer.RenderBeginTag(HtmlTextWriterTag.Img);
                            writer.RenderEndTag();

                            writer.RenderEndTag(); // A
                        }

                        if (cmdField.ShowCancelButton)
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, Page.ClientScript.GetPostBackEventReference(this, "Cancel"));
                            writer.RenderBeginTag(HtmlTextWriterTag.A);
                            
                            writer.AddAttribute(HtmlTextWriterAttribute.Src, cmdField.CancelImageUrl);
                            writer.AddAttribute(HtmlTextWriterAttribute.Alt, cmdField.CancelText);
                            writer.RenderBeginTag(HtmlTextWriterTag.Img);
                            writer.RenderEndTag();

                            writer.RenderEndTag(); // A
                        }
                    }

                    writer.RenderEndTag(); // Td
                }
            }
            
            writer.RenderEndTag();  // Tr
        }
    }
}
