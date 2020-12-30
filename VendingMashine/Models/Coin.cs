using System;

namespace VendingMashine.Models
{
	public class Coin: IEquatable<Coin>
	{
		public int Value { get; }
		public Coin(int value)
		{
			Value = value;
		}

		public bool Equals(Coin other)
		{
			return  Value.Equals(other.Value);
		}
	}
}
