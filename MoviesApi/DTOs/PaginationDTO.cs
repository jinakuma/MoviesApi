namespace MoviesApi.DTOs
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;
        private int _recordsPerPage = 10;
        private readonly int maxAmount = 50;

        public int RecordPerPage
        {
            get => _recordsPerPage;
            set => _recordsPerPage = (value > maxAmount) ? maxAmount : value;
        }
    }
}

