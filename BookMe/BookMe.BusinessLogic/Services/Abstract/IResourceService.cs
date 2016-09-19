using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.OperationResult;

namespace BookMe.BusinessLogic.Services.Abstract
{
    public interface IResourceService
    {
        OperationResult<IEnumerable<ResourceDTO>> GetAll();

        OperationResult.OperationResult AddResource(ResourceDTO resource);
    }
}