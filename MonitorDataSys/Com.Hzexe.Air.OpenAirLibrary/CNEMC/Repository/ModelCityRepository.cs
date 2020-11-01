﻿namespace CNEMC.Repository
{
    using Env.CnemcPublish.RiaServices;
    using System;
    using OpenRiaServices.DomainServices.Client;
    using Env.CnemcPublish.DAL;

    public class ModelCityRepository : RepositoryBase<ModelCityConfig>
    {
        public ModelCityRepository(EnvCnemcPublishDomainContext service) : base(service)
        {
        }

        public virtual void AirCondition(Action<byte[]> completeAction, int cityCode)
        {
            try
            {
                base.Service.GetAirByCity(cityCode).Completed += (delegate (object sender, EventArgs e) {
                    InvokeOperation<byte[]> operation = sender as InvokeOperation<byte[]>;
                    if (operation != null)
                    {
                        completeAction(operation.Value);
                    }
                });
            }
            catch (Exception e)
            {

            }
            
        }

        public virtual void AllModelCities(Action<byte[]> completeAction)
        {
            try
            {
                base.Service.GetAllModelCities().Completed += (delegate (object sender, EventArgs e) {
                    InvokeOperation<byte[]> operation = sender as InvokeOperation<byte[]>;
                    if (operation != null)
                    {
                        completeAction(operation.Value);
                    }
                });
            }
            catch (Exception e)
            {

            }
            
        }
    }
}

