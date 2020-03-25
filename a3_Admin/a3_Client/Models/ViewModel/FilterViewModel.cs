using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace a3_Client.Models.ViewModels
{
    public class FilterViewModel
    {
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public string Keywords { get; set; }
    }
}
