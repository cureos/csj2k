// Copyright (c) 2007-2017 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using System;
using System.Text;

using RandomAccessIO = CSJ2K.j2k.io.RandomAccessIO;
using ICCProfile = CSJ2K.Icc.ICCProfile;

namespace CSJ2K.Color.Boxes
{
    using CSJ2K.j2k.fileformat;

    /// <summary> This class models the Image Header box contained in a JP2
    /// image.  It is a stub class here since for colormapping the
    /// knowlege of the existance of the box in the image is sufficient.
    /// 
    /// </summary>
    /// <version> 	1.0
    /// </version>
    /// <author> 	Bruce A. Kern
    /// </author>
    public sealed class ImageHeaderBox : JP2Box
    {
        private long height;

        private long width;

        private int nc;

        private short bpc;

        private short c;

        private bool unk;

        private bool ipr;

        /// <summary> Construct an ImageHeaderBox from an input image.</summary>
        /// <param name="in_Renamed">RandomAccessIO jp2 image
        /// </param>
        /// <param name="boxStart">offset to the start of the box in the image
        /// </param>
        public ImageHeaderBox(RandomAccessIO in_Renamed, int boxStart)
            : base(in_Renamed, boxStart)
        {
            readBox();
        }

        /// <summary>Return a suitable String representation of the class instance. </summary>
        public override string ToString()
        {
            StringBuilder rep = new StringBuilder("[ImageHeaderBox ").Append(eol).Append("  ");
            rep.Append("height= ").Append(Convert.ToString(height)).Append(", ");
            rep.Append("width= ").Append(Convert.ToString(width)).Append(eol).Append("  ");

            rep.Append("nc= ").Append(Convert.ToString(nc)).Append(", ");
            rep.Append("bpc= ").Append(Convert.ToString(bpc)).Append(", ");
            rep.Append("c= ").Append(Convert.ToString(c)).Append(eol).Append("  ");

            rep.Append("image colorspace is ").Append(new StringBuilder(unk == true ? "known" : "unknown"));
            rep.Append(", the image ")
                .Append(new StringBuilder(ipr ? "contains " : "does not contain "))
                .Append("intellectual property")
                .Append("]");

            return rep.ToString();
        }

        /// <summary>Analyze the box content. </summary>
        internal void readBox()
        {
            byte[] bfr = new byte[14];
            in_Renamed.seek(dataStart);
            in_Renamed.readFully(bfr, 0, 14);

            height = ICCProfile.getInt(bfr, 0);
            width = ICCProfile.getInt(bfr, 4);
            nc = ICCProfile.getShort(bfr, 8);
            bpc = (short)(bfr[10] & 0x00ff);
            c = (short)(bfr[11] & 0x00ff);
            unk = bfr[12] == 0;
            ipr = bfr[13] == 1;
        }

        static ImageHeaderBox()
        {
            type = FileFormatBoxes.IMAGE_HEADER_BOX;
        }
    }
}
