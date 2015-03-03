using System;
using System.Collections.Generic;
using System.Linq;

namespace battleships
{
	public static class MoreEnumerable
	{
		public static double Median(this IReadOnlyList<int> items)
		{
		    var n = items.Count;
		    
            double median;
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
	}
}