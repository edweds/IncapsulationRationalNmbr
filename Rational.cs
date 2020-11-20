using System;

namespace Incapsulation.RationalNumbers
{
    public class Rational
    {
        public Rational(int numerator, int denominator = 1, bool isReduce = true)
        {
            if (denominator == 0)
            {
                IsNan = true;
            }
            if (numerator != 0)
            {
                if (!isReduce)
                {
                    Numerator = numerator;
                    Denominator = denominator;
                }
                else
                {
                    Numerator = FractionReductor(numerator, denominator)[0];
                    Denominator = FractionReductor(numerator, denominator)[1];
                }                
            }
            else
            {
                Numerator = 0;
                Denominator = 1;
            }
        }
        private int numerator;
        private int denominator;
       
        public int Numerator
        {
            get { return numerator; }
            set { numerator = value; }
        }
        public int Denominator
        {
            get { return denominator; }
            set
            {
                // only numerator may have below zero value
                if (value < 0) 
                    numerator *= -1; 
                denominator = Math.Abs(value);                    
            }
        }
        public bool IsNan { get; set; }

        public static Rational operator+ (Rational value1,Rational value2)
        {
            if (value1.IsNan)
                return value1;
            if (value2.IsNan)
                return value2;
            var result = new Rational(1,1);
            if (value1.Denominator == value2.Denominator)
                result = new Rational(value1.Numerator + value2.Numerator, value2.Denominator);
            else
            {
                var arrayEquatedRationals = RationalValuesEquator(value1, value2);
                result = new Rational(arrayEquatedRationals[0].Numerator + arrayEquatedRationals[1].Numerator, arrayEquatedRationals[0].Denominator);
            }
            return result;
        }

        public static Rational operator -(Rational value1, Rational value2)
        {
            if (value1.IsNan)
                return value1;
            if (value2.IsNan)
                return value2;
            var result = new Rational(1, 1);
            if (value1.Denominator == value2.Denominator)
                result = new Rational(value1.Numerator - value2.Numerator, value1.Denominator);
            else
            {
                var arrayEquatedRationals = RationalValuesEquator(value1, value2);
                result = new Rational(arrayEquatedRationals[0].Numerator - arrayEquatedRationals[1].Numerator, arrayEquatedRationals[0].Denominator);
            }
            return result;
        }

        public static Rational operator *(Rational value1, Rational value2)
        {
            if (value1.IsNan)
                return value1;
            if (value2.IsNan)
                return value2;
            var result = new Rational(value1.Numerator * value2.Numerator, value1.Denominator * value2.Denominator);
            return result;
        }

        public static Rational operator /(Rational value1, Rational value2)
        {
            if (value1.IsNan)
                return value1;
            if (value2.IsNan)
                return value2;
            var result = new Rational(value1.Numerator * value2.Denominator, value1.Denominator * value2.Numerator);
            return result;
        }

        public static implicit operator double (Rational value)
        {
            if (!value.IsNan)
                return (double)value.Numerator / value.Denominator;
            return double.NaN;
        }
        public static implicit operator Rational(int value)
        {
            return new Rational(value);
        }
        public static explicit operator int(Rational value)
        {
            if (!value.IsNan)
            {
                if (value.Numerator % value.Denominator == 0)
                {
                    return value.Numerator / value.Denominator;
                }
                else
                    throw new InvalidCastException();
            }
            else
                throw new ArgumentException();
        }


        //This method converting Rational number to Double value number
        public double ConvertToDouble()
        {
            return 0;
        }
        //This method Equating Rational values to the Lowest Common Denominator
        public static Rational[] RationalValuesEquator (Rational value1, Rational value2)
        {
            int[] orderedDenominators = Ascender(value1.Denominator, value2.Denominator);
            int lowestCommonDen = LowestCommonDenominator(orderedDenominators);
            var equatedRational1 = NewNumeratorCalculation(value1, lowestCommonDen,false);
            var equatedRational2 = NewNumeratorCalculation(value2, lowestCommonDen,false);
            return new Rational[] { equatedRational1, equatedRational2 };
        }
        //THis method find Lower Common Denominator for Rational numbers
        public static int LowestCommonDenominator(int[] orderedDenominators)
        {
            int i=2;
            int result = orderedDenominators[1];
            if ( result% orderedDenominators[0] == 0)
                return result;
            while (result% orderedDenominators [0]!= 0)
            {
                result *= i;
                i++;
            }
            return result;
        }
        //This method arrange two values in order of Ascending
        public static int[] Ascender(int val1,int val2)
        {
            var result=new int[2];
            if (val1 > val2)
            {
                result[0] = val2;
                result[1] = val1;
            }
            else
            {
                result[0] = val1;
                result[1] = val2;
            }
            return result;
        }
        //new Enumerator value method based on new denumerator
        public static Rational NewNumeratorCalculation(Rational oldRational, int newDenominator, bool reduce = true)
        {
            int k = newDenominator / oldRational.Denominator;
            return new Rational(oldRational.Numerator*k,newDenominator,reduce);
        }
        //biggest common Denominator Method
        public static int BiggestCommonDenominator (int number1, int number2)
        {
            var arr = Ascender(number1, number2);
            int lNumber = arr[0];
            int hNumber = arr[1];
            while(hNumber%lNumber!=0)
            {
                int tmp = hNumber % lNumber;
                hNumber = lNumber;
                lNumber = tmp;
            }
            lNumber = Math.Abs(lNumber);
            return lNumber;
        }
        public static int[] FractionReductor(int numerator, int denominator)
        {
            int k = 1;
            if (denominator !=0)
                k = BiggestCommonDenominator(numerator, denominator);
            return new int[] { numerator / k, denominator / k };
                           
        }
    }
}
