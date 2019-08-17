using System.Collections.Generic;

namespace Siteswaps
{
    public class StatusSiteswap
    {
        private CyclicArray<HandStatus> status;

        public StatusSiteswap(Siteswap siteswap)
        {
            status = new CyclicArray<HandStatus>(siteswap.MaxThrow());

        }
        
        
    }
}