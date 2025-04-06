using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba2_processes
{
    public class Simulation
    {
        private Random random = new Random();
        public List<double> Times = new List<double>();
        public List<double> TimesPuassons = new List<double>();

        public Simulation(double mk, double sigma, double tk)
        {
            double lambdaK = 1 / mk;
            int k = Convert.ToInt32(1 / Math.Pow(sigma, 2) / Math.Pow(lambdaK, 2));
            double lambda = lambdaK * k;
            double t = 0;

            while(t < tk)
            {
                
                for(int n = 0; n < k; n++)
                {
                    double r = random.NextDouble();
                    t -= 1 / lambda * Math.Log(r);
                    if (t < tk)
                        TimesPuassons.Add(t);
                }
                if (t < tk)
                    Times.Add(t);
            }
        }
    }
}
