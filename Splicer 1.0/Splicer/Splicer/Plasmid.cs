using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Splicer
{

    public class Plasmid
    {
        int m_restrictionSites;
        int m_sitesFilled;

        public Plasmid()
        {
        }

        public Plasmid(int siteCount)
        {
            m_restrictionSites = siteCount;
            m_sitesFilled = 0;
        }

        public int restrictionSitesQuantity
        {
            get
            {
                return this.m_restrictionSites;
            }
            set
            {
                this.m_restrictionSites = value;
            }
        }

        public int sitesFilled
        {
            get
            {
                return m_sitesFilled;
            }
            set
            {
                m_sitesFilled = value;
            }
        }

    }

}
