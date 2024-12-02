namespace Comnet.Common.Helpers
{
    public class StringValueAttribute : System.Attribute
    {
        public string StringValue { get; protected set; }

        /// <summary>
        /// Constructor used to init a StringValue Attribute
        /// </summary>
        /// <param name="value"></param>
        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }
    }
}
