// Copyright (c) 2007-2017 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using System;
using System.Text;

using CSJ2K.j2k.util;
using CSJ2K.j2k.io;

namespace CSJ2K.Color.Boxes
{
    using CSJ2K.j2k.fileformat;

    /// <summary> This class models the Color Specification Box in a JP2 image.
	/// 
	/// </summary>
	/// <version> 	1.0
	/// </version>
	/// <author> 	Bruce A. Kern
	/// </author>
	public sealed class ColorSpecificationBox : JP2Box
	{
        // Return an enumeration for the colorspace method. 
	    public ColorSpace.MethodEnum Method { get; private set; }

		// Return an enumeration for the colorspace. 
	    public ColorSpace.CSEnum ColorSpace { get; private set; }

		// Return a String representation of the colorspace. 
	    public string ColorSpaceString => ColorSpace.ToString();

		// Return a String representation of the colorspace method. 
	    public string MethodString => Method.ToString();

		/* Retrieve the ICC Profile from the image as a byte []. */
	    public byte[] ICCProfile { get; private set; }

        /// <summary> Construct a ColorSpecificationBox from an input image.</summary>
        /// <param name="in_Renamed">RandomAccessIO jp2 image
        /// </param>
        /// <param name="boxStart">offset to the start of the box in the image
        /// </param>
        public ColorSpecificationBox(RandomAccessIO in_Renamed, int boxStart):base(in_Renamed, boxStart)
		{
			readBox();
		}
		
		/// <summary>Analyze the box content. </summary>
		private void  readBox()
		{
			byte[] boxHeader = new byte[256];
			in_Renamed.seek(dataStart);
			in_Renamed.readFully(boxHeader, 0, 11);
			switch (boxHeader[0])
			{
				
				case 1: 
					Method = Color.ColorSpace.MethodEnum.ENUMERATED;
                    int cs = Icc.ICCProfile.getInt(boxHeader, 3);
					switch (cs)
					{
						case 16: 
							ColorSpace = Color.ColorSpace.CSEnum.sRGB;
							break; // from switch (cs)...
						
						case 17: 
							ColorSpace = Color.ColorSpace.CSEnum.GreyScale;
							break; // from switch (cs)...
						
						case 18: 
							ColorSpace = Color.ColorSpace.CSEnum.sYCC;
							break; // from switch (cs)...
                        case 20:
                            ColorSpace = Color.ColorSpace.CSEnum.esRGB;
                            break;

                        #region Known but unsupported colorspaces
                        case 3:
                            FacilityManager.getMsgLogger().printmsg(MsgLogger_Fields.WARNING, "Unsupported enumerated colorspace YCbCr(2) in color specification box");
                            ColorSpace = Color.ColorSpace.CSEnum.Unknown;
                            break;
                        case 4:
                            FacilityManager.getMsgLogger().printmsg(MsgLogger_Fields.WARNING, "Unsupported enumerated colorspace YCbCr(3) in color specification box");
                            ColorSpace = Color.ColorSpace.CSEnum.Unknown;
                            break;
                        case 9:
                            FacilityManager.getMsgLogger().printmsg(MsgLogger_Fields.WARNING, "Unsupported enumerated colorspace PhotoYCC in color specification box");
                            ColorSpace = Color.ColorSpace.CSEnum.Unknown;
                            break;
                        case 11:
                            FacilityManager.getMsgLogger().printmsg(MsgLogger_Fields.WARNING, "Unsupported enumerated colorspace CMY in color specification box");
                            ColorSpace = Color.ColorSpace.CSEnum.Unknown;
                            break;
                        case 12:
                            FacilityManager.getMsgLogger().printmsg(MsgLogger_Fields.WARNING, "Unsupported enumerated colorspace CMYK in color specification box");
                            ColorSpace = Color.ColorSpace.CSEnum.Unknown;
                            break;
                        case 13:
                            FacilityManager.getMsgLogger().printmsg(MsgLogger_Fields.WARNING, "Unsupported enumerated colorspace YCCK in color specification box");
                            ColorSpace = Color.ColorSpace.CSEnum.Unknown;
                            break;
                        case 14:
                            FacilityManager.getMsgLogger().printmsg(MsgLogger_Fields.WARNING, "Unsupported enumerated colorspace CIELab in color specification box");
                            ColorSpace = Color.ColorSpace.CSEnum.Unknown;
                            break;
                        case 15:
                            FacilityManager.getMsgLogger().printmsg(MsgLogger_Fields.WARNING, "Unsupported enumerated colorspace Bi-Level(2) in color specification box");
                            ColorSpace = Color.ColorSpace.CSEnum.Unknown;
                            break;
                        case 19:
                            FacilityManager.getMsgLogger().printmsg(MsgLogger_Fields.WARNING, "Unsupported enumerated colorspace CIEJab in color specification box");
                            ColorSpace = Color.ColorSpace.CSEnum.Unknown;
                            break;
                        case 21:
                            FacilityManager.getMsgLogger().printmsg(MsgLogger_Fields.WARNING, "Unsupported enumerated colorspace ROMM-RGB in color specification box");
                            ColorSpace = Color.ColorSpace.CSEnum.Unknown;
                            break;
                        case 22:
                            FacilityManager.getMsgLogger().printmsg(MsgLogger_Fields.WARNING, "Unsupported enumerated colorspace YPbPr(1125/60) in color specification box");
                            ColorSpace = Color.ColorSpace.CSEnum.Unknown;
                            break;
                        case 23:
                            FacilityManager.getMsgLogger().printmsg(MsgLogger_Fields.WARNING, "Unsupported enumerated colorspace YPbPr(1250/50) in color specification box");
                            ColorSpace = Color.ColorSpace.CSEnum.Unknown;
                            break;
                        case 24:
                            FacilityManager.getMsgLogger().printmsg(MsgLogger_Fields.WARNING, "Unsupported enumerated colorspace e-sYCC in color specification box");
                            ColorSpace = Color.ColorSpace.CSEnum.Unknown;
                            break;
                        #endregion

                        default: 
							FacilityManager.getMsgLogger().printmsg(MsgLogger_Fields.WARNING, "Unknown enumerated colorspace (" + cs + ") in color specification box");
							ColorSpace = Color.ColorSpace.CSEnum.Unknown;
							break;
						
					}
					break; // from switch (boxHeader[0])...
				
				case 2: 
					Method = Color.ColorSpace.MethodEnum.ICC_PROFILED;
                    int size = Icc.ICCProfile.getInt(boxHeader, 3);
					ICCProfile = new byte[size];
					in_Renamed.seek(dataStart + 3);
					in_Renamed.readFully(ICCProfile, 0, size);
					break; // from switch (boxHeader[0])...
				
				default: 
					throw new ColorSpaceException("Bad specification method (" + boxHeader[0] + ") in " + this);
           			
			}
		}
		
		/// <summary>Return a suitable String representation of the class instance. </summary>
		public override string ToString()
		{
			StringBuilder rep = new StringBuilder("[ColorSpecificationBox ");
			rep.Append("method= ").Append(Convert.ToString(Method)).Append(", ");
			rep.Append("colorspace= ").Append(Convert.ToString(ColorSpace)).Append("]");
			return rep.ToString();
		}

	    static ColorSpecificationBox()
	    {
	        type = FileFormatBoxes.COLOUR_SPECIFICATION_BOX;
	    }
	}
}