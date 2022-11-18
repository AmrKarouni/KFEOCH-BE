using KFEOCH.Contexts;
using KFEOCH.Models;
using KFEOCH.Models.Views;
using KFEOCH.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KFEOCH.Services
{
    public class OfficeService : IOfficeService
    {
        private readonly ApplicationDbContext _db;
        public OfficeService(ApplicationDbContext db)
        {
            _db = db;
        }
        public Office GetById(int id)
        {
            var office = _db.Offices?.Find(id);
            return office ?? new Office();
        }
    }
}
