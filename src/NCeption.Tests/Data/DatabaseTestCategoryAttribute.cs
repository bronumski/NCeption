using NUnit.Framework;

namespace NCeption.Data
{
    public class DatabaseTestCategoryAttribute : CategoryAttribute
    {
        public DatabaseTestCategoryAttribute() : base("DatabaseTests")
        {
            
        } 
    }
}