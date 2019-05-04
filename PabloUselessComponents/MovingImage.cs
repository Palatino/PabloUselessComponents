using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Grasshopper;

namespace PabloUselessComponents
{

    class MovingImage
    {
        private List<Bitmap> possibleImages = new List<Bitmap>(){

            Properties.Resources.DVD,
            Properties.Resources.DVD2,
            Properties.Resources.DVD3,
            Properties.Resources.DVD4,
            Properties.Resources.DVD5,
            Properties.Resources.DVD6,
            Properties.Resources.DVD7,
        };

        Random random = new Random();
        Bitmap image { get; }
        float x { get; set; }
        float y { get; set; }
        float x_increment { get; set; }
        float y_increment { get; set; }
        Grasshopper.GUI.Canvas.GH_Canvas canvas = Grasshopper.Instances.ActiveCanvas;

        public MovingImage()
        {
            //Get a random image from the list of pics
            image = possibleImages[random.Next(possibleImages.Count)];

            //Set some random opacity level to the image

            float opacity = (float) (random.NextDouble() * 0.7);
            image = new Bitmap(Helpers.SetImageOpacity(image, opacity));

            //Create elements within the are where the components are
            x = Cubensis.min_x -100 + (float)(random.NextDouble() * (100+(Cubensis.max_x - Cubensis.min_x)));
            y = Cubensis.min_y - 100 + (float)(random.NextDouble() * (100 + (Cubensis.max_y - Cubensis.min_y)));

            x_increment = random.Next(10)-5;
            y_increment = random.Next(10) - 5;
        }

        public void Draw()
        {
            canvas.Graphics.DrawImage(image, new PointF(x,y));

        }

        public void Update()
        {
            if (x < 0) x_increment *= -1;
            if (y < 0) y_increment *= -1;

            PointF pivotP = new PointF(x, y);
            RectangleF imgRec = new RectangleF((float)x, (float)y, image.Width, image.Height);



            foreach (RectangleF rec in Cubensis.boundaries)
            {
                if(rec.IntersectsWith(imgRec))
                {
                    if((x+0.5*image.Width)<rec.Left || (x + 0.5 * image.Width) > rec.Left + rec.Width)
                    {
                        x_increment *= -1;
                    }

                    if ((y+0.5*image.Height) < rec.Top || (y + 0.5 * image.Height) > rec.Top + rec.Height)
                    {
                        y_increment *= -1;
                    }
                    
                }
            }
            x += x_increment;
            y += y_increment;
        }



    }
}
