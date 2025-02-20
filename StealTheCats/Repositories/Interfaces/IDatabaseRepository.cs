﻿using StealTheCatsApi.Models;

namespace StealTheCatsApi.Repositories.Interfaces
{
    public interface IDatabaseRepository
    {
        Task<bool> SaveCatAsync(Cat cat);
        Task<Cat> GetCatByIdAsync(int id);
        Task<IEnumerable<Cat>> GetCatsAsync(int page, int pageSize);
        Task<IEnumerable<Cat>> GetCatsByTagAsync(string tag, int page, int pageSize);
    }
}
