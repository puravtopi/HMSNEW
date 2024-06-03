using HMS.Models;
using iTextSharp.text;
using System.Collections.Generic;

namespace HMS.Interface
{
    public interface IConsultantServices
    {
        public ConsultantDashboardModel GetDashboardCount(int UserId);
        public List<ConsultantDashboardModel> GetDashboardChartCount(int UserId,int Selectedyear);
        public ConsultantDashboardModel GetDashboardAvrageCount(int UserId);

        public List<ReceptionWiseCountModel> GetReceptionWiseCounts(int ClinicId, int UserId);
    }
}
