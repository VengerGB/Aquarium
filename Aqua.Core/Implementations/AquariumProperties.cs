namespace Aqua.Core
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public class AquariumProperties
    {
        private Dictionary<string, int> Properties;

        private PerformanceCounter cpuCounter;

        public AquariumProperties(Dictionary<string, int> properties)
        {
            this.Properties = properties;
            this.StartCpuUsageCounter();
        }

        public int Algae
        {
            get
            {
                return GetOrInit("Algae");
            }

            set
            {
                this.Properties["Algae"] = value;
            }
        }

        private int GetOrInit(string propertyName)
        {
            if (!this.Properties.ContainsKey(propertyName))
            {
                this.Properties[propertyName] = 0;
                return 0;
            }
            else
            {
                return this.Properties[propertyName];
            }
        }

        public int Width
        {
            get
            {
                return GetOrInit("TankWidth");
            }

            set
            {
                this.Properties["TankWidth"] = value;
            }
        }


        public int Height
        {
            get
            {
                return GetOrInit("TankHeight");
            }

            set
            {
                this.Properties["TankHeight"] = value;
            }
        }

        public int Occupants
        {
            get
            {
                return GetOrInit("Occupants");
            }

            set
            {
                this.Properties["Occupants"] = value;
            }
        }

        public float CpuUsage
        {
            get
            {
                if (cpuCounter != null)
                {
                    return cpuCounter.NextValue();
                }

                return 0.0f;
            }
        }

        private void StartCpuUsageCounter()
        {
            this.cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";
        }
    }
}
