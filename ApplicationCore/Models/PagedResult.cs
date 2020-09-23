using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Models
{
    public class PagedResult<T>
    {
        public IList<T> Items { get; set; }
        public int CurrentPage { get; set; }
        public int PagesCount { get; set; }
        public int ResultsPerPage { get; set; }
    }
}
