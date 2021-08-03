namespace YukikoSite.Models {
    public class NewsContentItem {
        public int Id { get; set; }
        public string ItemPath { get; set; }

        public int NewsItemId { get; set; }
        public NewsItem NewsItem { get; set; }
    }
}
