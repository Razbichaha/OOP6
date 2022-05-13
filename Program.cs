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
            Management management = new Management();
            Menu menu = new();

            CreateHumans(management);

            bool isContinueCycle = true;

            while (isContinueCycle)
            {
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "к":

                        management.BuyProduct();

                        break;
                    case "пр":


                        break;
                    case "пм":


                        break;
                    case "пи":


                        break;
                    default:

                        management.ShowInwentoryAll();

                        break;
                }
            }
        }
        private static void CreateHumans(Management management)
        {

            int moneySeller = 1000000;
            string nameSeller = "Mamon";
            string characterRoleSeller = "Торговец";

            management.CreateSeller(nameSeller, characterRoleSeller, moneySeller);

            int moneyBuyer = 1000;
            Console.Write("Выберите имя - ");
            string nameBuyer = Console.ReadLine();
            string characterRoleBuyer = "Игрок";

            management.CreateBuyer(nameBuyer, characterRoleBuyer, moneyBuyer);

        }

    }

    class Menu
    {

        internal void OutputHeader()
        {
            Console.Clear();
            Console.WriteLine("Присутствуют следующие команды");
            Console.WriteLine("Купить товар - к");
            Console.WriteLine("Продать товар - пр");
            Console.WriteLine("Показать прилавок торговца - пм");
            Console.WriteLine("Показать ваш инвентарь - пи");
        }

        internal void NotCorrectInput()
        {
            Console.WriteLine("Введено не корректное значение");
        }

    }

    class Management
    {
        private Seller _seller;
        private Buyer _buyer;

        internal void CreateBuyer(string name, string characterRole, int money)
        {
            _buyer = new Buyer(name, characterRole, money);
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
            int quantityMin = 1;
            int quantityMax = 1000;
            int weightMin = 1;
            int weightMax = 100;
            int volumeMin = 1;
            int volumeMax = 10;
            int numberProducts = 10;
            string name = "Товар - ";

            List<Product> products = new();
            Random random = new();

            for (int i = 0; i < numberProducts; i++)
            {

                int cost = random.Next(costMin, costMax);
                int wieght = random.Next(weightMin, weightMax);
                int volume = random.Next(volumeMin, volumeMax);
                int quantity = random.Next(quantityMin, quantityMax);

                Product product = new(name + i.ToString(), quantity, cost, wieght, volume);
                products.Add(product);
            }

            return products;
        }

        internal void ShowInwentoryAll()
        {
            Menu menu = new();
            Console.Clear();

            menu.OutputHeader();

            Console.WriteLine("Асортимент продавца - " + _seller.Name + "\n");

            _seller.ShowInventory();

            Console.WriteLine("\nИнвентарь господина " + _buyer.Name);

            _buyer.ShowInventory();
            Console.WriteLine("\nВ кошельке - " + _buyer.Money + " Денег");
        }

        internal void BuyProduct()
        {
            Console.Write("Выберите товар который хотите купить - ");
            string product;
            product = Console.ReadLine();
            if (_seller.IsThereProduct(product))
            {
                AomuntProduct(product);
            }
            else
            {
                Console.WriteLine("Выбранный товар отсутствует.");
            }
        }

        private void AomuntProduct(string product)
        {
            Menu menu = new();

            int amount = 0;
            Console.Write("Сколько товара вам нужно - ");
            string amountString = Console.ReadLine();
            if (IsNumber(amountString, ref amount))
            {
                if (amount > 0)
                {
                    int buerMoney = _buyer.Money;
                    _seller.Transaction(product, amount, ref buerMoney);
                    if (_buyer.IsThereProduct(product))
                    {


                    }else
                    {
                        Product tempProduct=new()
                        _buyer.ProductAdd()

                    }
                }
                else
                { menu.NotCorrectInput(); }
            }
            else
            { menu.NotCorrectInput(); }
        }

        internal bool IsNumber(string text, ref int number)
        {
            bool isNumber = int.TryParse(text, out number);

            return isNumber;
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
                Console.WriteLine(item.Name + " Колличество - " + item.Quantity + "  Цена - " + item.Cost + " Вес - " + item.Weight + " Объем - " + item.Volume);
            }
        }

        internal bool IsThereProduct(string productCheck)
        {
            bool isProduct = false;

            foreach (Product product in _inventory)
            {
                if (product.Name == productCheck)
                {
                    isProduct = true;
                }
            }


            return isProduct;
        }

        //internal bool IsNumber(string text, ref int number)
        //{
        //    bool isNumber = int.TryParse(text, out number);

        //    return isNumber;
        //}
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

        internal bool Transaction(string product, int amount, ref int money)
        {
            bool transactionSuccessful = false;

            foreach (Product tempProduct in _inventory)
            {
                if (tempProduct.Name == product)
                {
                    int price = tempProduct.Quantity * amount;
                    if (price <= money)
                    {
                        if (tempProduct.QuantityAdd(amount))
                        {
                            money -= price;
                        } else
                        {
                            Console.WriteLine("Операция отклонена, не достаточное колличество товара");
                        }
                    }
                    else
                    {
                        Console.WriteLine("У вас не достаточно денег для покупки");
                    }
                    break;
                }
            }

            return transactionSuccessful;
        }

    }

    class Product
    {
        internal string Name { get; private set; }
        internal int Cost { get; private set; }
        internal int Weight { get; private set; }
        internal int Volume { get; private set; }
        internal int Quantity { get; private set; }

        internal Product(string name, int quantity, int cost, int weight, int volume)
        {
            Name = name;
            Quantity = quantity;
            Cost = cost;
            Weight = weight;
            Volume = volume;
        }

        internal bool QuantityAdd(int volume)
        {
            bool isEnoughGuantity = false;

            if (Volume>=volume)
            {
                Volume -= volume;
                isEnoughGuantity = true;
            }
            return isEnoughGuantity;
        }
    }
}
