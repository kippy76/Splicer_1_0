using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Splicer
{
    public class Brick
    {
        private int m_id;
        private string m_name;
        private bool m_picked;

        public Brick()
        {
        }

        public Brick(int id, string name)
        {
            this.m_id = id;
            this.m_name = name;
            this.m_picked = false;
        }

        public int id
        {
            get
            {
                return this.m_id;
            }
            set
            {
                this.m_id = value;
            }
        }

        public string name
        {
            get
            {
                return this.m_name;
            }
            set
            {
                this.m_name = name;
            }
        }

        public bool picked
        {
            get
            {
                return this.m_picked;
            }
            set
            {
                this.m_picked = value;
            }
        }

    }
}
