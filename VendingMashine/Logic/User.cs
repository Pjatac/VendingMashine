using System;
using System.Collections.Generic;
using VendingMashine.Models;
using VendingMashine.INFRA;

namespace VendingMashine.Logic
{
	public class User: IUser
	{
		protected SingleMashine Device;
		public User()
		{
			Device = SingleMashine.GetInstance(null, null);
		}
		public void AddToInput(Coin coin)
		{
			if (Device.Cashinput == null)
				Device.Cashinput = coin;
			else
				throw new Exception("Sorry, the receiver broken");
		}
		public void TakeFromReturn()
		{
			if (Device.Cashreturn.Count > 0)
			{
				Console.WriteLine("You take: ");
				foreach (Coin coin in Device.Cashreturn)
					Console.Write($"{coin.Value} ");
				Device.Cashreturn = new List<Coin>();
				Console.WriteLine();
			}
		}
	}
}
