using projectDydaTomasz.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectDydaTomasz.Core.Interfaces
{
    public interface IApartmentService
    {
        public void CreateApartment(Apartment newApartment);
        public List<Apartment> GetApartments(string property, string searchTerm);
        public void UpdateApartment(Apartment updatingApartment);
        public void DeleteApartment(string searchTerm);
    }
}
