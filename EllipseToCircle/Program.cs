using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace EllipseToCircle
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Insert a: ");
            int a = int.Parse(Console.ReadLine());
            Console.Clear();
            Console.Write("Insert b: ");
            int b = int.Parse(Console.ReadLine());
            Console.Clear();
            Ellipse ellipse = new Ellipse(a,b);
            ellipse.FindIfTheEllipseCanBeCutToCircle();
            Console.ReadLine();
        }
    }
    class Ellipse
    {
        bool ellipseLies;
        float a;
        float b;
        Coordinates S = new Coordinates(0, 0);
        Coordinates B;
        public Ellipse(int a, int b)
        {
            if (a > b) { ellipseLies = true; }
            else { ellipseLies = false; }
            int greater = Math.Max(a, b);
            int lower = Math.Min(a, b);
            this.a = greater;
            this.b = lower;
            B = new Coordinates(greater, 0);        
        }
        float DistanceOfTwoPoints(Coordinates point1, Coordinates point2)
        {
            float distance = (float)Math.Sqrt(Math.Pow(point1.x - point2.x, 2) + Math.Pow(point1.y - point2.y, 2));
            return distance;
        }
        public void FindIfTheEllipseCanBeCutToCircle()
        {
            Coordinates bestPoint = FindOptimalPointAGreaterThanB();
            float x = bestPoint.x;
            float y = bestPoint.y;
            if (!ellipseLies)
            {
                x = y;
                y = bestPoint.x;
            }
            Console.WriteLine("--The result of the search for the optimal point--");
            Coordinates pointOnEllipseSofarBest2 = new Coordinates(bestPoint.x, GetYCoordinateOnEllipse(bestPoint.x));
            float BestSoFarDistanceToPointOnEllipse2 = DistanceOfTwoPoints(bestPoint, pointOnEllipseSofarBest2);
            float BestSoFarDistanceToB2 = DistanceOfTwoPoints(bestPoint, B);
            float BestSoFarDifference2 = Difference(BestSoFarDistanceToPointOnEllipse2, BestSoFarDistanceToB2);
            Console.WriteLine("Optimal point: [" + x + "][" + y + "]");
            Console.WriteLine("Distance of the best point from point [best point, F(best point)] on the ellipse: " + BestSoFarDistanceToPointOnEllipse2 + '\n' + "Distance of the best point to point B: " + BestSoFarDistanceToB2 + '\n' + "The difference of these distances: " + BestSoFarDifference2);
            var result = HowManyDistancesMatch(bestPoint);
            Console.WriteLine("--The result of finding distance matches on the quarter circle from optimal point--");
            Console.WriteLine("Most frequant distance: " + result.mostFrequentDistance);
            Console.WriteLine("Frequancy: "+result.Frequency);
        }
        (float mostFrequentDistance, int Frequency) HowManyDistancesMatch(Coordinates optimalPoint)
        {
            Coordinates currentPoint = new Coordinates(B.x, B.y);
            Dictionary<float, int> distances = new Dictionary<float, int>();
            //From B to OptimalPoint, ellipse lies on the x-axis
            float offSet = 0.001f;
            float distanceCurrentToOptimal = DistanceOfTwoPoints(currentPoint, optimalPoint);
            bool run = true;
            while(run)
            {
                currentPoint.x = currentPoint.x - offSet;
                float newDistanceCurrentToOptimal = DistanceOfTwoPoints(currentPoint, optimalPoint);
                if (newDistanceCurrentToOptimal > distanceCurrentToOptimal)
                {
                    run = false;
                }
                else
                {
                    Coordinates pointOnEllipseCurrent = new Coordinates(currentPoint.x, GetYCoordinateOnEllipse(currentPoint.x));
                    float CurrentDistanceToPointOnEllipseFromOptimal = DistanceOfTwoPoints(optimalPoint, pointOnEllipseCurrent);
                    if(distances.ContainsKey(CurrentDistanceToPointOnEllipseFromOptimal))
                    {
                        distances[CurrentDistanceToPointOnEllipseFromOptimal]++;
                    }
                    else
                    {
                        distances.Add(CurrentDistanceToPointOnEllipseFromOptimal, 1);
                    }
                }
            }
            float mostFrequentDistance = distances.First().Key;
            foreach(var item in distances)
            {
                if (distances[item.Key] > distances[mostFrequentDistance])
                {
                    mostFrequentDistance = item.Key;
                }
            }
            return (mostFrequentDistance, distances[mostFrequentDistance]);
        }
        Coordinates FindOptimalPointAGreaterThanB()
        {
            float distanceFromStoB = DistanceOfTwoPoints(S, B);
            float offSetToSetRandomBestPointSoFar = distanceFromStoB / 19; //Divided by my age
            Coordinates bestPointSoFar = new Coordinates(B.x-offSetToSetRandomBestPointSoFar, B.y);
            Coordinates currentPoint = new Coordinates(B.x, B.y);
            //From B to S, ellipse lies on the x-axis
            float offSet = 0.000001f;
            float distanceCurrentToS = DistanceOfTwoPoints(currentPoint, S);
            bool run = true;
            while (run)
            {
                currentPoint.x = currentPoint.x - offSet;
                float newDistanceCurrentToS = DistanceOfTwoPoints(currentPoint, S);
                if (newDistanceCurrentToS > distanceCurrentToS)
                {
                    run = false;
                }
                else
                {
                    distanceCurrentToS = newDistanceCurrentToS;
                    Coordinates pointOnEllipseCurrent = new Coordinates(currentPoint.x, GetYCoordinateOnEllipse(currentPoint.x));
                    float CurrentDistanceToPointOnEllipse = DistanceOfTwoPoints(currentPoint, pointOnEllipseCurrent);
                    float CurrentDistanceToB = DistanceOfTwoPoints(currentPoint, B);
                    float CurrentDifference = Difference(CurrentDistanceToPointOnEllipse, CurrentDistanceToB);

                    Coordinates pointOnEllipseSofarBest = new Coordinates(bestPointSoFar.x, GetYCoordinateOnEllipse(bestPointSoFar.x));
                    float BestSoFarDistanceToPointOnEllipse = DistanceOfTwoPoints(bestPointSoFar, pointOnEllipseSofarBest);
                    float BestSoFarDistanceToB = DistanceOfTwoPoints(bestPointSoFar, B);
                    float BestSoFarDifference = Difference(BestSoFarDistanceToPointOnEllipse, BestSoFarDistanceToB);
                    if (CurrentDifference < BestSoFarDifference)
                    {
                        bestPointSoFar.x = currentPoint.x;
                        bestPointSoFar.y = currentPoint.y;
                    }
                }
            }
            return bestPointSoFar;
        }
        float Difference(float n1, float n2)
        {
            return Math.Abs(n1 - n2);
        }
        float GetYCoordinateOnEllipse(float x)
        {
            return (float)Math.Sqrt(Math.Pow(b, 2) - (Math.Pow(x * b, 2) / Math.Pow(a, 2)));
        }
    }
    class Coordinates
    {
        public Coordinates(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public float x;
        public float y;
    }
}