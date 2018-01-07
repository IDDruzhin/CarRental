using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;

namespace CarRental
{
    class BDController : IBDController
    {
        private IMongoDatabase db;
        private IMongoCollection<CarProperty> carProperties;
        private IMongoCollection<Car> cars;
        private IMongoCollection<Customer> customers;
        private IMongoCollection<Preference> preferences;
        private IMongoCollection<Preference> preferencesArchive;
        private IMongoCollection<Order> orders;
        private IMongoCollection<Order> ordersArchive;
        private IMongoCollection<Penalty> penalties;
        private IMongoCollection<Payment> paymentsArchive;

        void IBDController.AddCar(Car car)
        {
            cars.InsertOne(car);
            //Добавляется новая машина. Даже если такая уже есть в БД.
        }

        CarProperty IBDController.AddCarProperty(CarProperty carProperty)
        {

            var filter = Builders<CarProperty>.Filter.Eq(x => x.description, carProperty.description);
            var update = Builders<CarProperty>.Update.SetOnInsert(x => x.description, carProperty.description);
            var result = carProperties.FindOneAndUpdate(filter, update, new FindOneAndUpdateOptions<CarProperty, CarProperty> { IsUpsert = true, ReturnDocument = ReturnDocument.After });
            return result;
            //Возвращается найденное свойство или добавленное, если такого не было
        }

        List<CarProperty> IBDController.FindCarProperties(List<String> descriptions)
        {

            var filter = Builders<CarProperty>.Filter.In(x => x.description, descriptions);
            var result = carProperties.Find(filter).ToList();
            return result;
            //Поиск существующих требований по описанию
        }

        ObjectId IBDController.AddCustomer(Customer customer)
        {
            var filter = Builders<Customer>.Filter.Eq(x => x.passport, customer.passport);
            var update = Builders<Customer>.Update.Set(x => x.phoneNumber, customer.phoneNumber).SetOnInsert(x => x.name, customer.name).SetOnInsert(x => x.surname, customer.surname).SetOnInsert(x => x.patronymic, customer.patronymic).SetOnInsert(x => x.discountRate, customer.discountRate);
            var result = customers.FindOneAndUpdate(filter, update, new FindOneAndUpdateOptions<Customer, Customer> { IsUpsert = true, ReturnDocument = ReturnDocument.After });
            return result.id;
            //Если такой номер паспорта уже есть, то меняется только телефонный номер. Возвращатся ID для найденного клиента или нового, если такого не было
        }

        void IBDController.AddOrder(Order order)
        {
            orders.InsertOne(order);
        }

        void IBDController.AddPayment(Payment payment)
        {
            paymentsArchive.InsertOne(payment);
        }

        void IBDController.AddPenalty(Penalty penalty)
        {
            penalties.InsertOne(penalty);
        }

        void IBDController.AddPreference(Preference preference)
        {
            preferences.InsertOne(preference);
        }

        List<Car> IBDController.GetAllCars()
        {
            return cars.Find(new BsonDocument()).ToList();
        }
        List<Car> IBDController.GetAllAvilibleCars()
        {
            List<Car> carsList = cars.Find(new BsonDocument()).ToList();
            List<Order> active_orders = orders.Find(new BsonDocument()).ToList();
            List<Car> avialableCarsList = new List<Car>();
            foreach (var car in carsList)
            {
                if (null == active_orders.Find(x => x.carId == car.id))
                    avialableCarsList.Add(car);
                else
                {
                    bool IsFree = true;
                    foreach (var order in active_orders)
                    {
                        if ((order.carId == car.id) && (order.startDate <= DateTime.Now) && (order.finishDate >= DateTime.Now))
                            IsFree = false;
                    }
                    if (IsFree)
                        avialableCarsList.Add(car);
                }
            }
            return avialableCarsList;
        }
        List<Car> IBDController.GetAllInUseCars()
        {
            List<Car> carsList = cars.Find(new BsonDocument()).ToList();
            List<Order> active_orders = orders.Find(new BsonDocument()).ToList();
            List<Car> InUseCarsList = new List<Car>();
            foreach (var car in carsList)
            {
                if (null != active_orders.Find(x => x.carId == car.id))
                {
                    bool IsFree = true;
                    foreach (var order in active_orders)
                    {
                        if ((order.carId == car.id) && (order.startDate <= DateTime.Now) && (order.finishDate >= DateTime.Now))
                            IsFree = false;
                    }
                    if (!IsFree)
                        InUseCarsList.Add(car);
                }
            }
            return InUseCarsList;
        }
        List<Order> IBDController.GetActiveOrders()
        {
            return orders.Find(new BsonDocument()).ToList();
        }

        List<Order> IBDController.GetArchiveOrders()
        {
            return ordersArchive.Find(new BsonDocument()).ToList();
        }

        void IBDController.Init(IMongoDatabase database)
        {
            db = database;
            cars = db.GetCollection<Car>("cars");
            carProperties = db.GetCollection<CarProperty>("car_properties");
            customers = db.GetCollection<Customer>("customers");
            preferences = db.GetCollection<Preference>("preferences");
            orders = db.GetCollection<Order>("orders");
            ordersArchive = db.GetCollection<Order>("orders_archive");
            penalties = db.GetCollection<Penalty>("penalties");
            paymentsArchive = db.GetCollection<Payment>("payments_archive");
            preferencesArchive = db.GetCollection<Preference>("preferences_archive");
        }

        List<CarProperty> IBDController.GetAllCarProperties()
        {
            return carProperties.Find(new BsonDocument()).ToList();
        }

        List<Customer> IBDController.GetAllCustomers()
        {
            return customers.Find(new BsonDocument()).ToList();
        }
        List<Customer> IBDController.GetBestCustomers()
        {
            List<Order> active_orders = orders.Find(new BsonDocument()).ToList();
            Dictionary<ObjectId, int> count_orders = new Dictionary<ObjectId, int>();
            List<Customer> bestCustomrs = new List<Customer>();
            foreach (var order in active_orders)
            {
                int count = 0;
                if (!count_orders.ContainsKey(order.customerId))
                    count_orders.Add(order.customerId, 1);
                else
                {
                    count_orders.TryGetValue(order.customerId, out count);
                    count++;
                    if (count == 10)
                        bestCustomrs.Add(customers.Find(x => x.id == order.customerId).ToList()[0]);

                }
            }
            return bestCustomrs;
        }
        List<Preference> IBDController.GetAllPreferences()
        {
            return preferences.Find(new BsonDocument()).ToList();
        }

        List<Penalty> IBDController.GetAllPenalties()
        {
            return penalties.Find(new BsonDocument()).ToList();
        }

        List<Payment> IBDController.GetAllPayments()
        {
            return paymentsArchive.Find(new BsonDocument()).ToList();
        }

        List<string> IBDController.GetAllCarModels()
        {
            return cars.Distinct(x=>x.model,x=>true).ToList();
        }

        void IBDController.AcceptPreference(ObjectId id)
        {
            var filter = Builders<Preference>.Filter.Eq(x => x.id, id);
            var result = preferences.FindOneAndDelete(filter);
            preferencesArchive.InsertOne(result);
        }

        void IBDController.CompleteOrder(ObjectId id)
        {
            var filter = Builders<Order>.Filter.Eq(x => x.id, id);
            var result = orders.FindOneAndDelete(filter);
            ordersArchive.InsertOne(result);
        }
        ObjectId IBDController.CheckPreference(Preference p)
        {
            var filter = new BsonDocument("$and", new BsonArray
            {
                         new BsonDocument("pricePerDay",new BsonDocument("$lte", p.maxPricePerDay)),
                         new BsonDocument("model", p.carModel)
            });
            var tmp_cars = cars.Find(filter).ToList();
            foreach (var car in tmp_cars)
            {
                bool comp = true;
                foreach (var prop in p.properties)
                {
                    if (null == car.properties.Find(x => x.id == prop.id))
                        comp = false;
                }
                if (comp)
                {
                    List<Order> active_orders = orders.Find(new BsonDocument()).ToList();
                    bool InUse = false;
                    foreach (var order in active_orders)
                    {
                        if ((order.carId == car.id) && ((order.startDate <= p.startDate && order.finishDate > p.startDate) || (order.startDate > p.startDate && order.startDate < p.startDate.AddDays(p.rentalPeriod))))
                            InUse = true;
                    }
                    if (!InUse)
                        return car.id;
                }
            }
            return ObjectId.Empty;
        }
    }
}
