using projectDydaTomasz.Core.Interfaces;
using projectDydaTomasz.Core.Models;
using projectDydaTomaszCore.Interfaces;

namespace projectDydaTomasz.Core.Services
{
    public class CarService : ICarService
    {
        private readonly IDatabaseConnection<Car> _carRepository;

        public CarService(IDatabaseConnection<Car> carService)
        {
            _carRepository = carService;
        }
        public void CreateCar(Car newCar)
        {
            _carRepository.AddToDb(newCar);
        }

        public List<Car> GetCars(string property, string searchTerm)
        {
            var carList = _carRepository.GetFilteredDataList(property, searchTerm);
            return carList;
        }
        public void UpdateCar(Car updatingCar)
        {
            _carRepository.UpdateData("carId", updatingCar.carId, updatingCar);
        }


        public void DeleteCar(string searchTerm)
        {
            _carRepository.DeleteData("carId", searchTerm);
        }
    }
}
