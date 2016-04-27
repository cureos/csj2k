// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using System.IO;

using Android.App;
using Android.Graphics;
using Android.Widget;
using Android.OS;

namespace CSJ2K.Android.Test
{
    [Activity(Label = "CSJ2K.Android.Test", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Bitmap bitmap;
            using (var stream = new MemoryStream())
            {
                this.Assets.Open("file2.jp2").CopyTo(stream);
                stream.Seek(0, SeekOrigin.Begin);
                bitmap = J2kImage.FromStream(stream).As<Bitmap>();
            }

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            var imageView = FindViewById<ImageView>(Resource.Id.imageView1);

            imageView.SetImageBitmap(bitmap);
        }
    }
}

