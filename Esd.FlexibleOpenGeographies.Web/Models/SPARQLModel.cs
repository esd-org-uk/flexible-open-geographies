using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VDS.RDF;
using VDS.RDF.Query;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    public class SPARQLModel
    {
        public List<string[]> Rows = new List<string[]>();

        public SPARQLModel()
        {
        }

        public SPARQLModel(List<SparqlResult> results)
        {
            if (results.Count == 0)
            {
                return;
            }
            List<string> row = new List<string>();
            foreach (string variable in results[0].Variables)
            {
                row.Add(variable);
            }
            Rows.Add(row.ToArray());
            foreach (SparqlResult result in results)
            {
                row = new List<string>();
                foreach(string variable in result.Variables)
                {
                    INode iNode = result.Value(variable);
                    if (iNode is UriNode)
                    {
                        UriNode node = (UriNode)iNode;
                        if (node != null)
                        {
                            row.Add(node.Uri.ToString());
                        }
                    }
                    else
                    {
                        LiteralNode node = (LiteralNode)iNode;
                        if (node != null)
                        {
                            row.Add(node.Value);
                        }
                    }
                }
                Rows.Add(row.ToArray());
                if (Rows.Count == 501)
                {
                    break;
                }
            }            
        }
    }
}
