using System;
using System.Collections.Generic;
using System.Linq;
using VendingMashine.Models;
using VendingMashine.INFRA;
using System.Text;

namespace VendingMashine.Logic
{
	public class Manager : IManager
	{
		//only for tests
		public SingleMashine Device;
		private StringBuilder sb = new StringBuilder();
		public Manager(List<Slot> slots, List<Coin> cashbox)
		{
			Device = SingleMashine.getInstance(slots, cashbox);
		}
		public Dictionary<string, int> GetDrinks()
		{
			Dictionary<string, int> drinks = new Dictionary<string, int>();
			foreach (var slot in Device.Slots)
			{
				drinks.Add(slot.Type.Name, slot.Type.Cost);
			}
			return drinks;
		}
		public void SetDrinkToOrder(int drinkNumber)
		{
			if (drinkNumber >= 0 && drinkNumber < Device.Slots.Count)
				Device.SetDrinkToOrder(drinkNumber);
			else
				throw new Exception("Wrong drink select");
		}
		public void RemoveDrinkFromOrder()
		{
			Device.CurrentOrder.Choice = null;
		}
		public Resp<List<Coin>> ReturnCashback(int backSumm)
		{
			var toReturn = Device.PrepareCashback(backSumm);
			if (toReturn.Result)
			{
				List<Coin> coins = Device.GetChashback(toReturn.Data);
				var resp = new Resp<List<Coin>>(coins);
				resp.Result = true;
				return resp;
			}
			else
			{
				var resp = new Resp<List<Coin>>(null);
				return resp;
			}
		}
		public string GiveOutOrder()
		{
			if (CheckDrinkCost())
				if (CheckDrinkCount())
				{
					int cashback = Device.CurrentOrder.Balance - Device.CurrentOrder.Choice.Cost;
					if (cashback > 0)
					{
						var resp = ReturnCashback(cashback);
						if (resp.Result)
						{
							AddCoinsToReturn(resp.Data);
							var toGiveOut = Device.Delivery();
							Device.CurrentOrder = new Order();
							return $"Please, take your cashback and take your {toGiveOut.Name}";
						}
						else 
						{
							return "Sorry, we can not give cacheback.";
						}
					}
					else
					{
						var toGiveOut = Device.Delivery();
						Device.CurrentOrder = new Order();
						return $"Please, take your {toGiveOut.Name}";
					}
				}
				else
				{
					RemoveDrinkFromOrder();
					return "Sorry, this drink was ended";
				}
			else
			{
				RemoveDrinkFromOrder();
				return "Sorry, not enough money";
			}
		}
		public bool CheckDrinkCount()
		{
			return (Device.Slots.FirstOrDefault(sl => sl.Type == Device.CurrentOrder.Choice).GetCurrentCapacity() > 0);
		}

		public bool CheckDrinkCost()
		{
			return (Device.CurrentOrder.Choice.Cost <= Device.CurrentOrder.Balance);
		}
		public void FillDrinks()
		{
			foreach (var slot in Device.Slots)
			{
				slot.Fill();
			}
		}

		public Coin TakeFromInput()
		{
			Coin coin = Device.Cashinput;
			Device.Cashinput = null;
			return coin;
		}

		public Resp<string> AddCoinToOrder()
		{
			Coin coin = TakeFromInput();
			if (CheckCoin(coin))
			{
				Device.CurrentOrder.AddCoin(coin);
				Device.AddCoin(coin);
				var resp = new Resp<string>("Ok");
				resp.Result = true;
				return resp;
			}
			else
			{
				AddCoinToReturn(coin);
				var resp = new Resp<string>("Sorry, unusable coin");
				return resp;
			}			
		}
		public Coin GetCoinFromCashbox(Coin coin)
		{
			Device.GetCoin(coin);
			return coin;
		}
		public void AddCoinToReturn(Coin coin)
		{
			Device.Cashreturn.Add(coin);
		}

		public int GetBalance()
		{
			return Device.CurrentOrder.Balance;
		}
		public void AddCoinsToReturn(List<Coin> coins)
		{
			foreach (Coin coin in coins)
			{
				GetCoinFromCashbox(coin);
				AddCoinToReturn(coin);
			}
		}
		public string ReturnOrderCash()
		{
			if (Device.CurrentOrder.Balance > 0)
			{
				var toReturn = Device.PrepareCashback(Device.CurrentOrder.Balance);
				if (toReturn.Result)
				{
					List<Coin> coins = Device.GetChashback(toReturn.Data);
					AddCoinsToReturn(coins);
					Device.CurrentOrder = new Order();
					return "Please, take your money";
				}
				else
				{
					return "Sorry, we cannot give cashback";
				}
			}
			return "Your ballance is 0";
		}

		public bool CheckCoin(Coin coin)
		{
			return Settings.coinsValues.Contains(coin.Value);
		}

		public int CashReturnCount()
		{
			return Device.Cashreturn.Count();
		}

		public Resp<string> ShowReport()
		{
			sb.Clear();
			sb.Append($"Cash: {Device.GetCashCount()} \n");
			int i = 0;
			foreach (var slot in Device.Slots)
			{
				sb.Append($"Slot[{i}] with {slot.Type.Name} ({slot.GetCurrentCapacity()} pcs) \n");
				i++;
			}
			return new Resp<string>(sb.ToString());
		}
	}
}
