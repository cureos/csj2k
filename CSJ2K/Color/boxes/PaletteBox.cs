// Copyright (c) 2007-2017 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using System;

using ICCProfile = CSJ2K.Icc.ICCProfile;
using RandomAccessIO = CSJ2K.j2k.io.RandomAccessIO;

namespace CSJ2K.Color.Boxes
{
    using System.Text;

    using CSJ2K.j2k.fileformat;

    /// <summary> This class models the palette box contained in a JP2
    /// image.
    /// 
    /// </summary>
    /// <version> 	1.0
    /// </version>
    /// <author> 	Bruce A. Kern
    /// </author>
    public sealed class PaletteBox : JP2Box
    {
        /// <summary>Return the number of palette entries. </summary>
        public int NumEntries { get; private set; }

        /// <summary>Return the number of palette columns. </summary>
        public int NumColumns { get; private set; }

        private short[] bitdepth;

        private int[][] entries;

        /// <summary> Construct a PaletteBox from an input image.</summary>
        /// <param name="in_Renamed">RandomAccessIO jp2 image
        /// </param>
        /// <param name="boxStart">offset to the start of the box in the image
        /// </param>
        public PaletteBox(RandomAccessIO in_Renamed, int boxStart)
            : base(in_Renamed, boxStart)
        {
            readBox();
        }

        /// <summary>Analyze the box content. </summary>
        private void readBox()
        {
            byte[] bfr = new byte[4];
            int i;

            // int entry;

            // Read the number of palette entries and columns per entry.
            in_Renamed.seek(dataStart);
            in_Renamed.readFully(bfr, 0, 3);
            NumEntries = ICCProfile.getShort(bfr, 0) & 0x0000ffff;
            NumColumns = bfr[2] & 0x0000ffff;

            // Read the bitdepths for each column
            bitdepth = new short[NumColumns];
            bfr = new byte[NumColumns];
            in_Renamed.readFully(bfr, 0, NumColumns);
            for (i = 0; i < NumColumns; ++i)
            {
                bitdepth[i] = (short)(bfr[i] & 0x00fff);
            }

            entries = new int[NumEntries * NumColumns][];

            bfr = new byte[2];
            for (i = 0; i < NumEntries; ++i)
            {
                entries[i] = new int[NumColumns];

                int j;
                for (j = 0; j < NumColumns; ++j)
                {
                    int bd = getBitDepth(j);
                    bool signed = isSigned(j);

                    int b;
                    switch (getEntrySize(j))
                    {
                        case 1: // 8 bit entries
                            in_Renamed.readFully(bfr, 0, 1);
                            b = bfr[0];
                            break;

                        case 2: // 16 bits
                            in_Renamed.readFully(bfr, 0, 2);
                            b = ICCProfile.getShort(bfr, 0);
                            break;

                        default:
                            throw new ColorSpaceException("palettes greater than 16 bits deep not supported");
                    }

                    int m;
                    if (signed)
                    {
                        // Do sign extension if high bit is set.
                        if ((b & (1 << (bd - 1))) == 0)
                        {
                            // high bit not set.
                            m = (1 << bd) - 1;
                            entries[i][j] = m & b;
                        }
                        else
                        {
                            // high bit set.
                            // TODO CONVERSION PROBLEM?
                            m = unchecked((int)(0xffffffff << bd));
                            entries[i][j] = m | b;
                        }
                    }
                    else
                    {
                        // Clear all high bits.
                        m = (1 << bd) - 1;
                        entries[i][j] = m & b;
                    }
                }
            }
        }

        /// <summary>Are entries signed predicate. </summary>
        public bool isSigned(int column)
        {
            return (bitdepth[column] & 0x80) == 1;
        }

        /// <summary>Are entries unsigned predicate. </summary>
        public bool isUnSigned(int column)
        {
            return !isSigned(column);
        }

        /// <summary>Return the bitdepth of palette entries. </summary>
        public short getBitDepth(int column)
        {
            return (short)((bitdepth[column] & 0x7f) + 1);
        }

        /// <summary>Return an entry for a given index and column. </summary>
        public int getEntry(int column, int entry)
        {
            return entries[entry][column];
        }

        /// <summary>Return a suitable String representation of the class instance. </summary>
        public override string ToString()
        {
            StringBuilder rep =
                new StringBuilder("[PaletteBox ").Append("nentries= ")
                    .Append(Convert.ToString(NumEntries))
                    .Append(", ncolumns= ")
                    .Append(Convert.ToString(NumColumns))
                    .Append(", bitdepth per column= (");
            for (int i = 0; i < NumColumns; ++i)
                rep.Append(getBitDepth(i))
                    .Append(isSigned(i) ? "S" : "U")
                    .Append(i < NumColumns - 1 ? ", " : string.Empty);
            return rep.Append(")]").ToString();
        }

        private int getEntrySize(int column)
        {
            int bd = getBitDepth(column);
            return bd / 8 + (bd % 8) == 0 ? 0 : 1;
        }

        static PaletteBox()
        {
            type = FileFormatBoxes.PALETTE_BOX;
        }
    }
}
