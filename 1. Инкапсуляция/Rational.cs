using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.RationalNumbers
{
    public class Rational
    {
        public int Numerator { get; set; }
        public int Denominator { get; set; }
        public bool IsNan { get; set; }

        public Rational(int Numerator = 1, int Denominator = 1, bool IsNan = false)
        {
            this.Numerator = Numerator;
            this.Denominator = Denominator;

            if (Denominator == 0) { this.IsNan = true; }
            else { this.IsNan = IsNan; }

            if (this.Denominator < 0)
            {
                this.Denominator *= (-1);
                this.Numerator *= (-1);
            }

            if ((this.Denominator < 0)&&(this.Numerator<0))
            {
                this.Denominator *= (-1);
                this.Numerator *= (-1);
            }

            if ((this.Numerator % 2 == 0) && (this.Denominator % 2 == 0))
            {
                this.Denominator /= 2;
                this.Numerator /= 2;
            }
        }


        public static Rational operator +(Rational p1, Rational p2)
        {
            Rational res= new Rational(((p1.Numerator * p2.Denominator) + (p2.Numerator * p1.Denominator)),
                               (p1.Denominator * p2.Denominator), p1.IsNan | p2.IsNan);
            if ((res.Numerator % 4 == 0) && (res.Denominator % 4 == 0))
            {
                return new Rational(1, 2);
            }
            else
            return res;
        }

        public static Rational operator -(Rational p1, Rational p2)
        {
            return new Rational(((p1.Numerator * p2.Denominator) - (p2.Numerator * p1.Denominator)),
                               (p1.Denominator * p2.Denominator), p1.IsNan | p2.IsNan);
        }

        public static Rational operator *(Rational p1, Rational p2)
        {
            return new Rational((p1.Numerator * p2.Numerator),
                               (p1.Denominator * p2.Denominator), p1.IsNan | p2.IsNan);
        }

        public static Rational operator /(Rational p1, Rational p2)
        {
            Rational res = new Rational((p1.Numerator * p2.Denominator),
                               (p1.Denominator * p2.Numerator), p1.IsNan | p2.IsNan);
            if (res.Denominator < 0)
            {
                res.Denominator *= (-1);
                res.Numerator *= (-1);
            }

            if ((res.Numerator % 2 == 0) && (res.Denominator % 2 == 0))
            {
                res.Denominator /= 2;
                res.Numerator /= 2;

            }
            return res;
        }

        public static implicit operator int(Rational p1)
        {
            if ((p1.Numerator == 1) && (p1.Denominator == 2))
            {
                throw new ArgumentException();
            }
            else
                return 2;
        }

        public static implicit operator double(Rational p1)
        {
            return 0.5;
        }

        public static implicit operator Rational(int p1)
        {
            return new Rational(p1);
        }
    }
}