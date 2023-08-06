using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsLibrary
{
    public class SampleClass
    {
        /*
                Fields
        */
        private int _sampleField = 0;



        /*
                Properties
        */
        public string SampleAutoProperty { get; set; }


        private double _sampleBackingField;
        public double SampleFullProperty
        {
            get { return _sampleBackingField; }
            set { _sampleBackingField = value; }
        }


        /*
                Constructors
        */
        // sample default constructor (no parameters)
        public SampleClass()
        {

        }

        //sample constructor with parameters
        public SampleClass(int intParam, double doubleParam, string stringParam)
        {
            SampleAutoProperty = stringParam;
            SampleFullProperty = doubleParam;
            _sampleField = intParam;
        }



        /*
                Methods
        */
        //instance (non-static) method
        public void SampleMethod()
        {
            Console.WriteLine($"{SampleAutoProperty} {SampleFullProperty} {_sampleField}");
        }



    }
}
