// Copyright (c) 2007-2017 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using System;
using System.Collections.Generic;
using System.Text;

using ICCProfile = CSJ2K.Icc.ICCProfile;
using RandomAccessIO = CSJ2K.j2k.io.RandomAccessIO;

namespace CSJ2K.Color.Boxes
{

    /// <summary> This class maps the components in the codestream
    /// to channels in the image.  It models the Component
    /// Mapping box in the JP2 header.
    /// 
    /// </summary>
    /// <version> 	1.0
    /// </version>
    /// <author> 	Bruce A. Kern
    /// </author>
    public sealed class ComponentMappingBox : JP2Box
    {
        /* Return the number of mapped channels. */

        public int NChannels { get; private set; }

        private readonly List<byte[]> map = new List<byte[]>(10);

        /// <summary> Construct a ComponentMappingBox from an input image.</summary>
        /// <param name="in_Renamed">RandomAccessIO jp2 image
        /// </param>
        /// <param name="boxStart">offset to the start of the box in the image
        /// </param>
        public ComponentMappingBox(RandomAccessIO in_Renamed, int boxStart)
            : base(in_Renamed, boxStart)
        {
            readBox();
        }

        /// <summary>Analyze the box content. </summary>
        internal void readBox()
        {
            NChannels = (boxEnd - dataStart) / 4;
            in_Renamed.seek(dataStart);
            for (int offset = dataStart; offset < boxEnd; offset += 4)
            {
                byte[] mapping = new byte[4];
                in_Renamed.readFully(mapping, 0, 4);
                map.Add(mapping);
            }
        }

        /* Return the component mapped to the channel. */

        public int getCMP(int channel)
        {
            byte[] mapping = map[channel];
            return ICCProfile.getShort(mapping, 0) & 0x0000ffff;
        }

        /// <summary>Return the channel type. </summary>
        public short getMTYP(int channel)
        {
            byte[] mapping = map[channel];
            return (short)(mapping[2] & 0x00ff);
        }

        /// <summary>Return the palette index for the channel. </summary>
        public short getPCOL(int channel)
        {
            byte[] mapping = map[channel];
            return (short)(mapping[3] & 0x000ff);
        }

        /// <summary>Return a suitable String representation of the class instance. </summary>
        public override String ToString()
        {
            StringBuilder rep = new StringBuilder("[ComponentMappingBox ").Append("  ");
            rep.Append("nChannels= ").Append(Convert.ToString(NChannels));

            foreach (var bfr in map)
            {
                rep.Append(eol).Append("  ").Append("CMP= ").Append(Convert.ToString(getCMP(bfr))).Append(", ");
                rep.Append("MTYP= ").Append(Convert.ToString(getMTYP(bfr))).Append(", ");
                rep.Append("PCOL= ").Append(Convert.ToString(getPCOL(bfr)));
            }

            rep.Append("]");
            return rep.ToString();
        }

        private int getCMP(byte[] mapping)
        {
            return ICCProfile.getShort(mapping, 0) & 0x0000ffff;
        }

        private short getMTYP(byte[] mapping)
        {
            return (short)(mapping[2] & 0x00ff);
        }

        private short getPCOL(byte[] mapping)
        {
            return (short)(mapping[3] & 0x000ff);
        }

        static ComponentMappingBox()
        {
            {
                type = 0x636d6170;
            }
        }
    }
}
