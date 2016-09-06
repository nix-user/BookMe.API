using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Parsers.Abstract
{
    public abstract class BaseParser
    {
        protected const string RetrivalErrorMessage = "Could not retrieve requested items";

        protected BaseParser(ClientContext clientContext)
        {
            this.Context = clientContext;
        }

        public void CheckConnection()
        {
            this.Context.Load(this.Context.Web.Lists);
            this.Context.ExecuteQuery();
        }

        protected ClientContext Context { get; set; }

        protected CamlQuery CreateCamlQuery(string queryConditions)
        {
            return new CamlQuery()
            {
                ViewXml = "<View Scope=\"RecursiveAll\">" + queryConditions + "</View>"
            };
        }

        protected ListItemCollection LoadCollectionFromServer(ListItemCollection collection)
        {
            this.Context.Load(collection);
            this.Context.ExecuteQuery();
            return collection;
        }
    }
}