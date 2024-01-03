namespace DB_Assigment.DTOs
{
    public class ApiResponse
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
        public int total_pages { get; set; }
        public List<TvSeries> data { get; set; }
    }
    public class TvSeries
    {
        public string name { get; set; }
        public string runtime_of_series { get; set; }
        public string certificate { get; set; }
        public string runtime_of_episodes { get; set; }
        public string genre { get; set; }
        public double imdb_rating { get; set; }
        public string overview { get; set; }
        public int no_of_votes { get; set; }
        public int id { get; set; }
    }
}
