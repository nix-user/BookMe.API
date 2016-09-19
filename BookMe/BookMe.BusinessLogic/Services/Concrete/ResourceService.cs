using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.OperationResult;
using BookMe.BusinessLogic.Repository;
using BookMe.BusinessLogic.Services.Abstract;
using BookMe.Core.Models;

namespace BookMe.BusinessLogic.Services.Concrete
{
    public class ResourceService : IResourceService
    {
        private readonly IRepository<Resource> resourceRepository;

        public ResourceService(IRepository<Resource> resourceRepository)
        {
            this.resourceRepository = resourceRepository;
        }

        public OperationResult<IEnumerable<ResourceDTO>> GetAll()
        {
            try
            {
                var resources = this.resourceRepository.Entities;
                var resourcesDto = Mapper.Map<IEnumerable<ResourceDTO>>(resources);

                return new OperationResult<IEnumerable<ResourceDTO>>
                {
                    Result = resourcesDto,
                    IsSuccessful = true
                };
            }
            catch (Exception)
            {
                return new OperationResult<IEnumerable<ResourceDTO>> { IsSuccessful = false };
            }
        }

        public OperationResult.OperationResult AddResource(ResourceDTO resource)
        {
            try
            {
                var resourceEntity = Mapper.Map<Resource>(resource);
                this.resourceRepository.Insert(resourceEntity);
                this.resourceRepository.Save();
                return new OperationResult.OperationResult { IsSuccessful = true };
            }
            catch (Exception)
            {
                return new OperationResult.OperationResult { IsSuccessful = false };
            }
        }
    }
}