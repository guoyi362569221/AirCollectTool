namespace CNEMC.Repository
{
    using Env.CnemcPublish.RiaServices;
    using System;
    using OpenRiaServices.DomainServices.Client;
    using Env.CnemcPublish.DAL;

    public class CityDayAQIPublishHistoryRepository : RepositoryBase<CityDayAQIPublishHistory>
    {
        public CityDayAQIPublishHistoryRepository(EnvCnemcPublishDomainContext service) : base(service)
        {
        }

        public virtual void GetCityDayAqiHistoryByCondition(int cityCode, Action<byte[]> completedAction)
        {
            try
            {
                base.Service.GetCityDayAqiHistoryByCondition(cityCode).Completed += (delegate (object sender, EventArgs e) {
                    InvokeOperation<byte[]> operation = sender as InvokeOperation<byte[]>;
                    if (operation != null)
                    {
                        completedAction(operation.Value);
                    }
                });
            }
            catch (Exception e)
            {

            }
            
        }

        public virtual void GetServerTime(Action<DateTime> completedAction)
        {
            try
            {
                base.Service.GetServerTime().Completed += (delegate (object sender, EventArgs e) {
                    InvokeOperation<DateTime> operation = sender as InvokeOperation<DateTime>;
                    if (operation != null)
                    {
                        completedAction(operation.Value);
                    }
                });
            }
            catch (Exception e)
            {

            }
            
        }
    }
}

