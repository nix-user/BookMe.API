using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.Interfaces.SharePoint;
using BookMe.BusinessLogic.OperationResult;
using BookMe.BusinessLogic.Repository;
using BookMe.BusinessLogic.Services.Abstract;
using BookMe.Core.Models;

namespace BookMe.BusinessLogic.Services.Concrete
{
    public class ResourceService : IResourceService
    {
        private readonly IRepository<Resource> resourceRepository;
        private readonly ISharePointResourceService sharePointResourceService;

        public ResourceService(IRepository<Resource> resourceRepository, ISharePointResourceService sharePointResourceService)
        {
            this.resourceRepository = resourceRepository;
            this.sharePointResourceService = sharePointResourceService;
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

        public OperationResult<IEnumerable<ResourceDTO>> GetAvailableResources(ResourceFilterParameters resourceFilterParameters)
        {
            var allResourcesRetrieval = this.GetAll();
            if (!allResourcesRetrieval.IsSuccessful)
            {
                return new OperationResult<IEnumerable<ResourceDTO>>() { IsSuccessful = false };
            }

            var operationResult = this.sharePointResourceService.GetAvailableResources(resourceFilterParameters, allResourcesRetrieval.Result);
            return new OperationResult<IEnumerable<ResourceDTO>>()
            {
                IsSuccessful = operationResult.IsSuccessful,
                Result = operationResult.Result
            };
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