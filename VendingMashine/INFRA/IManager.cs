using System.Collections.Generic;
using VendingMashine.Models;

namespace VendingMashine.INFRA
{
	public interface IManager
	{
		//List of drinks with price for represent
		Dictionary<string, int> GetDrinks();
		void SetDrinkToOrder(int drinkNumber);
		void RemoveDrinkFromOrder();
		Resp<List<Coin>> ReturnCashback(int backSumm);
		string GiveOutOrder();
		bool CheckDrinkCount();

		bool CheckDrinkCost();
		void FillDrinks();
		
		Coin TakeFromInput();

		Resp<string> AddCoinToOrder();
		Coin GetCoinFromCashbox(Coin coin);
		void AddCoinToReturn(Coin coin);

		int GetBalance();
		void AddCoinsToReturn(List<Coin> coins);
		string ReturnOrderCash();

		bool CheckCoin(Coin coin);
		int CashReturnCount();

		Resp<string> ShowReport();
	}
}
