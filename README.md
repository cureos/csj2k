# Portable CSJ2K - A Managed JPEG2000 Codec

Copyright (c) 1999-2000 JJ2000 Partners, original C# port (c) 2007-2012 Jason S. Clary, adaptation to Portable Class Library, Windows Store, Windows Phone, WPF and Silverlight extensions (c) 2013 Anders Gustafsson, Cureos AB   

Licensed and distributable under the terms of the [BSD license](http://www.opensource.org/licenses/bsd-license.php)

## Summary

This is a Portable Class Library adaptation of [CSJ2K](http://csj2k.codeplex.com/), which provides JPEG 2000 decoding and encoding functionality to .NET based platforms. *CSJ2K* is by itself a C# port of the Java 
package *jj2000*, version 5.1. This Portable Class Library adaptation of *CSJ2K* makes it possible to implement JPEG decoding and encoding on the following platforms:

* Windows Store apps
* Windows Phone version 7 and higher
* Silverlight version 4 and higher
* .NET Framework version 4 and higher

The code is still applicable to .NET 2.0 and later as well; the Windows Forms based original class library is maintained here for reference.

Along with the *CSJ2K* Portable Class Library there are also platform specific complementary libraries for bitmap processing and file handling. In particular, the .NET Framework complementary library implements bitmap processing
for `WriteableBitmap`, thus facilitating JPEG 2000 decoding in WPF based applications.

Included are very basic Windows Store, WPF and Silverlight test applications for reading and displaying JPEG 2000 files. There is also a Windows Phone 8 application, although this application has not yet been confirmed to work.

## Installation

Apart from buidling the relevant class libraries from source, prebuilt packages for the supported platforms can also be obtained via [NuGet](https://nuget.org/packages/CSJ2K/).

## Usage

The Portable Class Library provides interfaces for bitmap rendering, file I/O and logging. It is the responsiblity of the end application to register implementations of these interfaces before JPEG 2000 decoding and encoding
can be performed. A static convenience method available on all platforms is implemented for this purpose:

```csharp
CSJ2KSetup.RegisterCreators();
```

To decode a JPEG 2000 encoded image, call one of the following methods:

```csharp
    public class J2kImage
    {
        public static object FromFile(string filename);
        public static object FromBytes(byte[] j2kdata);
        public static object FromStream(Stream stream);
	}
```

The returned `object` is a regular bitmap, typically a `WriteableBitmap` or, in the case of the Windows Forms targeted library, a `System.Drawing.Bitmap`.

Please note that a public API for JPEG 2000 **encoding** is not yet provided; only **decoding** is easily performed at this stage.

## Dependencies

The `WriteableBitmap` implementations make use of the [WriteableBitmap Extensions library](http://writeablebitmapex.codeplex.com/).

## Links

* [CSJ2K web site on Codeplex](http://csj2k.codeplex.com/)
* [JJ2000 original site; link no longer appears to be working?](http://jj2000.epfl.ch/)
* [Guide to the practical implementation of JPEG2000](http://www.jpeg.org/jpeg2000guide/guide/contents.html)