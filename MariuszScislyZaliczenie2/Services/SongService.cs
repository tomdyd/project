using MariuszScislyZaliczenie2.Interfaces;
using MariuszScislyZaliczenie2.Models;
using MariuszScislyZaliczenie2.Interfaces;

namespace MariuszScislyZaliczenie2.Services
{
    public class SongService : ISongService
    {
        private readonly IDatabaseConnection<Song> _songRepository;

        public SongService(IDatabaseConnection<Song> songService)
        {
            _songRepository = songService;
        }
        public void CreateSong(Song newSong)
        {
            _songRepository.AddToDb(newSong);
        }

        public List<Song> GetSongs(string property, string searchTerm)
        {
            var songList = _songRepository.GetFilteredDataList(property, searchTerm);
            return songList;
        }
        public void UpdateSong(Song updatingSong)
        {
            _songRepository.UpdateData("songId", updatingSong.songId, updatingSong);
        }


        public void DeleteSong(string searchTerm)
        {
            _songRepository.DeleteData("songId", searchTerm);
        }
    }
}
