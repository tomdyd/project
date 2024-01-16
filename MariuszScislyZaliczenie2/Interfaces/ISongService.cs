using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MariuszScislyZaliczenie2.Models;


namespace MariuszScislyZaliczenie2.Interfaces
{
    public interface ISongService
    {
        public void CreateSong(Song newSong);
        public List<Song> GetSongs(string property, string searchTerm);
        public void UpdateSong(Song updatingSong);
        public void DeleteSong(string searchTerm);
    }
}