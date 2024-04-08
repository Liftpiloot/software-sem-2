using System.Runtime.Serialization;

namespace IronApp.Models;

public class DataPointModel
{
    //DataContract for Serializing Data - required to serve in JSON format
    [DataContract]
    public class DataPoint
    {
        public DataPoint(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
 
        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "x")]
        public double? X = null;
 
        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public double? Y = null;
    }
}