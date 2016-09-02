using Microsoft.SharePoint.Client;

namespace BookMe.ShareProint.Data.Converters.Abstract
{
    public interface IResourceParser
    {
        ListItemCollection GetAll();
    }
}
