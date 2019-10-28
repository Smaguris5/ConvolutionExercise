using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AIProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap d = new Bitmap("original_image.bmp"); //Importing image as bitmap

            int[,] image = new int[4, 4];//Setting up the arrays
            int[,] convolutedImage = new int[4, 4];
            int[,] convolutedImageAdjusted = new int[4, 4];
            int[,] filter = new int[3, 3]
            {
                {1, 2, 1},
                {0, 0, 0},
                {-1, -2, -1}
            };
            //int[,] image = new int[4, 4] //setting image manually
            //{
            //    {128, 255, 196, 64},
            //    {20, 128, 255, 96},
            //    {210, 20, 128, 255},
            //    {210, 210, 20, 128}
            //};

            for (var x = 0; x < d.Width; x++)//retreiving each pixel from imported bitmap and recording each color value to an array NOTE: as of now it only accurately retrieves greyscale values
            {
                for (var y = 0; y < d.Height; y++)
                {
                    var s = d.GetPixel(x, y);
                    image[y,x] = s.G;
                }
            }

            for (int hpos=0; hpos<image.GetLength(0); hpos++)//Convoluting the array
            {
                for (int vpos = 0; vpos < image.GetLength(1); vpos++)
                {
                    convolutedImage[hpos, vpos] = Convolute(image, filter, hpos, vpos);
                }
            }

            Console.WriteLine("Original Array:"); //outputting arrays
            ArrayOutput(image);

            Console.Write(Environment.NewLine + Environment.NewLine);

            Console.WriteLine("Filter:");
            ArrayOutput(filter);

            Console.Write(Environment.NewLine + Environment.NewLine);

            Console.WriteLine("Convoluted Array:"); 
            ArrayOutput(convolutedImage);

            Adjust(convolutedImage, convolutedImageAdjusted);
            Console.Write(Environment.NewLine + Environment.NewLine);
            Console.WriteLine("Adjusted to greyscale values:");
            ArrayOutput(convolutedImageAdjusted);


            Bitmap bitmap = new Bitmap(image.GetLength(0), image.GetLength(1)); //Creating bitmap which is ultimatelly exported as a BMP
            //CreateBitmap(bitmap, image); //saving original image as BMP (only in case it is initialised maunally and not loaded from image)
            //bitmap.Save("original_image.bmp");

            CreateBitmap(bitmap, convolutedImageAdjusted);
            bitmap.Save("convoluted_image.bmp");

            Console.ReadKey();
        }

        static void CreateBitmap(Bitmap bitmap, int[,] img) //creating a bitmap image using values from the array
        {
            for (var x = 0; x < bitmap.Width; x++)
            {
                for (var y = 0; y < bitmap.Height; y++)
                {
                    int f = img[x, y];
                    bitmap.SetPixel(y, x, Color.FromArgb(255, f, f, f));
                }
            }
        }

        static void ArrayOutput(int[,] arr) // Standard method to output each 2d array
        {
            for (int hpos = 0; hpos < arr.GetLength(0); hpos++)
            {
                for (int vpos = 0; vpos < arr.GetLength(1); vpos++)
                {
                    Console.Write(" | " + arr[hpos, vpos]);
                }
                Console.Write(" |" + Environment.NewLine + " ---------------------------" + Environment.NewLine);
            }
        }

        static int Convolute(int[,] image, int[,] filter, int hpos, int vpos)
        {
            int originalNo = image[hpos % image.GetLength(0), vpos % image.GetLength(1)];
            int convolutedNo = 0;

            int mod(int x, int m) //setting modulus, which in this case does the wrapping of an array
            {
                return (x % m + m) % m;
            }

            for (int h = -1; h < filter.GetLength(0)-1; h++)//Convoluting each entry in the array
            {
                for (int v = -1; v < filter.GetLength(1)-1; v++)
                {
                    convolutedNo += image[mod(hpos + h, image.GetLength(0)), mod(vpos + v, image.GetLength(1))] * filter[h+1, v+1];
                }
            }
            

            return convolutedNo;
        }

        static void Adjust(int[,] unadjusted, int[,] adjusted) // Adjusting to greyscale
        {
            for (int hpos = 0; hpos < unadjusted.GetLength(0); hpos++)
            {
                for (int vpos = 0; vpos < unadjusted.GetLength(1); vpos++)
                {
                    adjusted[hpos, vpos] = unadjusted[hpos, vpos];
                    if (adjusted[hpos, vpos] > 255)
                        adjusted[hpos, vpos] = 255;
                    if (adjusted[hpos, vpos] < 0)
                        adjusted[hpos, vpos] = 0;
                }
            }
        }
    }
}
