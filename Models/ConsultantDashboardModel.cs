using System.Collections.Generic;

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
        public PatientMasterModel patientMastersList { get; set; }
        public int TotalServiceCount { get; set; }
        public int TodayPatientCount { get; set; }
        public int YesterdayPatientCount { get; set; }

        public string ChangeDirection { get; set; }
        public float PercentageChange { get; set; } 
        public int TodayRevenue { get; set; }
        public int YesterdayRevenue { get; set; }
        public int TodayServiceCount { get; set; }
        public int YesterdayServiceCount { get; set; }

        public string RevenueChangeDirection { get; set;}
        public float RevenuePercentageChange { get; set; }
        public string ServiceChangeDirection { get; set; }
        public float ServicePercentageChange { get; set; }

    }
}
