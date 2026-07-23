using System;
using System.Text;

namespace ForFreePrimitivesTests
{
	public class Tools
	{
		public static string Dump<T>(T[] arr)
		{
			return Dump(arr, 0, arr.Length - 1);
		}

		public static string Dump<T>(T[] arr, int left)
		{
			return Dump(arr, left, arr.Length - 1);
		}

		public static string Dump<T>(T[] arr, int left, int right)
		{
			var sb = new StringBuilder();
			for (var i = left; i <= Math.Min(right, arr.Length - 1); i += 1)
			{
				if (sb.Length > 0)
					sb.Append(",");
				sb.Append(arr[i]);
			}
			return sb.ToString();
		}
	}
}
