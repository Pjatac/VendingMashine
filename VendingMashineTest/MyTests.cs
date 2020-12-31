using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMashine.Logic;
using VendingMashine.Models;

namespace VendingMashineTest
{
    [TestClass]
	public class MashineTests
	{
		public Manager manager;
		public User user;
		public MashineTests()
		{
			Init();
		}
		public void Init()
		{
			var slot = new Slot(new Drink("drink", 1));
			slot.Fill();
			var slot1 = new Slot(new Drink("drink1", 7));
			slot1.Fill();
			manager = new Manager(new List<Slot>() { slot, slot1 }, new List<Coin>());
			user = new User();
		}
        [TestMethod]
        public void TestFreeSlot()
        //Current order of our mashine must set Choice to null and stay Ballance without changes
        {
            for (int i = 0; i <= Settings.MAX_SLOT_CAPACITY; i++)
            {
                user.AddToInput(new Coin(1));
                manager.AddCoinToOrder();
                manager.SetDrinkToOrder(0);
                manager.GiveOutOrder();
            }
            Assert.IsNull(manager.Device.CurrentOrder.Choice);
            Assert.IsTrue(manager.Device.CurrentOrder.Balance == 1);
            manager.Device.CurrentOrder.Balance = 0;
			manager.Device.Slots[0].Fill();
        }

        [TestMethod]
        public void TestUnknownCoin()
        //Current order of our mashine must stay Ballance without changes, and unknown coin move to cashreturn
        {
			Coin coin = new Coin(2);
            user.AddToInput(coin);
            manager.AddCoinToOrder();
            Assert.AreEqual(manager.Device.CurrentOrder.Balance, 0);
            var backCoin = manager.Device.Cashreturn[0];
            Assert.AreEqual(backCoin, coin);
            manager.Device.Cashreturn = new List<Coin>();
        }

        [TestMethod]
        public void TestCashbacks1()
        {
			manager.Device.AddCoin(new Coin(1));
            manager.Device.CurrentOrder.Balance = 8;
            manager.SetDrinkToOrder(1);
            manager.GiveOutOrder();
            var coin = new Coin(1);
            var backCoin = manager.Device.Cashreturn[0];
            Assert.IsTrue(coin.Value == backCoin.Value);
            manager.Device.Cashreturn = new List<Coin>();
        }
        [TestMethod]
		public void TestCashbacks2()
		{
			Init();
			manager.Device.AddCoin(new Coin(1));
			manager.Device.AddCoin(new Coin(1));
			manager.Device.CurrentOrder.Balance = 9;
			manager.SetDrinkToOrder(1);
			manager.GiveOutOrder();
			var coins = new List<Coin>() { new Coin(1) , new Coin(1) };
			for (int i = 0; i < 2; i++)
				Assert.IsTrue(coins[i].Value == manager.Device.Cashreturn[i].Value);
			manager.Device.Cashreturn = new List<Coin>();
			manager.Device.Slots[1].Fill();
		}
		[TestMethod]
		public void TestCashbacks5()
		{
			Init();
			manager.Device.AddCoin(new Coin(5));
			manager.Device.CurrentOrder.Balance = 12;
			manager.SetDrinkToOrder(1);
			manager.GiveOutOrder();
			var coin = new Coin(5);
			var backCoin = manager.Device.Cashreturn[0];
			Assert.IsTrue(coin.Value == backCoin.Value);
			manager.Device.Cashreturn = new List<Coin>();
			manager.Device.Slots[1].Fill();
		}
        [TestMethod]
        public void TestCashbacks10()
        {
			manager.Device.AddCoin(new Coin(10));
            manager.Device.CurrentOrder.Balance = 17;
            manager.SetDrinkToOrder(1);
            manager.GiveOutOrder();
            var coin = new Coin(10);
            var backCoin = manager.Device.Cashreturn[0];
            Assert.IsTrue(coin.Value == backCoin.Value);
            manager.Device.Cashreturn = new List<Coin>();
			manager.Device.Slots[1].Fill();
		}
        [TestMethod]
		public void TestCashbacks5plus1()
		{
			manager.Device.AddCoin(new Coin(5));
			manager.Device.AddCoin(new Coin(1));
			manager.Device.CurrentOrder.Balance = 13;
			manager.SetDrinkToOrder(1);
			manager.GiveOutOrder();
			var coins = new List<Coin>() { new Coin(1), new Coin(5) };
			for (int i = 0; i < 2; i++)
				Assert.IsTrue(coins[i].Value == manager.Device.Cashreturn[i].Value);
			manager.Device.Cashreturn = new List<Coin>();
			manager.Device.Slots[1].Fill();
		}
        [TestMethod]
        public void TestCashbacks10plus1()
        {
			manager.Device.AddCoin(new Coin(10));
            manager.Device.AddCoin(new Coin(1));
            manager.Device.CurrentOrder.Balance = 18;
            manager.SetDrinkToOrder(1);
            manager.GiveOutOrder();
            var coins = new List<Coin>() { new Coin(1), new Coin(10) };
            for (int i = 0; i < 2; i++)
                Assert.IsTrue(coins[i].Value == manager.Device.Cashreturn[i].Value);
            manager.Device.Cashreturn = new List<Coin>();
			manager.Device.Slots[1].Fill();
		}
        [TestMethod]
		public void TestCashbacks10plus5()
		{
			manager.Device.AddCoin(new Coin(10));
			manager.Device.AddCoin(new Coin(5));
			manager.Device.CurrentOrder.Balance = 22;
			manager.SetDrinkToOrder(1);
			manager.GiveOutOrder();
			var coins = new List<Coin>() { new Coin(5), new Coin(10) };
			for (int i = 0; i < 2; i++)
				Assert.IsTrue(coins[i].Value == manager.Device.Cashreturn[i].Value);
			manager.Device.Cashreturn = new List<Coin>();
			manager.Device.Slots[1].Fill();
		}
		[TestMethod]
		public void TestCashbacks10plus5plus1()
		{
			Init();
			manager.Device.AddCoin(new Coin(10));
			manager.Device.AddCoin(new Coin(5));
			manager.Device.AddCoin(new Coin(1));
			manager.Device.CurrentOrder.Balance = 23;
			manager.SetDrinkToOrder(1);
			manager.GiveOutOrder();
			var coins = new List<Coin>() { new Coin(1), new Coin(5), new Coin(10) };
			for (int i = 0; i < 3; i++)
				Assert.IsTrue(coins[i].Value == manager.Device.Cashreturn[i].Value);
			manager.Device.Cashreturn = new List<Coin>();
			manager.Device.Slots[1].Fill();
		}
	}
}
