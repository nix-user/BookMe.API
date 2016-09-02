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
            var resourcesList = this.context.Web.Lists.GetByTitle(ListNames.Resources);

            ListItemCollection items = resourcesList.GetItems(this.CreateCamlQuery(string.Empty));

            return this.LoadCollectionFromServer(items);
        }
    }
}
