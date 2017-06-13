using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    /// <summary>
    ///  Holds the edge with its 2 vertices : From , To 
    ///  Calculates The weight of the edge
    /// </summary>
    public class Edge
    {
        RGBPixel From;
        RGBPixel To;
        public double Weight;
        public Edge(RGBPixel From , RGBPixel To ,double Weight)
        {
            this.Weight = Weight;
            this.From = From;
            this.To = To;
        }
        /// <summary>
        /// takes two vertices and construct an edge between them
        /// </summary>
        /// <param name="F">RGBpixel contains the From Vertex</param>
        /// <param name="T">RGBpixel contains the To Vertex</param>
        public Edge(RGBPixel F, RGBPixel T)
        {
            From = F;
            To = T;
            Weight = Math.Sqrt(((To.red - From.red) * (To.red - From.red))
                            + ((To.green - From.green) * (To.green - From.green)) +
                              ((To.blue - From.blue) * (To.blue - From.blue)));
        }
        public RGBPixel Either()
        {
            return From;
        }
        public RGBPixel Other(RGBPixel X)
        {
            if (X.blue == From.blue && X.green == From.green && X.red == From.red)
                return To;
            return From;
        }
        Double GetWeight()
        {
            return Weight;
        }
    }
    /// <summary>
    /// Holds the pixel color in 3 byte values: red, green and blue
    /// </summary>
    public struct RGBPixel
    {
        public int Number;
        public byte red, green, blue;
        public string RGBString;
        /// <summary>
        /// Converts red ,blue and green bytes to strings , Then Concatenates them in one string
        /// </summary>
        public void ConvertRGBToString()
        {
            string r, g, b;
            r = Convert.ToString(red);
            g = Convert.ToString(green);
            b = Convert.ToString(blue);
            while (r.Length != 3)
            {
                r = "0" + r;
            }
            while (g.Length != 3)
            {
                g = "0" + g;
            }
            while (b.Length != 3)
            {
                b = "0" + b;
            }
            RGBString = r + g + b;
        }
    }
}
