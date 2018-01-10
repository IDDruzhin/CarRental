using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;

namespace CarRental
{

    public class CarProperty
    {
        public ObjectId id { get; set; }
        public String description { get; set; }
    }

    public class Car
    {
        public ObjectId id { get; set; }
        public String model { get; set; }
        public List<CarProperty> properties { get; set; }
        public double pricePerDay { get; set; }
    }

    public class Customer
    {
        public ObjectId id { get; set; }
        public String name { get; set; }
        public String surname { get; set; }
        public String patronymic { get; set; }
        public String passport { get; set; }
        public String phoneNumber { get; set; }
        public double discountRate { get; set; }
    }

    public class Preference
    {
        public ObjectId id { get; set; }
        public List<CarProperty> properties { get; set; }
        public int rentalPeriod { get; set; }
        public ObjectId customerId { get; set; }
        public DateTime startDate { get; set; }
        //public int rentalPeriod { get; set; }  //second rentalPeriod?
        public String carModel { get; set; }
        public double maxPricePerDay { get; set; }
    }

    public class Order
    {
        public ObjectId id { get; set; }
        public ObjectId carId { get; set; }
        public ObjectId customerId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime finishDate { get; set; }
        public ObjectId preferenceId { get; set; }
    }

    public class Penalty
    {
        public ObjectId id { get; set; }
        public bool penaltyType { get; set; }
        public double amount { get; set; }
    }

    public class Payment
    {
        //public ObjectId id { get; set; }
        public ObjectId orderId { get; set; }
        public ObjectId penaltyId { get; set; }
        public double totalPrice { get; set; }
    }

}
