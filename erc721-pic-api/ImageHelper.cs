using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace erc721_pic_api
{
    public class ImageHelper
    {
        private static ColorScheme[] Schemes = new ColorScheme[]
        {
            new ColorScheme()
            {
                detail = new Rgba32(142,0,0),
                dark = new Rgba32(45,45,45),
                light = new Rgba32(160,160,160),
                middle = new Rgba32(123,123,123)
            },
            new ColorScheme()
            {
                detail = new Rgba32(255,124,18),
                dark = new Rgba32(165,120,94),
                light = new Rgba32(244,207,187),
                middle = new Rgba32(207,163,115)
            },
            new ColorScheme() 
            {
                detail = new Rgba32(0,0,0),
                dark = new Rgba32(74,44,22),
                light = new Rgba32(140,105,58),
                middle = new Rgba32(102,52,16)
            },
            new ColorScheme() 
            {
                detail = new Rgba32(19,101,54),
                dark = new Rgba32(124,64,46),
                light = new Rgba32(197,158,144),
                middle = new Rgba32(164,113,96)
            },
            new ColorScheme() 
            {
                detail = new Rgba32(18,0,173),
                dark = new Rgba32(248,158,101),
                light = new Rgba32(254,223,148),
                middle = new Rgba32(253,197,129)
            },
            new ColorScheme()
            {
                detail = new Rgba32(0,0,0),
                dark = new Rgba32(99,100,103),
                light = new Rgba32(173,175,178),
                middle = new Rgba32(147,149,152)
            },
            new ColorScheme() 
            {
                detail = new Rgba32(220,0,173),
                dark = new Rgba32(48,40,38),
                light = new Rgba32(145,102,30),
                middle = new Rgba32(60,53,51)
            },
            new ColorScheme() 
            {
                detail = new Rgba32(255,255,255),
                dark = new Rgba32(138,140,142),
                light = new Rgba32(209,211,212),
                middle = new Rgba32(189,190,193)
            },
            new ColorScheme()
            {
                detail = new Rgba32(220,93,66),
                dark = new Rgba32(85,43,26),
                light = new Rgba32(104,74,58),
                middle = new Rgba32(220,221,223)
            },
            new ColorScheme()
            {
                detail = new Rgba32(0,0,0),
                dark = new Rgba32(56,37,38),
                light = new Rgba32(151,85,30),
                middle = new Rgba32(214,212,212)
            },
            new ColorScheme()
            {
                detail = new Rgba32(255,255,255),
                dark = new Rgba32(95,38,9),
                light = new Rgba32(145,81,28),
                middle = new Rgba32(102,52,16)
            },
            new ColorScheme()
            {
                detail = new Rgba32(45,45,45),
                dark = new Rgba32(5,5,5),
                light = new Rgba32(5,5,5),
                middle = new Rgba32(5,5,5)
            },
            new ColorScheme()
            {
                detail =  new Rgba32(245,245,245),
                dark = new Rgba32(245,245,245),
                light = new Rgba32(245,245,245),
                middle = new Rgba32(245,245,245)
            },
            new ColorScheme()
            {
                detail = new Rgba32(142,0,0),
                dark = new Rgba32(0,255,0),
                light = new Rgba32(255,255,0),
                middle = new Rgba32(0,110,255)
            },
            new ColorScheme()
            {
                detail =  new Rgba32(18,0,173),
                dark = new Rgba32(65,30,98),
                light = new Rgba32(195,59,130),
                middle = new Rgba32(124,59,130)
            },
            new ColorScheme()
            {
                detail = new Rgba32(45,45,45),
                dark =   new Rgba32(184,36,185),
                middle = new Rgba32(220,0,220),
                light =  new Rgba32(220,78,220)
            }
        };

        public static byte[] pictureFromGenes(string Genes, int size = 32)
        {
            Genes = normalizeGeneString(Genes);

            Image imageFile = Image.Load("./Assets/background1.png");
            Image<Rgba32> tail = (Image<Rgba32>)Image.Load("./Assets/tail" + tailNumber(Genes) + ".png");
            ApplyColorScheme(ref tail, ColorSchemeFromGenes(Genes, 61));
            Image<Rgba32> body = (Image<Rgba32>)Image.Load("./Assets/body1.png");
            ApplyColorScheme(ref body, ColorSchemeFromGenes(Genes, 57));
            Image<Rgba32> head = (Image<Rgba32>)Image.Load("./Assets/head" + headNumber(Genes) + ".png");
            ApplyColorScheme(ref head, ColorSchemeFromGenes(Genes, 53));
            Image<Rgba32> eyes = (Image<Rgba32>)Image.Load("./Assets/eyes1.png");
            ApplyColorScheme(ref eyes, ColorSchemeFromGenes(Genes, 49));
            Image<Rgba32> ears = (Image<Rgba32>)Image.Load("./Assets/ears" + earNumber(Genes) + ".png");
            ApplyColorScheme(ref ears, ColorSchemeFromGenes(Genes, 45));

            imageFile.Mutate(x => x.DrawImage(tail, new GraphicsOptions()));
            imageFile.Mutate(x => x.DrawImage(body, new GraphicsOptions()));
            imageFile.Mutate(x => x.DrawImage(head, new GraphicsOptions()));
            imageFile.Mutate(x => x.DrawImage(eyes, new GraphicsOptions()));
            imageFile.Mutate(x => x.DrawImage(ears, new GraphicsOptions()));

            imageFile.Mutate(x => x.Resize(size, size, KnownResamplers.NearestNeighbor));

            using var ms = new MemoryStream();
            var encoder = new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder();
            imageFile.Save(ms, encoder);

            return ms.ToArray();
        }

        public static string normalizeGeneString(string input)
        {
            if (input.Length > 64)
            {
                input = input.Substring(0, 64);
            }
            if (input.StartsWith("0x"))
            {
                input = input.Substring(2);
            }
            input = input.ToUpper();
            if (input.Length < 64)
            {
                int missing = 64 - input.Length;
                for (int i = 0; i < missing; i++)
                {
                    input = "0" + input;
                }
            }
            Console.WriteLine(input.Length);
            Console.WriteLine(input);
            return input;
        }

        public static int tailNumber(string normGenes)
        {
            int i = Convert.ToInt32(normGenes.Substring(60, 1), 16);
            if (i > 1)
                i = 1;
            return i;
        }

        public static int earNumber(string normGenes)
        {
            int i = Convert.ToInt32(normGenes.Substring(44, 1), 16);
            if (i > 3)
                i = 3;
            return i;
        }

        public static int headNumber(string normGenes)
        {
            int i = Convert.ToInt32(normGenes.Substring(52, 1), 16);
            if (i > 1)
                i = 1;
            return i;
        }

        private static void ApplyColorScheme(ref Image<Rgba32> work, ColorScheme colors)
        {
            for (int y = 0; y < work.Height; y++)
            {
                Span<Rgba32> pixelRowSpan = work.GetPixelRowSpan(y);
                for (int x = 0; x < work.Width; x++)
                {
                    if (pixelRowSpan[x].A > 10)
                        if (pixelRowSpan[x].R == 244) // light
                            pixelRowSpan[x] = colors.light;
                        else if (pixelRowSpan[x].R == 165) // dark
                            pixelRowSpan[x] = colors.dark;
                        else if (pixelRowSpan[x].R == 207) // middle
                            pixelRowSpan[x] = colors.middle;
                        else if (pixelRowSpan[x].R == 142) // detail
                            pixelRowSpan[x] = colors.detail;
                }
            }
        }

        private static ColorScheme ColorSchemeFromGenes(string Genes, int IndexStart)
        {
            int i = Convert.ToInt32(Genes.Substring(IndexStart, 1), 16);
            int j = Convert.ToInt32(Genes.Substring(IndexStart + 1, 1), 16);
            int k = Convert.ToInt32(Genes.Substring(IndexStart + 2, 1), 16);
            int l = Convert.ToInt32(Genes.Substring(44, 1), 16);
            return new ColorScheme()
            {
                dark = Schemes[i].dark,
                light = Schemes[j].light,
                middle = Schemes[k].middle,
                detail = Schemes[l].detail
            };
        }

        class ColorScheme
        {
            public Rgba32 light = new Rgba32();
            public Rgba32 middle = new Rgba32();
            public Rgba32 dark = new Rgba32();
            public Rgba32 detail = new Rgba32();
        }

    }
}
