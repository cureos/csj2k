// Copyright (c) 2007-2017 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using System;
using System.Collections.Generic;

using FileFormatBoxes = CSJ2K.j2k.fileformat.FileFormatBoxes;
using ICCProfile = CSJ2K.Icc.ICCProfile;
using RandomAccessIO = CSJ2K.j2k.io.RandomAccessIO;

namespace CSJ2K.Color.Boxes
{
	/// <summary> The abstract super class modeling the aspects of
	/// a JP2 box common to all such boxes.
	/// 
	/// </summary>
	/// <version> 	1.0
	/// </version>
	/// <author> 	Bruce A. Kern
	/// </author>
	public abstract class JP2Box
	{
		/// <summary>Platform dependant line terminator </summary>
		public static readonly string eol = Environment.NewLine;

		/// <summary>Box type</summary>
		public static int type;
		
		/// <summary>Return a String representation of the Box type. </summary>
		public static string getTypeString(int t)
		{
			return BoxType.get_Renamed(t);
		}
		
		/// <summary>Length of the box.</summary>
		public int length;

		/// <summary>input file</summary>
		protected RandomAccessIO in_Renamed;

		/// <summary>offset to start of box</summary>
		protected int boxStart;

		/// <summary>offset to end of box</summary>
		protected int boxEnd;

		/// <summary>offset to start of data in box</summary>
		protected int dataStart;

        /// <summary> Construct a JP2Box from an input image.</summary>
        /// <param name="in_Renamed">RandomAccessIO jp2 image
        /// </param>
        /// <param name="boxStart">offset to the start of the box in the image
        /// </param>
        protected JP2Box(RandomAccessIO in_Renamed, int boxStart)
		{
			byte[] boxHeader = new byte[16];
			
			this.in_Renamed = in_Renamed;
			this.boxStart = boxStart;
			
			this.in_Renamed.seek(this.boxStart);
			this.in_Renamed.readFully(boxHeader, 0, 8);
			
			dataStart = boxStart + 8;
            length = ICCProfile.getInt(boxHeader, 0);
			boxEnd = boxStart + length;
			if (length == 1)
				throw new ColorSpaceException("extended length boxes not supported");
		}
		
		/// <summary>Return the box type as a String. </summary>
		public virtual string getTypeString()
		{
			return BoxType.get_Renamed(type);
		}
		
		/// <summary>JP2 Box structure analysis help </summary>
		private static class BoxType
		{
			private static readonly Dictionary<int, string> map;
			
			public static string get_Renamed(int type)
			{
				return map[type];
			}
			
			static BoxType()
			{
			    map = new Dictionary<int, string>
			              {
			                  { FileFormatBoxes.BITS_PER_COMPONENT_BOX, "BITS_PER_COMPONENT_BOX" },
			                  { FileFormatBoxes.CAPTURE_RESOLUTION_BOX, "CAPTURE_RESOLUTION_BOX" },
			                  { FileFormatBoxes.CHANNEL_DEFINITION_BOX, "CHANNEL_DEFINITION_BOX" },
			                  { FileFormatBoxes.COLOUR_SPECIFICATION_BOX, "COLOUR_SPECIFICATION_BOX" },
			                  { FileFormatBoxes.COMPONENT_MAPPING_BOX, "COMPONENT_MAPPING_BOX" },
			                  { FileFormatBoxes.CONTIGUOUS_CODESTREAM_BOX, "CONTIGUOUS_CODESTREAM_BOX" },
			                  { FileFormatBoxes.DEFAULT_DISPLAY_RESOLUTION_BOX, "DEFAULT_DISPLAY_RESOLUTION_BOX" },
			                  { FileFormatBoxes.FILE_TYPE_BOX, "FILE_TYPE_BOX" },
			                  { FileFormatBoxes.IMAGE_HEADER_BOX, "IMAGE_HEADER_BOX" },
			                  { FileFormatBoxes.INTELLECTUAL_PROPERTY_BOX, "INTELLECTUAL_PROPERTY_BOX" },
			                  { FileFormatBoxes.JP2_HEADER_BOX, "JP2_HEADER_BOX" },
			                  { FileFormatBoxes.JP2_SIGNATURE_BOX, "JP2_SIGNATURE_BOX" },
			                  { FileFormatBoxes.PALETTE_BOX, "PALETTE_BOX" },
			                  { FileFormatBoxes.RESOLUTION_BOX, "RESOLUTION_BOX" },
			                  { FileFormatBoxes.URL_BOX, "URL_BOX" },
			                  { FileFormatBoxes.UUID_BOX, "UUID_BOX" },
			                  { FileFormatBoxes.UUID_INFO_BOX, "UUID_INFO_BOX" },
			                  { FileFormatBoxes.UUID_LIST_BOX, "UUID_LIST_BOX" },
			                  { FileFormatBoxes.XML_BOX, "XML_BOX" }
			              };
			}
		}
	}
}
