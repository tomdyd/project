using projectDydaTomasz.Core.Interfaces;
using projectDydaTomasz.Core.Models;
using projectDydaTomaszCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectDydaTomasz.Core.Services
{
    public class ApartmentService : IApartmentService
    {
        private readonly IDatabaseConnection<Apartment> _apartmentRepository;

        public ApartmentService(IDatabaseConnection<Apartment> apartmentRepository)
        {
            _apartmentRepository = apartmentRepository;
        }

        public void CreateApartment(Apartment newApartment)
        {
            _apartmentRepository.AddToDb(newApartment);
        }

        public void DeleteApartment(string searchTerm)
        {
            _apartmentRepository.DeleteData("apartmentId", searchTerm);
        }

        public List<Apartment> GetApartments(string property, string searchTerm)
        {
            var apartmentList = _apartmentRepository.GetFilteredDataList(property, searchTerm);
            return apartmentList;
        }

        public void UpdateApartment(Apartment updatingApartment)
        {
            _apartmentRepository.UpdateData("apartmentId", updatingApartment.apartmentId, updatingApartment);
        }
    }
}
