using System.Collections.Generic;

namespace Assign1.Model
{
    public class Book
    {
        public string Title { get; set; }
        public string SmallThumbnail { get; set; }
        public List<string> Authors { get; set; }
        public string Publisher { get; set; }
        public string PublishedDate { get; set; }
        public string Description { get; set; }
        public string ISBN_10_Id { get; set; }
    }

}