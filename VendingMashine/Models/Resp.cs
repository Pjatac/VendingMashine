namespace VendingMashine.Models
{
	public class Resp<T>
	{
		public bool Result { get; set; }
		public T Data; 
		public Resp(T data)
		{
			Data = data;
		}
        public override string ToString()
        {
            return Data.ToString();
        }
    }
}
