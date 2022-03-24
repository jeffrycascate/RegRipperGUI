namespace RegRipperAndAddIn
{
    internal class MethodInvoker
    {
        private object readStdErr;

        public MethodInvoker(object readStdErr)
        {
            this.readStdErr = readStdErr;
        }
    }
}