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
            var database = client.GetDatabase("MyTestDB");
            IBDController controller = new BDController();
            controller.Init(database);
            List<String> info = new List<string>();
            info.Add("0. Выход");
            info.Add("1. Отправка предпочтения");
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
            while (true)
            {
                line = Console.ReadLine();
                switch (line)
                {
                    case "0":
                        return;
                    case "3":
                        Console.WriteLine("Модель:");
                        String inputModel = Console.ReadLine();
                        Console.WriteLine("Добавить свойство? (0 - нет, 1 - да)");
                        List<CarProperty> inputCarProperties = new List<CarProperty>();
                        String addProp = Console.ReadLine();
                        if (addProp=="1")
                        {
                            List<String> inputStringsProperties = new List<string>();
                            List<CarProperty> propertiesList = controller.GetAllCarProperties();
                            while (addProp == "1")
                            {
                                Console.WriteLine("Имеющиеся свойства:");
                                foreach (var p in propertiesList)
                                {
                                    Console.WriteLine(p.description);
                                }
                                inputStringsProperties.Add(Console.ReadLine());
                                Console.WriteLine("Добавить свойство? (0 - нет, 1 - да)");
                                addProp = Console.ReadLine();
                            }
                            foreach (var s in inputStringsProperties)
                            {
                                inputCarProperties.Add(controller.AddCarProperty(new CarProperty { description = s }));
                            }
                        }
                        Console.WriteLine("Стоимость проката за день:");
                        double inputPrice = Convert.ToDouble(Console.ReadLine());
                        controller.AddCar(new Car { model = inputModel, properties = inputCarProperties, pricePerDay = inputPrice });
                        break;
                    case "6":
                        List<Car> carsList = controller.GetAllCars();
                        for (int i=0; i<carsList.Count();i++)
                        {
                            Console.Write(i+1);
                            Console.Write(". Модель: ");
                            Console.Write(carsList[i].model);
                            Console.Write(" |Свойства: ");
                            List<CarProperty> propList = carsList[i].properties;
                            if (propList!=null)
                            {
                                for (int j = 0; j < propList.Count() - 1; j++)
                                {
                                    Console.Write(propList[j].description);
                                    Console.Write(", ");
                                }
                                Console.Write(propList.Last().description);
                            }  
                            Console.Write(" |Стоимость проката за день: ");
                            Console.Write(carsList[i].pricePerDay);
                            Console.WriteLine();
                        }
                        break;
                    case "11":
                        List<Customer> customersList = controller.GetAllCustomers();
                        if (customersList!=null)
                        {
                            for (int i = 0; i < customersList.Count(); i++)
                            {
                                Console.Write(i+1);
                                Console.Write(". Имя: ");
                                Console.Write(customersList[i].name);
                                Console.Write(" Фамилия: ");
                                Console.Write(customersList[i].surname);
                                Console.Write(" Отчество: ");
                                Console.Write(customersList[i].patronymic);
                                Console.Write(" Паспорт: ");
                                Console.Write(customersList[i].passport);
                                Console.Write(" Телефонный номер: ");
                                Console.Write(customersList[i].phoneNumber);
                                Console.Write(" Скидка: ");
                                Console.Write(customersList[i].discountRate);
                            }
                        }
                        Console.WriteLine();
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
