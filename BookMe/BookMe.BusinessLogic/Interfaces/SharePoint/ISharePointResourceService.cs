using System.Collections;
using System.Collections.Generic;
using BookMe.Core.Models;

namespace BookMe.BusinessLogic.Interfaces.SharePoint
{
    public interface ISharePointResourceService
    {
        IEnumerable<Resource> GetAll();
    }
}