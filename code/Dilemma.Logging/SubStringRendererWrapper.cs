namespace Dilemma.Logging
{
    [NLog.LayoutRenderers.LayoutRenderer("substring")]
    [NLog.LayoutRenderers.AmbientProperty("Start")]
    [NLog.LayoutRenderers.AmbientProperty("Length")]
    public class SubStringRendererWrapper : NLog.LayoutRenderers.Wrappers.WrapperLayoutRendererBase
    {
        [System.ComponentModel.DefaultValue(0)]
        public int Start { get; set; }

        [System.ComponentModel.DefaultValue(-1)]
        public int Length { get; set; }

        public SubStringRendererWrapper()
        {
            Start = 0;
            Length = -1;
        }

        protected override string Transform(string text)
        {
            if (Length < 0)
            {
                return text;
            }

            return text.Substring(Start, Length);
        }
    }

    [NLog.LayoutRenderers.LayoutRenderer("emailsubject")]
    public class EmailSubjectRendererWrapper : NLog.LayoutRenderers.Wrappers.WrapperLayoutRendererBase
    {
        protected override string Transform(string text)
        {
            return text.Replace(System.Environment.NewLine, " ");
        }
    }
}
