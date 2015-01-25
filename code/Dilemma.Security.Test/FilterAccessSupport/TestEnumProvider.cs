namespace Dilemma.Security.Test.FilterAccessSupport
{
    public class TestEnumProvider
    {
        private static TestEnumProvider instance;
        
        private TestEnum testEnum = TestEnum.X;

        private TestEnumProvider() { }

        public static TestEnum Value
        {
            get
            {
                if (instance == null)
                {
                    instance = new TestEnumProvider();
                }

                return instance.testEnum;    
            }

            set
            {
                if (instance == null)
                {
                    instance = new TestEnumProvider();
                }

                instance.testEnum = value;
            }
        }
    }
}