using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using CarRental;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        IBDController controller;
        MongoClient client;

        private void clean()
        {
            client.DropDatabase("CarRentalDBTest");
        }

        [TestInitialize]
        public void init()
        {
            client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("CarRentalDBTest");
            controller = new BDController();
            controller.Init(database);
        }

        [TestMethod]
        public void AddCarProp()
        {
            // arrange
            clean();

            CarProperty carPr = new CarProperty();
            carPr.description = "климат контроль";

            // act
            controller.AddCarProperty(carPr);
            var props = controller.GetAllCarProperties();
            
            // asset
            Assert.AreEqual(1, props.Count);
            Assert.AreEqual("климат контроль", props[0].description);
        }

        [TestMethod]
        public void AddCarPropSame()
        {
            // arrange
            clean();
            
            CarProperty carPr1 = new CarProperty();
            carPr1.description = "подогрев сидения";
            CarProperty carPr2 = new CarProperty();
            carPr2.description = "подогрев сидения";

            // act
            controller.AddCarProperty(carPr1);
            controller.AddCarProperty(carPr2);
            var props = controller.GetAllCarProperties();

            // asset
            Assert.AreEqual(1, props.Count);
        }

        [TestMethod]
        public void AddCarPropDif()
        {
            // arrange
            clean();

            CarProperty carPr1 = new CarProperty();
            carPr1.description = "подогрев сидения";
            CarProperty carPr2 = new CarProperty();
            carPr2.description = "климат контроль";

            // act
            controller.AddCarProperty(carPr1);
            controller.AddCarProperty(carPr2);
            var props = controller.GetAllCarProperties();

            // asset
            Assert.AreEqual(props.Count, 2);
            Assert.AreNotEqual(props[0].id, props[1].id);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AddCarPropNull()
        {
            // arrange
            clean();
            controller.AddCarProperty(null);

            // act
            controller.GetAllCarProperties();
        }

        [TestMethod]
        public void FindCarProp()
        {
            // arrange
            clean();

            CarProperty carPr1 = new CarProperty();
            carPr1.description = "подогрев сидения";
            CarProperty carPr2 = new CarProperty();
            carPr2.description = "климат контроль";

            // act
            controller.AddCarProperty(carPr1);
            controller.AddCarProperty(carPr2);
            List<String> props = new List<string>(new String[]{ "климат контроль" });
            var carProps = controller.FindCarProperties(props);

            // asset
            Assert.IsNotNull(carProps);
            Assert.AreEqual(1, carProps.Count);
            Assert.AreEqual("климат контроль", carProps[0].description);
        }

        [TestMethod]
        public void FindCarProp2()
        {
            // arrange
            clean();

            CarProperty carPr1 = new CarProperty();
            carPr1.description = "подогрев сидения";
            CarProperty carPr2 = new CarProperty();
            carPr2.description = "климат контроль";

            // act
            controller.AddCarProperty(carPr1);
            controller.AddCarProperty(carPr2);
            List<String> props = new List<string>(new String[] { "плазмовая пушка" });
            var carProps = controller.FindCarProperties(props);

            // asset
            Assert.AreEqual(0, carProps.Count);
        }

        [TestMethod]
        public void AddCarWithoutProp()
        {
            // arrange
            clean();

            Car car = new Car();
            car.model = "Ford";
            car.pricePerDay = 1500;

            // act
            controller.AddCar(car);
            var cars = controller.GetAllCars();

            // assert
            Assert.AreEqual(1, cars.Count);
            Assert.AreEqual(1500, cars[0].pricePerDay);
            Assert.AreEqual("Ford", cars[0].model);
            Assert.IsNull(cars[0].properties);
        }

        [TestMethod]
        public void AddCarWithProp()
        {
            // arrange
            clean();

            Car car = new Car();
            car.model = "Opel";
            car.pricePerDay = 4000;
            car.properties = new List<CarProperty>();

            CarProperty prop = new CarProperty();
            prop.description = "климат контроль";
            car.properties.Add(prop);

            // act
            controller.AddCar(car);
            var properties = controller.GetAllCarProperties();
            var cars = controller.GetAllCars();

            // assert
            Assert.AreEqual(1, cars.Count);
            Assert.AreEqual(4000, cars[0].pricePerDay);
            Assert.AreEqual("Opel", cars[0].model);
            Assert.AreEqual("климат контроль", cars[0].properties[0].description);
            Assert.AreEqual(1, properties.Count);
            Assert.AreEqual("климат контроль", properties[0].description);
        }

        [TestMethod]
        public void AddCustomer()
        {
            // arrange
            clean();
            Customer cust = new Customer();
            cust.name = "Вася";
            cust.surname = "Пупкин";
            cust.passport = "1234 567890";
            cust.phoneNumber = "12345";

            // act
            controller.AddCustomer(cust);
            var customers = controller.GetAllCustomers();

            // asset
            Assert.AreEqual(1, customers.Count);
            Assert.AreEqual("Вася", customers[0].name);
            Assert.IsNull(customers[0].patronymic);
            Assert.AreEqual("Пупкин", customers[0].surname);
            Assert.AreEqual("1234 567890", customers[0].passport);
            Assert.AreEqual("12345", customers[0].phoneNumber);
            Assert.AreEqual(0, customers[0].discountRate);
        }

        [TestMethod]
        public void AddCustomerChangePhone()
        {
            // arrange
            clean();
            Customer cust = new Customer();
            cust.name = "Вася";
            cust.surname = "Пупкин";
            cust.passport = "1234 567890";
            cust.phoneNumber = "12345";

            Customer cust2 = new Customer();
            cust2.name = "Вася";
            cust2.surname = "Пупкин";
            cust2.passport = "1234 567890";
            cust2.phoneNumber = "54321";

            // act
            controller.AddCustomer(cust);
            controller.AddCustomer(cust2);
            var customers = controller.GetAllCustomers();

            // asset
            Assert.AreEqual(1, customers.Count);
            Assert.AreEqual("Вася", customers[0].name);
            Assert.AreEqual("Пупкин", customers[0].surname);
            Assert.AreEqual("1234 567890", customers[0].passport);
            Assert.AreEqual("54321", customers[0].phoneNumber);
        }

        [TestMethod]
        public void GetAllAvilibleCars()
        {
            // arrange
            clean();
            Car car = new Car();
            car.model = "Opel";
            car.pricePerDay = 4000;
            car.properties = new List<CarProperty>();
            CarProperty prop = new CarProperty();
            prop.description = "климат контроль";
            car.properties.Add(prop);
            
            controller.AddCar(car);

            // act
            var cars = controller.GetAllAvilibleCars();

            // asset
            Assert.AreEqual(1, cars.Count);
        }

        [TestMethod]
        public void GetAllInUseCars()
        {
            // arrange
            clean();
            Car car = new Car();
            car.model = "Opel";
            car.pricePerDay = 4000;
            car.properties = new List<CarProperty>();
            CarProperty prop = new CarProperty();
            prop.description = "климат контроль";
            car.properties.Add(prop);

            controller.AddCar(car);

            // act
            var cars = controller.GetAllInUseCars();

            // asset
            Assert.AreEqual(0, cars.Count);
        }

        private Preference initSomeCarsAndPref()
        {
            Car car = new Car();
            car.model = "Opel";
            car.pricePerDay = 4000;
            car.properties = new List<CarProperty>();
            CarProperty prop = new CarProperty();
            prop.description = "климат контроль";
            car.properties.Add(prop);
            controller.AddCar(car);

            car = new Car();
            car.model = "Opel";
            car.pricePerDay = 3000;
            controller.AddCar(car);

            car = new Car();
            car.model = "Opel";
            car.pricePerDay = 2000;
            controller.AddCar(car);

            Customer cust = new Customer();
            cust.name = "Вася";
            cust.surname = "Пупкин";
            cust.passport = "1234 567890";
            cust.phoneNumber = "12345";

            Preference preference = new Preference { };
            preference.customerId = controller.AddCustomer(cust);
            preference.startDate = DateTime.Now.AddDays(1);
            preference.rentalPeriod = 3;
            preference.maxPricePerDay = 2000;
            preference.carModel = "Opel";
            preference.properties = new List<CarProperty>();

            return preference;
        }

        [TestMethod]
        public void Preference1()
        {
            // arrange
            clean();
            var preference = initSomeCarsAndPref();
            preference.maxPricePerDay = 1000;

            // act
            controller.AddPreference(preference);
            var check1 = controller.CheckPreference(preference);

            // asset
            Assert.AreEqual(ObjectId.Empty, check1);
        }

        [TestMethod]
        public void Preference2()
        {
            // arrange
            clean();
            var preference = initSomeCarsAndPref();
            preference.maxPricePerDay = 3000;

            // act
            controller.AddPreference(preference);
            var check1 = controller.CheckPreference(preference);

            // asset
            Assert.AreNotEqual(ObjectId.Empty, check1);
        }

        [TestMethod]
        public void Preference3()
        {
            // arrange
            clean();
            var preference = initSomeCarsAndPref();
            preference.maxPricePerDay = 3000;
            preference.properties = controller.FindCarProperties(new List<string>(new String[] { "климат контроль" }));

            // act
            controller.AddPreference(preference);
            var check1 = controller.CheckPreference(preference);

            // asset
            Assert.AreEqual(ObjectId.Empty, check1);
        }

        [TestMethod]
        public void Preference4()
        {
            // arrange
            clean();
            var preference = initSomeCarsAndPref();
            preference.maxPricePerDay = 5000;
            preference.properties = controller.FindCarProperties(new List<string>(new String[] { "климат контроль" }));

            // act
            controller.AddPreference(preference);
            var check1 = controller.CheckPreference(preference);

            // asset
            Assert.AreNotEqual(ObjectId.Empty, check1);
        }
    }
}
