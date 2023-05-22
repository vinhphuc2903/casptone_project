using System;

namespace CapstoneProject.Areas.Film.Models.FilmModels.Schemas
{
	public class FilmDetail
	{
        public IEnumerable<ShowtimeInfo> ListShowtimeInfo { get; set; }
        public FilmInfo? FilmInfos { get; set; }
        public IEnumerable<string>? ListTypeFilm { get; set; }
    }
}

