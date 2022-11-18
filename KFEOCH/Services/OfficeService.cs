﻿using KFEOCH.Contexts;
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
        public async Task<ResultWithMessage> PutOfficeAsync(int id, Office model)
        {
            if (id != model.Id)
            {
                return new ResultWithMessage { Success = false, Message = $@"Bad Request" };
            }
            var office = _db.Offices.Find(id);
            _db.Entry(office).State = EntityState.Detached;
            if (office == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Not Found !!!" };
            }
            office = model;
            _db.Entry(office).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
    }
}
