// Copyright (c) 2007-2017 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using System;
using System.Text;

using ChannelDefinitionBox = CSJ2K.Color.Boxes.ChannelDefinitionBox;
using ColorSpecificationBox = CSJ2K.Color.Boxes.ColorSpecificationBox;
using ComponentMappingBox = CSJ2K.Color.Boxes.ComponentMappingBox;
using FileFormatBoxes = CSJ2K.j2k.fileformat.FileFormatBoxes;
using HeaderDecoder = CSJ2K.j2k.codestream.reader.HeaderDecoder;
using ImageHeaderBox = CSJ2K.Color.Boxes.ImageHeaderBox;
using PaletteBox = CSJ2K.Color.Boxes.PaletteBox;
using ParameterList = CSJ2K.j2k.util.ParameterList;
using RandomAccessIO = CSJ2K.j2k.io.RandomAccessIO;

namespace CSJ2K.Color
{
	/// <summary> This class analyzes the image to provide colorspace
	/// information for the decoding chain.  It does this by
	/// examining the box structure of the JP2 image.
	/// It also provides access to the parameter list information,
	/// which is stored as a public final field.
	/// 
	/// </summary>
	/// <seealso cref="ICCProfile">
	/// </seealso>
	/// <version> 	1.0
	/// </version>
	/// <author> 	Bruce A. Kern
	/// </author>
	public class ColorSpace
    {
        /// <summary> Retrieve the ICC profile from the images as
        /// a byte array.
        /// </summary>
        /// <returns> the ICC Profile as a byte [].
        /// </returns>
        public virtual byte[] ICCProfile => csbox.ICCProfile;

        /// <summary>Return the colorspace method (Profiled, enumerated, or palettized). </summary>
        public virtual MethodEnum Method => csbox.Method;

        /// <summary>Return number of channels in the palette. </summary>
        public virtual PaletteBox PaletteBox => pbox;

        /// <summary>Return number of channels in the palette. </summary>
        public virtual int PaletteChannels => pbox == null ? 0 : pbox.NumColumns;

        /// <summary>Is palettized predicate. </summary>
        public virtual bool Palettized => pbox != null;

        public static readonly string eol = Environment.NewLine;

        /// <summary>Parameter Specs </summary>
        public ParameterList pl;

        /// <summary>Parameter Specs </summary>
        private HeaderDecoder hd;

        /* Image box structure as pertains to colorspacees. */
        private PaletteBox pbox;

        private ComponentMappingBox cmbox;

        private ColorSpecificationBox csbox;

        private ChannelDefinitionBox cdbox;

        private ImageHeaderBox ihbox;

        /// <summary>Input image </summary>
        private readonly RandomAccessIO in_Renamed;

        /// <summary>Indent a String that contains newlines. </summary>
        public static string indent(string ident, StringBuilder instr)
        {
            return indent(ident, instr.ToString());
        }

        /// <summary>Indent a String that contains newlines. </summary>
        public static string indent(string ident, string instr)
        {
            StringBuilder tgt = new StringBuilder(instr);
            char eolChar = eol[0];
            int i = tgt.Length;
            while (--i > 0)
            {
                if (tgt[i] == eolChar) tgt.Insert(i + 1, ident);
            }

            return ident + tgt;
        }

        /// <summary> public constructor which takes in the image, parameterlist and the
        /// image header decoder as args.
        /// </summary>
        /// <param name="in_Renamed">input RandomAccess image file.
        /// </param>
        /// <param name="hd">provides information about the image header.
        /// </param>
        /// <param name="pl">provides parameters from the default and commandline lists. 
        /// </param>
        public ColorSpace(RandomAccessIO in_Renamed, HeaderDecoder hd, ParameterList pl)
        {
            this.pl = pl;
            this.in_Renamed = in_Renamed;
            this.hd = hd;
            getBoxes();
        }

        /// <summary> Retrieve the various boxes from the JP2 file.</summary>
        /// <exception cref="ColorSpaceException"/>
        private void getBoxes()
        {
            // byte[] data;
            int type;
            long len;
            int boxStart = 0;
            byte[] boxHeader = new byte[16];
            int i = 0;

            // Search the toplevel boxes for the header box
            while (true)
            {
                in_Renamed.seek(boxStart);
                in_Renamed.readFully(boxHeader, 0, 16);

                // TODO CONVERSION PROBLEM?
                len = Icc.ICCProfile.getInt(boxHeader, 0);
                if (len == 1) len = Icc.ICCProfile.getLong(boxHeader, 8); // Extended

                // length
                type = Icc.ICCProfile.getInt(boxHeader, 4);

                // Verify the contents of the file so far.
                if (i == 0 && type != FileFormatBoxes.JP2_SIGNATURE_BOX)
                {
                    throw new ColorSpaceException("first box in image not " + "signature");
                }

                if (i == 1 && type != FileFormatBoxes.FILE_TYPE_BOX)
                {
                    throw new ColorSpaceException("second box in image not file");
                }

                if (type == FileFormatBoxes.CONTIGUOUS_CODESTREAM_BOX)
                {
                    throw new ColorSpaceException("header box not found in image");
                }

                if (type == FileFormatBoxes.JP2_HEADER_BOX)
                {
                    break;
                }

                // Progress to the next box.
                ++i;
                boxStart = (int)(boxStart + len);
            }

            // boxStart indexes the start of the JP2_HEADER_BOX,
            // make headerBoxEnd index the end of the box.
            long headerBoxEnd = boxStart + len;

            if (len == 1) boxStart += 8; // Extended length header

            for (boxStart += 8; boxStart < headerBoxEnd; boxStart = (int)(boxStart + len))
            {
                in_Renamed.seek(boxStart);
                in_Renamed.readFully(boxHeader, 0, 16);
                len = Icc.ICCProfile.getInt(boxHeader, 0);
                if (len == 1) throw new ColorSpaceException("Extended length boxes " + "not supported");
                type = Icc.ICCProfile.getInt(boxHeader, 4);

                switch (type)
                {
                    case FileFormatBoxes.IMAGE_HEADER_BOX:
                        ihbox = new ImageHeaderBox(in_Renamed, boxStart);
                        break;

                    case FileFormatBoxes.COLOUR_SPECIFICATION_BOX:
                        csbox = new ColorSpecificationBox(in_Renamed, boxStart);
                        break;

                    case FileFormatBoxes.CHANNEL_DEFINITION_BOX:
                        cdbox = new ChannelDefinitionBox(in_Renamed, boxStart);
                        break;

                    case FileFormatBoxes.COMPONENT_MAPPING_BOX:
                        cmbox = new ComponentMappingBox(in_Renamed, boxStart);
                        break;

                    case FileFormatBoxes.PALETTE_BOX:
                        pbox = new PaletteBox(in_Renamed, boxStart);
                        break;
                }
            }

            if (ihbox == null) throw new ColorSpaceException("image header box not found");

            if (pbox == null && cmbox != null || pbox != null && cmbox == null) throw new ColorSpaceException("palette box and component " + "mapping box inconsistency");
        }

        /// <summary>Return the channel definition of the input component. </summary>
        public virtual int getChannelDefinition(int c)
        {
            return cdbox == null ? c : cdbox.getCn(c + 1);
        }

        /// <summary>Return the colorspace (sYCC, sRGB, sGreyScale). </summary>
        public virtual CSEnum getColorSpace()
        {
            return csbox.ColorSpace;
        }

        /// <summary>Return bitdepth of the palette entries. </summary>
        public virtual int getPaletteChannelBits(int c)
        {
            return pbox == null ? 0 : pbox.getBitDepth(c);
        }

        /// <summary> Return a palettized sample</summary>
        /// <param name="channel">requested 
        /// </param>
        /// <param name="index">of entry
        /// </param>
        /// <returns> palettized sample
        /// </returns>
        public virtual int getPalettizedSample(int channel, int index)
        {
            return pbox == null ? 0 : pbox.getEntry(channel, index);
        }

        /// <summary>Signed output predicate. </summary>
        public virtual bool isOutputSigned(int channel)
        {
            return pbox != null ? pbox.isSigned(channel) : hd.isOriginalSigned(channel);
        }

        /// <summary>Return a suitable String representation of the class instance. </summary>
        public override string ToString()
        {
            StringBuilder rep =
                new StringBuilder("[ColorSpace is ").Append(csbox.MethodString)
                    .Append(Palettized ? "  and palettized " : " ")
                    .Append(Method == MethodEnum.ENUMERATED ? csbox.ColorSpaceString : string.Empty);
            if (ihbox != null)
            {
                rep.Append(eol).Append(indent("    ", ihbox.ToString()));
            }

            if (cdbox != null)
            {
                rep.Append(eol).Append(indent("    ", cdbox.ToString()));
            }

            if (csbox != null)
            {
                rep.Append(eol).Append(indent("    ", csbox.ToString()));
            }

            if (pbox != null)
            {
                rep.Append(eol).Append(indent("    ", pbox.ToString()));
            }

            if (cmbox != null)
            {
                rep.Append(eol).Append(indent("    ", cmbox.ToString()));
            }

            return rep.Append("]").ToString();
        }

        /// <summary> Are profiling diagnostics turned on</summary>
        /// <returns> yes or no
        /// </returns>
        public virtual bool debugging()
        {
            string tmp;
            return pl.TryGetValue("colorspace_debug", out tmp) && tmp.ToUpper().Equals("ON");
        }

        public enum MethodEnum
        {
            ICC_PROFILED,

            ENUMERATED
        }

        public enum CSEnum
        {
            sRGB,

            GreyScale,

            sYCC,

            esRGB,

            Illegal,

            Unknown
        }

    }
}