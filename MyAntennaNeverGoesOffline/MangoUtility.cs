using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAntennaNeverGoesOffline
{
    class MangoUtility
    {
        private float electricalGenerationFactor;
        private float factoredTime = 0F;

        public MangoUtility(float _electricalGenerationFactor)
        {
            electricalGenerationFactor = _electricalGenerationFactor;
        }

        public float SetTime()
        {
            float maxPermitted = 100.0F;        // ie 100%
            float minPermitted = 0.001F;          // ie 0.001% (basically not 0)
            float baseAdjustment = 0F;      // for balancing...

            if (electricalGenerationFactor >= maxPermitted)         // if greater than 100% use 100%
            {
                factoredTime = maxPermitted;
                return factoredTime;
            }
            else if (electricalGenerationFactor <= minPermitted)    // if less than 0.001%, use 0.001%
            {
                factoredTime = minPermitted;
                return factoredTime;
            }
            else
            {
                factoredTime = (200.0f - electricalGenerationFactor) * 0.01f;
                    
                    //electricalGenerationFactor * baseAdjustment;     // or use factor * 0.001 (adjustment) to give factored time.
                
                    return factoredTime;
            }

        }

      


    }
}
