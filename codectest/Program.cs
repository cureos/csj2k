namespace codectest
{
	using System;
	using System.Drawing;
	using System.IO;

	using CSJ2K;

	class Program
	{
		static void Main(string[] args)
		{
			File.Delete("file11.jp2");
			File.Delete("file12.jp2");
			File.Delete("file13.jp2");

			using (var ppm = File.OpenRead("a1_mono.ppm"))
			{
				var enc = J2kImage.ToBytes(ppm);
				File.WriteAllBytes("file11.jp2", enc);
			}

			using (var ppm = File.OpenRead("a2_colr.ppm"))
			{
				var enc = J2kImage.ToBytes(ppm);
				File.WriteAllBytes("file12.jp2", enc);
			}

			using (var ppm = File.OpenRead("c1p0_05_0.pgx"))
			{
				var enc = J2kImage.ToBytes(ppm);
				File.WriteAllBytes("file13.jp2", enc);
			}

			for (int i = 1; i <= 13; i++)
			{
				try
				{
					HiPerfTimer timer = new HiPerfTimer();
					timer.Start();
					Bitmap image = (Bitmap)J2kImage.FromFile("file" + i + ".jp2");
					timer.Stop();
					Console.WriteLine("file" + i + ": " + timer.Duration + " seconds");

					Bitmap histogram=GenerateHistogram(image);

					if (image.Height > 2 * histogram.Height)
					{
						Graphics g = Graphics.FromImage(image);
						g.DrawImage(histogram, 0, 0);
					}

					ImageDialog dlg = new ImageDialog();
					dlg.Text = "file" + i + ".jp2";
					dlg.ClientSize = new Size(image.Width, image.Height);
					dlg.pictureBox1.Image = image;
					dlg.ShowDialog();
				}
				catch (Exception e)
				{
					Console.WriteLine("file" + i + ":\r\n" + e.Message);
					if (e.InnerException != null)
					{
						Console.WriteLine(e.InnerException.Message);
						Console.WriteLine(e.InnerException.StackTrace);
					}
					else Console.WriteLine(e.StackTrace);

				}
			}
		}
		static Bitmap GenerateHistogram(Bitmap image)
		{
			Bitmap histogram = new Bitmap(256, 100);

			int[] colorcounts = new int[256];

			// This is ungodly slow but it's just for diagnostics.
			for (int y = 0; y < image.Height; y++)
			{
				for (int x = 0; x < image.Width; x++)
				{
					Color c=image.GetPixel(x, y);
					colorcounts[c.R]++;
					colorcounts[c.G]++;
					colorcounts[c.B]++;
				}
			}

			int maxval = 0;
			for (int i = 0; i < 256; i++) if (colorcounts[i] > maxval) maxval = colorcounts[i];
			for (int i = 1; i < 255; i++)
			{
				//Console.WriteLine(i + ": " + histogram[i] + "," + (((float)histogram[i] / (float)maxval) * 100F));
				colorcounts[i] = (int)Math.Round(((double)colorcounts[i] / (double)maxval) * 100D);
			}
			for (int x = 0; x < 256; x++)
			{
				for (int y = 0; y < 100; y++)
				{
					if (colorcounts[x] >= (100 - y)) histogram.SetPixel(x, y, Color.Black);
					else histogram.SetPixel(x, y, Color.White);
				}
			}
			return histogram;
		}
	}
}
