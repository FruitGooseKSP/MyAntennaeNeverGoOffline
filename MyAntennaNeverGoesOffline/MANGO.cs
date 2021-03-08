using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace MyAntennaNeverGoesOffline
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class MANGO : MonoBehaviour
    {
        [KSPField]
        public List<Part> listOfAntennae;
       
        [KSPField]
        public List<Part> listOfGenerators;
       
        [KSPField]
        public int nBOfAntennae;
      
        [KSPField]
        public int nBOfGenerators;
      
        [KSPField]
        public bool processorPermitted;
      
        [KSPField]
        public float powerGen;
     
        [KSPField]
        public float newRate;
     
        [KSPField]
        public float mitAmount = 0F;
      
        [KSPField]
        public double globalRC = 0;
     
        [KSPField]
        public double meanRC = 0;
      
        [KSPField]
        public double newPowerPercentage;

        public void Start()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                try
                {
                    powerGen = 0.0F;
                    listOfAntennae = new List<Part>();
                    listOfGenerators = new List<Part>();

                    foreach (var parts in FlightGlobals.ActiveVessel.Parts)
                    {
                        if (parts.HasModuleImplementing<ModuleDataTransmitter>())
                        {
                            listOfAntennae.Add(parts);                                  // add data transmitters to a list
                        }

                        if (parts.HasModuleImplementing<ModuleDeployableSolarPanel>() || parts.HasModuleImplementing<ModuleGenerator>())
                        {
                            listOfGenerators.Add(parts);                                // add generators to a list
                        }
                    }

                    try
                    {
                        nBOfAntennae = listOfAntennae.Count();
                    }

                    catch
                    {
                        nBOfAntennae = 0;
                    }

                    try
                    {
                        nBOfGenerators = listOfGenerators.Count();
                    }
                    catch
                    {
                        nBOfGenerators = 0;
                    }

                    if (nBOfAntennae != 0 && nBOfGenerators != 0)                   // if vessel has both data transmitter and method
                    {                                                               // of generating power then activate the processor
                        processorPermitted = true;
                    }
                    else processorPermitted = false;


                    if (processorPermitted)
                    {
                        foreach (var part in listOfAntennae)                        // add each packet resource cost to the running total
                        {
                            globalRC += part.GetComponent<ModuleDataTransmitter>().packetResourceCost;
                            part.GetComponent<ModuleDataTransmitter>().packetResourceCost = 0.1F;       // then set low to prevent timeout
                        }

                        meanRC = globalRC / nBOfAntennae;                                           // get the average resource cost

                        foreach (var sp in listOfGenerators)
                        {
                            if (sp.HasModuleImplementing<ModuleDeployableSolarPanel>())
                            {
                                powerGen += sp.GetComponent<ModuleDeployableSolarPanel>().chargeRate;
                            }
                            else
                            {                                                           // add each generator's charge rate together
                                powerGen += 0.75F;
                            }
                        }

                        MangoUtility mU = new MangoUtility(powerGen, meanRC);           // send to utilities
                        newRate = mU.SetTime();                                         // get revised rates back
                        newPowerPercentage = mU.SetRate();

                        foreach (var antenna in listOfAntennae)
                        {
                            antenna.GetComponent<ModuleDataTransmitter>().packetInterval = newRate;        // change antennae rates using
                        }                                                                                   // new rate

                        foreach (var part in FlightGlobals.ActiveVessel.Parts)
                        {
                            if (part.Resources.Contains("ElectricCharge"))
                            {
                                double elecAmt = part.Resources.Get("ElectricCharge").amount;
                                elecAmt -= newPowerPercentage;
                                part.Resources.Get("ElectricCharge").amount = elecAmt;                  // take a 'payment' of electricity
                            }
                        }
                    }
                }


                catch { //"not in flight & not being handled correctly"                     // sometimes Editor scene throws Exception 
                }                                                                           // even though it shouldn't. Good old Unity.

            }
        }


    }
}
