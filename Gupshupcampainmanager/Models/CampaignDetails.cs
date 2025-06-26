namespace Gupshupcampainmanager.Models
{
    public class CampaignDetails
    {
        public int Id { get; set; }

        public string Desciption { get; set; }

        public string ImagePath { get; set; }

        public DateTime ScheduleOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
        public bool IsActive { get; set; }
    }


    public class CampaignDetailsRequest
    {

        public int Id { get; set; }
        public string Desciption { get; set; }

        public string ImagePath { get; set; }

    }

    public class CampaignDetailsResponse
    {
        public int Id { get; set; }

        public string Desciption { get; set; }

        public string ImagePath { get; set; }

        public DateTime ScheduleOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

    }

    public class CampaignMessageDetail
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Timestamp { get; set; }
        public string FailureReason { get; set; } // Nullable in database
        public string Name { get; set; }
    }
}
