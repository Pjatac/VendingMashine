namespace VendingMashine.Models
{
	public class Order
	{
		public int Balance { get; set; } = 0;
		public Drink Choice { get; set; }

		public void AddCoin(Coin coin)
		{
			Balance += coin.Value;
		}
	}
}
