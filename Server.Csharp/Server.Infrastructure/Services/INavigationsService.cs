using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Server.Data.Entities;
using Server.Data.Repositories;
using Server.Infrastructure.Models.Requests;
using Server.Infrastructure.Models.Responses;

namespace Server.Infrastructure.Services
{
    public interface INavigationsService
    {
        public Task<NavigationResponse> CreateAsync(CreateNavigationRequest request);
    }

    public class NavigationsService : INavigationsService
    {
        private readonly INavigationsRepository _navigationRepository;
        private readonly IMapper _mapper;

        public NavigationsService(INavigationsRepository navigationRepository, IMapper mapper)
        {
            _navigationRepository = navigationRepository;
            _mapper = mapper;
        }

        public async Task<NavigationResponse> CreateAsync(CreateNavigationRequest request)
        {
            Navigation navigationToCreate = _mapper.Map<Navigation>(request);

            Navigation createdNavigation = await _navigationRepository.CreateAsync(navigationToCreate);
            
            return _mapper.Map<NavigationResponse>(createdNavigation);
        }
    }

}
