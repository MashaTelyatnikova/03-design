using System;
using System.Collections.Generic;
using System.Linq;
using battleships.GameUtils;

namespace battleships
{
	public static class MoreEnumerable
	{
		public static IEnumerable<T> Generate<T>(Func<T> tryGetItem)
		{
			while (true)
				yield return tryGetItem();
			// ReSharper disable once FunctionNeverReturns
		}

		public static double Median(this IReadOnlyList<int> items)
		{
		    var n = items.Count;
		    var median = 0.0;
		    if (n%2 != 0)
		    {
		        median = items[(n + 1)/2 - 1];
		    }
		    else
		    {
		        median = (items[n/2 - 1] + items[n/2])/2.0;
		    }
		    return median;
		}

	    public static double Sigma(this IReadOnlyList<int> items)
	    {
	        var mean = items.Average();
            return Math.Sqrt(items.Average(s => (s - mean) * (s - mean)));
	    }

	    public static IEnumerable<T> Repeat<T>(this Func<T> getItem)
	    {
	        while (true)
	            yield return getItem();
	    }
	}
}