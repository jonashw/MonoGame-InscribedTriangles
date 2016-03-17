using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameRubiks
{
    public static class GeometricTextureFactory
    {
        public enum CircleTextureStyle
        {
            Filled,
            Outlined,
            OutlinedAndFilled
        }
        public static Texture2D Circle(GraphicsDevice graphicsDevice, int radius, Color color, CircleTextureStyle style = CircleTextureStyle.OutlinedAndFilled)
        {
            var outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds...
            var circle = new Texture2D(graphicsDevice, outerRadius, outerRadius);

            var data = new Color[outerRadius * outerRadius];

            // Colour the entire texture transparent first.
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = Color.Transparent;
            }

            // Work out the minimum step necessary using trigonometry + sine approximation.
            double angleStep = 1f / radius;

            if (style == CircleTextureStyle.Outlined || style == CircleTextureStyle.OutlinedAndFilled)
            {
                for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
                {
                    // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
                    var x = (int)Math.Round(radius + radius * Math.Cos(angle));
                    var y = (int)Math.Round(radius + radius * Math.Sin(angle));
                    var index = y*outerRadius + x + 1;
                    data[index] = color;
                }
            }

            if (style == CircleTextureStyle.Filled || style == CircleTextureStyle.OutlinedAndFilled)
            {
                for (var x = 0; x <= outerRadius; x++)
                {
                    for (var y = 0; y <= outerRadius; y++)
                    {
                        if (Math.Pow(x - radius, 2) + Math.Pow(y - radius, 2) <= radius*radius)
                        {
                            var index = y*outerRadius + x + 1;
                            data[index] = color;
                        }
                    }
                }
            }

            circle.SetData(data);
            return circle;
        }
    }
}