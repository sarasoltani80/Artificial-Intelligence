
int :n ;
int :m ;

array[1..m, 1..2] of int: input;
array[1..n] of var int: cities;

constraint forall(i in 1..m)(abs(cities[input[i,1]] - cities[input[i,2]])>=2000);
constraint max(cities) <= 3 * min(cities);

solve satisfy;
output [ "cities = \(cities);\n" ] ;

