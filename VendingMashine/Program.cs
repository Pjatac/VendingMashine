using System;
using System.Collections.Generic;
using VendingMashine.Logic;
using VendingMashine.Models;

namespace VendingMashine
{
	class Program
	{
		static void Main(string[] args)
        {
            bool exit = false;
            Manager manager = new Manager(CreateAndFillSlots(), CreateCacheback());
            User user = new User();
            while (!exit)
            {
                Console.WriteLine($"Please, choose a command:\n 0)Exit\n 1)Add coin\n 2)Take from return\n 3)Break operations\n 4)Choose you drink\n 5)Show report\n");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "0":
                        exit = true;
                        break;
                    case "1":
                        string coins = string.Empty;
                        foreach (int value in Settings.coinsValues)
                            coins += value + " ";
                        Console.WriteLine($"Input coin value( {coins})");
                        int coinValue;
                        var input = Int32.TryParse(Console.ReadLine(), out coinValue);
                        if (input)
                        {
                            user.AddToInput(new Coin(coinValue));
                            var res = manager.AddCoinToOrder();
                            if(!res.Result)
                                Console.WriteLine(res);
                        }
                        else
                        {
                            Console.WriteLine("Wrong input");
                        }
                        break;
                    case "2":
                        user.TakeFromReturn();
                        break;
                    case "3":
                        Console.WriteLine(manager.ReturnOrderCash());
                        break;
                    case "4":
                        var drinks = manager.GetDrinks();
                        Console.WriteLine("Please, select");
                        int i = 0;
                        foreach (var drink in drinks)
                        {
                            Console.Write($"{i}) {drink.Key}-{drink.Value} ");
                            i++;
                        }
                        int drinkInput;
                        input = Int32.TryParse(Console.ReadLine(), out drinkInput);
                        if (input && drinkInput >= 0 && drinkInput < manager.GetDrinks().Count)
                        {
                            manager.SetDrinkToOrder(drinkInput);
                            Console.WriteLine(manager.GiveOutOrder());
                        }
                        else
                        {
                            Console.WriteLine("Wrong input");
                        }
                        break;
                    case "5":
                        Console.WriteLine(manager.ShowReport());
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("Unknown operation input");
                        break;
                }
                Console.WriteLine($"Your ballance is: {manager.GetBalance()} Cash return coins count {manager.CashReturnCount()}");
            }
        }

        private static List<Slot> CreateAndFillSlots()
        {
            Slot slot1 = new Slot(new Drink("Beer", 15));
            slot1.Fill();
            Slot slot2 = new Slot(new Drink("Cola", 7));
            slot2.Fill();
            Slot slot3 = new Slot(new Drink("Sprite", 4));
            slot3.Fill();
            Slot slot4 = new Slot(new Drink("Tea", 5));
            slot4.Fill();
            Slot slot5 = new Slot(new Drink("XL", 12));
            slot5.Fill();
            return new List<Slot>() { slot1, slot2, slot3, slot4, slot5 };
        }

        public static List<Coin> CreateCacheback()
        {
            return new List<Coin> { new Coin(1), new Coin(1), new Coin(1), new Coin(5), new Coin(5) };
        }
    }
}
