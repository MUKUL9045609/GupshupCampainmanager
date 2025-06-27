using Dapper;
using Gupshupcampaignmanager.Models.Common;
using Gupshupcampainmanager.Models;
using Gupshupcampainmanager.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;

namespace Gupshupcampainmanager.Service
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly IGenericRepository<CampaignDetails> _repository;
        private static readonly Logger _Nlogger = LogManager.GetCurrentClassLogger();
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

        public async Task<IEnumerable<CampaignMessageDetail>> GetCampaignMessageDetail()
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                return await _repository.GetListByValuesAsync<CampaignMessageDetail>("sp_Get_CampainMessageDetails", parameters);
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

                return await _repository.GetByValuesAsync<CampaignDetailsResponse>("Sp_Get_CampaignDetails_ById", parameters);
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

                await _repository.DeleteAsync("Sp_Delete_CampaignDetails_ById", parameters);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<int> InsertCustomerAsync(CustomerViewModel request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Id", request.Id);
            parameters.Add("@CustomerName", request.CustomerName);
            parameters.Add("@MobileNo", request.MobileNo);
            //parameters.Add("@address", request.Address);

            return await _repository.InsertUpdateAsync("[sp_Insert_Update_Customerdetails]", parameters);

        }

        public async Task<IEnumerable<CustomerViewModel>> ContextListAsync(CustomerReqeust request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CustomerName", request.CustomerName);
            parameters.Add("@MobileNo", request.MobileNo);
            parameters.Add("@StartDate", request.StartDate);
            parameters.Add("@EndDate", request.EndDate);
            parameters.Add("@pageNumber", request.PageNumber);
            parameters.Add("@pageSize", request.PageSize);
            parameters.Add("@orderBy", request.orderBy);
            parameters.Add("@orderDirection", request.orderDirection);

            return await _repository.GetListByValuesAsync<CustomerViewModel>("[sp_GetContextList]", parameters);
        }
        public async Task<int> ContextListCountAsync(CustomerReqeust request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CustomerName", request.CustomerName);
            parameters.Add("@MobileNo", request.MobileNo);
            parameters.Add("@StartDate", request.StartDate);
            parameters.Add("@EndDate", request.EndDate);

            return await _repository.GetByValuesAsync<int>("[sp_GetContextListCount]", parameters);
        }
        public async Task<CampaignDetailsResponse> ActiveCampaign(bool IsActive)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("IsActive", IsActive);

                return await _repository.GetByValuesAsync<CampaignDetailsResponse>("sp_Get_ActiveCampaignDetails", parameters);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<bool> InsertOrUpdateSmsStatusAsync(SmsStatusRequest model)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("Type", model.Type);
                parameters.Add("Status", model.Status);
                parameters.Add("MessageId", model.MessageId);
                parameters.Add("PhoneNumber", model.PhoneNumber);
                parameters.Add("Timestamp", model.Timestamp);
                parameters.Add("FailureReason", model.FailureReason);
                parameters.Add("RawJson", model.RawJson);
                parameters.Add("Name", model.Name);
                parameters.Add("Timestamp", model.Timestamp < (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue ? null : model.Timestamp);


                return await _repository.InsertMultipleAsync("sp_InsertOrUpdateSmsStatusLog", parameters);
            }
            catch (Exception ex)
            {
                _Nlogger.Info("Error AT  InsertOrUpdateSmsStatusAsync " + ex.InnerException);

                return false;
            }
        }

        public async Task<int> InsertCustomerDetailFromBulkUpload(SmsStatusRequest model)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();

                
                parameters.Add("MobileNo", model.PhoneNumber);
                parameters.Add("Name", model.Name);
                

                return await _repository.GetByValuesAsync<int>("sp_Insert_CustomerDetailFromBulkUpload", parameters);
            }
            catch (Exception ex)
            {
                _Nlogger.Info("Error AT  InsertOrUpdateSmsStatusAsync " + ex.InnerException);

                return 0;
            }
        }


        public async Task<bool> DeActiveCampaign(int Id, bool IsActive)
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

        public async Task<int> EditCustomerAsync(CustomerViewModel request)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("@Id", request.Id);
                parameters.Add("@CustomerName", request.CustomerName);
                parameters.Add("@MobileNo", request.MobileNo);
                //parameters.Add("@address", request.Address);


                return _repository.GetByValuesAsync<int>("[sp_Insert_Update_Customerdetails]", parameters).Result;
            }
            catch (Exception)
            {

                return 0;
            }


        }

        public async Task<CustomerViewModel> GetCustomerDetailByIdAsync(int Id)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Id", Id);
                return await _repository.GetByValuesAsync<CustomerViewModel>("sp_Get_CustomerDetails_ById", parameters);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<bool> DeleteCustomer(int Id)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Id", Id);
               return await _repository.DeleteAsync("sp_Delete_CustomerDetails_ById", parameters);
            }
            catch (Exception)
            {

                return false;
            }
        
        }
    }
}
