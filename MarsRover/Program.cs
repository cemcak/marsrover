using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace MarsRover
{
    class Program
    {
        // Girilen veri ve kurallara göre yeni veriler hesaplanır
        public static JObject Calculate(string currentDirection, JObject oldLocation, string roverMoovments)
        {
            var currentXCoordinate = (int)oldLocation["x"];
            var currentYCoordinate = (int)oldLocation["y"];
            var newCoordinatesObj = new JObject();

            var returnObj = new JObject();
            returnObj["direction"] = "";
            returnObj["moovment_coordinate"] = "";
            returnObj["moovment"] = false;

            for (int i = 0; i < roverMoovments.Length; i++)
            {
                var order = roverMoovments[i].ToString();
                if (currentDirection == "N")
                {
                    if (order == "L")
                    {
                        returnObj["direction"] = "W";
                    }
                    if (order == "R")
                    {
                        returnObj["direction"] = "E";
                    }
                    if (order == "M")
                    {
                        returnObj["direction"] = "N";
                        newCoordinatesObj["x"] = currentXCoordinate;
                        newCoordinatesObj["y"] = currentYCoordinate + 1;
                        returnObj["new_coordinates"] = newCoordinatesObj;
                    }
                }
                if (currentDirection == "E")
                {
                    if (order == "L")
                    {
                        returnObj["direction"] = "N";
                    }
                    if (order == "R")
                    {
                        returnObj["direction"] = "S";
                    }
                    if (order == "M")
                    {
                        returnObj["direction"] = "E";
                        newCoordinatesObj["x"] = currentXCoordinate - 1;
                        newCoordinatesObj["y"] = currentYCoordinate;
                        returnObj["new_coordinates"] = newCoordinatesObj;
                    }
                }
                if (currentDirection == "W")
                {
                    if (order == "L")
                    {
                        returnObj["direction"] = "S";
                        returnObj["moovment"] = false;
                    }
                    if (order == "R")
                    {
                        returnObj["direction"] = "N";
                        returnObj["moovment"] = false;
                    }
                    if (order == "M")
                    {
                        returnObj["direction"] = "W";
                        newCoordinatesObj["x"] = currentXCoordinate + 1;
                        newCoordinatesObj["y"] = currentYCoordinate;
                        returnObj["new_coordinates"] = newCoordinatesObj;
                    }
                }
                if (currentDirection == "S")
                {
                    if (order == "L")
                    {
                        returnObj["direction"] = "E";
                    }
                    if (order == "R")
                    {
                        returnObj["direction"] = "W";
                    }
                    if (order == "M")
                    {
                        returnObj["direction"] = "S";
                        newCoordinatesObj["x"] = currentXCoordinate;
                        newCoordinatesObj["y"] = currentYCoordinate - 1;
                        returnObj["new_coordinates"] = newCoordinatesObj;
                    }
                }
            }
            return returnObj;
        }

        public static JArray Request(JArray calculateDatas) // Ekrandan aldığımız verileri topladıktan ve yeni veri girişi olmayacağından emin olduktan sonra Calculate fonksiyonu çağırılır
        {
            var returnArrayData = new JArray();
            foreach (var item in calculateDatas)
            {
                var newLocationAndDirection = Calculate((string)item["roverHeadingOld"], (JObject)item["coordinates"], (string)item["roverMoovments"]);
                returnArrayData.Add(newLocationAndDirection);
            }
            return returnArrayData;
        }

        public static JArray ConsoleProcess(JArray arrayData) // Recursive olarak ekrandan veri almayı sağlıyoruz.
        {
            var roverLocation = Console.ReadLine();
            if (string.IsNullOrEmpty(roverLocation))
            {
                var res = Request(arrayData);
                return res;
            } else
            {
                var splittingRoverLocation = roverLocation.Split(" ");
                var roverXCoordinateOld = Convert.ToInt32(splittingRoverLocation[0]);
                var roverYCoordinateOld = Convert.ToInt32(splittingRoverLocation[1]);
                var roverHeadingOld = splittingRoverLocation[2];
                var coordinates = new JObject();
                coordinates["x"] = roverXCoordinateOld;
                coordinates["y"] = roverYCoordinateOld;
                string roverMoovments = Console.ReadLine();
                var objectData = new JObject();
                objectData["roverHeadingOld"] = roverHeadingOld;
                objectData["coordinates"] = coordinates;
                objectData["roverMoovments"] = roverMoovments;
                arrayData.Add(objectData);
                return ConsoleProcess(arrayData);
            }
        }
                
        static void Main()
        {
            int x = 0;
            int y = 0;
            var objectData = new JObject();
            Console.WriteLine("Please enter:");
            string coordinates2 = Console.ReadLine();
            var splittingCoordinates = coordinates2.Split(" ");
            x = Convert.ToInt32(splittingCoordinates[0]);
            y = Convert.ToInt32(splittingCoordinates[1]);
            var roverLocation = Console.ReadLine();
            var splittingRoverLocation = roverLocation.Split(" ");
            var roverXCoordinateOld = Convert.ToInt32(splittingRoverLocation[0]);
            var roverYCoordinateOld = Convert.ToInt32(splittingRoverLocation[1]);
            var roverHeadingOld = splittingRoverLocation[2];
            var coordinates = new JObject();
            coordinates["x"] = roverXCoordinateOld;
            coordinates["y"] = roverYCoordinateOld;
            string roverMoovments = Console.ReadLine();
            var arraysData = new JArray();
            objectData["roverHeadingOld"] = roverHeadingOld;
            objectData["coordinates"] = coordinates;
            objectData["roverMoovments"] = roverMoovments;
            arraysData.Add(objectData);
            var resArray = ConsoleProcess(arraysData);
            foreach (var item in resArray)
            {
                Console.WriteLine(item["new_coordinates"]["x"].ToString() + " " + item["new_coordinates"]["y"].ToString() + " " + item["direction"].ToString());
            }
        }
    }
}
