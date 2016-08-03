using KasaGE;

namespace SDK_Usage_SampleApp
{
    public class Test
    {
        public void T()
        {
            var ecr = new Dp25("COM1");
            ecr.FeedPaper(10);
            ecr.Dispose();
        }
    }
}