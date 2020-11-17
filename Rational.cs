using System;

namespace Incapsulation.RationalNumbers
{
    public class Rational
    {
        private int numerator;
        private int denominator;
        public Rational(int numerator, int denominator=1)
        {
            if (denominator == 0)
            {
                IsNan = true;
                throw new DivideByZeroException();
            }
            if (numerator!=0)
            {
                Numerator = numerator / BiggestCommonDenominator(numerator, denominator);
                Denominator = denominator / BiggestCommonDenominator(numerator, denominator);
            }
            else
            {
                Numerator = 0;
                Denominator = 1;
            }
        }

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
        //This method Equating Rational values to the Lowest Common Denominator
        public static Rational[] RationalValuesEquator (Rational value1, Rational value2)
        {
            int[] orderedDenominators = Ascender(value1.Denominator, value2.Denominator);
            int lowestCommonDen = LowestCommonDenominator(orderedDenominators);
            var equatedRational1 = NewNumeratorCalculation(value1, lowestCommonDen);
            var equatedRational2 = NewNumeratorCalculation(value2, lowestCommonDen);
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
        public static Rational NewNumeratorCalculation (Rational oldRational, int newDenominator)
        {
            int k = newDenominator / oldRational.Denominator;
            return new Rational(oldRational.Numerator*k,newDenominator);
        }
        //biggest common Denominator Method
        public static int BiggestCommonDenominator (int number1, int number2)
        {
            int lNumber = Ascender(number1, number2)[0];
            int hNumber = Ascender(number1, number2)[1];
            while(hNumber%lNumber!=0)
            {
                int tmp = hNumber % lNumber;
                hNumber = lNumber;
                lNumber = tmp;
            }
            lNumber = Math.Abs(lNumber);
            return lNumber;
        }
        public static Rational FractionReductor (Rational value)
        {
            int k = BiggestCommonDenominator(value.Numerator, value.Denominator);
            return new Rational(value.Numerator / k, value.Denominator / k);
        }
    }
}
