using System;
using BookMe.Auth.Providers.Abstract;
using BookMe.ShareProint.Data.Parsers.Abstract;
using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Parsers.Concrete
{
    public class ResourceParser : BaseParser, IResourceParser
    {
        public ResourceParser(ClientContext context, ICredentialsProvider credentialsProvider) : base(context, credentialsProvider)
        {
        }

        public ListItemCollection GetAll()
        {
            try
            {
                var resourcesList = this.Context.Web.Lists.GetByTitle(ListNames.Resources);

                ListItemCollection items = resourcesList.GetItems(this.CreateCamlQuery(string.Empty));

                return this.LoadCollectionFromServer(items);
            }
            catch
            {
                throw new ParserException(RetrivalErrorMessage);
            }
        }
    }
}