using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Entities;
using MoviesApi.Helpers;

namespace MoviesApi.DTOs
{
    public class MovieCreationDTO
    {
     
        
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Trailer { get; set; }
        public bool InTheaters { get; set; }
        public DateTime ReleaseDate { get; set; }
        public IFormFile Poster { get; set; }

        [ModelBinder(BinderType=typeof(TypeBinder<List<int>>))]
        public List<int> GenresIds { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> MovieTheatersId { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<MoviesActorsCreationDTO>))]
        public List<MoviesActorsCreationDTO> Actors { get; set; }
        
    }
}
