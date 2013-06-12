using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Splicer
{
    public class Timer
    {
        private DateTime m_startTime;
        private int m_durationSecs;
        bool m_expired;

        public Timer()
        {
            m_durationSecs = 0;
            m_expired = true;
            m_startTime = DateTime.Now;
        }

        public void start(int durationInSecs)
        {
            m_durationSecs = durationInSecs;
            m_startTime = DateTime.Now;
            m_expired = false;
        }

        public int duration
        {
            get
            {
                return m_durationSecs;
            }
            set
            {
                m_durationSecs = value;
            }
        }

        public int secondsRemaining()
        {
            int timeLeft = m_durationSecs - ((int)((TimeSpan)(DateTime.Now - m_startTime)).TotalSeconds);
            return timeLeft < 0 ? 0 : timeLeft;
        }

        public bool expired()
        {
            if (!m_expired)
            {
                TimeSpan elapsed = DateTime.Now - m_startTime;
                if (elapsed.TotalSeconds > m_durationSecs)
                {
                    m_expired = true;
                }
            }
            return m_expired;
        }

    }
}
