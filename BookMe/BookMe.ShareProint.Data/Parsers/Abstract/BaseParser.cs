using System.Net;
using BookMe.Auth.Providers.Abstract;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Parsers.Abstract
{
    public abstract class BaseParser
    {
        protected const string RetrivalErrorMessage = "Could not retrieve requested items";
        protected const string NixDomain = "nix";
        protected const string AddingErrorMessage = "Could not add item to collection";

        private readonly ICredentialsProvider credentialsProvider;

        protected BaseParser(ClientContext clientContext, ICredentialsProvider credentialsProvider)
        {
            this.Context = clientContext;
            this.credentialsProvider = credentialsProvider;

#if !DEBUG
            var credentials = credentialsProvider.Credentials;
            this.Context.Credentials = new NetworkCredential(credentials?.UserName, credentials?.Password, NixDomain);
#endif
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