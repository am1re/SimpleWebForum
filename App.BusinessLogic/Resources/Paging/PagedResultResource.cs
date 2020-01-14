using App.Data.Repositories.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BusinessLogic.Resources.Paging
{
    public class PagedResultResource<T> : PagedResultResourceBase where T : class
    {
        public IEnumerable<T> Data { get; set; }

        public PagedResultResource()
        {

        }

        public PagedResultResource(PagedResultResourceBase source)
        {
            CurrentPage = source.CurrentPage;
            PageCount = source.PageCount;
            PageSize = source.PageSize;
            RowCount = source.RowCount;
        }

        public PagedResultResource(PagedResultBase source)
        {
            CurrentPage = source.CurrentPage;
            PageCount = source.PageCount;
            PageSize = source.PageSize;
            RowCount = source.RowCount;
        }
    }

    public class PagedResultResourceBase
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }

        public int FirstRowOnPage
        {
            get { return (CurrentPage - 1) * PageSize + 1; }
        }

        public int LastRowOnPage
        {
            get { return Math.Min(CurrentPage * PageSize, RowCount); }
        }
    }
}
