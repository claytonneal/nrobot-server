using System;

namespace NRobot.Server.Test.Keywords
{

#pragma warning disable 1591


    /// <summary>
    /// Class containing different keyword method signatures
    /// </summary>
    public class TestKeywords
    {

#region IntegerReturnTypes


        public int Int_ReturnType()
        {
            return 1;
        }

        public Int32 Int32_ReturnType()
        {
            return 1;
        }

        public Int64 Int64_ReturnType()
        {
            return 1;
        }

#endregion

#region StringReturnTypes

        public string stringalias_ReturnType()
        {
            return "1";
        }

        public String string_ReturnType()
        {
            return "1";
        }


#endregion

#region DoubleReturnTypes

        public double DoubleAlias_ReturnType()
        {
            return 1;
        }

        public Double Double_ReturnType()
        {
            return 1;
        }

#endregion

#region BooleanReturnTypes

        public Boolean Boolean_ReturnType()
        {
            return true;
        }

        public bool BooleanAlias_ReturnType()
        {
            return true;
        }

#endregion

#region StringArrayReturnType

        public string[] StringArray_ReturnType()
        {
            return new String[] { "1", "2", "3" };
        }

#endregion

#region VoidReturnType

        public void Void_ReturnType() {  }

#endregion

#region StringParameters

        public void String_ParameterType(string arg1, string arg2) { }
       
#endregion

#region NoParameters

        public void No_Parameters() { }

#endregion

#region MethodAccess

        public void Public_Method() { }

        public static void PublicStatic_Method() { }


#endregion

#region UnsupportedReturnTypes

        public Single Single_ReturnType()
        {
            return 1;
        }

        public Decimal Decimal_ReturnType()
        {
            return 1;
        }

        public int[] IntegerArray_ReturnType()
        {
            return new int[] { 1, 2, 3 };
        }

        public double[] DoubleArray_ReturnType()
        {
            return new double[] { 1, 2, 3 };
        }

        public bool[] BooleanArray_ReturnType()
        {
            return new bool[] { true, false };
        }

#endregion

#region UnsupportedParameterTypes

        public void Integer_ParameterType(int arg1, int arg2) { }

        public void Boolean_ParameterType(bool arg1, bool arg2) { }

        public void Double_ParameterType(double arg1, double arg2) { }

        public void Mixed_ParameterType(string arg1, bool arg2, int arg3, double arg4) { }

        public void StringArrary_ParameterType(string[] arg1) { }

#endregion

#region UnSupportedMethodAccess

        private void Private_Method() { }

        internal void Internal_Method() { }

        protected void Protected_Method() { }

        [Obsolete]
        public void Obsolete_Method() { }

        private static void PrivateStatic_Method() { }

        internal static void InternalStatic_Method() { }

        protected static void ProtectedStatic_Method() { }

#endregion


    }

#pragma warning restore 1591

}
