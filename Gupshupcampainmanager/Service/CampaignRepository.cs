using Dapper;
using Gupshupcampainmanager.Models;
using Gupshupcampainmanager.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Gupshupcampainmanager.Service
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly IGenericRepository<CampaignDetails> _repository;
        public CampaignRepository(IGenericRepository<CampaignDetails> repository)
        {
            _repository = repository;
        }
        public async Task<CampaignDetailsResponse> InsertCampainDetails(CampaignDetailsRequest model)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("Desciption", model.Desciption);
                parameters.Add("ImagePath", model.ImagePath);
                parameters.Add("Id", model.Id);

                return await _repository.GetByValuesAsync<CampaignDetailsResponse>("sp_Insert_CampainDetails", parameters);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<IEnumerable<CampaignDetails>> GetCampainDetails()
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                return await _repository.GetListByValuesAsync<CampaignDetails>("sp_Get_CampaignDetails", parameters);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<CampaignDetailsResponse> GetCampainDetailsById(int Id)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("Id", Id);

                return await _repository.GetByValuesAsync<CampaignDetailsResponse>("sp_Get_CampaignDetails_ById", parameters);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<bool> DeletCampainDetailsById(int Id)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("Id", Id);

                 await _repository.DeleteAsync("sp_Delete_CampaignDetails_ById", parameters);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<bool> DeActiveCampaign(int Id , bool IsActive)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("Id", Id);
                parameters.Add("IsActive", IsActive);

                await _repository.DeleteAsync("Sp_Change_Campaign_Status", parameters);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


    }
}
