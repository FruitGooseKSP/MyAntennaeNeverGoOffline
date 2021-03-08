using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAntennaNeverGoesOffline
{
    class MangoUtility
    {
        private float electricalGenerationFactor;
        private double antennaResourceCost;
        private float factoredTime = 0F;
        private double factoredCost = 0;

        public MangoUtility(float _electricalGenerationFactor, double _antennaResourceCost)
        {
            electricalGenerationFactor = _electricalGenerationFactor;
            antennaResourceCost = _antennaResourceCost;
        }

        public float SetTime()
        {
            float maxPermitted = 100.0F;        // ie 100%
            float minPermitted = 1.0F;          // ie 1% (basically not 0)
            float baseAdjustment = 0.25F / 100.0F;      // for balancing...

            if (electricalGenerationFactor >= maxPermitted)         // if greater than 100% use 100%
            {
                factoredTime = maxPermitted;
                return factoredTime;
            }
            else if (electricalGenerationFactor <= minPermitted)    // if less than 1%, use 1%
            {
                factoredTime = minPermitted;
                return factoredTime;
            }
            else
            {
                factoredTime = electricalGenerationFactor * baseAdjustment;     // or use factor * 0.25 (adjustment) to give factored time.
                return factoredTime;
            }

        }

        public double SetRate()
        {
            if (antennaResourceCost > 25)
            {
                return -50;
            }
            else return -25;
        }


    }
}
