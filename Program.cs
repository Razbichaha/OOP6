using System;
using System.Collections.Generic;

namespace OOP6
{
    class Program
    {
        static void Main(string[] args)
        {
            Management management = new Management();

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

                        management.SellProduct();

                        break;
                    case "пм":

                        management.ShowStore();

                        break;
                    case "пи":

                        management.ShowInventoryBuyer();

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

        internal void ShowNotCorrectInput()
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
            _seller.InventoryAdd(GenerateRandomProducts());
        }

        private List<Product> GenerateRandomProducts()
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
            string name = "Т";

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

            Console.WriteLine("\nАсортимент продавца - " + _seller.Name + "\n");

            _seller.ShowInventory();

            Console.WriteLine("\nИнвентарь господина " + _buyer.Name+"\n");

            _buyer.ShowInventory();
            Console.Write("\nВ кошельке - " + _buyer.Money + " Денег\n");
        }

        internal void ShowInventoryBuyer()
        {
            Menu menu = new();
            Console.Clear();
            menu.OutputHeader();
            _buyer.ShowInventory();
        }

        internal void ShowStore()
        {
            Menu menu = new();
            Console.Clear();
            menu.OutputHeader();
            _seller.ShowInventory();
        }

        internal void BuyProduct()
        {
            Console.Write("Выберите товар который хотите купить - ");

            string product;
            product = Console.ReadLine();

            if (_seller.IsThereProduct(product))
            {
                BuyTransaction(product);
            }
            else
            {
                Console.WriteLine("Выбранный товар отсутствует.");
            }

            ShowInwentoryAll();
        }

        private void BuyTransaction(string product)
        {
            Menu menu = new();

            int quantityProduct = 0;
            Console.Write("Сколько товара вам нужно - ");
            string numberString = Console.ReadLine();

            if (IsNumber(numberString, ref quantityProduct))
            {
                if (quantityProduct > 0)
                {
                    int buyerMoney = _buyer.Money;
                    string name = "";
                    int quantity = 0;
                    int cost = 0;
                    int weight = 0;
                    int volume = 0;
                    Product tempProduct;
                    int cap = 100000;

                    _seller.SellTransaction(product, quantityProduct, ref cap);

                    if (_buyer.IsThereProduct(product))
                    {
                        _buyer.BuyTransaction(product, quantityProduct, ref buyerMoney);
                        _buyer.MoneyAdd(buyerMoney);
                    }
                    else
                    {
                        _seller.GetValuesProduct(product, ref name, ref quantity, ref cost, ref weight, ref volume);

                        buyerMoney -= cost * quantityProduct;
                        _buyer.MoneyAdd(buyerMoney);

                        tempProduct = new(name, quantityProduct, cost, weight, volume);
                        _buyer.ProductAdd(tempProduct);
                    }
                }
                else
                { menu.ShowNotCorrectInput(); }
            }
            else
            { menu.ShowNotCorrectInput(); }
        }

        internal void SellProduct()
        {
            Console.Write("Выберите товар который хотите продать - ");
            string product;
            product = Console.ReadLine();

            if (_seller.IsThereProduct(product))
            {
                Console.Write("Сколько товара вы хотите продать - ");
                string numberString = Console.ReadLine();
                int quantityProduct = 0;

                IsNumber(numberString, ref quantityProduct);

                int buyerMoney = _buyer.Money;

                _buyer.SellTransaction(product, quantityProduct, ref buyerMoney);
                _buyer.MoneyAdd(buyerMoney);

                int moneySeller = _seller.Money;

                _seller.BuyTransaction(product, quantityProduct, ref moneySeller);
            }
            else
            {
                Console.WriteLine("Выбранный товар отсутствует.");
            }

            _buyer.ClearColection();
            ShowInwentoryAll();
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
            foreach (var product in _inventory)
            {
                Console.WriteLine("Товар - "+product.Name + " Колличество - " + product.Quantity + "  Цена - " + product.Cost + " Вес - " + product.Weight + " Объем - " + product.Volume);
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

        internal void GetValuesProduct(string product, ref string name, ref int quantity, ref int cost, ref int weight, ref int volume)
        {
            foreach (Product item in _inventory)
            {
                if (item.Name == product)
                {
                    name = item.Name;
                    quantity = item.Quantity;
                    cost = item.Cost;
                    weight = item.Weight;
                    volume = item.Volume;
                }
            }
        }

        internal void SellTransaction(string product, int quantity, ref int money)
        {
            foreach (Product tempProduct in _inventory)
            {
                if (tempProduct.Name == product)
                {
                    int tempQuantity = tempProduct.Quantity;

                    if (tempQuantity >= quantity & quantity > 0)
                    {
                        tempQuantity -= quantity;
                        tempProduct.Quantity = tempQuantity;
                        money += (tempProduct.Cost * quantity);
                    }
                    else
                    {
                        Console.WriteLine("Количество менше или равно 0.");
                    }
                    break;
                }
            }
        }

        internal bool BuyTransaction(string product, int quantity, ref int money)
        {
            bool transactionSuccessful = false;

            foreach (Product tempProduct in _inventory)
            {
                if (tempProduct.Name == product)
                {
                    int price = tempProduct.Cost * quantity;
                    if (price <= money & quantity > 0)
                    {
                        tempProduct.Quantity += quantity;
                        money -= price;
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

        internal void ClearColection()
        {
            for (int i = 0; i < _inventory.Count; i++)
            {
                if (_inventory[i].Quantity==0)
                {
                    _inventory.RemoveAt(i);
                }
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
        internal int Quantity { get; set; }

        internal Product(string name, int quantity, int cost, int weight, int volume)
        {
            Name = name;
            Quantity = quantity;
            Cost = cost;
            Weight = weight;
            Volume = volume;
        }
    }
}
