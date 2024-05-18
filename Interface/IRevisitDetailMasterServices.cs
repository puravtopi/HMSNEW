using HMS.Models;
using System.Collections.Generic;

namespace HMS.Interface
{
    public interface IRevisitDetailMasterServices
    {
        public RevisitDetailModel Insert(RevisitDetailModel model);
        public RevisitDetailModel GetTopOneRevisitDetail();
        public List<RevisitDetailModel> GetAll(int Patient_Id);
        public RevisitDetailModel GetAllById(int Id);
        RevisitDetailModel Update(RevisitDetailModel model);
        RevisitDetailModel DeteleRevisitDetail(int Id, int DeletedBy);
    }
}
