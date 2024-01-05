namespace Zaliczenie.Interfaces
{
    public interface IArt
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime Relased { get; set; }
        public int Rating { get; set; }

    }
}
