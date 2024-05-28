namespace HMS.Models
{
    public class ConsultantDashboardModel
    {
        public int TotalPatient { get; set; }
        public decimal TotalIncome { get; set; }
        public string Months { get; set; }
        public int TotalMonthCount { get; set;}
        public decimal SumOfTotalAmount { get; set; }
        public int PatientIsCheckedCount { get; set; }
        public int PatientIsCheckedPendingCount { get; set; }
        public string StartedTime { get; set; }
        public string EndedTime { get; set; }
        public decimal RevisitCount { get; set; }


    }
}
