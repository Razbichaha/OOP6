using System;
using System.Collections.Generic;

namespace OOP6
{
    //    Существует продавец, он имеет у себя список товаров,
    //    и при нужде, может вам его показать,
    //    также продавец может продать вам товар.
    //    После продажи товар переходит к вам, и вы можете также посмотреть свои вещи.

    //Возможные классы – игрок, продавец, товар.

    //Вы можете сделать так, как вы видите это.

    class Program
    {


        static void Main(string[] args)
        {
            int moneyBuyer = 1000;
            int moneySeller = 1000000;




            bool isContinueCycle = true;

            while (isContinueCycle)
            {
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "добавить":


                        break;
                    case "удалить":


                        break;
                    case "показать как":


                        break;
                    case "показать":


                        break;
                    default:


                        break;
                }
            }
        }
    }

    class Management
    {
        private Human _seller;
        private Human __buyer;

        internal void CreateBuyer()
        {


        }

        internal void CreateSeller(string name, string characterRole, int money)
        {
            _seller = new Seller(name, characterRole, money);

            _seller.InventoryAdd(RandomProductGenerator());

        }

        private List<Product> RandomProductGenerator()
        {
            int costMin = 1;
            int costMax = 1000;
            int weightMin = 1;
            int weightMax = 100;
            int volumeMin = 1;
            int volumeMax = 10;
            int numberProducts = 10;
            string name = "Товар ";

            List<Product> products = new();
            Random random = new();

            for (int i = 0; i < numberProducts; i++)
            {
                name += (i + 1);
                int cost = random.Next(costMin, costMax);
                int wieght = random.Next(weightMin, weightMax);
                int volume = random.Next(volumeMin, volumeMax);

                Product product = new(name, cost, wieght, volume);
                products.Add(product);
            }

            return products;
        }

        internal void Show()
        {



        }

    }

    abstract class Human
    {
        protected List<Product> _inventory = new List<Product>();

        internal string Name { get; private set; }

        internal string CharacterRole { get; private set; }

        internal int Money { get; private set; }

        internal Human(string name, string characterRole, int money)
        {
            Name = name;
            CharacterRole = characterRole;
            Money = money;
        }

        internal void MoneyAdd(int money)
        {
            Money = money;
        }

        internal void ProductAdd(Product product)
        {
            _inventory.Add(product);
        }

        internal void InventoryAdd(List<Product> products)
        {
            _inventory = products;
        }

        internal void ShowInventory()
        {
            foreach (var item in _inventory)
            {
                Console.WriteLine(item.Name + " Цена - " + item.Cost + " Вес - " + " Объем - " + item.Volume);
            }
        }
    }

    internal class Buyer : Human
    {
        public Buyer(string name, string characterRole, int money) : base(name, characterRole, money)
        {
        }


    }

    internal class Seller : Human
    {
        public Seller(string name, string characterRole, int money) : base(name, characterRole, money)
        {
        }

    }

    class Product
    {
        internal string Name { get; private set; }
        internal int Cost { get; private set; }
        internal int Weight { get; private set; }
        internal int Volume { get; private set; }

        internal Product(string name, int cost, int weight, int volume)
        {
            Name = name;
            Cost = cost;
            Weight = weight;
            Volume = volume;
        }
    }

}
