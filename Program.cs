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
            Console.WriteLine("Hello World!");
        }

        private void RandomProductGenerator()
        {



        }

    }

    class Management
    {




    }

    class Buyer
    {
        List<Product> _pocket = new();

        internal int Money { get; private set; }



    }

    class Trader
    {

        List<Product> _store = new();



    }

    class Product
    {
        internal int Cost { get; private set; }
        internal int Weight { get; private set; }
        internal int Volume { get; private set; }

        Product(int cost, int weight, int volume)
        {
            Cost = cost;
            Weight = weight;
            Volume = volume;
        }


    }

}
