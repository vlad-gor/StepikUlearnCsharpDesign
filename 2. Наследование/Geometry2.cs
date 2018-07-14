using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.Geometry
{
    // Интерфейс
    public interface IVisitor
    {
        void Visit(Ball ball);
        void Visit(Cube cube);
        void Visit(Cyllinder cyllinder);
    }

    // Базовый класс
    public abstract class Body
	{
        public abstract double GetVolume();
        public abstract void Accept(IVisitor visitor);
       
    }

    // Заготовка класса для задачи на Visitor

    // Вычисление площади поверхности
    public class SurfaceAreaVisitor : IVisitor
    {
       public double SurfaceArea { get; private set; }

       public void Visit(Ball ball)
        {
            SurfaceArea= 4.0 * Math.PI * ball.Radius * ball.Radius;
        }

        public void Visit(Cube cube)
        {
            SurfaceArea = 6 * cube.Size * cube.Size;
        }

        public void Visit(Cyllinder cyllinder)
        {
            SurfaceArea = 2 * Math.PI * cyllinder.Radius * (cyllinder.Height + cyllinder.Radius);
        }
    }

    public class DimensionsVisitor : IVisitor
    {
        public Dimensions Dimensions { get; private set; }

        public void Visit(Ball ball)
        {
            Dimensions = new Dimensions(2*ball.Radius, 2 * ball.Radius);
        }

        public void Visit(Cube cube)
        {
           Dimensions = new Dimensions(cube.Size, cube.Size);
        }

        public void Visit(Cyllinder cyllinder)
        {
           Dimensions = new Dimensions(2*cyllinder.Radius, cyllinder.Height);
        }      
    }

    // Шар

    public class Ball : Body
    {
        public double Radius { get; set; }      

        public override double GetVolume()
        {
            return 4.0 * Math.PI * Math.Pow(Radius, 3) / 3;
        }             

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    // Куб
	public class Cube : Body
	{
		public double Size { get; set; }     

        public override double GetVolume()
        {
            return Math.Pow(Size, 3);
        }      

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    // Цилиндр
	public class Cyllinder : Body
	{
		public double Height { get; set; }
		public double Radius { get; set; }       

        public override double GetVolume()
        {
            var c = this as Cyllinder;
            return Math.PI * Math.Pow(c.Radius, 2) * c.Height;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }      
}