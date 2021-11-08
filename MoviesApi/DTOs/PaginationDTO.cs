namespace MoviesApi.DTOs
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;
        private int recordsPerPage = 5;
        private const int MaxAmount = 50;

        public int RecordsPerPage
        {
            get => recordsPerPage;
            set => recordsPerPage = (value > MaxAmount) ? MaxAmount : value;
        }
    }
}

