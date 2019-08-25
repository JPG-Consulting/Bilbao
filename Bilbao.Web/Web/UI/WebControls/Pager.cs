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
    //[ToolboxData("<{0}:Pager runat=server></{0}:Pager>")]
    //[ToolboxItem(true)]
    [
        ParseChildren(true),
        PersistChildren(false),
        Themeable(true),
        SupportsEventValidation
    ]
    public class Pager : Control, INamingContainer, IPostBackEventHandler
    {
        private int _pageSize = 10;
        private int _pageIndex = 0;
        private long _totalRowCount = 0;
        private int _displayedPages = 9;

        private bool _usePostBack = true;

        private string _onClientClick = string.Empty;

        // Texts
        private string _previousText = "«";
        private string _nextText = "»";
        private string _firstText = "First";
        private string _lastText = "Last";

        // Visibility
        private bool _renderFirstLastButtons = true;

        private string _cssClass = "pagination";

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

        public string OnClientClient
        {
            get { return (_onClientClick != null) ? _onClientClick.Trim() : string.Empty; }
            set
            {
                if (value == null)
                    _onClientClick = string.Empty;
                else
                    _onClientClick = value.Trim();
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
        
        [DefaultValue(true)]
        public bool RenderFirstLastButtons
        {
            get
            {
                return _renderFirstLastButtons;
            }

            set
            {
                _renderFirstLastButtons = value;
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // We can't try to find another control in the designer in Init.
            if (!DesignMode)
            {

                if (Page != null)
                {
                    Page.RegisterRequiresControlState(this);
                }
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
                    _totalRowCount = (long)state[3];
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

                _pageIndex = 1;
                //_pageIndex = 500000;
                _totalRowCount = 5000000;
            }

            RenderBeginTag(writer);

            RenderContents(writer);

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
            if (ShouldRenderFirstLastButtons())
                RenderFirstButton(writer);

            // Previous button
            if (this.TotalPageCount > 1)
                RenderPreviousButton(writer);

            // Páginas
            int startPage = (CurrentPage - ((int)Math.Floor(_displayedPages / 2.0m)));
            if (startPage < 1)
                startPage = 1;
            int endPage = startPage + (_displayedPages -1);
            if (endPage > this.TotalPageCount)
                endPage = this.TotalPageCount;

            if ((endPage - startPage) < _displayedPages)
            {
                startPage = endPage - (_displayedPages - 1);
                if (startPage < 1)
                    startPage = 1;
            }

            for (int currentPage = startPage; currentPage <= endPage; currentPage++)
            {
                RenderPageItem(writer, currentPage, currentPage.ToString(), true);
            }

            // Next
            if (this.TotalPageCount > 1)
                RenderNextButton(writer);

            // Last
            if (ShouldRenderFirstLastButtons())
                RenderLastButton(writer);
        }

        protected virtual void RenderEndTag(HtmlTextWriter writer)
        {
            writer.RenderEndTag();
        }

        private void RenderFirstButton(HtmlTextWriter writer)
        {
            RenderButton(writer, 1, _firstText, (CurrentPage > 1), "First");
        }

        private void RenderLastButton(HtmlTextWriter writer)
        {
            RenderButton(writer, this.TotalPageCount, _lastText, (CurrentPage < this.TotalPageCount), "Last");
        }

        private void RenderNextButton(HtmlTextWriter writer)
        {
            RenderButton(writer, CurrentPage + 1, _nextText, (CurrentPage < this.TotalPageCount), "Next");
        }

        private void RenderPreviousButton(HtmlTextWriter writer)
        {
            RenderButton(writer, CurrentPage - 1, _previousText, ((CurrentPage -1) > 0), "Previous");
        }

        private void RenderButton(HtmlTextWriter writer, int pageNumber, string text, bool enabled, string ariaLabel)
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
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-link");

                if (_usePostBack && (String.IsNullOrEmpty(OnClientClient)))
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, Page.ClientScript.GetPostBackEventReference(this, pageNumber.ToString()));
                else if (_usePostBack)
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, OnClientClient + (!OnClientClient.EndsWith(";") ? ";" : "") + Page.ClientScript.GetPostBackEventReference(this, pageNumber.ToString()));
                else if (!String.IsNullOrEmpty(OnClientClient))
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, OnClientClient);

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

            RenderPageLink(writer, pageNumber, text, (isActive || (!enabled)));

            writer.RenderEndTag(); // Li
        }

        private void RenderPageLink(HtmlTextWriter writer, int pageNumber, string text, bool disabledOrActive)
        {

            if (disabledOrActive)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-link");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-link");

                if (_usePostBack && (String.IsNullOrEmpty(OnClientClient)))
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, Page.ClientScript.GetPostBackEventReference(this, pageNumber.ToString()));
                else if (_usePostBack)
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, OnClientClient + (!OnClientClient.EndsWith(";") ? ";" : "") +  Page.ClientScript.GetPostBackEventReference(this, pageNumber.ToString()));
                else if (!String.IsNullOrEmpty(OnClientClient))
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, OnClientClient);

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

            object[] state = new object[4];

            state[0] = baseState;
            state[1] = (_pageIndex == 0) ? null : (object)_pageIndex;
            state[2] = (_pageSize == 10) ? null : (object)_pageSize;
            state[3] = (_totalRowCount < 0) ? null : (object)_totalRowCount;

            return state;
        }

        private bool ShouldRenderFirstLastButtons()
        {
            if (!_renderFirstLastButtons)
                return false;
            else if (this.TotalPageCount <= _displayedPages)
                return false;
            else
                return true;
        }

        public void RaisePostBackEvent(string eventArgument)
        {
#if NET35
            if ((eventArgument == null) || (eventArgument.Trim().Length == 0))
#else
            if (String.IsNullOrWhiteSpace(eventArgument))
#endif
                return;


            int currentPage;

            if ((int.TryParse(eventArgument, out currentPage)) && (currentPage > 0))
            {
                this.CurrentPage = currentPage;
            }
        }
    }
}
