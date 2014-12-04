using NUnit.Framework;

namespace NCeption.Web
{
    class WebTestCategoryAttribute : CategoryAttribute
    {
        public WebTestCategoryAttribute() : base("WebTests")
        {
            
        }
    }
}