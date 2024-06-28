using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace coloringgraph_hoosh
{
    class Program
    {
        static void Main(string[] args)
        {
            int max_number_colors(List<List<int>> _graph, int _vertices)
            {
                int num_colors = 1;

                for (int i = 0; i < _vertices; i++)
                {
                    int sum = 0;
                    for (int j = 0; j < _vertices; j++)
                    {
                        sum += _graph[i][j];
                    }
                    if (sum > num_colors)
                        num_colors = sum + 1;
                }
                return num_colors;
            }
            int fitness_func(List<int> individual, List<List<int>> _graph, int vertices)
            {
                int fitness = 0;
                for (int i = 0; i < vertices; i++)
                {
                    for (int j = i + 1; j < vertices; j++)
                    {
                        if (individual[i] == individual[j] && _graph[i][j] == 1)
                            fitness += 1;
                    }
                }
                return fitness;
            }
            List<int> create_individual(int vertices, int numberofcolors)
            {
                Random rand = new Random();
                List<int> individual = new List<int>();
                for (int i = 0; i < vertices; i++)
                {
                    individual.Add(rand.Next(1, numberofcolors));
                }
                return individual;
            }

            List<int> one_point_crossover(List<int> individual1, List<int> individual2, int vertices)
            {
                List<int> child_individual1 = new List<int>();
                List<int> child_individual2 = new List<int>();
                Random rand = new Random();

                int makan = rand.Next(1, vertices - 2);
                for (int i = 0; i < makan + 1; i++)
                {
                    child_individual1.Add(individual1[i]);
                    child_individual2.Add(individual2[i]);
                }
                for (int j = makan + 1; j < vertices; j++)
                {
                    child_individual1.Add(individual2[j]);
                    child_individual2.Add(individual1[j]);
                }
                return child_individual1.Concat(child_individual2).ToList();
            }

            List<int> mutation(List<int> individual, int numberofcolors, int vertices)
            {
                Random rand = new Random();
                double test = rand.NextDouble();
                if (test <= 0.5)
                {
                    int makan = rand.Next(0, vertices - 1);
                    individual[makan] = rand.Next(1, numberofcolors);
                }
                return individual;
            }

            List<List<int>> firstpopulation_create(int size, int vertices, int numberofcolors)
            {
                List<List<int>> first_population = new List<List<int>>();
                for (int i=0; i< size; i++)
                {
                    List<int> individual = create_individual(vertices, numberofcolors);
                    first_population.Add(individual);
                }
                return first_population;
            }

            List<List<int>> population_selection(List<List<int>> population, int size, int vertices, List<List<int>> _graph)
            {
                List<List<int>> created_population = new List<List<int>>();
                for (int i = 0; i < size - 1; i += 2)
                {
                    if (fitness_func(population[i], _graph, vertices) < fitness_func(population[i + 1], _graph, vertices))
                        created_population.Add(population[i]);
                    else
                        created_population.Add(population[i + 1]);

                }
                var rnd = new Random();
                var randomized = population.OrderBy(item => rnd.Next());

                for (int i = 0; i < size - 1; i += 2)
                {
                    if (fitness_func(population[i], _graph, vertices) < fitness_func(population[i + 1], _graph, vertices))
                        created_population.Add(population[i]);
                    else
                        created_population.Add(population[i + 1]);

                }
                return created_population;
            }

            string[] tokens = Console.ReadLine().Split();
            int vertices = Convert.ToInt32(tokens[0]);
            int edges = Convert.ToInt32(tokens[1]);
            List<List<int>> Matrix = new List<List<int>>();
            List<List<int>> graph = new List<List<int>>();
            // getting input
            for (int i = 0; i < edges; i++)
            {
                string[] tokens2 = Console.ReadLine().Split();
                List<int> list1 = new List<int>();
                list1.Add(Convert.ToInt32(tokens2[1]));
                list1.Add(Convert.ToInt32(tokens2[2]));
                Matrix.Add(list1);
            }

            // make adjacency graph
            for (int i = 0; i < vertices; i++)
            {
                List<int> list1 = new List<int>();
                for (int j = 0; j < vertices; j++)
                {
                    list1.Add(0);
                }
                graph.Add(list1);
            }
            //Console.WriteLine(graph[1][2]);

            // meghdar dahi be graph
            for (int i = 0; i < edges; i++)
            {
                graph[Matrix[i][0] - 1][Matrix[i][1] - 1] = 1;
                graph[Matrix[i][1] - 1][Matrix[i][0] - 1] = 1;
            }
            //Console.WriteLine(graph[0][1]);

            int number_of_colors = max_number_colors(graph, vertices);

            List<int> best_individual = new List<int>();

            DateTime now = DateTime.Now;

            while (DateTime.Now.Subtract(now).Seconds < 59)
            {
                int sizeofpopulation = 300;

                List<List<int>> first_population = firstpopulation_create(sizeofpopulation, vertices, number_of_colors);
                int fitness = fitness_func(first_population[0], graph, vertices);
                //List<int> best_individual = new List<int>();
                best_individual = first_population[0];
                while(fitness != 0 || DateTime.Now.Subtract(now).Seconds < 59)
                {
                    first_population = population_selection(first_population, sizeofpopulation, vertices, graph);
                    var rnd = new Random();
                    var randomized = first_population.OrderBy(item => rnd.Next());
                    List<List<int>> new_population = new List<List<int>>();
                    for(int i=0; i< sizeofpopulation; i += 2)
                    {
                        List<int> new_individual = new List<int>();
                       // Console.WriteLine(new_individual.Count);
                        new_individual = one_point_crossover(first_population[i], first_population[i + 1], vertices);
                        //Console.WriteLine(new_individual.Count);
                        List<int> child1 = new List<int>();
                        List<int> child2 = new List<int>();
                        for (int j=0; j< vertices; j++)
                        {
                            child1.Add(new_individual[j]);
                        }
                        for(int k= vertices; k< 2*vertices; k++)
                        {
                            child2.Add(new_individual[k]);  
                        }
                        new_population.Add(child1);
                        new_population.Add(child2);
                    }

                    for(int i=0; i<sizeofpopulation; i++)
                    {
                        new_population[i] = mutation(new_population[i], number_of_colors, vertices);
                    }

                    first_population = new_population;
                    best_individual = first_population[0];
                    fitness = fitness_func(first_population[0], graph, vertices);
                    
                    foreach(List<int> individual in first_population)
                    {
                        if (fitness_func(individual, graph, vertices) < fitness)
                        {
                            best_individual = individual;
                            fitness = fitness_func(individual, graph, vertices);
                            
                        }
                    }

                }
                if (fitness == 0)
                    number_of_colors -= 1;
                else
                {
                    number_of_colors += 1;
                    break;
                }

                if (number_of_colors <= 0)
                    number_of_colors = 1;
                    
            }

            Console.WriteLine("#################################");
            Console.WriteLine(number_of_colors);
            for(int c=0; c<vertices; c++)
            {
                Console.Write(c+1  + "  ");
                Console.WriteLine(best_individual[c]);
            }

        }
    }
}

