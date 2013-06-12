using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Splicer
{
    public class Solution
    {
        private Dictionary<int, int[]> answer;
        private Dictionary<int, string> goal;

        public Solution()
        {
            formulateLevels();
        }

        public int brickCountForLevel(int level)
        {
            if (level <= answer.Keys.Count)
            {
                return answer[level].Length;
            }
            return -1;
        }

        public string getGoalTextForLevel(int level)
        {
            return goal[level];
        }

        public List<int> getBrickIdsForSolution(int level)
        {
            List<int> result = new List<int>();
            foreach (int thisid in answer[level])
            {
                result.Add(thisid);
            }
            result.Sort();
            return result;
        }

        private void formulateLevels()
        {
            // BOOKMARK Level Solution Setup
            answer = new Dictionary<int, int[]>();
            goal = new Dictionary<int, string>();
            int level = 1;
            answer.Add(level, new int[] { 1, 11, 19});            
            goal.Add(level, " Nice and easy...\n\n Construct an E.coli based organism that produces the smell of bananas when exposed to light.");
            level++;
            answer.Add(level, new int[] { 3, 6, 5 });
            goal.Add(level, " A little trickier...\n\n An insulin regulator is needed for diabetic patients, housed in a bacteriophage that responds to the current blood sugar level.");
            level++;
            answer.Add(level, new int[] { 1, 9, 22, 16 });
            goal.Add(level, " Environmental Problem...\n\n We require an E.coli cell that is engineered to trap CO2 in deep water by capturing it and bringing it up to the surface.");
            level++;
            answer.Add(level, new int[] { 2, 10, 21, 22});
            goal.Add(level, " Space, the Final Frontier...\n\n The Space Agency need a Gram-positive bacteria for terra-forming that can convert CO2 to O2, and will survive temperature extremes.");
            level++;
            answer.Add(level, new int[] {1, 14, 26, 22, 12, 16 });
            goal.Add(level, " Diesel Desire...\n\n You have received funding for synthetic fuel production in the arctic circle. They need a gram-negative organism that produces red fatty acids, resistant to cold temperatures, smells of vanilla (to aid identification) and can float for collection.");
            level++;
            answer.Add(level, new int[] { 3, 21, 22, 28, 13, 14 });
            goal.Add(level, " Nuclear Safety...\n\n We need a new radiation detector, housed in a bacteriophage, that is temperature resistant, and glows yellow when radiation is detected.");
            level++;
        }

    }
}
