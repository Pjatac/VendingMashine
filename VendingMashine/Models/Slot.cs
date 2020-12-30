using System.Collections.Generic;

namespace VendingMashine.Models
{
	public class Slot
	{
		int CurrentCapacity { get; set; }
		public Drink Type { get; }

		Queue<Drink> Storage;
		public Slot(Drink drink)
		{
			CurrentCapacity = 0;
			this.Type = drink;
			this.Storage = new Queue<Drink>();
		}
		public int GetCurrentCapacity()
		{
			return CurrentCapacity;
		}
		public void Fill()
		{
			for (int i = CurrentCapacity; i < Settings.MAX_SLOT_CAPACITY; i++)
				Storage.Enqueue(Type);
			CurrentCapacity = Settings.MAX_SLOT_CAPACITY;
		}

		public Resp<Drink> Delivery()
		{
			if (CurrentCapacity > 0)
			{
				CurrentCapacity--;
				var resp = new Resp<Drink>(Storage.Dequeue());
				resp.Result = true;
				return resp;
			}
			else
			{
				var resp = new Resp<Drink>(null);
				resp.Result = false;
				return resp;
			}
		}
	}
}
