using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearn.Console
{
    public interface ISearchable
    {
        bool MatchesSearch(string searchkeyword);
        string GetSearchSummary();
    }
}
