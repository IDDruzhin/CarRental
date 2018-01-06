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
    interface IBDController
    {
        void Init(IMongoDatabase database);
        void AddCar(Car car);
        void AddCarProperty(CarProperty carProperty);
        ObjectId AddCustomer(Customer customer);
        void AddPreference(Preference preference);
        void AddOrder(Order order);
        void AddPenalty(Penalty penalty);
        void AddPayment(Payment payment);
        List<CarProperty> GetAllCarProperties();
        List<Car> GetAllCars();
        List<Customer> GetAllCustomers();
        List<Preference> GetAllPreferences();
        List<Order> GetActiveOrders();
        List<Order> GetArchiveOrders();
        List<Penalty> GetAllPenalties();
        List<Payment> GetAllPayments();
    }
}
