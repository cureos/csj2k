// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using System.IO;

namespace CSJ2K.Util
{
    using CSJ2K.j2k.image;
    using CSJ2K.j2k.image.input;

    internal class BitmapImageReader : ImgReader
    {
        private readonly Stream stream;

        #region FIELDS

        #endregion

        #region CONSTRUCTORS

        internal BitmapImageReader(Stream stream)
        {
            this.stream = stream;
        }

        #endregion

        #region METHODS

        public override void close()
        {
            this.stream.Dispose();
        }

        public override bool isOrigSigned(int c)
        {
            return false;
        }

        public override int getFixedPoint(int param1)
        {
            throw new System.NotImplementedException();
        }

        public override DataBlk getInternCompData(DataBlk param1, int param2)
        {
            throw new System.NotImplementedException();
        }

        public override DataBlk getCompData(DataBlk param1, int param2)
        {
            throw new System.NotImplementedException();
        }

        public override int getNomRangeBits(int param1)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
