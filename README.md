# What's the deal?
if you divide the ellipse into 3 parts by two parallel lines, leaving two equal parts and one "filling" between them. Is it possible to make a circle out of those two equal parts?

![Geometrie - GeoGebra - Google Chrome 14 04 2024 16_33_12](https://github.com/Otasmacour/EllipseToCircle/assets/111227700/490fa3f7-d7c8-4c7c-bab5-bc70f51b2d98)
![Geometrie - GeoGebra - Google Chrome 14 04 2024 16_33_128](https://github.com/Otasmacour/EllipseToCircle/assets/111227700/3aa76e4c-0216-4fcb-a6a6-924f55404448)
# How I approached this problem

![edit1](https://github.com/Otasmacour/EllipseToCircle/assets/111227700/6d724677-694f-41d5-ac65-68cde51d1d7e)

The two identical parts must be said to have identical or close together radius r1 and r2/distances from O (the optimally chosen point) to two points on the curves of the identical pieces cut from the ellipse. 
In the FindOptimalPoint() method, I look for the point on one half of the ellipse O for which the difference in its distance from B and the corresponding point on the ellipse is as small as possible. So I proceed on the major semi-axis along the points from B to S, and for each point I find the difference r1 and r2, and if I find a point that is an improvement for me in the sense of a smaller difference, it becomes the new best point. The smaller the offSet of steps along the axis I choose, the more optimal point I find (the smaller the difference of r1 and r2). For an offSet of 1*10^-7 I get a point with a difference of 2.384186E-07. Which is a number close to zero.

I'm counting on an ellipse whose center has coordinates [0,0] so expressing y as a function of x, which is needed to find the corresponding points on the ellipse for each point on the semi-axis from B to S, is easier.
![20240414_180417](https://github.com/Otasmacour/EllipseToCircle/assets/111227700/ba7821dd-06fe-4917-ad33-7053d9040a17)

I use this expression in the GetYCoordinateOnEllipse(float x) method. I ignore the absolute value because I'm only interested in the positive y-coordinate.

After I get the optimal point, I check if the 2 given resulting, equal shapes really form a circle. If so, it must be true that the distance from the optimal point to any point on the quarter circle is always the same.
![distances](https://github.com/Otasmacour/EllipseToCircle/assets/111227700/61ed4b42-8ec4-4fa4-8105-4cfaa6798d75)

In a similar way to the FindOptimalPoint() method, I pass points from B to O, plug their x-coordinates into GetYCoordinateOnEllipse(float x) to get the corresponding point on the ellipse, and calculate the distance from O for that point.
I evaluate the resulting distances by seeing how many are equal to each other, the result is that very few are. The shapes I have obtained by cutting the ellipse are definitely not half circles
![Desmos _ Graphing Calculator - Google Chrome 14 04 2024 15_35_33](https://github.com/Otasmacour/EllipseToCircle/assets/111227700/a4a84d06-9dc8-42e6-9d95-56b89c5472a1)

# Input 
```txt
number a, length of the semi-major axis
number b, the length of the semi-minor axis
```
# OutPut
```txt
Coordinates of the optimal point
Distance of the optimal point to according point on ellipse
Distance of the optimal point to point B
The difference of that distances
Number of calculated distances from points on quarter circle to optimal point
Most frequant distance
Frequancy od that distance
Percentage of the total distances calculated
```
# Example of Output
![C__Users_macou_source_repos_EllipseToCircle_EllipseToCircle_bin_Debug_EllipseToCircle exe 14 04 2024 18_45_21](https://github.com/Otasmacour/EllipseToCircle/assets/111227700/650384ff-3865-49ee-af84-59b2fff3648a)
Output for a=5 and b=3 as input. 

