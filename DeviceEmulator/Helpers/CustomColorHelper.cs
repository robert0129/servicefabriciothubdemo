// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

#region Using Directives



#endregion

namespace Microsoft.AzureCat.Samples.DeviceEmulator
{
    using System;
    using System.Drawing;

    /// <summary>
    /// Summary description for ColorHelper.
    /// </summary>
    internal static class CustomColorHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <returns></returns>
        public static Color CreateColorFromRGB(int red, int green, int blue)
        {
            //Corect Red element
            int r = red;
            if (r > 255)
            {
                r = 255;
            }
            if (r < 0)
            {
                r = 0;
            }
            //Corect Green element
            int g = green;
            if (g > 255)
            {
                g = 255;
            }
            if (g < 0)
            {
                g = 0;
            }
            //Correct Blue Element
            int b = blue;
            if (b > 255)
            {
                b = 255;
            }
            if (b < 0)
            {
                b = 0;
            }
            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blendColor"></param>
        /// <param name="baseColor"></param>
        /// <param name="opacity"></param>
        /// <returns></returns>
        public static Color OpacityMix(Color blendColor, Color baseColor, int opacity)
        {
            int r1 = blendColor.R;
            int g1 = blendColor.G;
            int b1 = blendColor.B;
            int r2 = baseColor.R;
            int g2 = baseColor.G;
            int b2 = baseColor.B;
            int r3 = (int) (((r1*((float) opacity/100)) + (r2*(1 - ((float) opacity/100)))));
            int g3 = (int) (((g1*((float) opacity/100)) + (g2*(1 - ((float) opacity/100)))));
            int b3 = (int) (((b1*((float) opacity/100)) + (b2*(1 - ((float) opacity/100)))));
            return CreateColorFromRGB(r3, g3, b3);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseColor"></param>
        /// <param name="blendColor"></param>
        /// <param name="opacity"></param>
        /// <returns></returns>
        public static Color SoftLightMix(Color baseColor, Color blendColor, int opacity)
        {
            int r1 = baseColor.R;
            int g1 = baseColor.G;
            int b1 = baseColor.B;
            int r2 = blendColor.R;
            int g2 = blendColor.G;
            int b2 = blendColor.B;
            int r3 = SoftLightMath(r1, r2);
            int g3 = SoftLightMath(g1, g2);
            int b3 = SoftLightMath(b1, b2);
            return OpacityMix(CreateColorFromRGB(r3, g3, b3), baseColor, opacity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseColor"></param>
        /// <param name="blendColor"></param>
        /// <param name="opacity"></param>
        /// <returns></returns>
        public static Color OverlayMix(Color baseColor, Color blendColor, int opacity)
        {
            int r3 = OverlayMath(baseColor.R, blendColor.R);
            int g3 = OverlayMath(baseColor.G, blendColor.G);
            int b3 = OverlayMath(baseColor.B, blendColor.B);
            return OpacityMix(CreateColorFromRGB(r3, g3, b3), baseColor, opacity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ibase"></param>
        /// <param name="blend"></param>
        /// <returns></returns>
        private static int SoftLightMath(int ibase, int blend)
        {
            float dbase = (float) ibase/255;
            float dblend = (float) blend/255;
            if (dblend < 0.5)
            {
                return (int) (((2*dbase*dblend) + (Math.Pow(dbase, 2))*(1 - (2*dblend)))*255);
            }
            return (int) (((Math.Sqrt(dbase)*(2*dblend - 1)) + ((2*dbase)*(1 - dblend)))*255);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ibase"></param>
        /// <param name="blend"></param>
        /// <returns></returns>
        private static int OverlayMath(int ibase, int blend)
        {
            double dbase = (double) ibase/255;
            double dblend = (double) blend/255;
            if (dbase < 0.5)
            {
                return (int) ((2*dbase*dblend)*255);
            }
            return (int) ((1 - (2*(1 - dbase)*(1 - dblend)))*255);
        }
    }
}