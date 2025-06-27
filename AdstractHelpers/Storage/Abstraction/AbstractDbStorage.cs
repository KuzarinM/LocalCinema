using AdstractHelpers.Storage.Abstraction.Interfases;
using AdstractHelpers.Storage.Abstraction.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AdstractHelpers.Storage.Abstraction
{
    /// <summary>
    /// Абстрактный репозиторий
    /// </summary>
    /// <typeparam name="TItem">Тип модели БД</typeparam>
    /// <typeparam name="TDto">Тип простово DTO</typeparam>
    /// <typeparam name="TFullDto">Тип полного DTO</typeparam>
    /// <typeparam name="TKey">Тип Id сущности БД</typeparam>
    public abstract class AbstractDbStorage<TItem, TKey> : IDbStorage<TItem, TKey>
        where TItem : class, IId<TKey>
        where TKey : struct
    {
        protected readonly ILogger _logger;
        protected readonly IMapper _mapper;
        protected readonly DbContext _dbContext;
        public AbstractDbStorage(
            ILogger logger,
            IMapper mapper,
            DbContext dbContext)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Получение запроса, чтобы можно было им пользоваться
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TItem> GetQueryable()
        {
            return AddIncludes(_dbContext.Set<TItem>().AsQueryable());
        }

        /// <summary>
        /// Получение полной модели по Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public virtual async Task<TItem?> GetItemById(TKey id)
        {
            try
            {
                var item = await AddIncludes(_dbContext.Set<TItem>().AsQueryable()).FirstOrDefaultAsync(x => x.Id.Equals(id));

                if (item == null)
                    return null;

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка при получении элемента по Id", ex);
                throw;
            }
        }


        /// <summary>
        /// Добавляем эелемент
        /// </summary>
        /// <typeparam name="TCreate">Модель из которой должен существовать маппинг в сущность БД</typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<TItem?> AddItem<TCreate>(TCreate model) => (await AddItems([model])).FirstOrDefault();

        /// <summary>
        /// Добавляем эелементы
        /// </summary>
        /// <typeparam name="TCreate">Модель из которой должен существовать маппинг в сущность БД</typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<List<TItem>> AddItems<TCreate>(List<TCreate> model)
        {
            try
            {
                var dbModels = _mapper.Map<List<TItem>>(model);

                if (dbModels.Count == 0)
                    return [];

                await _dbContext.AddRangeAsync(dbModels);

                await _dbContext.SaveChangesAsync();

                return dbModels;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка при добавлении элементов", ex);
                throw;
            }

        }

        /// <summary>
        /// Обновить сущность
        /// </summary>
        /// <typeparam name="TUpdate">Модель которую будем использовать для обновления</typeparam>
        /// <param name="id"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual async Task<TItem?> UpdateItem<TUpdate>(TKey id, TUpdate target)
        {
            try
            {
                var item = await AddIncludes(_dbContext.Set<TItem>()).FirstOrDefaultAsync(x => x.Id.Equals(id));

                if (item == null)
                    return null;

                await UpdateItem(item, target);

                await _dbContext.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка при обновлении элемента", ex);
                throw;
            }
        }

        /// <summary>
        /// Удалить элемент по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TItem?> DeleteElement(TKey id)
        {
            try
            {
                var item = await AddIncludes(_dbContext.Set<TItem>()).FirstOrDefaultAsync(x => x.Id.Equals(id));

                if (item == null)
                    return null;

                _dbContext.Remove(item);
                await _dbContext.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка при удалении элемента", ex);
                throw;
            }
        }

        /// <summary>
        /// Обновление элемента
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target">Обновляющая модель</param>
        /// <returns></returns>
        protected abstract Task<TItem> UpdateItem(TItem source, object target);

        /// <summary>
        /// Переопределяем добавление Include
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual IQueryable<TItem> AddIncludes(IQueryable<TItem> query) => query;

        public Task SaveChangesAsync() => _dbContext.SaveChangesAsync();
    }
}
