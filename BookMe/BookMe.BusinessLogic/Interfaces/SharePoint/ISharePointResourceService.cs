using System.Collections.Generic;
using BookMe.BusinessLogic.DTO;

namespace BookMe.BusinessLogic.Interfaces.SharePoint
{
    public interface ISharePointResourceService
    {
        IEnumerable<ResourceDTO> GetAll();
    }
}