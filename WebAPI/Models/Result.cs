namespace WebAPI.Models
{
    public class Result
    {
        public object Data { get; set; }
        public bool? Success { get; set; }
        public Paging Paging { get; set; }
    }

    public class Paging
    {
        public int Page { get; set; }
        public int RecordsPerPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
    }
}
