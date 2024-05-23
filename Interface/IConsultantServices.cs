using HMS.Models;

namespace HMS.Interface
{
    public interface IConsultantServices
    {
        public ConsultantDashboardModel GetDashboardCount(int UserId);
    }
}
