using Esd.FlexibleOpenGeographies.Web.Models;
using System;
using System.Configuration;
using System.Web.Mvc;
using VDS.RDF.Query;

namespace Esd.FlexibleOpenGeographies.Web.Controllers
{
    public class SPARQLController : BaseController
    {
        private const string SPARQLEndPoint = "SPARQLEndPoint";

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult Query()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Query(FormCollection formCollection)
        {
            ViewBag.EndPoint = ConfigurationManager.AppSettings[SPARQLEndPoint];
            string query = formCollection["Query"];
            ViewBag.DefaultQuery = query;
            try
            {
                SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri(ConfigurationManager.AppSettings[SPARQLEndPoint]));
                SparqlResultSet rset = endpoint.QueryWithResultSet(query);
                endpoint.Timeout = 20000;
                return View("SPARQL", new SPARQLModel(rset.Results)); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            return View("SPARQL", new SPARQLModel()); 
        }

        public ActionResult Index()
        {
            ViewBag.DefaultQuery = @"SELECT ?id ?title
WHERE
{
  ?s a <http://fog.id.esd.org.uk/AdministrativeWard> .
  ?s <http://www.w3.org/2004/02/skos/core#prefLabel> ?title .
  ?s <http://purl.org/dc/terms/identifier> ?id
}";
            ViewBag.EndPoint = ConfigurationManager.AppSettings[SPARQLEndPoint];
            return View("SPARQL", new SPARQLModel());            
        }
    }
}
