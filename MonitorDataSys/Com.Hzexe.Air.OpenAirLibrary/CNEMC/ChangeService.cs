namespace CNEMC
{
    using Env.CnemcPublish.RiaServices;
    using System;

    public class ChangeService
    {
        public ChangeContext changeService = new ChangeContext();
        public string time = DateTime.Now.ToString();

        public ChangeService()
        {
            try
            {
                TimeSpan sendTimeout = new TimeSpan(0, 10, 15);
                WcfTimeoutUtility.ChangeWcfTimeout(this.changeService, sendTimeout, sendTimeout);
            }
            catch (Exception e)
            {
                
            }
            
        }

        public override string ToString()
        {
            throw new NotImplementedException();
            /*
            this.changeService.GetEntityPm2_5(delegate (InvokeOperation<string> data) {
                this.time = data.();
            }, null);
            return this.time.ToString();
            */
        }
    }
}

