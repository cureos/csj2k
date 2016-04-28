// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using CSJ2K.Util;

namespace CSJ2K.iOS.Test
{
    using System;

    using CoreGraphics;

    using UIKit;

    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
            DotnetFileInfoCreator.Register();
            DotnetFileStreamCreator.Register();
            DotnetMsgLogger.Register();
            IOSImageCreator.Register();

            using (var stream = typeof(ViewController).Assembly.GetManifestResourceStream("CSJ2K.iOS.Test.Files.file2.jp2"))
            {
                var uiImage = J2kImage.FromStream(stream).As<UIImage>();
                var imageView = new UIImageView(new CGRect(0, 0, 600, 600)) { Image = uiImage };
                this.View.Add(imageView);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}