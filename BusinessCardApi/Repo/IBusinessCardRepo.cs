using BusinessCardApi.Models;

namespace BusinessCardApi.Repo
{
    public interface IBusinessCardRepo
    {
        IEnumerable<BusinessCard> GetAll();
        void CreateCard(BusinessCard card);
        BusinessCard? GetById(int id);
        void Delete(int id);
    }
}
