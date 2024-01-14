using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using projectDydaTomasz.Core.Models;
using projectDydaTomaszCore.Models;

namespace projectDydaTomasz.Core.Interfaces
{
    public interface ICarService
    {
        public void CreateCar(Car newCar);
        public List<Car> GetCars(string property, string searchTerm);
        public void UpdateCar(Car updatingCar);
        public void DeleteCar(string searchTerm);
    }
}