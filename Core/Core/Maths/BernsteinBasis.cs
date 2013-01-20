using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using SlimDX;

namespace FeralTic.Core.Maths
{
    public class BernsteinBasis
    {
        public static float[] ComputeBasis(int degree, float t)
        {
            float[] result = new float[degree+1];

            int[] coeff = GetBinomial(degree);

            float invT = 1.0f - t;

            for (int i = 0; i < degree+1; i++)
            {
                double res = coeff[i] * Math.Pow(invT, degree - i) * Math.Pow(t, i);

                result[i] = (float)res;
            }

            return result;
        }

        private static int[] GetBinomial(int degree)
        {
            int[] res = new int[1] { 1 };

            for (int j = 1; j < degree + 1; j++)
            {
                int[] curr = new int[res.Length + 1];
                for (int i = 0; i < res.Length + 1; i++)
                {
                    if (i == 0)
                    {
                        curr[i] = 1;
                    }
                    else
                    {
                        if (i == res.Length)
                        {
                            curr[i] = 1;
                        }
                        else
                        {
                            curr[i] = res[i - 1] + res[i];
                        }
                    }
                }
                res = curr;
            }
            return res;
        }
    }
}
