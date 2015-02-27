using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Esd.FlexibleOpenGeographies.Web.Services.Controllers
{
    public class BaseController : ApiController
    {
        protected void GetAreaAndAreaType(ref string area, ref string areaType)
        {
            if (!string.IsNullOrEmpty(area))
            {
                if (area.Contains(":"))
                {
                    var parts = area.Split(':');
                    area = parts[0];
                    areaType = parts[1];
                }
            }
        }
	}
}