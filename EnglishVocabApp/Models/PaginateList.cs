using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EnglishVocabApp.Models
{
    public class PaginateList<TDataModel, TViewModel>: List<TViewModel>
        where TViewModel: new()
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalItemsCount { get; set; }
        public int ShowedItemsCount { get; private set; }
        private PaginateList(List<TViewModel> items, int count, int pageIndex, int pageSize, int showedPages)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalItemsCount = count;
            ShowedItemsCount = showedPages;
            this.AddRange(items);
        }
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }
        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }
        public static async Task<PaginateList<TDataModel, TViewModel>> CreateAsync(IQueryable<TDataModel> source, 
            Expression<Func<TDataModel, TViewModel>> getViewModelByDataModelFunc, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(getViewModelByDataModelFunc)
                .ToListAsync();
            var showedPages = pageSize * (pageIndex - 1) + items.Count;
            return new PaginateList<TDataModel, TViewModel>(items, count, pageIndex, pageSize, showedPages);
        }
    }
}
