using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Converters.Abstract
{
    interface IResourceParser
    {
        ListItemCollection GetAll();
    }
}
