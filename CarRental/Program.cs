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
    class Program
    {
        static void Main(string[] args)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("CarRentalDB");
            IBDController controller = new BDController();
            controller.Init(database);
            List<String> info = new List<string>();
            info.Add("0. Выход");
            info.Add("1. Отправка предпочтения");  //+
            info.Add("2. Оформление сделки");
            info.Add("3. Добавить автомобиль"); //+
            info.Add("4. Завершение сделки");
            info.Add("5. Список доступных автомобилей");
            info.Add("6. Список всех автомобилей"); //+
            info.Add("7. Список автомобилей в прокате");
            info.Add("8. Список постоянных клиентов");
            info.Add("9. Список заключенных сделок");
            info.Add("10. Прибыль");
            info.Add("11. Список всех клиентов"); //+
            info.Add("12. Список популярных машин");
            info.Add("13. Список клиентов, приносящих доход больше среднего");
            info.Add("14. Эффективность системы штрафов");

            foreach (var s in info)
            {
                Console.WriteLine(s);
            }
            String line;
            List<CarProperty> inputProperties;
            while (true)
            {
                line = Console.ReadLine();
                switch (line)
                {
                    case "0":
                        return;
                    case "1":
                        Customer customer = new Customer { };
                        Console.WriteLine("Имя:");
                        customer.name = Console.ReadLine();
                        Console.WriteLine("Фамилия:");
                        customer.surname = Console.ReadLine();
                        Console.WriteLine("Отчество:");
                        customer.patronymic = Console.ReadLine();
                        Console.WriteLine("Паспорт:");
                        customer.passport = Console.ReadLine();
                        Console.WriteLine("Телефон:");
                        customer.phoneNumber = Console.ReadLine();

                        Preference preference = new Preference { };
                        preference.customerId = controller.AddCustomer(customer);
                        Console.WriteLine("Добавить свойство автомобиля? (0 - нет, 1 - да)");
                        line = Console.ReadLine();
                        if (line == "1")
                        {
                            List<String> inputStringsProperties = new List<string>();
                            List<CarProperty> propertiesList = controller.GetAllCarProperties();
                            while (line == "1")
                            {
                                Console.WriteLine("Имеющиеся свойства:");
                                foreach (var p in propertiesList)
                                {
                                    Console.WriteLine(p.description);
                                }
                                inputStringsProperties.Add(Console.ReadLine());
                                Console.WriteLine("Добавить свойство? (0 - нет, 1 - да)");
                                line = Console.ReadLine();
                            }
                            preference.properties = controller.FindCarProperties(inputStringsProperties);
                        }
                        Console.WriteLine("Стартовая дата:");
                        Console.WriteLine("Год:");
                        String inputStartYear = Console.ReadLine();
                        Console.WriteLine("Месяц:");
                        String inputStartMonth = Console.ReadLine();
                        Console.WriteLine("День:");
                        String inputStartDay = Console.ReadLine();
                        preference.startDate = new DateTime(System.Convert.ToInt32(inputStartYear), System.Convert.ToInt32(inputStartMonth), System.Convert.ToInt32(inputStartDay));
                        Console.WriteLine("Длительность проката в днях:");
                        preference.rentalPeriod = System.Convert.ToInt32(Console.ReadLine());
                        List<String> modelsList = controller.GetAllCarModels();
                        Console.WriteLine("Имеющиеся марки автомобилей:");
                        for (int i=0; i< modelsList.Count();i++)
                        {
                            Console.Write(i+1);
                            Console.Write(". ");
                            Console.WriteLine(modelsList[i]);
                        }
                        Console.WriteLine("Марка автомобиля:");
                        preference.carModel = Console.ReadLine();
                        Console.WriteLine("Максимальная стоимость проката за день:");
                        preference.maxPricePerDay = System.Convert.ToDouble(Console.ReadLine());
                        controller.AddPreference(preference);
                        Console.WriteLine("OK");
                        break;
                    case "3":
                        Car car = new Car { };
                        Console.WriteLine("Модель:");
                        car.model = Console.ReadLine();
                        Console.WriteLine("Добавить свойство? (0 - нет, 1 - да)");
                        line = Console.ReadLine();
                        if (line=="1")
                        {
                            List<String> inputStringsProperties = new List<string>();
                            List<CarProperty> propertiesList = controller.GetAllCarProperties();
                            while (line == "1")
                            {
                                Console.WriteLine("Имеющиеся свойства:");
                                foreach (var p in propertiesList)
                                {
                                    Console.WriteLine(p.description);
                                }
                                inputStringsProperties.Add(Console.ReadLine());
                                Console.WriteLine("Добавить свойство? (0 - нет, 1 - да)");
                                line = Console.ReadLine();
                            }
                            inputProperties = new List<CarProperty>();
                            foreach (var s in inputStringsProperties)
                            {
                                inputProperties.Add(controller.AddCarProperty(new CarProperty { description = s }));
                            }
                            car.properties = inputProperties;
                        }
                        Console.WriteLine("Стоимость проката за день:");
                        car.pricePerDay = Convert.ToDouble(Console.ReadLine());
                        controller.AddCar(car);
                        Console.WriteLine("OK");
                        break;
                    case "6":
                        List<Car> carsList = controller.GetAllCars();
                        for (int i=0; i<carsList.Count();i++)
                        {
                            Console.Write(i+1);
                            Console.Write(". Модель: ");
                            Console.Write(carsList[i].model);
                            Console.Write(" | Свойства: ");
                            List<CarProperty> propList = carsList[i].properties;
                            if (propList.Count()!=0)
                            {
                                for (int j = 0; j < propList.Count() - 1; j++)
                                {
                                    Console.Write(propList[j].description);
                                    Console.Write(", ");
                                }
                                Console.Write(propList.Last().description);
                            }  
                            Console.Write(" | Стоимость проката за день: ");
                            Console.Write(carsList[i].pricePerDay);
                            Console.WriteLine();
                        }
                        if (carsList.Count()==0)
                        {
                            Console.WriteLine();
                        }                        
                        break;
                    case "11":
                        List<Customer> customersList = controller.GetAllCustomers();
                        if (customersList.Count()!=0)
                        {
                            for (int i = 0; i < customersList.Count(); i++)
                            {
                                Console.Write(i+1);
                                Console.Write(". Имя: ");
                                Console.Write(customersList[i].name);
                                Console.Write(" | Фамилия: ");
                                Console.Write(customersList[i].surname);
                                Console.Write(" | Отчество: ");
                                Console.Write(customersList[i].patronymic);
                                Console.Write(" | Паспорт: ");
                                Console.Write(customersList[i].passport);
                                Console.Write(" | Телефонный номер: ");
                                Console.Write(customersList[i].phoneNumber);
                                Console.Write(" | Скидка: ");
                                Console.Write(customersList[i].discountRate);
                                Console.WriteLine();
                            }
                        }
                        else
                        {
                            Console.WriteLine();
                        }                       
                        break;
                    default:
                        foreach (var s in info)
                        {
                            Console.WriteLine(s);
                        }
                        break;     
                }
            }
        }
    }
}
