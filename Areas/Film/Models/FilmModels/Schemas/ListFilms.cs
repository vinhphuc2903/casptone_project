using System;
using CapstoneProject.Commons;

namespace CapstoneProject.Areas.Film.Models.FilmModels.Schemas
{
	public class ListFilms
	{
		public IEnumerable<FilmInfo> ListFilmInfos { get; set; }
		public Paging Paging { get; set; }
	}
}

