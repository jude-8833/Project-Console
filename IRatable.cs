using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearn.Console
{
    public interface IRatable
    {
        void AddRating(int stars, string rating);
        double GetAverageRating();
        int GetTotalRatings();
    }
}
