using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Splicer
{
    public class Model
    {
        private static int sh;
        private static int sw;
        private Plasmid m_plasmid;
        private Pallet m_pallet;
        private int m_level;
        private Solution m_solution;
        private Timer m_timer;
        private int m_timeLimit;
        private int m_activeDrawer;
        private int m_score;
        private int m_hiscore;
        public enum GameStates { LEVELSTART, PLAY, GAMEOVER, EXIT };
        private int GAMESTATE;

        public Model()
        {
            m_level = 1;
            m_score = m_hiscore = 0;
            activeDrawer = 0;
            m_solution = new Solution();
            m_timer = new Timer();
            GAMESTATE = (int)GameStates.LEVELSTART;
            m_plasmid = new Plasmid();
            m_pallet = new Pallet();
            m_timeLimit = 70;
            initLevel();
        }

        public int gameState
        {
            get
            {
                return GAMESTATE;
            }
            set
            {
                GAMESTATE = value;
            }
        }

        public int score
        {
            get
            {
                return m_score;
            }
            set
            {
                m_score = value;
            }
        }

        public int hiscore
        {
            get
            {
                return m_hiscore;
            }
            set
            {
                m_hiscore = value;
            }
        }

        public int activeDrawer
        {
            get
            {
                return m_activeDrawer;
            }
            set
            {
                m_activeDrawer = value;
            }
        }

        public int screenWidth
        {
            get
            {
                return sw;
            }
            set
            {
                sw = value;
            }
        }

        public int screenHeight
        {
            get
            {
                return sh;
            }
            set
            {
                sh = value;
            }
        }

        public int level
        {
            get
            {
                return m_level;
            }
            set
            {
                m_level = value;
            }
        }

        public Pallet pallet
        {
            get
            {
                return m_pallet;
            }
        }

        public Plasmid plasmid
        {
            get
            {
                return m_plasmid;
            }
        }

        public Timer timer
        {
            get
            {
                return m_timer;
            }
        }

        public int timeLimit
        {
            get
            {
                return m_timeLimit;
            }
            set
            {
                m_timeLimit = value;
            }
    }

        public Solution solution
        {
            get
            {
                return m_solution;
            }
        }

        public void scoreUp(int incr)
        {
            m_score += incr;
        }

        public void scoreDown(int decr)
        {
            m_score -= decr;
            if (m_score < 0)
            {
                m_score = 0;
            }
        }

        public void levelUp()
        {
            m_level++;
        }

        public void initLevel()
        {           
            m_plasmid.restrictionSitesQuantity = m_solution.brickCountForLevel(m_level);
            m_plasmid.sitesFilled = 0;
            foreach (Brick thisBrick in m_pallet.getAllSelectedBricks())
            {
                thisBrick.picked = false;
            }                       
        }

    }
}
