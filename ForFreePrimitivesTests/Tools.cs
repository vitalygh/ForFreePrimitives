using System;
using System.Text;

namespace ForFreePrimitivesTests
{
	public class Tools
	{
		public static string Dump(int[] nums)
		{
			return Dump(nums, 0, nums.Length - 1);
		}

		public static string Dump(int[] nums, int left)
		{
			return Dump(nums, left, nums.Length - 1);
		}

		public static string Dump(int[] nums, int left, int right)
		{
			var sb = new StringBuilder();
			for (var i = left; i <= Math.Min(right, nums.Length - 1); i += 1)
			{
				if (sb.Length > 0)
					sb.Append(",");
				sb.Append(nums[i]);
			}
			return sb.ToString();
		}
	}
}
