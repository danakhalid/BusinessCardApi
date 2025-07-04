using BusinessCardApi.DataDb;
using BusinessCardApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessCardApi.Repo
{
    public class BusinessCardRepo : IBusinessCardRepo
    {
        private readonly AppDbContext _appDbContext;
        public BusinessCardRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<BusinessCard> GetAll()
        {
            return _appDbContext.BusinessCards.ToList();
        }

        public BusinessCard? GetById(int id)
        {
            return _appDbContext.BusinessCards.FirstOrDefault(x => x.Id == id);
        }

        public void CreateCard(BusinessCard card)
        {
            _appDbContext.BusinessCards.Add(card);
            _appDbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var card = _appDbContext.BusinessCards.FirstOrDefault(x => x.Id == id);
            if (card != null)
            {
                _appDbContext.BusinessCards.Remove(card);
                _appDbContext.SaveChanges();
            }
        }
    }
}