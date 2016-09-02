using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CamlexNET;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Parsers
{
    class ResourceParser : BaseParser
    {
        public ListItemCollection GetAll()
        {
            var context = new ClientContext(Constants.BaseAddress);
            var resourcesList = context.Web.Lists.GetByTitle(ListNames.Resources);

            string query = string.Empty;
            var qry = new CamlQuery();
            qry.ViewXml = "<View Scope=\"RecursiveAll\">" + query + "</View>";


            ListItemCollection items = resourcesList.GetItems(qry);

            context.Load(items);
            context.ExecuteQuery();

            return items;
        }
    }
}
