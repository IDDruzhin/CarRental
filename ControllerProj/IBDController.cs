using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;

namespace CarRental
{
    public interface IBDController
    {
        void Init(IMongoDatabase database);
        void AddCar(Car car);
        CarProperty AddCarProperty(CarProperty carProperty);
        List<CarProperty> FindCarProperties(List<String> descriptions);
        ObjectId AddCustomer(Customer customer);
        void AddPreference(Preference preference);
        void AddOrder(Order order);
        void AddPenalty(Penalty penalty);
        void AddPayment(Payment payment);
        List<CarProperty> GetAllCarProperties();
        List<Car> GetAllCars();
        List<Car> GetAllAvilibleCars();
        List<Car> GetAllInUseCars();
        List<Customer> GetAllCustomers();
        List<Customer> GetBestCustomers();
        List<Preference> GetAllPreferences();
        List<Order> GetActiveOrders();
        List<Order> GetArchiveOrders();
        List<Penalty> GetAllPenalties();
        List<Payment> GetAllPayments();
        List<String> GetAllCarModels();
        void AcceptPreference(ObjectId id);
        void CompleteOrder(ObjectId id);
        ObjectId CheckPreference(Preference p);
    }
}
