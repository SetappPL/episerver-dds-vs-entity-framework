namespace Setapp.Business
{
    public class TimeComparision
    {
        public readonly double EntityFrameworkExecutionTime;
        public readonly double CustomTableExecutionTime;

        public TimeComparision(double entityFrameworkExecutionTime, double customTableExecutionTime)
        {
            EntityFrameworkExecutionTime = entityFrameworkExecutionTime;
            CustomTableExecutionTime = customTableExecutionTime;
        }
    }
}