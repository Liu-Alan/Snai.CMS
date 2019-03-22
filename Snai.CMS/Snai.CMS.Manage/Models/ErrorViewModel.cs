using System;

namespace Snai.CMS.Manage.Models
{
    public class ErrorViewModel: LayoutModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}