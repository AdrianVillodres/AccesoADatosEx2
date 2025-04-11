using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cat.itb.NF3EA2_VillodresAdrian.cruds
{
    class Program
    {

        static void Main(string[] args)
        {
            {
                BooksCRUD books = new BooksCRUD();
                CountriesCRUD countries = new CountriesCRUD();
                GradesCRUD grades = new GradesCRUD();
                PeopleCRUD people = new PeopleCRUD();
                ProductsCRUD products = new ProductsCRUD();
                RestaurantsCRUD restaurants = new RestaurantsCRUD();
                StudentsCRUD students = new StudentsCRUD();
                bool exit = false;

                while (!exit)
                {
                    Console.WriteLine("Choose an option:");
                    Console.WriteLine("EXERCICI 1: Importació de coleccions");
                    Console.WriteLine("1. Importar Coleccions");
                    Console.WriteLine("");
                    Console.WriteLine("EXERCICI 2: CONSULTES");
                    Console.WriteLine("     2: Comptar la població de tots els països d'Europa.");
                    Console.WriteLine("     3: Mostrar la capital, població i latlng de Madagascar.");
                    Console.WriteLine("     4: Mostrar títol, pàgines i categories dels llibres, ordenats per pàgines.");
                    Console.WriteLine("     5: Mostrar nom i tipus de cuina dels restaurants per codi postal.");
                    Console.WriteLine("     6: Mostrar dades dels restaurants al Bronx amb cuina xinesa.");
                    Console.WriteLine("     7: Mostrar llibres amb menys de 130 pàgines.");
                    Console.WriteLine("     8: Mostrar amics de Caroline Webster.");
                    Console.WriteLine("");
                    Console.WriteLine("EXERCICI 3: ACTUALITZAR DOCUMENTS");
                    Console.WriteLine("9: Actualitzar zipcode del carrer Driggs Avenue.");
                    Console.WriteLine("     10: Afegir el camp stockminim a productes amb preu superior a 2000.");
                    Console.WriteLine("     11: Afegir autor Sam Watters al llibre Code Generation in Action.");
                    Console.WriteLine("     12: Afegir el camp gama als productes segons el preu.");
                    Console.WriteLine("     13: Modificar les categories del producte 'MacBook Pro'.");
                    Console.WriteLine("     14: Actualitzar els productes entre 800 i 1000 de preu, establir Stock a 60.");
                    Console.WriteLine("     15: Afegir un nou codi a 'callingCodes' del país Iceland.");
                    Console.WriteLine("");
                    Console.WriteLine("EXERCICI 4: ELIMINAR DOCUMENTS");
                    Console.WriteLine("     16: Eliminar tots els restaurants del barri de Manhattan.");
                    Console.WriteLine("     17: Eliminar la primera categoria del producte 'iPhone 7'.");
                    Console.WriteLine("     18: Eliminar llibres amb menys de 100 pàgines.");
                    Console.WriteLine("     19: Eliminar el producte 'Apple TV'.");
                    Console.WriteLine("     20: Eliminar l'última categoria del llibre amb ISBN 1933988177.");
                    Console.WriteLine("     21: Eliminar tots els productes amb la categoria 'phone'.");
                    Console.WriteLine("     22: Eliminar el camp 'tags' dels professors 'teacher' a la col·lecció 'people'.");
                    Console.WriteLine("     23: Eliminar tots els països on es parli Espanyol a la col·lecció 'countries'.");
                    Console.WriteLine("");
                    Console.WriteLine("EXERCICI 5: ELIMINAR UNA COLLECTION");
                    Console.WriteLine("     24: Eliminar una col·lecció de la base de dades 'itb'.");
                    Console.WriteLine("");
                    Console.WriteLine("0. Exit");
                    Console.Write("Option: ");


                    switch (Console.ReadLine())
                    {
                        case "1":
                            Console.WriteLine("");

                            Console.WriteLine("");
                            break;
                        case "2":
                            Console.WriteLine("");

                            Console.WriteLine("");
                            break;
                        case "3":
                            Console.WriteLine("");

                            Console.WriteLine("");
                            break;
                        case "4":
                            Console.WriteLine("");

                            Console.WriteLine("");
                            break;
                        case "5":
                            Console.WriteLine("");

                            Console.WriteLine("");
                            break;
                        case "6":
                            Console.WriteLine("");

                            Console.WriteLine("");
                            break;
                        case "7":
                            Console.WriteLine("");

                            Console.WriteLine("");
                            break;
                        case "8":
                            Console.WriteLine("");

                            Console.WriteLine("");
                            break;
                        case "9":
                            Console.WriteLine("");

                            Console.WriteLine("");
                            break;
                        case "10":
                            Console.WriteLine("");

                            Console.WriteLine("");
                            break;
                        case "11":
                            Console.WriteLine("");

                            Console.WriteLine("");
                            break;
                        case "12":
                            Console.WriteLine("");

                            Console.WriteLine("");
                            break;
                        case "13":
                            Console.WriteLine("");

                            Console.WriteLine("");
                            break;
                        case "14":
                            Console.WriteLine("");

                            Console.WriteLine("");
                            break;
                        case "15":
                            Console.WriteLine("");

                            Console.WriteLine("");
                            break;
                        case "16":
                            Console.WriteLine("");

                            Console.WriteLine("");
                            break;
                        case "17":
                            Console.WriteLine("");

                            Console.WriteLine("");
                            break;
                        case "18":
                            Console.WriteLine("");

                            Console.WriteLine("");
                            break;
                        case "19":
                            Console.WriteLine("");

                            Console.WriteLine("");
                            break;
                        case "20":
                            Console.WriteLine("");

                            Console.WriteLine("");
                            break;
                        case "21":
                            Console.WriteLine("");

                            Console.WriteLine("");
                            break;
                        case "22":
                            Console.WriteLine("");

                            Console.WriteLine("");
                            break;
                        case "23":
                            Console.WriteLine("");

                            Console.WriteLine("");
                            break;
                        case "24":
                            Console.WriteLine("");

                            Console.WriteLine("");
                            break;
                        case "0":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Opción no válida, intente de nuevo.");
                            break;
                    }
                }

            }
        }
    }
}

