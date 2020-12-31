using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMashine.Models
{
	public class SingleMashine
	{
		private static SingleMashine instance;
		public List<Slot> Slots;
		public Order CurrentOrder = new Order();
		private readonly List<Coin> Cashbox;
		public int[] MonetsCount;
		public Coin Cashinput;
		public List<Coin> Cashreturn;
		public int CashSumm { get; private set; } = 0;

		protected SingleMashine(List<Slot> slots, List<Coin> cashbox)
		{
			Cashreturn = new List<Coin>();
			Cashbox = cashbox;
			MonetsCount = new int[Settings.coinsValues.Length];
			for (int i = 0; i < Settings.coinsValues.Length; i++)
			{
				MonetsCount[i] = cashbox.Where(c => c.Value == Settings.coinsValues[i]).Count();
				CashSumm += MonetsCount[i] * Settings.coinsValues[i];
			}
			Slots = slots;
			CurrentOrder = new Order();
		}

		public static SingleMashine GetInstance(List<Slot> slots, List<Coin> cashbox)
		{
            if (instance == null)
            {
                instance = new SingleMashine(slots, cashbox);
            }
            return instance;
		}

		public void SetDrinkToOrder(int drinkNumber)
		{
			if (drinkNumber >= 0 && drinkNumber < Slots.Count)
				CurrentOrder.Choice = Slots[drinkNumber].Type;
			else
				throw new Exception("Slot number error on drink select");
		}
		public int GetCashCount()
		{
			return CashSumm;
		}

		public void AddCoin(Coin coin)
		{
			Cashbox.Add(coin);
			MonetsCount[Array.IndexOf(Settings.coinsValues, coin.Value)]++;
			CashSumm += coin.Value;
		}

		public void GetCoin(Coin coin)
		{
			Cashbox.Remove(coin);
			MonetsCount[Array.IndexOf(Settings.coinsValues, coin.Value)]--;
			CashSumm -= coin.Value;
		}

		public List<Coin> GetChashback(int[] coinsCounts)
		{
			List<Coin> cashback = new List<Coin>();
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < coinsCounts[i]; j++)
					cashback.Add(new Coin(Settings.coinsValues[i]));
			}
			return cashback;
		}

		public Drink Delivery()
		{
			var res = Slots.FirstOrDefault(sl => sl.Type == CurrentOrder.Choice).Delivery();
			if (res.Result)
			{
				return res.Data;
			}
			else
			{
				throw new Exception("Slot delivery error");
			}
		}

		//public Resp<int[]> PrepareCashback(int summ)
		//{
		//	Resp<int[]> resp = new Resp<int[]>(null);
		//	resp.Result = false;
		//	int[] toReturn = new int[3];
		//	int count = summ / Settings.coinsValues[2];
		//	if (MonetsCount[2] >= count)
		//	{
		//		toReturn[2] = count;
		//		summ -= Settings.coinsValues[2] * count;
		//		count = summ / Settings.coinsValues[1];
		//		if (MonetsCount[1] >= count)
		//		{
		//			toReturn[1] = count;
		//			summ -= Settings.coinsValues[1] * count;
		//			count = summ;
		//			if (MonetsCount[0] >= count)
		//			{
		//				toReturn[0] = count;
		//				resp.Result = true;
		//				resp.Data = toReturn;
		//				return resp;
		//			}
		//			else
		//			{
		//				return resp;
		//			}
		//		}
		//		else
		//		{
		//			toReturn[1] = MonetsCount[1];
		//			summ -= Settings.coinsValues[1] * MonetsCount[1];
		//			count = summ;
		//			if (MonetsCount[0] >= count)
		//			{
		//				toReturn[0] = count;
		//				resp.Result = true;
		//				resp.Data = toReturn;
		//				return resp;
		//			}
		//			else
		//			{
		//				return resp;
		//			}
		//		}
		//	}
		//	else
		//	{
		//		toReturn[2] = MonetsCount[2];
		//		summ -= Settings.coinsValues[2] * MonetsCount[2];
		//		count = summ / Settings.coinsValues[1];
		//		if (MonetsCount[1] >= count)
		//		{
		//			toReturn[1] = count;
		//			summ -= Settings.coinsValues[1] * count;
		//			count = summ;
		//			if (MonetsCount[0] >= count)
		//			{
		//				toReturn[0] = count;
		//				resp.Result = true;
		//				resp.Data = toReturn;
		//				return resp;
		//			}
		//			else
		//			{
		//				return resp;
		//			}
		//		}
		//		else
		//		{
		//			toReturn[1] = MonetsCount[1];
		//			summ -= Settings.coinsValues[1] * MonetsCount[1];
		//			count = summ;
		//			if (MonetsCount[0] >= count)
		//			{
		//				toReturn[0] = count;
		//				resp.Result = true;
		//				resp.Data = toReturn;
		//				return resp;
		//			}
		//			else
		//			{
		//				return resp;
		//			}
		//		}
		//	}
		//}
		public Resp<int[]> PrepareCashback(int summ)
		{
            Resp<int[]> resp = new Resp<int[]>(null)
            {
                Result = false
            };
            int[] toReturn = new int[Settings.coinsValues.Length];
            for (int i = toReturn.Length -1; i >= 0 ; i--)
            {
				int currentValueCount = summ / Settings.coinsValues[i];
				if (MonetsCount[i] >= currentValueCount)
				{
					summ -= currentValueCount * Settings.coinsValues[i];
					toReturn[i] = currentValueCount;
				}
				else 
				{
					summ -= MonetsCount[i] * Settings.coinsValues[i];
					toReturn[i] = MonetsCount[i];
				}
				if (summ == 0)
					break;
			}
			if (summ == 0)
			{
				resp.Data = toReturn;
				resp.Result = true;
			}
			return resp;
		}
	}
}
