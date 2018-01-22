using SPList.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SPList.Controllers
{
    [Authorize]
    public class DetailsController : ApiController
    {
        [HttpGet]
        public async Task<IEnumerable<SPoAuth2Perm>> GetPerms(string id)
        {
            var res = await ServicePrincipal.GetOAuth2Permissions(id);
            return res;
        }
    }
}
