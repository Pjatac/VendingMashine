using System;

namespace VendingMashine.Models
{
	public class Drink
	{
		public string Name { get; }
		public int Cost { get; }
		public Drink(string name, int cost)
		{
			Name = name;
			if (cost > 0)
				Cost = cost;
			else
				throw new Exception("Cant create drink with negative price");
		}
	}
}
