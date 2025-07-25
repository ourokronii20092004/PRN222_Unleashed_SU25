﻿using DAL.Data;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        public readonly UnleashedContext _unleashedContext;
        public CategoryRepository(UnleashedContext unleashedContext)
        {
            _unleashedContext = unleashedContext;
        }
        //public async Task AddAsync(Category entity, CancellationToken cancellationToken = default)
        //{
        //    await _unleashedContext.AddAsync(entity, cancellationToken);
        //    await _unleashedContext.SaveChangesAsync(cancellationToken);
        //}

        public async Task<Category> AddAsync(Category category)
        {
            await _unleashedContext.AddAsync(category);
            await _unleashedContext.SaveChangesAsync();
            return category;
        }

        //public async Task Delete(Category entity, CancellationToken cancellationToken = default)
        //{
        //    _unleashedContext.Remove(entity);
        //    await _unleashedContext.SaveChangesAsync(cancellationToken);
        //}

        public async Task DeleteAsync(Category category)
        {
            _unleashedContext.Remove(category);
            await _unleashedContext.SaveChangesAsync();
        }

        //public async Task<IEnumerable<Category>> FindAsync(Expression<Func<Category, bool>> predicate, CancellationToken cancellationToken = default)
        //{
        //    return await _unleashedContext.Categories.Where(predicate).ToListAsync(cancellationToken);
        //}

        //public async Task<IEnumerable<Category>> GetAllAsync()
        //{
        //    return await _unleashedContext.Categories.OrderBy(c => c.CategoryId).ToListAsync();
        //}

        //public async Task<Category> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        //{
        //    return await _unleashedContext.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.CategoryId == id) ??
        //        throw new ArgumentException();
        //}

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _unleashedContext.Categories.FirstAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _unleashedContext.SaveChangesAsync();
        }

        //public async Task Update(Category entity, CancellationToken cancellationToken = default)
        //{
        //    _unleashedContext.Categories.Update(entity);
        //    await _unleashedContext.SaveChangesAsync();
        //}

        public async Task UpdateAsync(Category category)
        {
            _unleashedContext.Categories.Update(category);
            await _unleashedContext.SaveChangesAsync(); 
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _unleashedContext.Categories.OrderBy(c => c.CategoryId).ToListAsync();
        }
    }
}
