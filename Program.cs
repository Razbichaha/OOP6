using System;
using System.Collections.Generic;

namespace OOP6
{
    class Program
    {
        static void Main(string[] args)
        {
            Logic logic = new Logic();
            CreateHumans(logic);
            bool isContinueCycle = true;

            while (isContinueCycle)
            {
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "к":

                        logic.BuyProduct();

                        break;
                    case "пр":

                        logic.SellProduct();

                        break;
                    case "пм":

                        logic.ShowStore();

                        break;
                    case "пи":

                        logic.ShowInventoryBuyer();

                        break;
                    default:

                        logic.ShowInwentoryAll();

                        break;
                }
            }
        }
        private static void CreateHumans(Logic logic)
        {
            int moneySeller = 1000000;
            string nameSeller = "Mamon";
            string characterRoleSeller = "Торговец";
            logic.CreateSeller(nameSeller, characterRoleSeller, moneySeller);
            int moneyBuyer = 1000;
            Console.Write("Выберите имя - ");
            string nameBuyer = Console.ReadLine();
            string characterRoleBuyer = "Игрок";
            logic.CreateBuyer(nameBuyer, characterRoleBuyer, moneyBuyer);
        }
    }

    class Menu
    {
        internal void ShowOutputHeader()
        {
            Console.Clear();
            Console.WriteLine("Присутствуют следующие команды");
            Console.WriteLine("Купить товар - к");
            Console.WriteLine("Продать товар - пр");
            Console.WriteLine("Показать прилавок торговца - пм");
            Console.WriteLine("Показать ваш инвентарь - пи");
        }

        internal void ShowNotMessagCorrectInput()
        {
            Console.WriteLine("Введено не корректное значение");
        }
    }

    class Logic
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

        private List<ProductCell> GenerateRandomProducts()
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

            List<ProductCell> products = new();
            Random random = new();

            for (int i = 0; i < numberProducts; i++)
            {
                int cost = random.Next(costMin, costMax);
                int wieght = random.Next(weightMin, weightMax);
                int volume = random.Next(volumeMin, volumeMax);
                int quantity = random.Next(quantityMin, quantityMax);
                ProductCell product = new(name + i.ToString(), quantity, cost, wieght, volume);
                products.Add(product);
            }
            return products;
        }

        internal void ShowInwentoryAll()
        {
            Menu menu = new();
            Console.Clear();
            menu.ShowOutputHeader();
            Console.WriteLine("\nАсортимент продавца - " + _seller.Name + "\n");
            _seller.ShowInventory();
            Console.WriteLine("\nИнвентарь господина " + _buyer.Name + "\n");
            _buyer.ShowInventory();
            Console.Write("\nВ кошельке - " + _buyer.Money + " Денег\n");
        }

        internal void ShowInventoryBuyer()
        {
            Menu menu = new();
            Console.Clear();
            menu.ShowOutputHeader();
            _buyer.ShowInventory();
        }

        internal void ShowStore()
        {
            Menu menu = new();
            Console.Clear();
            menu.ShowOutputHeader();
            _seller.ShowInventory();
        }

        internal void BuyProduct()
        {
            Console.Write("Выберите товар который хотите купить - ");
            string product;
            product = Console.ReadLine();

            if (_seller.IsThereProduct(product))
            {
                MakeBuyTransaction(product);
            }
            else
            {
                Console.WriteLine("Выбранный товар отсутствует.");
            }
            ShowInwentoryAll();
        }

        private void MakeBuyTransaction(string product)
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
                    ProductCell tempProduct;
                    int cap = 100000;

                    _seller.MakeSellTransaction(product, quantityProduct, ref cap);

                    if (_buyer.IsThereProduct(product))
                    {
                        _buyer.MakeBuyTransaction(product, quantityProduct, ref buyerMoney);
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
                {
                    menu.ShowNotMessagCorrectInput();
                }
            }
            else
            {
                menu.ShowNotMessagCorrectInput();
            }
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
                _buyer.MakeSellTransaction(product, quantityProduct, ref buyerMoney);
                _buyer.MoneyAdd(buyerMoney);
                int moneySeller = _seller.Money;
                _seller.MakeBuyTransaction(product, quantityProduct, ref moneySeller);
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
        protected List<ProductCell> Inventory = new List<ProductCell>();

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

        internal void ProductAdd(ProductCell product)
        {
            Inventory.Add(product);
        }

        internal void InventoryAdd(List<ProductCell> products)
        {
            Inventory = products;
        }

        internal void ShowInventory()
        {
            foreach (var product in Inventory)
            {
                Console.WriteLine("Товар - " + product.Name + " Колличество - " + product.ReturnQuantity() + "  Цена - " + product.Cost + " Вес - " + product.Weight + " Объем - " + product.Volume);
            }
        }

        internal bool IsThereProduct(string productCheck)
        {
            bool isProduct = false;

            foreach (ProductCell product in Inventory)
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
            foreach (ProductCell item in Inventory)
            {
                if (item.Name == product)
                {
                    name = item.Name;
                    quantity = item.ReturnQuantity();
                    cost = item.Cost;
                    weight = item.Weight;
                    volume = item.Volume;
                }
            }
        }

        internal void MakeSellTransaction(string product, int quantity, ref int money)
        {
            foreach (ProductCell tempProduct in Inventory)
            {
                if (tempProduct.Name == product)
                {
                    int tempQuantity = tempProduct.ReturnQuantity();

                    if (tempQuantity >= quantity & quantity > 0)
                    {
                        tempQuantity -= quantity;
                        tempProduct.ChangeQuantity(tempQuantity);
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

        internal bool MakeBuyTransaction(string product, int quantity, ref int money)
        {
            bool transactionSuccessful = false;

            foreach (ProductCell tempProduct in Inventory)
            {
                if (tempProduct.Name == product)
                {
                    int price = tempProduct.Cost * quantity;
                    if (price <= money & quantity > 0)
                    {
                        int tempQuantity = tempProduct.ReturnQuantity() + quantity;
                        tempProduct.ChangeQuantity(tempQuantity);
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
            for (int i = 0; i < Inventory.Count; i++)
            {
                if (Inventory[i].ReturnQuantity() == 0)
                {
                    Inventory.RemoveAt(i);
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

    class ProductCell
    {
        private int _quantity;
        internal string Name { get; private set; }
        internal int Cost { get; private set; }
        internal int Weight { get; private set; }
        internal int Volume { get; private set; }

        internal ProductCell(string name, int quantity, int cost, int weight, int volume)
        {
            Name = name;
            _quantity = quantity;
            Cost = cost;
            Weight = weight;
            Volume = volume;
        }

        internal void ChangeQuantity(int quantity)
        {
            _quantity = quantity;
        }

        internal int ReturnQuantity()
        {
            return _quantity;
        }
    }
}
