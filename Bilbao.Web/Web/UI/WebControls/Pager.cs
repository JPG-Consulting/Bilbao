using System;
using System.ComponentModel;
using System.Web.UI;

namespace Bilbao.Web.UI.WebControls
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     Basado en 
    ///     <a href="https://docs.microsoft.com/es-es/dotnet/api/system.web.ui.webcontrols.datapager?view=netframework-4.8">DataPager</a>.
    ///   </para>
    /// </remarks>
    [
        ParseChildren(true),
        PersistChildren(false),
        Themeable(true),
        SupportsEventValidation
    ]
    public class Pager
#if NET35 
        : StandardControl,
#else
        : Control, 
#endif
        INamingContainer, System.Web.UI.WebControls.ICompositeControlDesignerAccessor
    {
        private int _pageSize = 10;
        private int _pageIndex = 0;
        private long _totalRowCount = 0;

        private string _previousText = "«";
        private string _nextText = "»";

        private string _cssClass = "paginator";

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     Ensure that the child controls have been created before returning the controls collection.
        ///   </para>
        /// </remarks>
        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }

        public int CurrentPage
        {
            get { return _pageIndex + 1; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("Value");

                this.PageIndex = value - 1;
            }
        }

        public string CssClass
        {
            get { return _cssClass; }
            set
            {
#if NET35
                if ((value == null) || (value.Trim().Length == 0))
#else
                if (String.IsNullOrWhiteSpace(value))
#endif
                    _cssClass = string.Empty;
                else
                    _cssClass = value.Trim();
            }
        }

        public int PageIndex
        {
            get { return _pageIndex; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");

                _pageIndex = value;
            }
        }

        [
            DefaultValue(10)
            //WebCategory("Paging"),
            //ResourceDescription("DataPager_PageSize"),
        ]
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value");

                if (value != _pageSize)
                {
                    _pageSize = value;
                }
            }
        }
        
        public long StartRowIndex
        {
            get { return (_pageIndex * _pageSize); }
        }

        public int TotalPageCount
        {
            get
            {
                if (_totalRowCount == 0)
                    return 0;

                return (int)Math.Ceiling((double)_totalRowCount / (double)_pageSize);
            }
        }

        public long TotalRowCount
        {
            get { return _totalRowCount; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");

                _totalRowCount = value;

                // Si el número de páginas es menor que CurrentPage establecemos el CurrentPage
                // al valor máximo de páginas.
                if (TotalPageCount < CurrentPage)
                    CurrentPage = TotalPageCount;
            }
        }

        protected override void LoadControlState(object savedState)
        {
            object[] state = savedState as object[];

            _pageSize = 10;
            _pageIndex = 0;
            _totalRowCount = 0;

            if (state != null)
            {
                base.LoadControlState(state[0]);

                if (state[1] != null)
                    _pageIndex = (int)state[1];

                if (state[2] != null)
                    _pageSize = (int)state[2];

                if (state[3] != null)
                    _totalRowCount = (long)state[2];
            }
            else
            {
                base.LoadControlState(savedState);
            }
        }

        protected override void LoadViewState(object savedState)
        {
            if (savedState == null)
                return;

            object[] state = savedState as object[];

            if (state != null)
            {
                base.LoadViewState(state[0]);

                //if (state[1] != null)
                //    ((IStateManager)Fields).LoadViewState(state[1]);
            }
            else
            {
                base.LoadViewState(savedState);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     Implementa el interfaz 
        ///     <see cref="System.Web.UI.WebControls.ICompositeControlDesignerAccessor"/>.
        ///   </para>
        /// </remarks>
        public void RecreateChildControls()
        {
            throw new NotImplementedException();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (DesignMode)
            {
                EnsureChildControls();

                _pageIndex = 0;
                _totalRowCount = long.MaxValue;
            }

            RenderBeginTag(writer);

            RenderEndTag(writer);    
        }

        protected virtual void RenderBeginTag(HtmlTextWriter writer)
        {
            if (!String.IsNullOrEmpty(this.CssClass))
                writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass);

            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);
        }

        protected virtual void RenderContents(HtmlTextWriter writer)
        {
            if (_totalRowCount < 1)
                return;

            // First
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-item");
            writer.RenderBeginTag(HtmlTextWriterTag.Li);

            if (_pageIndex > 0)
            {
            }
            else
            {
            }

            writer.RenderEndTag();

            // Previous button
            RenderPreviousButton(writer);

            // Páginas
            for (int currentPage = 1; currentPage <= this.TotalPageCount; currentPage++)
            {
                RenderPageItem(writer, currentPage, currentPage.ToString(), true);
            }

            // Next
            RenderNextButton(writer);
            
            // Last
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-item");
            writer.RenderBeginTag(HtmlTextWriterTag.Li);

            if (CurrentPage < TotalPageCount)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                //writer.Write(_nextText);
                writer.RenderEndTag();
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "disabled");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                //writer.Write();
                writer.RenderEndTag();
            }

            writer.RenderEndTag();
        }

        protected virtual void RenderEndTag(HtmlTextWriter writer)
        {
            writer.RenderEndTag();
        }

        private void RenderNextButton(HtmlTextWriter writer)
        {
            RenderButton(writer, GetHref(CurrentPage + 1), _nextText, ((CurrentPage + 1)>= this.TotalPageCount), "Next");
        }

        private void RenderPreviousButton(HtmlTextWriter writer)
        {
            RenderButton(writer, GetHref(CurrentPage + 1), _previousText, ((CurrentPage -1) > 0), "Previous");
        }

        private void RenderButton(HtmlTextWriter writer, string href, string text, bool enabled, string ariaLabel)
        {
            if (!enabled)
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-item disabled");
            else
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-item");

            writer.RenderBeginTag(HtmlTextWriterTag.Li);

            // A tag - start
            if (!enabled)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-link");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Href, href);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-link");
                if (!String.IsNullOrEmpty(ariaLabel))
                    writer.AddAttribute("aria-label", ariaLabel);
                writer.RenderBeginTag(HtmlTextWriterTag.A);
            }
            
            writer.Write(text);
            writer.RenderEndTag(); // A or Span tag

            writer.RenderEndTag();
        }

        private void RenderPageItem(HtmlTextWriter writer, int pageNumber, string text, bool enabled)
        {
            bool isActive = (pageNumber == CurrentPage);

            if ((!enabled) && (!isActive))
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-item disabled");
            else if (isActive)
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-item active");
            else
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-item");

            writer.RenderBeginTag(HtmlTextWriterTag.Li);

            RenderPageLink(writer, "#", text, (isActive || (!enabled)));

            writer.RenderEndTag(); // Li
        }

        private void RenderPageLink(HtmlTextWriter writer, string href, string text, bool disabledOrActive)
        {

            if (disabledOrActive)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-link");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Href, href);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-link");
                writer.RenderBeginTag(HtmlTextWriterTag.A);
            }

            writer.Write(text);

            writer.RenderEndTag(); // A or Span tag.
        }

        private string GetHref(int pageNumber)
        {
            return "#";
        }

        protected override object SaveControlState()
        {
            object baseState = base.SaveControlState();

            if (baseState != null)
            {
                object[] state = new object[4];

                state[0] = baseState;
                state[1] = (_pageIndex == 0) ? null : (object)_pageIndex;
                state[2] = (_pageSize == 10) ? null : (object)_pageSize;
                state[3] = (_totalRowCount < 0) ? null : (object)_totalRowCount;

                return state;
            }

            return baseState;
        }
    }
}
