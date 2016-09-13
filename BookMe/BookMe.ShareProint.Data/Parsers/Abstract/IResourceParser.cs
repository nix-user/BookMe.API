using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Parsers.Abstract
{
    public interface IResourceParser
    {
        ListItemCollection GetAll();
    }
}