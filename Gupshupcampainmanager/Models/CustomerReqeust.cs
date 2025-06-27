namespace Gupshupcampainmanager.Models
{
    public class CustomerReqeust
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string MobileNo { get; set; }
        //public string Address { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string orderBy { get; set; } = "createdOn";
        public string orderDirection { get; set; } = "asc";
    }
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string MobileNo { get; set; }
      ///  public string Address { get; set; }
    }
    public class customerResponse
    {
        public IEnumerable<CustomerViewModel> Customers { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
