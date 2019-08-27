using Bilbao.Web.Resources;
using System;
using System.ComponentModel;
using System.Web.UI;

namespace Bilbao.Web.UI.WebControls
{

    [
        ParseChildren(true),
        PersistChildren(false),
        Themeable(true),
        SupportsEventValidation
    ]
    public class Paginator : Control, INamingContainer, IPostBackEventHandler
    {
        #region Fields

        private int _currentPageNumber = 0;
        private int _displayedPages = 9;
        private int _pageSize = 50;
        private int _totalPageCount = 0;
        private long _totalRowCount = 0;

        private string _firstText = string.Empty;
        private string _lastText = string.Empty;
        private string _nextText = string.Empty;
        private string _previousText = string.Empty;

        private bool _showFirstLastButtons = false;

        #endregion

        #region Events

        /// <summary>
        ///   Evento que se produce cuando se cambia el número de página.
        /// </summary>
        public event EventHandler<PageChangedEventArgs> PageChanged;

        #endregion

        #region Properties

        /// <summary>
        ///   Obtiene o establece el número de la página actual.
        /// </summary>
        /// <value>
        ///   Número de la página actual.
        /// </value>
        /// <remarks>
        ///   <para>
        ///     El valor será <c>0</c> si <see cref="TotalRowCount"/> es <c>0</c>. 
        ///   </para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <para>
        ///     Cuando el valor es menor que <c>1</c> o mayor que <see cref="TotalPageCount"/>. 
        ///   </para>
        /// </exception>
        public int CurrentPageNumber
        {
            get { return _currentPageNumber; }
            set
            {
                if ((value < 1) || (value > _totalPageCount))
                    throw new ArgumentOutOfRangeException("value");
                
                _currentPageNumber = value;
            }
        }

        /// <summary>
        ///   Obtiene o establece el número de páginas que se mostrarán en el paginador.
        /// </summary>
        /// <value>
        ///   Número de páginas que se mostraran en el paginador.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <para>
        ///     Cuando el valor es menor que <c>1</c>.
        ///   </para>
        /// </exception>
        [DefaultValue(9)]
        public int DisplayedPages
        {
            get { return _displayedPages; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value");

                _displayedPages = value;
            }
        }

        /// <summary>
        ///   Obtiene o establece el texto que se mostrará en el botón <b>First</b>.
        /// </summary>
        /// <value>
        ///   Texto del botón <b>First</b>.
        /// </value>
        /// <remarks>
        ///   <para>
        ///     Si se deja vacío por defecto se mostrará <b>&raquo;</b>.
        ///   </para>
        /// </remarks>
        [DefaultValue("")]
        public string FirstText
        {
            get { return _firstText; }
            set
            {
                _firstText = (value != null) ? value.Trim() : string.Empty;
            }
        }

        /// <summary>
        ///   Obtiene o establece el texto que se mostrará en el botón <b>First</b>.
        /// </summary>
        /// <value>
        ///   Texto del botón <b>First</b>.
        /// </value>
        /// <remarks>
        ///   <para>
        ///     Si se deja vacío por defecto se mostrará <b>&raquo;</b>.
        ///   </para>
        /// </remarks>
        [DefaultValue("")]
        public string LastText
        {
            get { return _lastText; }
            set
            {
                _lastText = (value != null) ? value.Trim() : string.Empty;
            }
        }

        /// <summary>
        ///   Obtiene o establece el texto que se mostrará en el botón <b>Siguiente</b>.
        /// </summary>
        /// <value>
        ///   Texto del botón <b>Siguiente</b>.
        /// </value>
        /// <remarks>
        ///   <para>
        ///     Si se deja vacío por defecto se mostrará <b>&raquo;</b>.
        ///   </para>
        /// </remarks>
        [DefaultValue("")]
        public string NextText
        {
            get { return _nextText; }
            set
            {
                _nextText = (value != null) ? value.Trim() : string.Empty;
            }
        }

        /// <summary>
        ///   Obtiene o establece el número de filas (o registros) que se mostrarán por página.
        /// </summary>
        /// <value>
        ///   Número de filas (o registros) que se mostrarán por página.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <para>
        ///     Cuando el valor es menor que <c>1</c>.
        ///   </para>
        /// </exception>
        [DefaultValue(50)]
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value");

                if (_pageSize == value)
                    return;

                _pageSize = value;

                _totalPageCount = (int)Math.Ceiling(_totalRowCount / (double)value);

                if (_currentPageNumber > _totalPageCount)
                    _currentPageNumber = _totalPageCount;
                else if (_currentPageNumber < 1)
                    _currentPageNumber = 1;
            }
        }

        /// <summary>
        ///   Obtiene o establece el texto que se mostrará en el botón <b>Anterior</b>.
        /// </summary>
        /// <value>
        ///   Texto del botón <b>Anterior</b>.
        /// </value>
        /// <remarks>
        ///   <para>
        ///     Si se deja vacío por defecto se mostrará <b>&laquo;</b>.
        ///   </para>
        /// </remarks>
        [DefaultValue("")]
        public string PreviousText
        {
            get { return _previousText; }
            set
            {
                _previousText = (value != null) ? value.Trim() : string.Empty;
            }
        }

        /// <summary>
        ///   Obtiene o establece un valor que indica si se pintarán los botones de ir a la página
        ///   inicial y a la página final.
        /// </summary>
        [DefaultValue(false)]
        public bool ShowFirstLastButtons
        {
            get { return _showFirstLastButtons; }
            set { _showFirstLastButtons = value; }
        }

        /// <summary>
        ///   Obtiene el número total de páginas del paginador.
        /// </summary>
        /// <value>
        ///   Número total de páginas.
        /// </value>
        public int TotalPageCount
        {
            get { return _totalPageCount; }
        }

        /// <summary>
        ///   Obtiene o establece el número total de filas (o registros) que contiene el paginador.
        /// </summary>
        /// <value>
        ///   Número total de filas (o registros) que contiene el paginador.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <para>
        ///     Cuando el valor es menor que <c>0</c>.
        ///   </para>
        /// </exception>
        [DefaultValue(0)]
        public long TotalRowCount
        {
            get { return _totalRowCount; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");

                if (_totalRowCount == value)
                    return;

                _totalRowCount = value;

                if (value == 0)
                {
                    _currentPageNumber = 0;
                    _totalPageCount = 0;
                }
                else
                {
                    _totalPageCount = (int)Math.Ceiling(value / (double)_pageSize);

                    if (_currentPageNumber > _totalPageCount)
                        _currentPageNumber = _totalPageCount;
                    else if (_currentPageNumber < 1)
                        _currentPageNumber = 1;
                }
            }
        }

        #endregion

        #region Méthods

        protected override void LoadControlState(object savedState)
        {
            object[] allStates = savedState as object[];

            if (allStates == null)
            {
                base.LoadControlState(savedState);
                return;
            }
            
            // La base
            base.LoadControlState(allStates[0]);

            if (allStates[1] != null)
                _totalRowCount = (long)allStates[1];

            if (allStates[2] != null)
                _pageSize = (int)allStates[2];

            if (allStates[3] != null)
                _currentPageNumber = (int)allStates[3];

            if (allStates[4] != null)
                _showFirstLastButtons = (bool)allStates[4];

            if (allStates[5] != null)
                _previousText = (string)allStates[5];

            if (allStates[6] != null)
                _nextText = (string)allStates[6];

            // Calculamos el número de páginas
            _totalPageCount = (int)Math.Ceiling(_totalRowCount / (double)_pageSize);

            if (_currentPageNumber > _totalPageCount)
                _currentPageNumber = _totalPageCount;
            else if (_currentPageNumber < 1)
                _currentPageNumber = 1;
        }

        /// <summary>
        ///   Restaura información de estado de control de una solicitud de página anterior guardada por 
        ///   el método <see cref="Control.SaveControlState"/>.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            if (Page != null)
                Page.RegisterRequiresControlState(this);

            base.OnInit(e);
        }

        /// <summary>
        ///   Genera el evento <see cref="PageChanged"/>.
        /// </summary>
        /// <param name="e">
        ///   Objeto <see cref="PageChangedEventArgs"/> que contiene los datos del evento.
        /// </param>
        protected void OnPageChanged(PageChangedEventArgs e)
        {
            EventHandler<PageChangedEventArgs> handler = PageChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        
        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            RaisePostBackEvent(eventArgument);
        }

        protected virtual void RaisePostBackEvent(string eventArgument)
        {
            int pageNumber;

#if NET35
            if ((eventArgument == null) || (eventArgument.Trim().Length == 0) || (!int.TryParse(eventArgument, out pageNumber)))
#else
            if ((string.IsNullOrWhiteSpace(eventArgument)) || (!int.TryParse(eventArgument, out pageNumber)))
#endif

                throw new ArgumentException("eventArgument");

            // If the current page has not changed, there is nothing to do.
            if (pageNumber == _currentPageNumber)
                return;

            // Create the event arguments.
            PageChangedEventArgs eventArgs = new PageChangedEventArgs(pageNumber, _totalRowCount);

            // Raise event.
            OnPageChanged(eventArgs);

            // Ha cambiado el numero de registros?
            if (eventArgs.TotalRowCount != this.TotalRowCount)
            {
                _totalRowCount = eventArgs.TotalRowCount;
            }

            // Establecer la página actual.
            if (_currentPageNumber != eventArgs.CurrentPageNumber)
                _currentPageNumber = eventArgs.CurrentPageNumber;

            // Calculamos el número de páginas
            _totalPageCount = (int)Math.Ceiling(_totalRowCount / (double)_pageSize);

            if (_currentPageNumber > _totalPageCount)
                _currentPageNumber = _totalPageCount;
            else if (_currentPageNumber < 1)
                _currentPageNumber = 1;
        }

        protected override object SaveControlState()
        {
            object baseState = base.SaveControlState();

            object[] allStates = new object[7];

            allStates[0]= baseState;
            allStates[1] = _totalRowCount;
            allStates[2] = _pageSize;
            allStates[3] = _currentPageNumber;
            allStates[4] = _showFirstLastButtons;

            // Finales
            allStates[5] = _previousText;
            allStates[6] = _nextText;

            return allStates;
        }
        #endregion

        protected override void Render(HtmlTextWriter writer)
        {
            // Para poder visualizar el control.
            if (DesignMode)
            {
                TotalRowCount = 5000;
                CurrentPageNumber = 1;
            }

            RenderBeginTag(writer);

            RenderContents(writer);

            RenderEndTag(writer);
        }

        protected virtual void RenderBeginTag(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "pagination");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);
        }

        protected virtual void RenderEndTag(HtmlTextWriter writer)
        {
            writer.RenderEndTag();
        }

        protected virtual void RenderContents(HtmlTextWriter writer)
        {
            if (ShowFirstLastButtons)
                RenderFirstButton(writer);

            RenderPreviousButton(writer);

            RenderPageLinks(writer);

            RenderNextButton(writer);

            if (ShowFirstLastButtons)
                RenderLastButton(writer);
        }

        private void RenderFirstButton(HtmlTextWriter writer)
        {
            if (_currentPageNumber > 1)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-item");
                writer.RenderBeginTag(HtmlTextWriterTag.Li);

                writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-link");

                // PostBack
                if ((Page != null) && (Page.ClientScript != null))
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, Page.ClientScript.GetPostBackEventReference(this, "1"));
                
                writer.AddAttribute("aria-label", WebResources.Paginator_First);
                
                writer.RenderBeginTag(HtmlTextWriterTag.A);
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-item disabled");
                writer.RenderBeginTag(HtmlTextWriterTag.Li);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-link");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
            }

            if (!string.IsNullOrEmpty(FirstText))
                writer.Write(FirstText);
            else
                writer.Write("&laquo;");

            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        private void RenderLastButton(HtmlTextWriter writer)
        {
            if (_currentPageNumber < _totalPageCount)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-item");
                writer.RenderBeginTag(HtmlTextWriterTag.Li);

                writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-link");

                // PostBack
                if ((Page != null) && (Page.ClientScript != null))
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, Page.ClientScript.GetPostBackEventReference(this, _totalPageCount.ToString()));

                writer.AddAttribute("aria-label", WebResources.Paginator_Last);

                writer.RenderBeginTag(HtmlTextWriterTag.A);
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-item disabled");
                writer.RenderBeginTag(HtmlTextWriterTag.Li);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-link");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
            }

            if (!string.IsNullOrEmpty(LastText))
                writer.Write(LastText);
            else
                writer.Write("&raquo;");

            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected virtual void RenderNextButton(HtmlTextWriter writer)
        {
           

            if ((_currentPageNumber + 1) <= _totalPageCount)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-item");
                writer.RenderBeginTag(HtmlTextWriterTag.Li);

                writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-link");

                // PostBack
                if ((Page != null) && (Page.ClientScript != null))
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, Page.ClientScript.GetPostBackEventReference(this, (_currentPageNumber + 1).ToString()));

                writer.AddAttribute("aria-label", WebResources.Paginator_Next);

                writer.RenderBeginTag(HtmlTextWriterTag.A);
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-item disabled");
                writer.RenderBeginTag(HtmlTextWriterTag.Li);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-link");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
            }

            if (!string.IsNullOrEmpty(NextText))
                writer.Write(NextText);
            else if (ShowFirstLastButtons)
                writer.Write("&rsaquo;");
            else
                writer.Write("&raquo;");

            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected virtual void RenderPreviousButton(HtmlTextWriter writer)
        {
            if ((_currentPageNumber - 1) > 0)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-item");
                writer.RenderBeginTag(HtmlTextWriterTag.Li);

                writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-link");

                // PostBack
                if ((Page != null) && (Page.ClientScript != null))
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, Page.ClientScript.GetPostBackEventReference(this, (_currentPageNumber - 1).ToString()));

                writer.AddAttribute("aria-label", WebResources.Paginator_Previous);

                writer.RenderBeginTag(HtmlTextWriterTag.A);
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-item disabled");
                writer.RenderBeginTag(HtmlTextWriterTag.Li);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-link");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
            }

            if (!string.IsNullOrEmpty(PreviousText))
                writer.Write(PreviousText);
            else if (ShowFirstLastButtons)
                writer.Write("&lsaquo;");
            else
                writer.Write("&laquo;");

            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected virtual void RenderPageLinks(HtmlTextWriter writer)
        {
            int startPage = _currentPageNumber - (int)Math.Floor(_displayedPages / 2m);
            if (startPage < 1)
                startPage = 1;

            int endPage = startPage + (_displayedPages - 1);
            if (endPage > _totalPageCount)
                endPage = _totalPageCount;

            if (((endPage - startPage) < _displayedPages) && (startPage > 1))
            {
                startPage = endPage - (_displayedPages - 1);
                if (startPage < 1)
                    startPage = 1;
            }

            for (int pageNumber = startPage; pageNumber <= endPage; pageNumber++)
            {
                if (pageNumber != _currentPageNumber)
                {
                    // Pintamos la página
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-item");
                    writer.RenderBeginTag(HtmlTextWriterTag.Li);
                    
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-link");

                    // PostBack
                    if ((Page != null) && (Page.ClientScript != null))
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, Page.ClientScript.GetPostBackEventReference(this, pageNumber.ToString()));

                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                }
                else
                {
                    // Pintamos la página activa.
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-item active");
                    writer.RenderBeginTag(HtmlTextWriterTag.Li);

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "page-link");
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                }

                writer.Write(pageNumber.ToString());

                writer.RenderEndTag(); // A or Span

                writer.RenderEndTag(); // Li
            }
        }
    }
}
