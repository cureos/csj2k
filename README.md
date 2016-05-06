# CSJ2K - A Managed and Portable JPEG2000 Codec

Copyright (c) 1999-2000 JJ2000 Partners; original C# port (c) 2007-2012 Jason S. Clary; C# encoding and adaptation to Portable Class Library with platform specific support (c) 2013-2016 Anders Gustafsson, Cureos AB   

Licensed and distributable under the terms of the [BSD license](http://www.opensource.org/licenses/bsd-license.php)

**NOTE! The following information applies to the upcoming version 2.0 of CSJ2K. Please consult commit associated with release [0.9.1](https://github.com/cureos/csj2k/releases/tag/v.0.9.1) for details about 
0.9.x versions of CSJ2K.**

## Summary

This is a Portable Class Library adaptation of [CSJ2K](http://csj2k.codeplex.com/), which provides JPEG 2000 decoding and encoding functionality to .NET based platforms. *CSJ2K* is by itself a C# port of the Java 
package *jj2000*, version 5.1. This Portable Class Library adaptation of *CSJ2K* makes it possible to implement JPEG decoding and encoding on the following platforms:

* Windows Universal Platform (8.1 and 10)
* Windows Phone Silverlight version 8 and higher
* Silverlight version 5
* .NET Framework version 4 and higher
* Xamarin iOS
* Xamarin Android

The code is still applicable to .NET 3.5 and later as well; a .NET 3.5 dedicated class library is maintained here for reference.

Along with the *CSJ2K* Portable Class Library there are also platform specific replacement libraries for bitmap processing and file handling. In particular, the .NET Framework library implements bitmap processing
for `WriteableBitmap`, thus facilitating JPEG 2000 decoding in WPF based applications.

Included are very basic Universal Windows 8.1, WPF, Windows Phone 8 Silverlight, Silverlight 5, Xamarin Android and Xamarin iOS test applications for reading and displaying JPEG 2000 files.

## Installation

Apart from building the relevant class libraries from source, pre-built packages for the supported platforms can also be obtained via [NuGet](https://nuget.org/packages/CSJ2K/).

## Usage

The Portable Class Library provides interfaces for image rendering, file I/O and logging.

On .NET, both `System.Drawing.Bitmap` and `WriteableBitmap` images can be managed, but not simultaneously. By default, `WriteableBitmap` is managed. To work with `Bitmap` images instead, register the corresponding image creator:

    BitmapImageCreator.Register();

To switch back to `WriteableBitmap`:

    WriteableBitmapCreator.Register();

On other platforms, only one image manager is available and automatically selected.

### Decoding

To decode a JPEG 2000 encoded image, call one of the following methods:

```csharp
public class J2kImage
{
	public static PortableImage FromStream(Stream, ParameterList = null);
	public static PortableImage FromBytes(byte[], ParameterList = null);
	public static PortableImage FromFile(string, ParameterList = null);
}
```

`J2kImage.FromFile(string)` is not sufficiently implemented for Silverlight and Windows Phone.

The returned `PortableImage` offers a "cast" method `As<T>()` to obtain an image in the type relevant for the platform. When using the `BitmapImageCreator` on .NET, a cast to `Bitmap` would suffice:

    var bitmap = decodedImage.As<Bitmap>();
	
On platforms implementing `WriteableBitmap`, use:

    var wbm = decodedImage.As<WriteableBitmap>();
	
On Android, use:

    var bitmap = decodedImage.As<Android.Graphics.Bitmap>();

On iOS, depending on context, use:

    var image = decodedImage.As<CGImage>();
    var image = decodedImage.As<CIImage>();
    var image = decodedImage.As<UIImage>();

### Encoding

To encode an image, the following overloads are available:

```csharp
public class J2kImage
{
	public static byte[] ToBytes(object, ParameterList = null);
	public static byte[] ToBytes(BlkImgDataSrc, ParameterList = null);
}
```

The first overload takes an platform-specific image `object`. This is still works-in-progress, but a partial implementation is available for `System.Drawing.Bitmap` objects on .NET Desktop.

The second overload takes an *CSJ2K* specific object implementing the `BlkImgDataSrc` interface. When *Portable Graymap* (PGM), *Portable Pixelmap* (PPM) or JPEG2000 conformance testing format (PGX) objects are available as `Stream`s, 
it is possible to create `BlkImgDataSrc` objects using either of the following methods:

    J2kImage.CreateEncodableSource(Stream);
	J2kImage.CreateEncodableSource(IList<Stream>);
	
For *PGM* and *PPM* images, you would normally use the single `Stream` overload, whereas for *PGX* images, you may enter one `Stream` object per color component.

## Links

* [CSJ2K web site on Codeplex](http://csj2k.codeplex.com/)
* [Guide to the practical implementation of JPEG2000](http://www.jpeg.org/jpeg2000guide/guide/contents.html)
