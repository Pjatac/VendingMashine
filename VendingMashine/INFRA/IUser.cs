using VendingMashine.Models;

namespace VendingMashine.INFRA
{
	public interface IUser
	{
		void AddToInput(Coin coin);
		void TakeFromReturn();
	}
}
