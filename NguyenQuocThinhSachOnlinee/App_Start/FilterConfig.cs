﻿using System.Web.Mvc;

namespace NguyenQuocThinhSachOnlinee
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
