using CamlexNET.Interfaces;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Parsers
{
    public abstract class BaseParser
    {
        protected ClientContext context = new ClientContext(Constants.BaseAddress);

        protected CamlQuery CreateCamlQuery(string queryConditions)
        {
            return new CamlQuery()
            {
                ViewXml = "<View Scope=\"RecursiveAll\">" + queryConditions + "</View>"
            };
        }

        protected ListItemCollection LoadCollectionFromServer(ListItemCollection collection)
        {
            context.Load(collection);
            context.ExecuteQuery();
            return collection;
        }
    }
}