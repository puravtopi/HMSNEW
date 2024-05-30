using HMS.Models;
using iTextSharp.text;
using System.Collections.Generic;

namespace HMS.Interface
{
    public interface IReceptionistMasterServices
    {
        public ReceptionistDashboardModel GetDashboardCount(int UserId);
        public List<ReceptionistDashboardModel> GetDashboardChartCount(int UserId,int Selectedyear);
        public ReceptionistDashboardModel GetDashboardAvrageCount(int UserId);
    }
}
