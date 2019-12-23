using System;
using System.Collections.Generic;

namespace Game.General
{
    public class EventSystem
    {
        private static EventSystem instance;

        public static EventSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EventSystem();
                }

                return instance;
            }
        }
    }
}