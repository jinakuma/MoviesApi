using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApi.DTOs
{
    public class MovieTheaterDTO
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }


    }
}
