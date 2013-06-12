using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Splicer
{
    public class Pallet
    {
        Dictionary<string, List<Brick>> m_pallet;

        public Pallet()
        {
            m_pallet = new Dictionary<string, List<Brick>>();
            initPallet();
        }

        public Dictionary<string, List<Brick>> pallet
        {
            get
            {
                return m_pallet;
            }
        }

        public List<string> getDrawerNames()
        {
            List<string> names = new List<string>();
            foreach (string category in m_pallet.Keys)
            {
                names.Add(category);
            }
            return names;
        }

        public List<Brick> getAllBricks()
        {
            List<Brick> allBricks = new List<Brick>();
            foreach (string category in m_pallet.Keys)
            {
                foreach (Brick thisBrick in m_pallet[category])
                {
                    allBricks.Add(thisBrick);
                }
            }
            return allBricks;
        }

        public Brick getBrick(int id)
        {
            foreach (string category in m_pallet.Keys)
            {
                foreach (Brick thisBrick in m_pallet[category])
                {
                    if (thisBrick.id == id)
                    {
                        return thisBrick;
                    }
                }
            }
            return null;
        }

        public List<Brick> getAllSelectedBricks()
        {
            List<Brick> pickedBricks = new List<Brick>();
            foreach (string category in m_pallet.Keys)
            {
                foreach (Brick thisBrick in m_pallet[category])
                {
                    if (thisBrick.picked)
                    {
                        pickedBricks.Add(thisBrick);
                    }
                }
            }
            return pickedBricks;
        }

        public List<int> getAllSelectedBrickIds()
        {
            List<Brick> picked = getAllSelectedBricks();
            List<int> result = new List<int>();
            foreach (Brick thisBrick in picked)
            {
                result.Add(thisBrick.id);
            }
            result.Sort();
            return result;
        }

        public void setPicked(Brick brick, bool picked)
        {
            List<Brick> allBricks = getAllBricks();
            foreach (Brick thisBrick in allBricks)
            {
                if (brick == thisBrick)
                {
                    brick.picked = picked;
                }
            }
        }

        public void setPicked(int brickID, bool picked)
        {
            List<Brick> allBricks = getAllBricks();
            foreach (Brick thisBrick in allBricks)
            {
                if (brickID == thisBrick.id)
                {
                    thisBrick.picked = picked;
                }
            }
        }

        private void initPallet()
        {
            // BOOKMARK Biobrick Drawer & Contents Setup
            List<Brick> m_stack = new List<Brick>();
            // Chassis
            m_stack = new List<Brick>();
            m_stack.Add(new Brick(1, "Echerichia coli"));
            m_stack.Add(new Brick(2, "Bacillus subtilis"));
            m_stack.Add(new Brick(3, "T7 Bacteriophage"));
            m_stack.Add(new Brick(4, "Yeast"));
            m_pallet.Add("Chassis", m_stack);
            // Human
            m_stack = new List<Brick>();
            m_stack.Add(new Brick(5, "Insulin Production"));
            m_stack.Add(new Brick(6, "Glucose Detection"));
            m_stack.Add(new Brick(7, "Glucose Production"));
            m_stack.Add(new Brick(8, "Antibodies"));
            m_pallet.Add("Human", m_stack);
            // Plant
            m_stack = new List<Brick>();
            m_stack.Add(new Brick(9, "CO2 Capture"));
            m_stack.Add(new Brick(10, "Photosynthesis"));
            m_stack.Add(new Brick(11, "Banana Odour"));
            m_stack.Add(new Brick(12, "Vanilin"));
            m_stack.Add(new Brick(13, "GFP"));
            m_stack.Add(new Brick(14, "RFP"));
            m_pallet.Add("Plant", m_stack);
            // bacteria
            m_stack = new List<Brick>();
            m_stack.Add(new Brick(15, "Tetracycline Resistance"));
            m_stack.Add(new Brick(16, "Gas Vesicles"));
            m_stack.Add(new Brick(17, "C.diff Lysis"));
            m_stack.Add(new Brick(18, "E.coli Lysis"));
            m_pallet.Add("Bacteria", m_stack);
            // Sensory
            m_stack = new List<Brick>();
            m_stack.Add(new Brick(19, "Light Sensitivity"));
            m_stack.Add(new Brick(20, "Temperature Sensitivity"));
            m_stack.Add(new Brick(23, "Ph Sensitivity"));
            m_pallet.Add("Sensory", m_stack);
            // Virii
            m_stack = new List<Brick>();
            m_stack.Add(new Brick(21, "Heat Tolerant"));
            m_stack.Add(new Brick(22, "Cold Tolerant"));
            m_stack.Add(new Brick(27, "Radiation Tolerant"));
            m_pallet.Add("Virii", m_stack);
            // Misc
            m_stack = new List<Brick>();
            m_stack.Add(new Brick(24, "Heavy Metal Detection"));
            m_stack.Add(new Brick(25, "Ethanol Production"));
            m_stack.Add(new Brick(26, "Fatty Acid (C-12)  Production"));
            m_stack.Add(new Brick(28, "Radiation Detection"));
            m_pallet.Add("Misc.", m_stack);
            
        }
    }
}
