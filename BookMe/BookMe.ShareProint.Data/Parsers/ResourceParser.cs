using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.ShareProint.Data.Converters.Abstract;
using CamlexNET;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Parsers
{
    class ResourceParser : BaseParser, IResourceParser
    {
        public ListItemCollection GetAll()
        {
            var resourcesList = this.context.Web.Lists.GetByTitle(ListNames.Resources);

            ListItemCollection items = resourcesList.GetItems(this.CreateCamlQuery(string.Empty));

            return this.LoadCollectionFromServer(items);
        }
    }
}
