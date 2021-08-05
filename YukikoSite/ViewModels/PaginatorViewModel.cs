using System;

namespace YukikoSite.ViewModels {
    public class PaginatorViewModel {
        public readonly int PageSize;
        public readonly int CurrentPage;
        public readonly int TotalCount;
        public readonly string MapPath;

        public readonly int TotalPages;

        public PaginatorViewModel(int pageSize, int currentPage, int totalCount, string mapPath) {
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalCount = totalCount;
            MapPath = mapPath;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
        }

        public int NextPage() => CurrentPage + 1 > TotalPages ? TotalPages : CurrentPage + 1;
        public int PrevPage() => CurrentPage - 1 <= 0 ? 1 : CurrentPage - 1;
    }
}
