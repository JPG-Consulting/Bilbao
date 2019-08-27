using System;

namespace Bilbao.Web.UI.WebControls
{
    public class PageChangedEventArgs : EventArgs
    {
        private int _currentPageNumber = 0;
        private long _totalRowCount = 0;

        public int CurrentPageNumber
        {
            get { return _currentPageNumber; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value");

                _currentPageNumber = value;
            }
        }

        public long TotalRowCount
        {
            get { return _totalRowCount; }
            set
            {
                if (value < 0)
                    _totalRowCount = 0;
                else
                    _totalRowCount = value;
            }
        }
        
        internal PageChangedEventArgs(int currentPageNumber, long totalRowCount)
        {
            _currentPageNumber = currentPageNumber;
            _totalRowCount = totalRowCount;
        }
    }
}
