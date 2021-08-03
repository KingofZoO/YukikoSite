using System.Collections.Generic;

namespace YukikoSite.Models {
    public class NewsItem {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TitleImagePath { get; set; }

        public List<NewsContentItem> NewsContentItems { get; set; }
    }
}
