using HMS.Common;
using JW;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace HMS.Models
{
    public class BaseResponseModel
    {
        public string Message { get; set; }
        public int DbCode { get; set; }
        public string DbMsg { get; set; }
        public bool IsSuccess { get; set; }
        public int RowNum { get; set; }
        public Pager Pager { get; set; }
        public int PageSizeId { get; set; }
        public string PageSizeValue { get; set; }
        public string SearchString { get; set; }
        public string sortField { get; set; }
        public string sortOrder { get; set; }

        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string ActionName { get; set; }
        public string Area { get; set; }

        public List<PageSizeDdl> lstPageSizeDdl { get; set; }
    }
}
