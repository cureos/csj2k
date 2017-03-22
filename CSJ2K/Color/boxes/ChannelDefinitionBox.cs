// Copyright (c) 2007-2017 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using System;
using System.Collections.Generic;
using System.Text;

using ICCProfile = CSJ2K.Icc.ICCProfile;
using RandomAccessIO = CSJ2K.j2k.io.RandomAccessIO;

namespace CSJ2K.Color.Boxes
{
    using CSJ2K.j2k.fileformat;

    /// <summary> This class maps the components in the codestream
    /// to channels in the image.  It models the Component
    /// Mapping box in the JP2 header.
    /// 
    /// </summary>
    /// <version> 	1.0
    /// </version>
    /// <author> 	Bruce A. Kern
    /// </author>
    public sealed class ChannelDefinitionBox : JP2Box
    {
        /// <summary>Return the number of channel definitions.</summary>
        public int NDefs { get; private set; }

        private readonly Dictionary<int, int[]> definitions = new Dictionary<int, int[]>();

        /// <summary> Construct a ChannelDefinitionBox from an input image.</summary>
        /// <param name="in_Renamed">RandomAccessIO jp2 image
        /// </param>
        /// <param name="boxStart">offset to the start of the box in the image
        /// </param>
        public ChannelDefinitionBox(RandomAccessIO in_Renamed, int boxStart)
            : base(in_Renamed, boxStart)
        {
            readBox();
        }

        /// <summary>Analyze the box content. </summary>
        private void readBox()
        {
            byte[] bfr = new byte[8];

            in_Renamed.seek(dataStart);
            in_Renamed.readFully(bfr, 0, 2);
            NDefs = ICCProfile.getShort(bfr, 0) & 0x0000ffff;

            int offset = dataStart + 2;
            in_Renamed.seek(offset);
            for (int i = 0; i < NDefs; ++i)
            {
                in_Renamed.readFully(bfr, 0, 6);
                int[] channel_def = new int[3];
                channel_def[0] = getCn(bfr);
                channel_def[1] = getTyp(bfr);
                channel_def[2] = getAsoc(bfr);
                definitions[channel_def[0]] = channel_def;
            }
        }

        /* Return the channel association. */
        public int getCn(int asoc)
        {
            foreach (var definition in definitions)
            {
                var bfr = definition.Value;
                if (asoc == getAsoc(bfr)) return getCn(bfr);
            }

            return asoc;
        }

        /* Return the channel type. */
        public int getTyp(int channel)
        {
            int[] bfr = definitions[channel];
            return getTyp(bfr);
        }

        /* Return the associated channel of the association. */
        public int getAsoc(int channel)
        {
            int[] bfr = definitions[channel];
            return getAsoc(bfr);
        }

        /// <summary>Return a suitable String representation of the class instance. </summary>
        public override string ToString()
        {
            StringBuilder rep =
                new StringBuilder("[ChannelDefinitionBox ").Append(eol).Append("  ");
            rep.Append("ndefs= ").Append(Convert.ToString(NDefs));

            foreach (var definition in definitions)
            {
                var bfr = definition.Value;
                rep.Append(eol)
                    .Append("  ")
                    .Append("Cn= ")
                    .Append(Convert.ToString(getCn(bfr)))
                    .Append(", ")
                    .Append("Typ= ")
                    .Append(Convert.ToString(getTyp(bfr)))
                    .Append(", ")
                    .Append("Asoc= ")
                    .Append(Convert.ToString(getAsoc(bfr)));
            }

            rep.Append("]");
            return rep.ToString();
        }

        /// <summary>Return the channel from the record.</summary>
        private static int getCn(byte[] bfr)
        {
            return ICCProfile.getShort(bfr, 0);
        }

        /// <summary>Return the channel type from the record.</summary>
        private static int getTyp(byte[] bfr)
        {
            return ICCProfile.getShort(bfr, 2);
        }

        /// <summary>Return the associated channel from the record.</summary>
        private static int getAsoc(byte[] bfr)
        {
            return ICCProfile.getShort(bfr, 4);
        }

        private static int getCn(int[] bfr)
        {
            return bfr[0];
        }

        private static int getTyp(int[] bfr)
        {
            return bfr[1];
        }

        private static int getAsoc(int[] bfr)
        {
            return bfr[2];
        }

        static ChannelDefinitionBox()
        {
            type = FileFormatBoxes.CHANNEL_DEFINITION_BOX;
        }
    }
}
