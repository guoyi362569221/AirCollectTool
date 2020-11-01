﻿namespace CNEMC.Repository
{
    using Env.CnemcPublish.RiaServices;
    using System;
    using OpenRiaServices.DomainServices.Client;
    using Env.CnemcPublish.DAL;
    using System.Collections.Generic;

    public class CityDayAQIPublishLiveRepository : RepositoryBase<CityDayAQIPublishLive>
    {
        public CityDayAQIPublishLiveRepository(EnvCnemcPublishDomainContext service) : base(service)
        {
        }

        public virtual void GetAllCityDayAqIs(Action<byte[]> completedAction)
        {
            try
            {
                base.Service.GetAllCityDayAQIModels().Completed += (delegate (object sender, EventArgs e) {
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

        public virtual void GetCityDayAQIBycityCode(int cityCode, Action<IEnumerable<CityDayAQIPublishLive>> completedAction)
        {
            try
            {
                EntityQuery<CityDayAQIPublishLive> cityDayAQIModelByCitycodeQuery = base.Service.GetCityDayAQIModelByCitycodeQuery(cityCode);
                base.ProcessCollection(completedAction, cityDayAQIModelByCitycodeQuery);
            }
            catch (Exception e)
            {

            }
            
        }
    }
}

